using System.Collections.Generic;
using System.IO;
using System.Xml;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using WeifenLuo.WinFormsUI.Docking;

using ZIDE.Models;
using ZIDE.Services.Scripting;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// Represents a script document container
    /// </summary>
    public sealed partial class ScriptDocumentForm : DockContent, IScriptForm
    {
        /// <summary>
        /// The document associated with this scripts document form
        /// </summary>
        private readonly ZScriptDocument _document;

        /// <summary>
        /// Gets the text editor control for this script document form
        /// </summary>
        public TextEditorControl TextEditorControl
        {
            get { return te_textEditor; }
        }

        /// <summary>
        /// Gets or sets a value specifying whether this form is currently being closed by either the user or code
        /// </summary>
        public bool IsClosing { get; set; }

        /// <summary>
        /// Gets the document associated with this scripts document form
        /// </summary>
        public ZScriptDocument Document
        {
            get { return _document; }
        }

        /// <summary>
        /// Initializes a new instance of the ScriptDocumentForm class
        /// </summary>
        /// <param name="document">The document to associate with this scripts document form</param>
        public ScriptDocumentForm(ZScriptDocument document)
        {
            InitializeComponent();

            _document = document;

            Text = document.DocumentName;
            te_textEditor.Text = Document.Contents;

            // Add the realtime syntax check service
            var syntaxService = new RealtimeSyntaxCheckService(this);
        }
    }
}