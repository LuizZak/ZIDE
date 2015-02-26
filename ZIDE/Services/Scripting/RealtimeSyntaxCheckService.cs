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

namespace ZIDE.Services.Scripting
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
        /// Instance of the messages margin for placing messages into
        /// </summary>
        private readonly MessagesMargin _messagesMargin;

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

            _messagesMargin = new MessagesMargin(_form.TextEditorControl.ActiveTextAreaControl.TextArea);
            _form.TextEditorControl.ActiveTextAreaControl.TextArea.InsertLeftMargin(0, _messagesMargin);
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
            _messagesMargin.MessageContainer = generator.MessageContainer;
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

            AddMessageMarkers();
            AddMessageIcons();
        }

        /// <summary>
        /// Adds the message markers to the text
        /// </summary>
        private void AddMessageMarkers()
        {
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
        /// Adds the message icons to the text
        /// </summary>
        private void AddMessageIcons()
        {
            
        }

        /// <summary>
        /// Adds markers on the text base on a given code scope's contents
        /// </summary>
        private void AddMemberMarkers(CodeScope scope)
        {
            // Add the definition markers
            var definitions = scope.GetAllDefinitionsRecursive();

            foreach (var definition in definitions.OfType<ValueHolderDefinition>().Where(d => d.Context != null && d.Context is ZScriptParser.ValueHolderDeclContext))
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
            foreach (var usage in usages.Where(u => u.Definition != null && u.Context != null))
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

        /// <summary>
        /// Text editor margin class used to display little icons for the messages
        /// </summary>
        class MessagesMargin : AbstractMargin
        {
            /// <summary>
            /// Gets or sets the message container to pull the messages from
            /// </summary>
            public MessageContainer MessageContainer { get; set; }

            /// <summary>
            /// Tooltip used to show the warning/error under the line the user is pointing at
            /// </summary>
            private readonly ToolTip _toolTip;

            // 
            // AbstractMargin.Size override
            // 
            public override Size Size
            {
                get { return new Size(20, -1); }
            }

            /// <summary>
            /// Initializes a new instance of the MessagesMargin class
            /// </summary>
            /// <param name="textArea">The text area to attach this messages margin to</param>
            public MessagesMargin(TextArea textArea)
                : base(textArea)
            {
                _toolTip = new ToolTip();

                textArea.MouseHover += TextArea_OnMouseHover;
            }

            private void TextArea_OnMouseHover(object sender, EventArgs eventArgs)
            {
                // Check if the mouse is over any error/message
                if (MessageContainer == null)
                    return;

                Point mousepos = TextArea.PointToClient(Control.MousePosition);

                foreach (var message in MessageContainer.AllMessages)
                {
                    int lineNumber = textArea.Document.GetVisibleLine(message.Line - 1);
                    int lineHeight = textArea.TextView.FontHeight;
                    int yPos = (lineNumber * lineHeight) - textArea.VirtualTop.Y;
                    if (IsLineInsideRegion(yPos, yPos + lineHeight, 0, TextArea.Size.Height))
                    {
                        if (lineNumber == textArea.Document.GetVisibleLine(message.Line - 2))
                        {
                            // marker is inside folded region, do not draw it
                            continue;
                        }

                        Rectangle target = new Rectangle(2, yPos, 16, 16);

                        if (target.Contains(mousepos))
                        {
                            _toolTip.Show(message.Message, TextArea, new Point(0, yPos + lineHeight), 500);
                            break;
                        }
                    }
                }
            }

            // 
            // AbstractMargin.HandleMouseLeave override
            // 
            public override void HandleMouseLeave(EventArgs e)
            {
                base.HandleMouseLeave(e);

                _toolTip.Hide(TextArea);
            }

            // 
            // AbstractMargin.Paint override
            // 
            public override void Paint(Graphics g, Rectangle rect)
            {
                if (rect.Width <= 0 || rect.Height <= 0)
                {
                    return;
                }

                // paint background
                g.FillRectangle(SystemBrushes.Control, new Rectangle(drawingPosition.X, rect.Top, drawingPosition.Width - 1, rect.Height));
                g.DrawLine(SystemPens.ControlDark, drawingPosition.Right - 1, rect.Top, drawingPosition.Right - 1, rect.Bottom);

                // paint icons
                if (MessageContainer != null)
                {
                    foreach (var message in MessageContainer.AllMessages)
                    {
                        int lineNumber = textArea.Document.GetVisibleLine(message.Line - 1);
                        int lineHeight = textArea.TextView.FontHeight;
                        int yPos = (lineNumber * lineHeight) - textArea.VirtualTop.Y;
                        if (IsLineInsideRegion(yPos, yPos + lineHeight, rect.Y, rect.Bottom))
                        {
                            if (lineNumber == textArea.Document.GetVisibleLine(message.Line - 2))
                            {
                                // marker is inside folded region, do not draw it
                                continue;
                            }

                            Rectangle origin = new Rectangle(0, 0, 16, 16);
                            Rectangle target = new Rectangle(2, yPos, 16, 16);

                            Image image = message is Warning ? Properties.Resources.code_warning : Properties.Resources.code_error;

                            g.DrawImage(image, target, origin, GraphicsUnit.Pixel);
                            //mark.Draw(this, g, new Point(0, yPos));
                        }
                    }
                }

                base.Paint(g, rect);
            }

            static bool IsLineInsideRegion(int top, int bottom, int regionTop, int regionBottom)
            {
                if (top >= regionTop && top <= regionBottom)
                {
                    // Region overlaps the line's top edge.
                    return true;
                }
                if (regionTop > top && regionTop < bottom)
                {
                    // Region's top edge inside line.
                    return true;
                }

                return false;
            }
        }
    }
}