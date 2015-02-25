using System;
using System.Collections.Generic;
using System.Linq;

using ZIDE.Controllers.Interfaces;
using ZIDE.Models;

namespace ZIDE.Controllers
{
    /// <summary>
    /// Controls script file management on the interface
    /// </summary>
    public class ScriptsController : IDocumentEventSource
    {
        /// <summary>
        /// Number used during creation of new untitled documents to guarantee uniqueness
        /// </summary>
        private int _untitledSufixNum;

        /// <summary>
        /// The main controller this scripts controller is binded to
        /// </summary>
        private readonly MainController _mainController;

        /// <summary>
        /// List of currently opened script documents
        /// </summary>
        private readonly List<ZScriptDocument> _openDocuments;

        /// <summary>
        /// The main controller this scripts controller is binded to
        /// </summary>
        public MainController MainController
        {
            get { return _mainController; }
        }

        /// <summary>
        /// Gets an array of all currently opened script documents
        /// </summary>
        public ZScriptDocument[] OpenDocuments
        {
            get { return _openDocuments.ToArray(); }
        }

        /// <summary>
        /// Delegate for a document-related event
        /// </summary>
        /// <param name="sender">The object that fired the event</param>
        /// <param name="eventArgs">The arguments for the event</param>
        public delegate void DocumentEventHandler(object sender, DocumentEventArgs eventArgs);

        /// <summary>
        /// Event fired whenever a document has been opened by this ScriptsController
        /// </summary>
        public event DocumentEventHandler DocumentOpened;

        /// <summary>
        /// Event fired whenever a document has been closed by this ScriptsController
        /// </summary>
        public event DocumentEventHandler DocumentClosed;

        /// <summary>
        /// Initializes a new instance of the ScriptsController class
        /// </summary>
        /// <param name="mainController">A main controller to bind to this scripts controller</param>
        public ScriptsController(MainController mainController)
        {
            _mainController = mainController;

            _mainController.OnApplicationStart += MainController_OnOnApplicationStart;
            _openDocuments = new List<ZScriptDocument>();
        }

        /// <summary>
        /// Open the default document on this script controller
        /// </summary>
        private void OpenDefaultDocument()
        {
            // If no other documents are open, open a default empty document
            if (_openDocuments.Count == 0)
            {
                CreateNewDocument();
            }
        }

        /// <summary>
        /// Creates and opens a new empty document on this scripts controller
        /// </summary>
        /// <returns>The document that was created</returns>
        public ZScriptDocument CreateNewDocument()
        {
            var documentName = GetNameForUntitledDocument();

            var document = new ZScriptDocument(documentName);

            OpenDocument(document);

            return document;
        }

        /// <summary>
        /// Returns a document in this ScriptsController by name, returning null if none was found
        /// </summary>
        /// <param name="documentName">The document name to search for</param>
        /// <returns>A ZScriptDocument that matches the given nane; or null, if none was found</returns>
        public ZScriptDocument GetDocumentByName(string documentName)
        {
            return _openDocuments.FirstOrDefault(d => d.DocumentName == documentName);
        }

        /// <summary>
        /// Opens a new document on this ScriptsController
        /// </summary>
        /// <param name="document">The document to open</param>
        public void OpenDocument(ZScriptDocument document)
        {
            // Add the document to the list of documents
            _openDocuments.Add(document);

            // Fire event
            if (DocumentOpened != null)
            {
                DocumentOpened(this, new DocumentEventArgs(document));
            }
        }

        /// <summary>
        /// Closes a given document on this ScriptsController.
        /// If the document is not currently opened on this scripts controller, nothing is done
        /// </summary>
        /// <param name="document">The document to close</param>
        public void CloseDocument(ZScriptDocument document)
        {
            if (!_openDocuments.Contains(document))
                return;

            _openDocuments.Remove(document);

            // Fire event
            if (DocumentClosed != null)
            {
                DocumentClosed(this, new DocumentEventArgs(document));
            }
        }

        /// <summary>
        /// Generates and returns a unique name for an untitled document on this scripts controller
        /// </summary>
        /// <returns>A unique name for an untitled document on this ScriptsController</returns>
        private string GetNameForUntitledDocument()
        {
            // Always increment by one
            _untitledSufixNum++;

            const string prefix = "Untitled ";

            // Iterate until no more collisions occur
            while (GetDocumentByName(string.Format("{0}{1}", prefix, _untitledSufixNum)) != null)
            {
                _untitledSufixNum++;
            }

            return prefix + _untitledSufixNum;
        }

        #region Event handlers

        // 
        // OnApplicationStarted event
        // 
        private void MainController_OnOnApplicationStart(object sender, EventArgs eventArgs)
        {
            OpenDefaultDocument();
        }

        #endregion
    }

    /// <summary>
    /// Event arguments for a document-related event
    /// </summary>
    public class DocumentEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the document that was opened
        /// </summary>
        public ZScriptDocument Document { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DocumentOpenedEventArgs class
        /// </summary>
        /// <param name="document">The document to bind with this event</param>
        public DocumentEventArgs(ZScriptDocument document)
        {
            Document = document;
        }
    }
}