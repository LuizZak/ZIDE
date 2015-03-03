namespace ZIDE.Models
{
    /// <summary>
    /// Represents a script document for use in testbeds
    /// </summary>
    public class ZTestbedScriptDocument : ZScriptDocument
    {
        /// <summary>
        /// Gets or sets the name of the function to execute when testing the testbed document
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets or sets the arguments for this script document
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the ZTestbedScriptDocument class
        /// </summary>
        /// <param name="documentName">The name of the testbed document</param>
        public ZTestbedScriptDocument(string documentName)
            : base(documentName)
        {

        }
    }
}