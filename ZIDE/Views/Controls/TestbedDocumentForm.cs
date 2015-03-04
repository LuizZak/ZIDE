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
using System.Linq;
using System.Windows.Forms;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using ZIDE.Models;
using ZIDE.Services.Scripting;
using ZIDE.Utils;

using ZScript.CodeGeneration;
using ZScript.CodeGeneration.Analysis;
using ZScript.CodeGeneration.Definitions;
using ZScript.Elements;
using ZScript.Runtime;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// Represents a form which enables the user to write and execute scripts
    /// </summary>
    public partial class TestbedDocumentForm : ScriptDocumentForm
    {
        /// <summary>
        /// The currently selected function definition
        /// </summary>
        private FunctionDefinition _selectedFunction;

        /// <summary>
        /// The current runtime generator instance
        /// </summary>
        private ZRuntimeGenerator _runtimeGenerator;

        /// <summary>
        /// The current scope collected with the runtime generator
        /// </summary>
        private CodeScope _codeScope;

        /// <summary>
        /// Whether the current parse is the first parse of the form - used to automatically select the 'main' function, once parsing is over
        /// </summary>
        private bool _firstParse = true;

        /// <summary>
        /// Gets or sets the currently selected function definition
        /// </summary>
        public FunctionDefinition SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                _selectedFunction = value;
                UpdateInterface();
            }
        }

        /// <summary>
        /// Initializes a new instance of the TestbedDocumentForm class
        /// </summary>
        /// <param name="document">The document to display the form with</param>
        public TestbedDocumentForm(ZTestbedScriptDocument document)
            : base(document)
        {
            InitializeComponent();

            if (tscb_startingFunction.ComboBox != null)
            {
                tscb_startingFunction.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            // Add the realtime syntax check service
            syntaxService.ScriptParsed += SyntaxService_OnScriptParsed;
        }

        /// <summary>
        /// Initializes a new instance of the TestbedDocumentForm class
        /// </summary>
        public TestbedDocumentForm()
            : this(new ZTestbedScriptDocument("Untitled Testbed"))
        {

        }

        // 
        // OnShown override
        // 
        protected override void OnShown(EventArgs e)
        {
            var highlightStrategy = te_output.Document.HighlightingStrategy as DefaultHighlightingStrategy;
            if (highlightStrategy != null)
            {
                highlightStrategy.SetColorFor("Default", new HighlightBackground("WindowText", "Info", false, false));
            }

            // Parse the script
            syntaxService.Parse();

            te_textEditor.Select(te_textEditor.Text.Length - 2, 0);
            te_textEditor.VerticalScroll.Value = 0;
            te_textEditor.ActiveTextAreaControl.VerticalScroll.Value = 0;
            te_textEditor.ActiveTextAreaControl.VScrollBar.Value = 0;
        }

        /// <summary>
        /// Updates the form with the contents of a given code scope
        /// </summary>
        /// <param name="codeScope">The code scope to update the form with. Providing null will clear the contents of the form</param>
        public void UpdateWithScope(CodeScope codeScope)
        {
            _codeScope = codeScope;

            if (codeScope == null)
            {
                DisableFunctionControls();

                UpdateInterface();

                return;
            }

            EnableFunctionControls();

            // Store the name of the current function selected
            var currentFunction = tscb_startingFunction.SelectedItem as FunctionDefinition;

            tscb_startingFunction.Items.Clear();

            // Load the top-functions
            LoadFunctions(codeScope);

            if (currentFunction != null)
            {
                // Try to re-select the function
                var func = codeScope.GetDefinitionByName<FunctionDefinition>(currentFunction.Name);

                tscb_startingFunction.SelectedItem = func;
                SelectFunction(func);
            }
        }

        /// <summary>
        /// Loads the functions from a given code scope into the functioncombo box
        /// </summary>
        /// <param name="codeScope">The code scope containg the functions to run</param>
        private void LoadFunctions(CodeScope codeScope)
        {
            tscb_startingFunction.Items.Clear();

            var functions = codeScope.GetDefinitionsByType<TopLevelFunctionDefinition>();

            foreach (var func in functions.Where(f => f.CallableTypeDef.RequiredArgumentsCount == 0))
            {
                tscb_startingFunction.Items.Add(func);
            }
        }

        /// <summary>
        /// Selects a given function definition on this testbed document form.
        /// If null is provided, the currently selected function definition is unselected
        /// </summary>
        /// <param name="definition">The function definition to select</param>
        private void SelectFunction(FunctionDefinition definition)
        {
            SelectedFunction = definition;
            tscb_startingFunction.SelectedItem = definition;
        }

        /// <summary>
        /// Executes the function selected
        /// </summary>
        private void Execute()
        {
            if (SelectedFunction == null || _runtimeGenerator == null || _runtimeGenerator.MessageContainer.HasErrors)
                return;

            var runner = new FunctionRunner(_runtimeGenerator, SelectedFunction);
            runner.Execute(tstb_arguments.Text);
            var output = "";

            if (!runner.Successful)
            {
                output += "Execution failed!: \r\n";
            }
            else
            {
                output += "Execution succeeded!: \r\n";
            }

            output += runner.Output;

            te_output.Document.TextContent = output;
            te_output.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.WholeTextArea));
        }

        #region Interface-related methods

        /// <summary>
        /// Updates the interface
        /// </summary>
        private void UpdateInterface()
        {
            UpdateArgumentsInterface();

            bool enableExecution = SelectedFunction != null && _codeScope != null && !_runtimeGenerator.MessageContainer.HasErrors;
            tsb_execute.Enabled = enableExecution;

            if (!enableExecution)
            {
                tsb_execute.ToolTipText = tsb_execute.Text = @"There are errors in the script and the script cannot be executed";
            }
            else
            {
                tsb_execute.ToolTipText = tsb_execute.Text = @"Execute (F5)";
            }
        }

        /// <summary>
        /// Updates the portion of the interface related to argument inputting
        /// </summary>
        private void UpdateArgumentsInterface()
        {
            var visible = SelectedFunction != null && SelectedFunction.Parameters.Length > 0;

            tstb_arguments.Visible = visible;
            tsl_arguments.Visible = visible;
            tsl_leftParam.Visible = visible;
            tsl_rightParam.Visible = visible;
        }

        /// <summary>
        /// Disables the function selection/execution controls
        /// </summary>
        private void DisableFunctionControls()
        {
            tscb_startingFunction.Enabled = false;
            tstb_arguments.Enabled = false;
            tsb_execute.Enabled = false;
        }

        /// <summary>
        /// Re-enables the function selection/execution controls
        /// </summary>
        private void EnableFunctionControls()
        {
            tscb_startingFunction.Enabled = true;
            tstb_arguments.Enabled = true;
            tsb_execute.Enabled = true;
        }

        #endregion

        #region Event handlers

        // 
        // OnKeyDown override
        // 
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.F5)
            {
                Execute();
                e.Handled = true;
            }
        }

        // 
        // Script Parsed event handler
        // 
        private void SyntaxService_OnScriptParsed(object sender, ScriptParsedEventArgs eventArgs)
        {
            _runtimeGenerator = eventArgs.RuntimeGenerator;
            UpdateWithScope(eventArgs.BaseScope);

            if (_firstParse && _codeScope != null)
            {
                _firstParse = false;
                // Start with the default 'main' function
                SelectFunction(_codeScope.GetDefinitionByName<FunctionDefinition>("main"));
            }
        }

        // 
        // Starting Function toolstrip combobox selection changed
        // 
        private void tscb_startingFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectFunction(tscb_startingFunction.SelectedItem as FunctionDefinition);
        }

        // 
        // Execute toolstrip button click
        // 
        private void tsb_execute_Click(object sender, EventArgs e)
        {
            Execute();
        }

        #endregion

        /// <summary>
        /// Class capable of running functions with a set of provided function arguments
        /// </summary>
        class FunctionRunner : IRuntimeOwner
        {
            /// <summary>
            /// The runtime generator containing the code scope and function to run
            /// </summary>
            private readonly ZRuntimeGenerator _runtimeGenerator;

            /// <summary>
            /// The function definition to execute
            /// </summary>
            private readonly FunctionDefinition _functionDefinition;

            /// <summary>
            /// Whether the function execution was successful
            /// </summary>
            private bool _successful;

            /// <summary>
            /// Gets a value specifying whether the function execution was successful
            /// </summary>
            public bool Successful
            {
                get { return _successful; }
            }

            /// <summary>
            /// Gets the output of the function execution
            /// </summary>
            public string Output { get; private set; }

            /// <summary>
            /// Initializes a new instance of the FunctionRunner clas
            /// </summary>
            /// <param name="runtimeGenerator">The runtime generator to generate the runtime and execute the function on</param>
            /// <param name="functionDefinition">The function definition to execute</param>
            public FunctionRunner(ZRuntimeGenerator runtimeGenerator, FunctionDefinition functionDefinition)
            {
                _functionDefinition = functionDefinition;
                _runtimeGenerator = runtimeGenerator;
            }

            /// <summary>
            /// Executes the function, using a given string as an argument list
            /// </summary>
            /// <param name="arguments">The arguments to use when executing the function</param>
            public void Execute(string arguments)
            {
                try
                {
                    var runtime = _runtimeGenerator.GenerateRuntime(this);

                    var ret = runtime.CallFunction(_functionDefinition.Name);

                    if(!string.IsNullOrEmpty(Output))
                        Output += "\r\n";

                    Output += ret;

                    _successful = true;
                }
                catch (Exception e)
                {
                    _successful = false;

                    Output = e.ToString();
                }
            }

            // 
            // IRuntimeOwner.CallFunction implementation
            // 
            public object CallFunction(ZExportFunction func, params object[] parameters)
            {
                if (func.Name == "print")
                {
                    foreach (var p in parameters)
                    {
                        Output += p + " ";
                    }

                    Output += "\r\n";

                    return parameters;
                }

                return null;
            }

            // 
            // IRuntimeOwner.RespondsToFunction implementation
            // 
            public bool RespondsToFunction(ZExportFunction func)
            {
                return func.Name == "print";
            }

            // 
            // IRuntimeOwner.CreateType implementation
            // 
            public object CreateType(string typeName, params object[] parameters)
            {
                throw new InvalidOperationException("Cannot create types in Testbed scripts");
            }
        }
    }
}