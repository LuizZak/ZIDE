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
using System.ComponentModel;
using System.Windows.Forms;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// A panel that displays a warning/error icon
    /// </summary>
    public partial class WarningPanel : UserControl
    {
        private int fixedControlHeight = 28;
        private WarningPanelIcon _icon;

        /// <summary>
        /// Gets or sets the display icon for this warning panel
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public WarningPanelIcon Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                switch (value)
                {
                    case WarningPanelIcon.Error:
                        pb_error.Visible = true;
                        pb_warning.Visible = false;
                        break;
                    case WarningPanelIcon.Warning:
                        pb_error.Visible = false;
                        pb_warning.Visible = true;
                        break;
                }
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                lbl_alertLabel.Text = value;
            }
        }

        public WarningPanel()
        {
            InitializeComponent();
        }

        //
        // SetBoundsCore override used to lock the control's height
        //
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // If no fixed control height is specified, allow free resizing
            if (fixedControlHeight == -1)
            {
                base.SetBoundsCore(x, y, Math.Max(80, width), height, specified);
            }

            // EDIT: ADD AN EXTRA HEIGHT VALIDATION TO AVOID INITIALIZATION PROBLEMS
            // BITWISE 'AND' OPERATION: IF ZERO THEN HEIGHT IS NOT INVOLVED IN THIS OPERATION
            if ((specified & BoundsSpecified.Height) == 0 || (specified & BoundsSpecified.Width) == 0 || height == fixedControlHeight)
            {
                base.SetBoundsCore(x, y, Math.Max(80, width), fixedControlHeight, specified);
            }
        }
    }

    /// <summary>
    /// The icon for a warning panel
    /// </summary>
    public enum WarningPanelIcon
    {
        /// <summary>
        /// A warning icon
        /// </summary>
        Warning,
        /// <summary>
        /// An error icon
        /// </summary>
        Error
    }
}
