using System;
using ICSharpCode.TextEditor;

using WeifenLuo.WinFormsUI.Docking;

using ZIDE.Models;
using ZIDE.Services.Scripting;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// Represents a script document container
    /// </summary>
    public partial class ScriptDocumentForm : DockContent, IScriptForm
    {
        /// <summary>
        /// The syntax service used by this script document form to provide syntax support
        /// </summary>
        protected RealtimeSyntaxCheckService syntaxService;

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

            te_textEditor.Text = Document.Contents;

            // Add the realtime syntax check service
            syntaxService = new RealtimeSyntaxCheckService(this);
        }

        /// <summary>
        /// Initializes a new instance of the ScriptDocumentForm class
        /// </summary>
        public ScriptDocumentForm()
            : this(new ZScriptDocument("Untitled document"))
        {

        }

        // 
        // Form.OnShown override
        // 
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Text = _document.DocumentName;
        }
    }
}