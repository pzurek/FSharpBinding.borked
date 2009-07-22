// 
// FSharpBindingCompilerManager.cs
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
using System.Text;
using System.Text.RegularExpressions;

using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace FSharpBinding
{
	public class FSharpBindingCompilerManager
	{
		static void AddOption (StringBuilder sb, string option, string val)
		{
			sb.Append ('"');
			sb.Append (option);
			sb.Append (val);
			sb.Append ('"');
			sb.AppendLine ();
		}
		
		public static BuildResult Compile (ProjectItemCollection projectItems,
		                                   DotNetProjectConfiguration configuration,
		                                   IProgressMonitor monitor)
		{
			return new BuildResult();
		}

		static string GetCompilerName() {
			//HACK: for the moment there is only one compiler available and
			//assumption is made that the system is cofigured to recognize fsc
			//as a valid command.
			return "fsc";
		}

		static BuildResult ParseOut (string stdout, string stderr)
		{
			BuildResult result = new BuildResult ();
			
			StringBuilder compilerOutput = new StringBuilder ();
			bool typeLoadException = false;
			foreach (string s in new string[] { stdout, stderr }) {
				StreamReader sr = File.OpenText (s);
				while (true) {
					if (typeLoadException) {
						compilerOutput.Append (sr.ReadToEnd ());
						break;
					}
					string curLine = sr.ReadLine();
					compilerOutput.AppendLine (curLine);
					
					if (curLine == null) 
						break;
					
					curLine = curLine.Trim();
					if (curLine.Length == 0) 
						continue;
					
					if (curLine.StartsWith ("Unhandled Exception: System.TypeLoadException") || 
					    curLine.StartsWith ("Unhandled Exception: System.IO.FileNotFoundException")) {
						//result.ClearErrors (); - something apparently not supported by this version of MD
						typeLoadException = true;
					}
					
					BuildError error = CreateErrorFromString (curLine);
					
					if (error != null)
						result.Append (error);
				}
				sr.Close();
			}
			if (typeLoadException) {
				Regex reg  = new Regex (@".*WARNING.*used in (mscorlib|System),.*", RegexOptions.Multiline);
				if (reg.Match (compilerOutput.ToString ()).Success)
					result.AddError ("", 0, 0, "", "Error: A referenced assembly may be built with an incompatible CLR version. See the compilation output for more details.");
				else
					result.AddError ("", 0, 0, "", "Error: A dependency of a referenced assembly may be missing, or you may be referencing an assembly created with a newer CLR version. See the compilation output for more details.");
			}
			result.CompilerOutput = compilerOutput.ToString ();
			return result;
		}

		static Regex regexError = new Regex (@"^(\s*(?<file>.*)\((?<line>\d*)(,(?<column>\d*[\+]*))?\)(:|)\s+)*(?<level>\w+)\s*(?<number>.*\d):\s*(?<message>.*)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		
		static BuildError CreateErrorFromString (string error_string)
		{
			// When IncludeDebugInformation is true, prevents the debug symbols stats from braeking this.
			if (error_string.StartsWith ("WROTE SYMFILE") ||
			    error_string.StartsWith ("OffsetTable") ||
			    error_string.StartsWith ("Compilation succeeded") ||
			    error_string.StartsWith ("Compilation failed"))
				return null;
			
			Match match = regexError.Match(error_string);
			if (!match.Success) 
				return null;
			
			BuildError error = new BuildError ();
			error.FileName = match.Result ("${file}") ?? "";
			
			string line = match.Result ("${line}");
			error.Line = !string.IsNullOrEmpty (line) ? Int32.Parse (line) : 0;
			
			string col = match.Result ("${column}");
			if (!string.IsNullOrEmpty (col)) 
				error.Column = col == "255+" ? -1 : Int32.Parse (col);
			
			error.IsWarning   = match.Result ("${level}") == "warning";
			error.ErrorNumber = match.Result ("${number}");
			error.ErrorText   = match.Result ("${message}");
			return error;
		}	
	}
}
