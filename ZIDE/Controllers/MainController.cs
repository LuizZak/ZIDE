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
using ZIDE.Controllers.Interfaces;
using ZIDE.Models;
using ZIDE.Views;

namespace ZIDE.Controllers
{
    /// <summary>
    /// Represents the main controller for the application
    /// </summary>
    public class MainController
    {
        /// <summary>
        /// The main form for the application
        /// </summary>
        private readonly MainForm _mainForm;

        /// <summary>
        /// The scripts controller binded to this main controller
        /// </summary>
        private readonly ScriptsController _scriptsController;

        /// <summary>
        /// The projects controller binded to this main controller
        /// </summary>
        private readonly ProjectsController _projectsController;

        /// <summary>
        /// Event handler fired when the application has started.
        /// This event is fired by the constructor for the MainController class after all initialization is performed
        /// </summary>
        public event EventHandler OnApplicationStart;

        /// <summary>
        /// Gets the main form for the application
        /// </summary>
        public MainForm MainForm
        {
            get { return _mainForm; }
        }

        /// <summary>
        /// Getsthe document event sources for the application
        /// </summary>
        public IDocumentEventSource DocumentEventSource
        {
            get { return _scriptsController; }
        }

        /// <summary>
        /// Gets the projects controller binded to this main controller
        /// </summary>
        public ProjectsController ProjectsController
        {
            get { return _projectsController; }
        }

        /// <summary>
        /// Initializes a new instance of the MainController class
        /// </summary>
        /// <param name="mainForm">The main form to attach this controller to</param>
        public MainController(MainForm mainForm)
        {
            _mainForm = mainForm;

            _scriptsController = new ScriptsController(this);
            _projectsController = new ProjectsController(this);
        }

        /// <summary>
        /// Creates a new document on the program
        /// </summary>
        /// <returns>The document that was just created</returns>
        public ZScriptDocument CreateNewDocument()
        {
            return _scriptsController.CreateNewDocument();
        }

        /// <summary>
        /// Creates a new document on the program
        /// </summary>
        /// <returns>The document that was just created</returns>
        public ZTestbedScriptDocument CreateNewTestbedDocument()
        {
            return _scriptsController.CreateNewTestbedDocument();
        }

        /// <summary>
        /// Closes a given document from the application
        /// </summary>
        /// <param name="document">The document to close</param>
        public void CloseDocument(ZScriptDocument document)
        {
            _scriptsController.CloseDocument(document);
        }

        /// <summary>
        /// Signals the controller the application has started - should be used by the main form after it is opened
        /// </summary>
        public void StartApplication()
        {
            // Fire application started event
            if (OnApplicationStart != null)
            {
                OnApplicationStart(this, new EventArgs());
            }
        }
    }
}