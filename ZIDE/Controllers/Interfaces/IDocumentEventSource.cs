namespace ZIDE.Controllers.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes that dispatch events related to document management
    /// </summary>
    public interface IDocumentEventSource
    {
        /// <summary>
        /// Event fired whenever a document has been opened by this IDocumentEventSource
        /// </summary>
        event ScriptsController.DocumentEventHandler DocumentOpened;

        /// <summary>
        /// Event fired whenever a document has been closed by this IDocumentEventSource
        /// </summary>
        event ScriptsController.DocumentEventHandler DocumentClosed;
    }
}