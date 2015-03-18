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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscb_startingFunction = new System.Windows.Forms.ToolStripComboBox();
            this.tsl_arguments = new System.Windows.Forms.ToolStripLabel();
            this.tsl_leftParam = new System.Windows.Forms.ToolStripLabel();
            this.tstb_arguments = new System.Windows.Forms.ToolStripTextBox();
            this.tsl_rightParam = new System.Windows.Forms.ToolStripLabel();
            this.tsb_execute = new System.Windows.Forms.ToolStripButton();
            this.tsb_cancel = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cb_debugTokens = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.te_output = new ICSharpCode.TextEditor.TextEditorControl();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // te_textEditor
            // 
            this.te_textEditor.Size = new System.Drawing.Size(799, 273);
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
            this.tsb_execute,
            this.tsb_cancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(799, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(99, 22);
            this.toolStripLabel1.Text = "Starting function:";
            // 
            // tscb_startingFunction
            // 
            this.tscb_startingFunction.Name = "tscb_startingFunction";
            this.tscb_startingFunction.Size = new System.Drawing.Size(121, 25);
            this.tscb_startingFunction.SelectedIndexChanged += new System.EventHandler(this.tscb_startingFunction_SelectedIndexChanged);
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
            // tstb_arguments
            // 
            this.tstb_arguments.Name = "tstb_arguments";
            this.tstb_arguments.Size = new System.Drawing.Size(100, 25);
            // 
            // tsl_rightParam
            // 
            this.tsl_rightParam.Name = "tsl_rightParam";
            this.tsl_rightParam.Size = new System.Drawing.Size(11, 22);
            this.tsl_rightParam.Text = ")";
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
            this.tsb_execute.Click += new System.EventHandler(this.tsb_execute_Click);
            // 
            // tsb_cancel
            // 
            this.tsb_cancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_cancel.Image = global::ZIDE.Properties.Resources.emblem_unreadable;
            this.tsb_cancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_cancel.Name = "tsb_cancel";
            this.tsb_cancel.Size = new System.Drawing.Size(23, 22);
            this.tsb_cancel.Text = "Cancel";
            this.tsb_cancel.Click += new System.EventHandler(this.tsb_cancel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.te_textEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cb_debugTokens);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.te_output);
            this.splitContainer1.Size = new System.Drawing.Size(799, 495);
            this.splitContainer1.SplitterDistance = 273;
            this.splitContainer1.TabIndex = 3;
            // 
            // cb_debugTokens
            // 
            this.cb_debugTokens.AutoSize = true;
            this.cb_debugTokens.Location = new System.Drawing.Point(51, 2);
            this.cb_debugTokens.Name = "cb_debugTokens";
            this.cb_debugTokens.Size = new System.Drawing.Size(87, 17);
            this.cb_debugTokens.TabIndex = 4;
            this.cb_debugTokens.Text = "Debug mode";
            this.cb_debugTokens.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Output:";
            // 
            // te_output
            // 
            this.te_output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.te_output.BackColor = System.Drawing.SystemColors.Info;
            this.te_output.IsReadOnly = false;
            this.te_output.Location = new System.Drawing.Point(0, 19);
            this.te_output.Name = "te_output";
            this.te_output.ShowLineNumbers = false;
            this.te_output.ShowMatchingBracket = false;
            this.te_output.ShowVRuler = false;
            this.te_output.Size = new System.Drawing.Size(799, 199);
            this.te_output.TabIndex = 2;
            // 
            // TestbedDocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 520);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.Name = "TestbedDocumentForm";
            this.Text = "Untitled document";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscb_startingFunction;
        private System.Windows.Forms.ToolStripButton tsb_execute;
        private System.Windows.Forms.ToolStripLabel tsl_arguments;
        private System.Windows.Forms.ToolStripTextBox tstb_arguments;
        private System.Windows.Forms.ToolStripLabel tsl_leftParam;
        private System.Windows.Forms.ToolStripLabel tsl_rightParam;
        private ICSharpCode.TextEditor.TextEditorControl te_output;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_debugTokens;
        private System.Windows.Forms.ToolStripButton tsb_cancel;
    }
}