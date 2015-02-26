using System.Collections.Generic;
using System.IO;
using System.Xml;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using WeifenLuo.WinFormsUI.Docking;

using ZIDE.Models;
using ZIDE.Services;
using ZIDE.Services.Scripting;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// Represents a script document container
    /// </summary>
    public sealed partial class ScriptDocumentForm : DockContent
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

            // Setup the text editor on the document
            te_textEditor.Document.TextEditorProperties.ConvertTabsToSpaces = true;
            te_textEditor.Document.TextEditorProperties.ShowSpaces = true;
            te_textEditor.Document.TextEditorProperties.ShowTabs = true;

            InitializeSyntaxHighlighting();

            // Add the realtime syntax check service
            var syntaxService = new RealtimeSyntaxCheckService(this);
        }

        /// <summary>
        /// Initializes the syntax highlighting on this form
        /// </summary>
        void InitializeSyntaxHighlighting()
        {
            HighlightingManager.Manager.AddSyntaxModeFileProvider(new ZScriptSyntaxModeProvider()); // Attach to the text editor
            te_textEditor.SetHighlighting("ZScript"); // Activate the highlighting, use the name from the SyntaxDefinition node
        }

        /// <summary>
        /// Basic syntax mode provider
        /// </summary>
        class ZScriptSyntaxModeProvider : ISyntaxModeFileProvider
        {
            /// <summary>
            /// List of syntax modes provided by this ZScriptSyntaxModeProvider
            /// </summary>
		    List<SyntaxMode> _syntaxModes;

            /// <summary>
            /// Gets a collection of syntax modes provided by this ZScriptSyntaxModeProvider
            /// </summary>
            public ICollection<SyntaxMode> SyntaxModes
            {
                get { return _syntaxModes; }
            }

            /// <summary>
            /// Initializes a new instance of the ZScriptSyntaxModeProvider class
            /// </summary>
            public ZScriptSyntaxModeProvider()
		    {
			    UpdateSyntaxModeList();
		    }
		    
            /// <summary>
            /// Updates the syntax mode list
            /// </summary>
		    public void UpdateSyntaxModeList()
		    {
                _syntaxModes = new List<SyntaxMode> { new SyntaxMode("ZScript", "ZScript", ".xshd") };
		    }

            /// <summary>
            /// Gets an XML reader for the contents ofthe given syntax mode file
            /// </summary>
            /// <param name="syntaxMode">The syntax mode to get</param>
            /// <returns>An XML reader for the given syntax mode</returns>
            public XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
		    {
                var bytes = Properties.Resources.ZScript_Mode;
                return new XmlTextReader(new MemoryStream(bytes));
		    }
        }
    }
}