﻿#region License information
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

using System.Collections.Generic;

namespace ZIDE.Models
{
    /// <summary>
    /// Repreents a ZScript module project
    /// </summary>
    public class ZScriptProject
    {
        /// <summary>
        /// Gets the default extension for a ZScriptProject file
        /// </summary>
        public static string DefaultExtension = "zsp";

        /// <summary>
        /// Gets the display name of this project
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the path for the project settings file for this project
        /// </summary>
        public string ProjectFilePath { get; private set; }

        /// <summary>
        /// Gets the project path for this ZScriptProject
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// Gets the list of documents for this project
        /// </summary>
        public IList<ZScriptDocument> DocumentList { get; }

        /// <summary>
        /// Initializes a new instance of the ZScriptProject class
        /// </summary>
        /// <param name="basePath">The base path for the project files</param>
        /// <param name="name">The name of this project</param>
        /// <param name="projectFilePath">The project file path for this project</param>
        public ZScriptProject(string basePath, string name, string projectFilePath)
        {
            BasePath = basePath;
            Name = name;
            ProjectFilePath = projectFilePath;
            DocumentList = new List<ZScriptDocument>();
        }
    }
}