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
using System.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using ZIDE.Controllers;
using ZIDE.Controllers.Interfaces;
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
        public MainController MainController => _mainController;

        /// <summary>
        /// Initializes a new instance of the MainForm class
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _mainController = new MainController(this);

            SetupEvents();

            CreateInterface();
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

        /// <summary>
        /// Creates a new script file
        /// </summary>
        private void NewScript()
        {
            _mainController.CreateNewDocument();
        }

        /// <summary>
        /// Creates a new testbed file
        /// </summary>
        private void NewTestbed()
        {
            _mainController.CreateNewTestbedDocument();
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        private void NewProject()
        {
            _mainController.CreateNewProject();
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
        public void UpdateInterface()
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
            ScriptDocumentForm form;

            var zTestbedDoc = document as ZTestbedScriptDocument;
            if (zTestbedDoc != null)
            {
                form = new TestbedDocumentForm(zTestbedDoc);
            }
            else
            {
                form = new ScriptDocumentForm(document);
            }

            ShowDockContent(form);

            // Hookup a closing event
            form.FormClosing += ScriptForm_OnFormClosing;
            form.GotFocus += ScriptForm_GotFocus;
            form.LostFocus += ScriptForm_LostFocus;

            _openedDocumentForms.Add(form);
        }

        /// <summary>
        /// Displays a dock content form on this form
        /// </summary>
        /// <param name="dockContent">The dock content to show on this form</param>
        private void ShowDockContent(DockContent dockContent)
        {
            if (dp_mainPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                dockContent.MdiParent = this;
                dockContent.Show();
            }
            else
            {
                dockContent.Show(dp_mainPanel);
            }
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
            if (form != null && !form.IsClosing)
            {
                // Set the form as closing so events aren't fired twice
                form.IsClosing = true;
                // Close the form
                form.Close();
            }

            _openedDocumentForms.Remove(form);

            // Always have an empty document opened
            if (_openedDocumentForms.Count == 0)
            {
                _mainController.CreateNewDocument();
            }

            UpdateInterface();
        }

        #endregion

        #region Event handlers

        // 
        // OnShown event handler
        // 
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _mainController.StartApplication();
            UpdateInterface();
        }

        #region Main menu

        // 
        // Project main menu button click
        // 
        private void tsm_projectWindow_Click(object sender, EventArgs e)
        {
            _projectTreeForm.Show(dp_mainPanel);
        }

        // 
        // New Script main menu button click
        // 
        private void tsm_newScript_Click(object sender, EventArgs e)
        {
            NewScript();
        }

        // 
        // New Testbed main menu button click
        // 
        private void tsm_newTestBed_Click(object sender, EventArgs e)
        {
            NewTestbed();
        }

        // 
        // New Project main menu button click
        // 
        private void tsm_newProject_Click(object sender, EventArgs e)
        {
            NewProject();
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

        // 
        // GotFocus
        // 
        void ScriptForm_GotFocus(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        // 
        // LostFocus
        // 
        void ScriptForm_LostFocus(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        #endregion

        #endregion
    }
}