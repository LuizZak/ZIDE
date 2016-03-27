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

using ICSharpCode.TextEditor;

namespace ZIDE.Utils
{
    /// <summary>
    /// Provides useful static methods for the TextEditorControl class
    /// </summary>
    public static class TextEditorUtils
    {
        /// <summary>
        /// Selects a part of the text on the currently opened document
        /// </summary>
        /// <param name="control">The target for the method</param>
        /// <param name="start">The start of the selection</param>
        /// <param name="length">The range of characters to select</param>
        public static void Select(this TextEditorControl control, int start, int length)
        {
            TextLocation locStart = control.Document.OffsetToPosition(start);
            TextLocation locEnd = control.Document.OffsetToPosition(start + length);

            control.ActiveTextAreaControl.SelectionManager.SetSelection(locStart, locEnd);
            control.ActiveTextAreaControl.Caret.Position = locEnd;
            control.ActiveTextAreaControl.Caret.DesiredColumn = control.ActiveTextAreaControl.Caret.Column;

            if (length == 0)
            {
                control.ActiveTextAreaControl.SelectionManager.ClearSelection();
            }
        }

        /// <summary>
        /// Returns the point on screen the given character is located at
        /// </summary>
        /// <param name="control">The target for the method</param>
        /// <param name="index">The index of the character to return the position of</param>
        public static Point GetPositionFromCharIndex(this TextEditorControl control, int index)
        {
            TextLocation locStart = control.Document.OffsetToPosition(index);
            TextLocation oldCaretLocation = control.ActiveTextAreaControl.Caret.Position;
            control.ActiveTextAreaControl.Caret.Position = locStart;
            Point point = control.ActiveTextAreaControl.Caret.ScreenPosition;
            control.ActiveTextAreaControl.Caret.Position = oldCaretLocation;

            return point;
        }

        /// <summary>
        /// Returns the character index at the given position on the control
        /// </summary>
        /// <param name="control">The target for the method</param>
        /// <param name="point">The position to pick the character index from</param>
        /// <returns>An offset, representing the index of the character that was under that point</returns>
        public static int GetCharIndexFromPosition(this TextEditorControl control, Point point)
        {
            Point logicalPoint = point;

            logicalPoint.X -= control.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.X;
            logicalPoint.Y -= control.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.Y;

            TextLocation location = control.ActiveTextAreaControl.TextArea.TextView.GetLogicalPosition(logicalPoint);

            return control.Document.PositionToOffset(location);
        }
    }
}