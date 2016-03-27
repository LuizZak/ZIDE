namespace ZIDE.Views.ModelViews
{
    partial class NewProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_importFromPath = new System.Windows.Forms.CheckBox();
            this.btn_browse = new System.Windows.Forms.Button();
            this.tb_projectPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_projectName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.wp_validation = new ZIDE.Views.Controls.WarningPanel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cb_importFromPath);
            this.groupBox1.Controls.Add(this.btn_browse);
            this.groupBox1.Controls.Add(this.tb_projectPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_projectName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 101);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // cb_importFromPath
            // 
            this.cb_importFromPath.AutoSize = true;
            this.cb_importFromPath.Location = new System.Drawing.Point(67, 72);
            this.cb_importFromPath.Name = "cb_importFromPath";
            this.cb_importFromPath.Size = new System.Drawing.Size(241, 17);
            this.cb_importFromPath.TabIndex = 5;
            this.cb_importFromPath.Text = "Automatically import existing script files in path";
            this.cb_importFromPath.UseVisualStyleBackColor = true;
            this.cb_importFromPath.CheckedChanged += new System.EventHandler(this.cb_importFromPath_CheckedChanged);
            // 
            // btn_browse
            // 
            this.btn_browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_browse.Image = global::ZIDE.Properties.Resources.folder;
            this.btn_browse.Location = new System.Drawing.Point(458, 44);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(94, 23);
            this.btn_browse.TabIndex = 4;
            this.btn_browse.Text = "Browse...";
            this.btn_browse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_browse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_browse.UseVisualStyleBackColor = true;
            // 
            // tb_projectPath
            // 
            this.tb_projectPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_projectPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tb_projectPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.tb_projectPath.Location = new System.Drawing.Point(67, 46);
            this.tb_projectPath.Name = "tb_projectPath";
            this.tb_projectPath.Size = new System.Drawing.Size(385, 20);
            this.tb_projectPath.TabIndex = 3;
            this.tb_projectPath.TextChanged += new System.EventHandler(this.tb_projectPath_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Path:";
            // 
            // tb_projectName
            // 
            this.tb_projectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_projectName.Location = new System.Drawing.Point(67, 19);
            this.tb_projectName.Name = "tb_projectName";
            this.tb_projectName.Size = new System.Drawing.Size(485, 20);
            this.tb_projectName.TabIndex = 1;
            this.tb_projectName.TextChanged += new System.EventHandler(this.tb_projectName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ok.Image = global::ZIDE.Properties.Resources.action_check;
            this.btn_ok.Location = new System.Drawing.Point(414, 119);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 29);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "Ok";
            this.btn_ok.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_ok.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Image = global::ZIDE.Properties.Resources.action_delete;
            this.btn_cancel.Location = new System.Drawing.Point(495, 119);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 29);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // wp_validation
            // 
            this.wp_validation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wp_validation.Icon = ZIDE.Views.Controls.WarningPanelIcon.Error;
            this.wp_validation.Location = new System.Drawing.Point(12, 120);
            this.wp_validation.Name = "wp_validation";
            this.wp_validation.Size = new System.Drawing.Size(396, 28);
            this.wp_validation.TabIndex = 4;
            // 
            // NewProjectForm
            // 
            this.AcceptButton = this.btn_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(582, 160);
            this.Controls.Add(this.wp_validation);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(5000, 198);
            this.MinimumSize = new System.Drawing.Size(598, 198);
            this.Name = "NewProjectForm";
            this.Text = "New Project";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.TextBox tb_projectPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_projectName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.CheckBox cb_importFromPath;
        private Controls.WarningPanel wp_validation;
    }
}