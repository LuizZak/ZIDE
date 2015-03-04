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
        public ScriptDocumentForm()
            : this(new ZScriptDocument("Untitled document"))
        {

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
            te_textEditor.Document.DocumentChanged += Document_OnDocumentChanged;

            // Add the realtime syntax check service
            syntaxService = new RealtimeSyntaxCheckService(this);
        }

        // 
        // Document changed event listener
        // 
        private void Document_OnDocumentChanged(object sender, DocumentEventArgs documentEventArgs)
        {
            _document.Contents = documentEventArgs.Document.TextContent;
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