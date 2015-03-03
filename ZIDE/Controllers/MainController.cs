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