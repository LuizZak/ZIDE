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

using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZIDE.Controllers;
using ZIDE.Views.Controls;

namespace ZIDE.Views.ModelViews
{
    /// <summary>
    /// A dialog form for managing creation of projects
    /// </summary>
    public partial class NewProjectForm : Form
    {
        /// <summary>
        /// Gets the project creation settings for this form
        /// </summary>
        public ProjectCreationSettings Settings => SettingsFromForm();

        private readonly MainController _controller;

        /// <summary>
        /// Initializes a new instance of the NewProjectForm class
        /// </summary>
        /// <param name="controller">A valid controller for intermediating the validation of the settings of this form</param>
        public NewProjectForm(MainController controller)
        {
            _controller = controller;

            InitializeComponent();

            ValidateForm();
        }
        
        bool ValidateForm()
        {
            wp_validation.Visible = false;
            
            var result = _controller.ProjectsController.ValidateProjectCreationSettings(Settings);

            var hasWarnings = result.Count(w => !_controller.ProjectsController.IsProjectValidationWarningFatal(w)) > 0;
            var hasErrors = result.Count(_controller.ProjectsController.IsProjectValidationWarningFatal) > 0;

            // Update warnings panel
            wp_validation.Visible = hasWarnings || hasErrors;
            wp_validation.Icon = hasErrors ? WarningPanelIcon.Error : WarningPanelIcon.Warning;

            // Update interface state
            tb_projectName.BackColor = Color.White;
            tb_projectPath.BackColor = Color.White;

            foreach (var error in result)
            {
                switch (error)
                {
                    case ProjectCreationValidationWarning.InvalidProjectName:
                        tb_projectName.BackColor = Color.LightPink;
                        wp_validation.Text = @"Invalid characters in project name";
                        break;

                    case ProjectCreationValidationWarning.InvalidProjectPath:
                        tb_projectPath.BackColor = Color.LightPink;
                        wp_validation.Text = @"Invalid characters in project path";
                        break;

                    case ProjectCreationValidationWarning.ProjectPathIsFileName:
                        tb_projectPath.BackColor = Color.LightPink;
                        wp_validation.Text = @"Project path points to an existing file - please point to an existing directory or a directory to be created";
                        break;
                }
            }

            // Count fatal errors
            return !hasErrors;
        }

        ProjectCreationSettings SettingsFromForm()
        {
            return new ProjectCreationSettings(tb_projectName.Text, tb_projectPath.Text, cb_importFromPath.Checked);
        }

        #region Event Handlers

        private void btn_ok_Click(object sender, System.EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btn_cancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tb_projectName_TextChanged(object sender, System.EventArgs e)
        {
            ValidateForm();
        }

        private void tb_projectPath_TextChanged(object sender, System.EventArgs e)
        {
            ValidateForm();
        }

        private void cb_importFromPath_CheckedChanged(object sender, System.EventArgs e)
        {
            ValidateForm();
        }

        #endregion
    }
}
