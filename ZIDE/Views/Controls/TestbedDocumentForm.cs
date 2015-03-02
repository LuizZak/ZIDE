using System;
using System.Windows.Forms;

using ICSharpCode.TextEditor;

using WeifenLuo.WinFormsUI.Docking;

using ZIDE.Services.Scripting;

using ZScript.CodeGeneration.Analysis;
using ZScript.CodeGeneration.Definitions;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// Represents a form which enables the user to write and execute scripts
    /// </summary>
    public partial class TestbedDocumentForm : DockContent, IScriptForm
    {
        /// <summary>
        /// The realtime syntax check service used to parse the code typed into this form
        /// </summary>
        private readonly RealtimeSyntaxCheckService _syntaxService;

        /// <summary>
        /// The currently selected function definition
        /// </summary>
        private FunctionDefinition _selectedFunction;

        /// <summary>
        /// Gets the text editor control for this form
        /// </summary>
        public TextEditorControl TextEditorControl
        {
            get { return te_textEditor; }
        }

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
        public TestbedDocumentForm()
        {
            InitializeComponent();

            if (tscb_startingFunction.ComboBox != null)
            {
                tscb_startingFunction.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            // Add the realtime syntax check service
            _syntaxService = new RealtimeSyntaxCheckService(this);
            _syntaxService.ScriptParsed += SyntaxService_OnScriptParsed;

            // Start with an unselected function
            SelectFunction(null);
        }

        /// <summary>
        /// Updates the form with the contents of a given code scope
        /// </summary>
        /// <param name="codeScope">The code scope to update the form with. Providing null will clear the contents of the form</param>
        public void UpdateWithScope(CodeScope codeScope)
        {
            if (codeScope == null)
            {
                DisableFunctionControls();

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

            var functions = codeScope.GetDefinitionsByType<FunctionDefinition>();

            foreach (var func in functions)
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
        }

        #region Interface-related methods

        /// <summary>
        /// Updates the interface
        /// </summary>
        private void UpdateInterface()
        {
            UpdateArgumentsInterface();
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
        // Script Parsed event handler
        // 
        private void SyntaxService_OnScriptParsed(object sender, ScriptParsedEventArgs eventArgs)
        {
            UpdateWithScope(eventArgs.BaseScope);
        }

        // 
        // Starting Function toolstrip combobox selection changed
        // 
        private void tscb_startingFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectFunction(tscb_startingFunction.SelectedItem as FunctionDefinition);
        }

        #endregion
    }
}