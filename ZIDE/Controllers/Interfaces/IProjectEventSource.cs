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
using ZIDE.Models;

namespace ZIDE.Controllers.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes that dispatch events related to project management
    /// </summary>
    public interface IProjectEventSource
    {
        /// <summary>
        /// Event fired whenever a project has been opened by this IProjectEventSource
        /// </summary>
        event ProjectEventHandler ProjectOpened;

        /// <summary>
        /// Event fired whenever a project has been closed by this IProjectEventSource
        /// </summary>
        event ProjectEventHandler ProjectClosed;

        /// <summary>
        /// Event fired whenever a project has been created by this IProjectEventSource
        /// </summary>
        event ProjectEventHandler ProjectCreated;
    }

    /// <summary>
    /// Delegate for a project-related event
    /// </summary>
    /// <param name="sender">The object that fired the event</param>
    /// <param name="eventArgs">The arguments for the event</param>
    public delegate void ProjectEventHandler(object sender, ProjectEventArgs eventArgs);

    /// <summary>
    /// Event arguments for a project-related event
    /// </summary>
    public class ProjectEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the document that was opened
        /// </summary>
        public ZScriptProject Project { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ProjectEventArgs class
        /// </summary>
        /// <param name="project">The project to bind with this event</param>
        public ProjectEventArgs(ZScriptProject project)
        {
            Project = project;
        }
    }
}
