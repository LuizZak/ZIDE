using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZIDE.Views.Controls
{
    public partial class ScriptDocumentForm
    {

        private void InitializeComponent()
        {
            this.te_textEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // te_textEditor
            // 
            this.te_textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.te_textEditor.IsReadOnly = false;
            this.te_textEditor.Location = new System.Drawing.Point(0, 0);
            this.te_textEditor.Name = "te_textEditor";
            this.te_textEditor.Size = new System.Drawing.Size(927, 682);
            this.te_textEditor.TabIndex = 0;
            // 
            // ScriptDocumentForm
            // 
            this.ClientSize = new System.Drawing.Size(927, 682);
            this.Controls.Add(this.te_textEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScriptDocumentForm";
            this.ResumeLayout(false);

        }

        protected ICSharpCode.TextEditor.TextEditorControl te_textEditor;
    }
}