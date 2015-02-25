namespace ZIDE.Models
{
    /// <summary>
    /// Represents a ZScript document
    /// </summary>
    public class ZScriptDocument
    {
        /// <summary>
        /// Gets or sets the display name for this ZScript document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the contents of this ZScript document
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// Creates a new instance of the ZScriptDocument class, with empty contents and an associated name
        /// </summary>
        /// <param name="documentName">The name for the document</param>
        public ZScriptDocument(string documentName)
        {
            DocumentName = documentName;
        }
    }
}