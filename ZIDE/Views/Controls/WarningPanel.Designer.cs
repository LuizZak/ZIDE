namespace ZIDE.Views.Controls
{
    partial class WarningPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarningPanel));
            this.lbl_alertLabel = new System.Windows.Forms.Label();
            this.pb_error = new System.Windows.Forms.PictureBox();
            this.pb_warning = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_error)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_warning)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_alertLabel
            // 
            this.lbl_alertLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_alertLabel.AutoSize = true;
            this.lbl_alertLabel.Location = new System.Drawing.Point(31, 7);
            this.lbl_alertLabel.Name = "lbl_alertLabel";
            this.lbl_alertLabel.Size = new System.Drawing.Size(0, 13);
            this.lbl_alertLabel.TabIndex = 9;
            // 
            // pb_error
            // 
            this.pb_error.Image = global::ZIDE.Properties.Resources.process_stop;
            this.pb_error.Location = new System.Drawing.Point(3, 3);
            this.pb_error.Name = "pb_error";
            this.pb_error.Size = new System.Drawing.Size(22, 22);
            this.pb_error.TabIndex = 11;
            this.pb_error.TabStop = false;
            // 
            // pb_warning
            // 
            this.pb_warning.Image = ((System.Drawing.Image)(resources.GetObject("pb_warning.Image")));
            this.pb_warning.Location = new System.Drawing.Point(3, 3);
            this.pb_warning.Name = "pb_warning";
            this.pb_warning.Size = new System.Drawing.Size(22, 22);
            this.pb_warning.TabIndex = 10;
            this.pb_warning.TabStop = false;
            // 
            // WarningPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pb_error);
            this.Controls.Add(this.lbl_alertLabel);
            this.Controls.Add(this.pb_warning);
            this.Name = "WarningPanel";
            this.Size = new System.Drawing.Size(304, 28);
            ((System.ComponentModel.ISupportInitialize)(this.pb_error)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_warning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pb_warning;
        private System.Windows.Forms.Label lbl_alertLabel;
        private System.Windows.Forms.PictureBox pb_error;
    }
}
