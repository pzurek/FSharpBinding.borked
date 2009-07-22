// 
// FSharpLanguageBinding.cs
//  
// Author:
//       Piotr Zurek <p.zurek@gmail.com>
// 
// Copyright (c) 2009 Piotr Zurek
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Xml;
using System.CodeDom.Compiler;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Dom;
using MonoDevelop.Projects.Dom.Parser;
using MonoDevelop.Projects.CodeGeneration;

using FSharpBinding;

namespace FSharpBinding
{	
	public class FSharpLanguageBinding : IDotNetLanguageBinding
	{
		public string Language {
			get { return "F#"; }
		}
		
		public string ProjectStockIcon {
			get { return "md-fsharp-project"; }
		}
		
		//HACK: this is just for the moment to get something to work.
		//F# source code files can have other extensions - fs, fsi, fsx
		public bool IsSourceCodeFile (string fileName) {
			return string.Compare (Path.GetExtension (fileName), ".fs", true) == 0;
		}
		
		public string GetFileName (string baseName)	{
			return baseName + ".fs";
		}
		
		public ConfigurationParameters CreateCompilationParameters (XmlElement projectOptions)
		{
			FSharpCompilerParameters pars = new FSharpCompilerParameters ();
			if (projectOptions != null) {
				string debugAtt = projectOptions.GetAttribute ("DefineDebug");
				if (string.Compare ("True", debugAtt, true) == 0)
					pars.DefineSymbols = "DEBUG";
			}
			return pars;
		}
		
		public BuildResult Compile (ProjectItemCollection projectItems,
		                            DotNetProjectConfiguration configuration,
		                            IProgressMonitor monitor)
		{
			return FSharpBindingCompilerManager.Compile (projectItems, configuration, monitor);
		}
	
		public ProjectParameters CreateProjectParameters (XmlElement projectOptions)
		{
			return new FSharpProjectParameters ();
		}
		
		public string SingleLineCommentTag { get { return "//"; } }
		public string BlockCommentStartTag { get { return "(*"; } }
		public string BlockCommentEndTag   { get { return "*)"; } }

		public IParser Parser {
			get { return null; }
		}
		
		public IRefactorer Refactorer {
			get { return null; }
		}
		
		public string CommentTag {
			get { return "//"; }
		}
		
		public CodeDomProvider GetCodeDomProvider ()
		{
            return null;
		}
		
		public ClrVersion[] GetSupportedClrVersions ()
		{
			return new ClrVersion[] { 
				ClrVersion.Net_1_1, 
				ClrVersion.Net_2_0, 
				ClrVersion.Clr_2_1/*,
				ClrVersion.Net_4_0*/  //HACK: My current version of MD doesn't support that
			};
		}
	}
}

