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

using System.Diagnostics;
using System.IO;
using System.Xml;
using ZIDE.Models;

namespace ZIDE.Controllers.Persistance
{
    /// <summary>
    /// Class capable of saving/loading projects from and to streams
    /// </summary>
    public class ProjectSaveLoader
    {
        private readonly string _path;
        private readonly Stream _stream;

        ~ProjectSaveLoader()
        {
            _stream?.Close();
        }

        /// <summary>
        /// Creates a new instance of the ProjectSaveLoader class with an associated .zsp stream
        /// </summary>
        /// <param name="stream">The stream to the target .zsp contents</param>
        public ProjectSaveLoader(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Creates a new instance of the ProjectSaveLoader class with an associated .zsp path
        /// </summary>
        /// <param name="path">The path to the target .zsp file</param>
        public ProjectSaveLoader(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Saves the given project to the underlying stream
        /// </summary>
        /// <param name="project">The project to save</param>
        public void SaveProject(ZScriptProject project)
        {
            var document = new XmlDocument();

            // Generate an XML structure
            var xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = document.DocumentElement;
            document.InsertBefore(xmlDeclaration, root);

            // Base project settings
            var projectNode = document.CreateElement(string.Empty, "project", string.Empty);
            projectNode.SetAttribute("projectName", project.Name);
            document.AppendChild(projectNode);

            // Project path
            var projectBasePath = document.CreateElement(string.Empty, "projectBasePath", string.Empty);
            projectBasePath.AppendChild(document.CreateTextNode(project.BasePath));
            projectNode.AppendChild(projectBasePath);

            // Project file
            var projectFile = document.CreateElement(string.Empty, "projectFile", string.Empty);
            projectFile.AppendChild(document.CreateTextNode(project.ProjectFilePath));
            projectNode.AppendChild(projectFile);

            if (_path != null)
            {
                document.Save(_path);
                return;
            }

            _stream.Position = 0;
            document.Save(_stream);
        }

        /// <summary>
        /// Loads the project from the underlying stream
        /// </summary>
        /// <returns>The project that was loaded</returns>
        public ZScriptProject LoadProject()
        {
            var xml = new XmlDocument();

            if (_path != null)
            {
                xml.Load(_path);
            }
            else
            {
                _stream.Position = 0;
                xml.Load(_stream);
            }

            var basePath = "";
            var projectName = "";
            var projectFilePath = "";

            // Get the project now
            var nodeProject = xml["project"];

            // Project name
            Debug.Assert(nodeProject != null, "projectNode != null");
            projectName = nodeProject.Attributes["projectName"].Value;

            // Project base path
            var nodeBasePath = nodeProject["projectBasePath"];
            Debug.Assert(nodeBasePath != null, "nodeBasePath != null");
            basePath = nodeBasePath.InnerText;

            // Project file path
            var nodeFilePath = nodeProject["projectFile"];
            Debug.Assert(nodeFilePath != null, "nodeFilePath != null");
            projectFilePath = nodeFilePath.InnerText;

            return new ZScriptProject(basePath, projectName, projectFilePath);
        }
    }
}
