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