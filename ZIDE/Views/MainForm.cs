using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using ZIDE.Controllers;
using ZIDE.Models;
using ZIDE.Views.Controls;

namespace ZIDE.Views
{
    /// <summary>
    /// Represents the class for the main form of the application
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The main controller for the application
        /// </summary>
        private readonly MainController _mainController;

        /// <summary>
        /// The project tree form for the application
        /// </summary>
        private ProjectTreeForm _projectTreeForm;

        /// <summary>
        /// List of currently opened document forms
        /// </summary>
        private readonly List<ScriptDocumentForm> _openedDocumentForms = new List<ScriptDocumentForm>();

        /// <summary>
        /// Gets the main controller for the application
        /// </summary>
        public MainController MainController
        {
            get { return _mainController; }
        }

        /// <summary>
        /// Initializes a new instance of the MainForm class
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _mainController = new MainController(this);

            SetupEvents();

            CreateInterface();
            UpdateView();

            _mainController.StartApplication();
        }

        /// <summary>
        /// Sets up events on the main controller
        /// </summary>
        private void SetupEvents()
        {
            // Setup events
            _mainController.DocumentEventSource.DocumentOpened += DocumentEventSource_DocumentOpened;
            _mainController.DocumentEventSource.DocumentClosed += DocumentEventSource_DocumentClosed;
        }

        #region Interface-related methods

        /// <summary>
        /// Creates the default interface modules
        /// </summary>
        public void CreateInterface()
        {
            // Add the project tree panel
            _projectTreeForm = new ProjectTreeForm { HideOnClose = true };

            if (dp_mainPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                _projectTreeForm.MdiParent = this;
                _projectTreeForm.Show();
            }
            else
            {
                _projectTreeForm.Show(dp_mainPanel);
                _projectTreeForm.DockTo(dp_mainPanel, DockStyle.Right);
            }
        }

        /// <summary>
        /// Updates the contents of all the views of the form
        /// </summary>
        public void UpdateView()
        {
            UpdateTitleBar();
        }

        /// <summary>
        /// Updates the title bar of the form
        /// </summary>
        public void UpdateTitleBar()
        {
            // Update the title bar with the application's name
            Text = @"ZIDE - ZScript IDE";
        }

        /// <summary>
        /// Adds a new document interface for the given document
        /// </summary>
        /// <param name="document">The document to create the interface for</param>
        void AddDocumentInterface(ZScriptDocument document)
        {
            ScriptDocumentForm form = new ScriptDocumentForm(document);

            if (dp_mainPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                form.MdiParent = this;
                form.Show();
            }
            else
            {
                form.Show(dp_mainPanel);
            }

            // Hookup a closing event
            form.FormClosing += ScriptForm_OnFormClosing;

            _openedDocumentForms.Add(form);
        }

        /// <summary>
        /// Returns a ScriptDocumentForm from this interface that is associated with the given document.
        /// Returns null, if no form on this interface is associated with the given document
        /// </summary>
        /// <param name="document">A valid ZScriptDocument object to get the associated form</param>
        /// <returns>
        /// A ScriptDocumentForm which has the <see cref="ScriptDocumentForm.Document"/> property
        /// matching the passed document; or null, if no opened form matches the document
        /// </returns>
        ScriptDocumentForm GetFormForDocument(ZScriptDocument document)
        {
            return _openedDocumentForms.FirstOrDefault(form => form.Document == document);
        }

        /// <summary>
        /// Removes a document from this interface
        /// </summary>
        /// <param name="document">The document to remove from this interface</param>
        void RemoveDocumentInterface(ZScriptDocument document)
        {
            // Find the form for the given document
            var form = GetFormForDocument(document);

            // Quit if the document is not present in this interface
            if (form == null || form.IsClosing)
                return;

            // Set the form as closing so events aren't fired twice
            form.IsClosing = true;
            // Close the form
            form.Close();

            _openedDocumentForms.Remove(form);
        }

        #endregion

        #region Event handlers

        #region Main menu

        // 
        // Project main menu button click
        // 
        private void tsm_projectWindow_Click(object sender, EventArgs e)
        {
            _projectTreeForm.Show(dp_mainPanel);
        }

        #endregion

        #region Toolbar

        // 
        // New File toolbar button click
        // 
        private void tsb_newFile_Click(object sender, EventArgs e)
        {
            _mainController.CreateNewDocument();
        }

        #endregion

        #region Document Event Source

        // 
        // DocumentOpened event
        // 
        void DocumentEventSource_DocumentOpened(object sender, DocumentEventArgs eventArgs)
        {
            AddDocumentInterface(eventArgs.Document);
        }

        // 
        // DocumentClosed event
        // 
        void DocumentEventSource_DocumentClosed(object sender, DocumentEventArgs eventArgs)
        {
            // Close the interface
            RemoveDocumentInterface(eventArgs.Document);
        }

        #endregion

        #region Script Forms

        // 
        // OnClosing
        // 
        private void ScriptForm_OnFormClosing(object sender, FormClosingEventArgs eventArgs)
        {
            ScriptDocumentForm form = (ScriptDocumentForm)sender;

            // Ignore closing if the form is already closing
            if (form.IsClosing)
                return;

            form.IsClosing = true;

            _mainController.CloseDocument(form.Document);
        }

        #endregion

        #endregion
    }
}