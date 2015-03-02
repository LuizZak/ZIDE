namespace ZIDE.Views.Controls
{
    partial class TestbedDocumentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.te_textEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tscb_startingFunction = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsb_execute = new System.Windows.Forms.ToolStripButton();
            this.tstb_arguments = new System.Windows.Forms.ToolStripTextBox();
            this.tsl_arguments = new System.Windows.Forms.ToolStripLabel();
            this.tsl_leftParam = new System.Windows.Forms.ToolStripLabel();
            this.tsl_rightParam = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // te_textEditor
            // 
            this.te_textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.te_textEditor.IsReadOnly = false;
            this.te_textEditor.Location = new System.Drawing.Point(0, 25);
            this.te_textEditor.Name = "te_textEditor";
            this.te_textEditor.Size = new System.Drawing.Size(799, 495);
            this.te_textEditor.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscb_startingFunction,
            this.tsl_arguments,
            this.tsl_leftParam,
            this.tstb_arguments,
            this.tsl_rightParam,
            this.tsb_execute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(799, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tscb_startingFunction
            // 
            this.tscb_startingFunction.Name = "tscb_startingFunction";
            this.tscb_startingFunction.Size = new System.Drawing.Size(121, 25);
            this.tscb_startingFunction.SelectedIndexChanged += new System.EventHandler(this.tscb_startingFunction_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(99, 22);
            this.toolStripLabel1.Text = "Starting function:";
            // 
            // tsb_execute
            // 
            this.tsb_execute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_execute.Image = global::ZIDE.Properties.Resources.go_next;
            this.tsb_execute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_execute.Name = "tsb_execute";
            this.tsb_execute.Size = new System.Drawing.Size(23, 22);
            this.tsb_execute.Text = "Execute";
            this.tsb_execute.ToolTipText = "Execute";
            // 
            // tstb_arguments
            // 
            this.tstb_arguments.Name = "tstb_arguments";
            this.tstb_arguments.Size = new System.Drawing.Size(100, 25);
            // 
            // tsl_arguments
            // 
            this.tsl_arguments.Name = "tsl_arguments";
            this.tsl_arguments.Size = new System.Drawing.Size(69, 22);
            this.tsl_arguments.Text = "Arguments:";
            // 
            // tsl_leftParam
            // 
            this.tsl_leftParam.Name = "tsl_leftParam";
            this.tsl_leftParam.Size = new System.Drawing.Size(11, 22);
            this.tsl_leftParam.Text = "(";
            // 
            // tsl_rightParam
            // 
            this.tsl_rightParam.Name = "tsl_rightParam";
            this.tsl_rightParam.Size = new System.Drawing.Size(11, 22);
            this.tsl_rightParam.Text = ")";
            // 
            // TestbedDocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 520);
            this.Controls.Add(this.te_textEditor);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TestbedDocumentForm";
            this.Text = "Testbed";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl te_textEditor;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscb_startingFunction;
        private System.Windows.Forms.ToolStripButton tsb_execute;
        private System.Windows.Forms.ToolStripLabel tsl_arguments;
        private System.Windows.Forms.ToolStripTextBox tstb_arguments;
        private System.Windows.Forms.ToolStripLabel tsl_leftParam;
        private System.Windows.Forms.ToolStripLabel tsl_rightParam;
    }
}