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

namespace ZIDE.Models
{
    /// <summary>
    /// Represents a ZScript document
    /// </summary>
    public class ZScriptDocument
    {
        /// <summary>
        /// Gets the default extension for a ZScript file
        /// </summary>
        public static string DefaultExtension = "zs";

        /// <summary>
        /// Gets or sets the display name for this ZScript document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the contents of this ZScript document
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// Gets or sets the savepath for this ZScriptDocument file.
        /// May be null, if the file has not been saved to disk yet
        /// </summary>
        public string SavePath { get; set; }

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