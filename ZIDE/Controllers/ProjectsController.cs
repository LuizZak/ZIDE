#region License information
/*
The MIT License (MIT)

Copyright (c) 2015 Luiz Fernando Silva

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZIDE.Controllers.Interfaces;
using ZIDE.Controllers.Persistance;
using ZIDE.Models;

namespace ZIDE.Controllers
{
    /// <summary>
    /// Controls project management on the interface
    /// </summary>
    public class ProjectsController : IProjectEventSource
    {
        /// <summary>
        /// The inner list of currently open projects
        /// </summary>
        private readonly List<ZScriptProject> _openProjects = new List<ZScriptProject>();

        /// <summary>
        /// Gets an array of the currently opened projects
        /// </summary>
        public ZScriptProject[] OpenProjects => _openProjects.ToArray();

        /// <summary>
        /// Gets the main controller this projects controller is binded to
        /// </summary>
        public MainController MainController { get; }
        
        public event ProjectEventHandler ProjectOpened;

        public event ProjectEventHandler ProjectClosed;

        public event ProjectEventHandler ProjectCreated;

        /// <summary>
        /// Initializes a new instance of the ProjectsController class
        /// </summary>
        /// <param name="mainController">The main controller to bind this projects controller to</param>
        public ProjectsController(MainController mainController)
        {
            MainController = mainController;
        }

        /// <summary>
        /// Creates a new project with the given settings
        /// If the project creation settings has invalid properties, an exception is raised
        /// </summary>
        /// <param name="settings">The settings to create the project from</param>
        /// <param name="openAfter">Whether to open the project on this controller after creation</param>
        /// <returns>A new ZScriptProject object that encapsulates the project created</returns>
        public ZScriptProject CreateProject(ProjectCreationSettings settings, bool openAfter)
        {
            var validation = ValidateProjectCreationSettings(settings);
            if (validation.Count(IsProjectValidationWarningFatal) > 0)
            {
                throw new ArgumentException($"Invalid settings in the project creation: {validation}", nameof(settings));
            }

            // Create the directory tree
            Directory.CreateDirectory(settings.Path);

            // Create the project file at the path
            string filePath = Path.GetFullPath(settings.Path) + Path.DirectorySeparatorChar + settings.ProjectName + "." + ZScriptProject.DefaultExtension;
            File.Create(filePath).Close();

            var project = new ZScriptProject(settings.Path, settings.ProjectName, filePath);

            SaveProjectToDisk(project);

            ProjectCreated?.Invoke(this, new ProjectEventArgs(project));

            if (openAfter)
            {
                OpenProject(project);
            }

            return project;
        }

        /// <summary>
        /// Opens the given project in this project controller
        /// </summary>
        /// <param name="project">The project to open</param>
        public void OpenProject(ZScriptProject project)
        {
            if (GetOpenProjectByProjectFilePath(project.ProjectFilePath) != null)
            {
                return;
            }

            _openProjects.Add(project);

            ProjectOpened?.Invoke(this, new ProjectEventArgs(project));
        }

        /// <summary>
        /// Closes the given project on this project controller
        /// </summary>
        /// <param name="project">The project to close</param>
        public void CloseProject(ZScriptProject project)
        {
            if (!_openProjects.Contains(project))
                return;

            _openProjects.Remove(project);
        }

        /// <summary>
        /// Saves the given ZScriptProject file to disk.
        /// The path to save to will be the ProjectFilePath property of the ZScriptProject object
        /// </summary>
        /// <param name="project">The project to save</param>
        public void SaveProjectToDisk(ZScriptProject project)
        {
            var saver = new ProjectSaveLoader(project.ProjectFilePath);

            saver.SaveProject(project);
        }

        /// <summary>
        /// Gets an open project by its project file path
        /// Returns null, if no project with the given file path was found
        /// </summary>
        /// <param name="projectFilePath">The project file path to search</param>
        /// <returns>A project currently opened with a matching project path; or null, if none exists</returns>
        public ZScriptProject GetOpenProjectByProjectFilePath(string projectFilePath)
        {
            return _openProjects.FirstOrDefault(project => Path.GetFullPath(project.ProjectFilePath) == Path.GetFullPath(projectFilePath));
        }

        /// <summary>
        /// Validates the given project creation settings, returning a validation dictionary containing key-pair values for the validation result
        /// The keys for the dictionary are the property names of the ProjectCreationSettings struct, and the list is a list of validation strings
        /// The validation is passed if an empty dictionary is returned
        /// </summary>
        /// <param name="settings">The settings to validate</param>
        /// <returns>The result of the validation</returns>
        public ProjectCreationValidationWarning[] ValidateProjectCreationSettings(ProjectCreationSettings settings)
        {
            var result = new List<ProjectCreationValidationWarning>();

            // Invalid project name (must be a valid file name)
            if (Path.GetInvalidFileNameChars().Any(s => settings.ProjectName.Contains(s)))
            {
                result.Add(ProjectCreationValidationWarning.InvalidProjectName);
            }

            // Invalid path
            if (Path.GetInvalidPathChars().Any(s => settings.Path.Contains(s)))
            {
                result.Add(ProjectCreationValidationWarning.InvalidProjectPath);
            }

            // Project path is an existing file name
            if (File.Exists(settings.Path))
            {
                result.Add(ProjectCreationValidationWarning.ProjectPathIsFileName);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns whether the given project validation warning is fatal (blocks project creation) or not
        /// </summary>
        /// <param name="warning">The warning to test</param>
        /// <returns>Whether the project creation warning is fatal</returns>
        public bool IsProjectValidationWarningFatal(ProjectCreationValidationWarning warning)
        {
            // Currently all two implemented warnings are fatal
            return true;
        }
    }

    /// <summary>
    /// A structure describing a set of project creation settings
    /// </summary>
    public struct ProjectCreationSettings
    {
        /// <summary>
        /// Gets or sets the path for creating the project
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the name of the project to create
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Whether to automatically import ZScript files at the project path during creation
        /// </summary>
        public bool AutoImportFilesAtPath;

        /// <summary>
        /// Creates a new instance of the ProjectCreationSettings struct with the given settings
        /// </summary>
        public ProjectCreationSettings(string projectName, string path, bool autoImportAtPath)
        {
            Path = path;
            ProjectName = projectName;
            AutoImportFilesAtPath = autoImportAtPath;
        }
    }

    /// <summary>
    /// A warning for a project creation settings validation process
    /// </summary>
    public enum ProjectCreationValidationWarning
    {
        InvalidProjectName,
        InvalidProjectPath,
        ProjectPathIsFileName
    }
}