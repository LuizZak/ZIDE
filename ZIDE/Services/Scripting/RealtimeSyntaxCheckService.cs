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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using Antlr4.Runtime;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using ZIDE.Utils;
using ZIDE.Views.Controls;

using ZScript.CodeGeneration;
using ZScript.CodeGeneration.Analysis;
using ZScript.CodeGeneration.Definitions;
using ZScript.CodeGeneration.Messages;
using ZScript.Parsing.ANTLR;

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
        private readonly IScriptForm _form;

        /// <summary>
        /// Gets the document associated with this realtime syntax check service
        /// </summary>
        private IDocument Document => _form.TextEditorControl.Document;

        /// <summary>
        /// Gets the runtime generator for the realtime syntax check service
        /// </summary>
        public ZRuntimeGenerator RuntimeGenerator { get; private set; }

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
        /// The background worker used to parse the script
        /// </summary>
        private readonly BackgroundWorker _backgroundWorker;

        /// <summary>
        /// Default interval before the parse timer performs the script parsing
        /// </summary>
        private const int ParseInterval = 100;

        /// <summary>
        /// Delegate for the ScriptParsed event
        /// </summary>
        /// <param name="sender">The sender for the event</param>
        /// <param name="eventArgs">The arguments for the event</param>
        public delegate void ScriptParsedEventHandler(object sender, ScriptParsedEventArgs eventArgs);

        /// <summary>
        /// Occurs whenever the parsing of a script has finished
        /// </summary>
        public event ScriptParsedEventHandler ScriptParsed;

        /// <summary>
        /// Initialzies a new instance of the RealtimeSyntaxCheckService class
        /// </summary>
        /// <param name="form">The form to provide the syntax checking service to</param>
        public RealtimeSyntaxCheckService(IScriptForm form)
        {
            _form = form;

            // Hookup the events
            Document.DocumentChanged += Document_OnDocumentChanged;

            _messageContainer = new MessageContainer();

            _parseTimer = new Timer { Interval = ParseInterval };
            _parseTimer.Tick += ParseTimer_OnTick;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += BackgroundWorker_OnDoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_OnRunWorkerCompleted;

            _messagesMargin = new MessagesMargin(_form.TextEditorControl.ActiveTextAreaControl.TextArea);
            _form.TextEditorControl.ActiveTextAreaControl.TextArea.InsertLeftMargin(0, _messagesMargin);

            InitializeSyntaxHighlighting();
        }

        /// <summary>
        /// Initializes the syntax highlighting on this form
        /// </summary>
        void InitializeSyntaxHighlighting()
        {
            // Setup the text editor on the document
            _form.TextEditorControl.Document.TextEditorProperties.ConvertTabsToSpaces = true;
            _form.TextEditorControl.Document.TextEditorProperties.ShowSpaces = true;
            _form.TextEditorControl.Document.TextEditorProperties.ShowTabs = true;

            HighlightingManager.Manager.AddSyntaxModeFileProvider(new ZScriptSyntaxModeProvider()); // Attach to the text editor
            _form.TextEditorControl.SetHighlighting("ZScript"); // Activate the highlighting, use the name from the SyntaxDefinition node
        }

        // 
        // Parse Timer Tick event
        // 
        private void ParseTimer_OnTick(object sender, EventArgs eventArgs)
        {
            _parseTimer.Stop();

            StartScriptParse();
        }

        // 
        // BackgroundWorker's DoWork event
        // 
        private void BackgroundWorker_OnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            RefreshScript();
        }

        // 
        // BackgroundWorker's RunWorkerCompleted event
        // 
        private void BackgroundWorker_OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            lock (Document)
            {
                ScriptParsed?.Invoke(this, new ScriptParsedEventArgs(_codeScope != null, _codeScope, RuntimeGenerator));

                _messageContainer = RuntimeGenerator.MessageContainer;
                _messagesMargin.MessageContainer = RuntimeGenerator.MessageContainer;

                UpdateDisplay();
            }
        }

        /// <summary>
        /// Parses the script
        /// </summary>
        public void Parse()
        {
            StartScriptParse();
        }

        /// <summary>
        /// Starts the script parsing, preparing the runtime generator and starting the parsing background worker
        /// </summary>
        void StartScriptParse()
        {
            // Quit if an operation is already in process on the background worker
            if (_backgroundWorker.IsBusy)
                return;

            // Parse the script
            RuntimeGenerator = new ZRuntimeGenerator(Document.TextContent);

            _backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Refreshes the script currently opened
        /// </summary>
        void RefreshScript()
        {
            RuntimeGenerator.ParseSources();

            if (!RuntimeGenerator.MessageContainer.HasSyntaxErrors)
            {
                // TODO: See how to deal with multiple scopes coming from multiple sources merged into one
                _codeScope = RuntimeGenerator.CollectDefinitions();
                //_codeScope = generator.SourceProvider.Sources[0].Definitions.CollectedBaseScope;
            }
            else
            {
                _codeScope = null;
            }
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
        }

        /// <summary>
        /// Adds the message markers to the text
        /// </summary>
        private void AddMessageMarkers()
        {
            // Add markers around warnings
            foreach (var warning in _messageContainer.Warnings)
            {
                Color color = Color.YellowGreen;
                string message = warning.Message;

                var locationStart = new TextLocation(warning.Column, warning.Line - 1);
                var locationEnd = new TextLocation(warning.Column + 1, warning.Line - 1);

                var context = IdentifierContextForContext(warning.Context);
                if (context != null)
                {
                    locationStart = new TextLocation(context.Start.Column, context.Start.Line - 1);
                    locationEnd = new TextLocation(context.Stop.Column + context.Stop.Text.Length, context.Stop.Line - 1);
                }

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.WaveLine, color)
                {
                    ToolTip = message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }

            // Add markers around code errors
            foreach (var codeError in _messageContainer.CodeErrors)
            {
                var locationStart = new TextLocation(codeError.Column, codeError.Line - 1);
                var locationEnd = new TextLocation(codeError.Column + 1, codeError.Line - 1);

                var context = IdentifierContextForContext(codeError.Context);
                if (context != null)
                {
                    locationStart = new TextLocation(context.Start.Column, context.Start.Line - 1);
                    locationEnd = new TextLocation(context.Stop.Column + context.Stop.Text.Length, context.Stop.Line - 1);
                }

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.WaveLine, Color.Blue)
                {
                    ToolTip = codeError.Message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }

            // Add markers around syntax errors
            foreach (var syntaxError in _messageContainer.SyntaxErrors)
            {
                var locationStart = new TextLocation(syntaxError.Column, syntaxError.Line - 1);
                var locationEnd = new TextLocation(syntaxError.Column + 1, syntaxError.Line - 1);

                if (syntaxError.Token != null)
                {
                    locationStart = new TextLocation(syntaxError.Token.Column, syntaxError.Token.Line - 1);
                    locationEnd = new TextLocation(syntaxError.Token.Column + syntaxError.Token.Text.Length, syntaxError.Token.Line - 1);
                }

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.WaveLine)
                {
                    ToolTip = syntaxError.Message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }
        }

        /// <summary>
        /// Returns a parse rule context that is the identifier of a given parse rule context object.
        /// If no valid identifier parse rule contexts are found, the given context is returned instead.
        /// Identifier contexts are found when the context is either a class, sequence, function, method, type field or type alias context
        /// </summary>
        /// <param name="context">The context to get the identifier out of</param>
        /// <returns>The identifier context for the context; or itself, if none could be found</returns>
        static ParserRuleContext IdentifierContextForContext(ParserRuleContext context)
        {
            if (context == null)
                return null;

            // Function
            var funcContext = context as ZScriptParser.FunctionDefinitionContext;
            if (funcContext != null)
            {
                return funcContext.functionName();
            }

            // Class
            var classContext = context as ZScriptParser.ClassDefinitionContext;
            if (classContext != null)
            {
                return classContext.className();
            }

            // Sequence
            var sequenceContext = context as ZScriptParser.SequenceBlockContext;
            if (sequenceContext != null)
            {
                return sequenceContext.sequenceName();
            }

            // Method
            var methodContext = context as ZScriptParser.ClassMethodContext;
            if (methodContext != null)
            {
                return methodContext.functionDefinition().functionName();
            }

            // Type alias
            var typeAlias = context as ZScriptParser.TypeAliasContext;
            if (typeAlias != null)
            {
                return typeAlias.typeAliasName();
            }

            // Type field
            var typeField = context as ZScriptParser.TypeAliasVariableContext;
            if (typeField != null)
            {
                return typeField.valueDeclareStatement().valueHolderDecl().valueHolderDefine().valueHolderName();
            }

            return context;
        }

        /// <summary>
        /// Adds markers on the text base on a given code scope's contents
        /// </summary>
        private void AddMemberMarkers(CodeScope scope)
        {
            // Add the definition markers
            var definitions = scope.GetAllDefinitionsRecursive();
            
            foreach (var definition in definitions.OfType<ValueHolderDefinition>().Where(d => d.ValueDefineContext != null && d.Context != null))
            {
                string message = definition.Type.ToString();
                var context = definition.ValueDefineContext;
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
                var locationEnd = new TextLocation(context.Stop.Column + context.Stop.Text.Length, context.Start.Line - 1);

                var offsetStart = Document.PositionToOffset(locationStart);
                var offsetEnd = Document.PositionToOffset(locationEnd);

                var marker = new TextMarker(offsetStart, offsetEnd - offsetStart, TextMarkerType.Invisible)
                {
                    ToolTip = message
                };

                Document.MarkerStrategy.AddMarker(marker);
            }
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
            public override Size Size => new Size(20, -1);

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

                foreach (var message in MessageContainer.AllMessages.Reverse())
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
            // AbstractMargin.HandleMouseDown override
            // 
            public override void HandleMouseDown(Point mousepos, MouseButtons mouseButtons)
            {
                // Check if the mouse is over any error/message
                if (MessageContainer == null)
                    return;

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

                        var context = IdentifierContextForContext(message.Context);

                        if (target.Contains(mousepos))
                        {
                            if (context != null)
                            {
                                var locationStart = new TextLocation(context.Start.Column, context.Start.Line - 1);
                                var locationEnd = new TextLocation(context.Stop.Column + context.GetText().Length, context.Stop.Line - 1);

                                var startOffset = TextArea.Document.PositionToOffset(locationStart);
                                var endOffset = TextArea.Document.PositionToOffset(locationEnd);

                                TextArea.MotherTextEditorControl.Select(startOffset, endOffset - startOffset);
                            }
                            else if (message.Token != null)
                            {
                                var locationStart = new TextLocation(message.Token.Column, message.Token.Line - 1);
                                var locationEnd = new TextLocation(message.Token.Column + message.Token.Text.Length, message.Token.Line - 1);

                                var startOffset = TextArea.Document.PositionToOffset(locationStart);
                                var endOffset = TextArea.Document.PositionToOffset(locationEnd);

                                TextArea.MotherTextEditorControl.Select(startOffset, endOffset - startOffset);
                            }
                            else
                            {
                                var locationStart = new TextLocation(message.Column, message.Line - 1);

                                var startOffset = TextArea.Document.PositionToOffset(locationStart);

                                TextArea.MotherTextEditorControl.Select(startOffset, 0);
                            }

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
            public ICollection<SyntaxMode> SyntaxModes => _syntaxModes;

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

    /// <summary>
    /// Event arguments for a ScriptParsed event
    /// </summary>
    public class ScriptParsedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value specifying whether the script parsing was successfull
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Gets the code scope that was parsed.
        /// When the script parse is successful, this value describes the collected base scope for the script.
        /// This value is null, if the parsing was unsuccessful
        /// </summary>
        public CodeScope BaseScope { get; private set; }

        /// <summary>
        /// Gets the runtime generator that was used to parse the code scope
        /// </summary>
        public ZRuntimeGenerator RuntimeGenerator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ScriptParsedEventArgs class
        /// </summary>
        /// <param name="succeeded">Whether the script parsing was successfull</param>
        /// <param name="baseScope">The base scope for the script parsed</param>
        /// <param name="runtimeGenerator">The runtime generator that was usd to parse the code scope</param>
        public ScriptParsedEventArgs(bool succeeded, CodeScope baseScope, ZRuntimeGenerator runtimeGenerator)
        {
            Succeeded = succeeded;
            BaseScope = baseScope;
            RuntimeGenerator = runtimeGenerator;
        }
    }
}