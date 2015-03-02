using ICSharpCode.TextEditor;

namespace ZIDE.Views.Controls
{
    /// <summary>
    /// Interface to be implemented by form document classes that contains a script text editor
    /// </summary>
    public interface IScriptForm
    {
        /// <summary>
        /// Gets the text editor control for this script document form
        /// </summary>
        TextEditorControl TextEditorControl { get; }
    }
}