using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using ZIDE.Views.Controls;

using ZScript.CodeGeneration;
using ZScript.CodeGeneration.Analysis;
using ZScript.CodeGeneration.Definitions;
using ZScript.CodeGeneration.Messages;

namespace ZIDE.Services
{
    /// <summary>
    /// Service that provides realtime syntax checking functionalities for script document forms
    /// </summary>
    public class RealtimeSyntaxCheckService
    {
        /// <summary>
        /// The first to provide syntax checking service to
        /// </summary>
        private readonly ScriptDocumentForm _form;

        /// <summary>
        /// Gets the document associated with this realtime syntax check service
        /// </summary>
        private IDocument Document
        {
            get { return _form.TextEditorControl.Document; }
        }

        /// <summary>
        /// The container for the error messages found during parsing
        /// </summary>
        private MessageContainer _messageContainer;

        /// <summary>
        /// The code scope for the current document
        /// </summary>
        private CodeScope _codeScope;

        /// <summary>
        /// Timer used to introduce a delay in parsing during keystrokes
        /// </summary>
        private readonly Timer _parseTimer;

        /// <summary>
        /// Default interval before the parse timer performs the script parsing
        /// </summary>
        private const int ParseInterval = 100;

        /// <summary>
        /// Initialzies a new instance of the RealtimeSyntaxCheckService class
        /// </summary>
        /// <param name="form">The form to provide the syntax checking service to</param>
        public RealtimeSyntaxCheckService(ScriptDocumentForm form)
        {
            _form = form;

            // Hookup the events
            Document.DocumentChanged += Document_OnDocumentChanged;

            _messageContainer = new MessageContainer();

            _parseTimer = new Timer { Interval = ParseInterval };

            _parseTimer.Tick += ParseTimer_OnTick;
        }

        // 
        // Parse Timer Tick event
        // 
        private void ParseTimer_OnTick(object sender, EventArgs eventArgs)
        {
            _parseTimer.Stop();

            AnalyzeSyntax();
        }

        /// <summary>
        /// Refreshes the script currently opened
        /// </summary>
        void RefreshScript()
        {
            // Parse the script
            var generator = new ZRuntimeGenerator(Document.TextContent);
            generator.ParseSources();

            if(!generator.MessageContainer.HasSyntaxErrors)
            {
                // TODO: See how to deal with multiple scopes coming from multiple sources merged into one
                _codeScope = generator.CollectDefinitions();
                //_codeScope = generator.SourceProvider.Sources[0].Definitions.CollectedBaseScope;
            }
            else
            {
                _codeScope = null;
            }

            _messageContainer = generator.MessageContainer;
        }

        /// <summary>
        /// Updates the display of the syntax
        /// </summary>
        void UpdateDisplay()
        {
            Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.WholeTextArea));
            Document.CommitUpdate();

            // Clear markers on document
            Document.MarkerStrategy.RemoveAll(m => true);

            if(_codeScope != null)
                AddMemberMarkers(_codeScope);

            if (!_messageContainer.HasMessages)
            {
                return;
            }

            // Add markers around syntax errors
            foreach (var syntaxError in _messageContainer.SyntaxErrors)
            {
                var locationStart = new TextLocation(syntaxError.Column, syntaxError.Line - 1);
                var locationEnd = new TextLocation(syntaxError.Column + 1, syntaxError.Line - 1);

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.WaveLine)
                {
                    ToolTip = syntaxError.Message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }

            // Add markers around code errors
            foreach (var codeError in _messageContainer.CodeErrors)
            {
                var locationStart = new TextLocation(codeError.Column, codeError.Line - 1);
                var locationEnd = new TextLocation(codeError.Column + 1, codeError.Line - 1);

                if (codeError.Context != null)
                {
                    locationEnd = new TextLocation(codeError.Context.Stop.Column, codeError.Context.Stop.Line - 1);
                }

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.WaveLine, Color.Blue)
                {
                    ToolTip = codeError.Message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }

            // Add markers around warnings
            foreach (var warnings in _messageContainer.Warnings)
            {
                Color color = Color.YellowGreen;
                string message = warnings.Message;

                var locationStart = new TextLocation(warnings.Column, warnings.Line - 1);
                var locationEnd = new TextLocation(warnings.Column + 1, warnings.Line - 1);

                if (warnings.Context != null)
                {
                    locationEnd = new TextLocation(warnings.Context.Stop.Column, warnings.Context.Stop.Line - 1);
                }

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.WaveLine, color)
                {
                    ToolTip = message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }
        }

        /// <summary>
        /// Adds markers on the text base on a given code scope's contents
        /// </summary>
        private void AddMemberMarkers(CodeScope scope)
        {
            // Add the definition markers
            var definitions = scope.GetAllDefinitionsRecursive();

            foreach (var definition in definitions.OfType<ValueHolderDefinition>().Where(d => d.Context != null))
            {
                string message = definition.Type.ToString();
                var context = (ZScriptParser.ValueHolderDeclContext)definition.Context;
                var declContext = context.let ?? context.var;

                var locationStart = new TextLocation(context.Start.Column, context.Start.Line - 1);
                var locationEnd = new TextLocation(context.Start.Column + declContext.Text.Length, context.Start.Line - 1);

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.Invisible)
                {
                    ToolTip = message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }

            var usages = scope.GetAllUsagesRecursive().ToArray();

            // Register valid usages
            foreach (var usage in usages.Where(u => u.Definition != null))
            {
                string message = usage.Definition.ToString();
                var context = usage.Context;

                var locationStart = new TextLocation(context.Start.Column, context.Start.Line - 1);
                var locationEnd = new TextLocation(context.Stop.Column, context.Start.Line - 1);

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.Invisible)
                {
                    ToolTip = message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }
        }

        /// <summary>
        /// Analyzes the syntax on the form's document
        /// </summary>
        private void AnalyzeSyntax()
        {
            RefreshScript();
            UpdateDisplay();
        }

        // 
        // TextContentChanged event handler
        // 
        private void Document_OnDocumentChanged(object sender, DocumentEventArgs eventArgs)
        {
            _parseTimer.Stop();
            _parseTimer.Start();

            _parseTimer.Interval = ParseInterval;
        }
    }
}