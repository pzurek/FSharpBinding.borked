// 
// FSharpCompilerParameters.cs
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

using MonoDevelop.Core.Gui.Components;
using MonoDevelop.Projects;
using MonoDevelop.Core.Serialization;

namespace FSharpBinding
{
	public class FSharpCompilerParameters : ConfigurationParameters
	{

//- OUTPUT FILES -
//--out:<file>                  
//                               Name of the output file (Short form: -o)
//--target:exe                   Build a console executable
//--target:winexe                Build a Windows executable
//--target:library               Build a library (Short form: -a)
//--target:module                Build a module that can be added to another
//                               assembly
//--delaysign[+|-]               Delay-sign the assembly using only the public
//                               portion of the strong name key
//--doc:<file>                   Write the xmldoc of the assembly to the given
//                               file
//--keyfile:<file>               Specify a strong name key file
//--keycontainer:<string>        Specify a strong name key container
//--platform:<string>            Limit which platforms this code can run on: x86,
//                               Itanium, x64 or anycpu. The default is anycpu
//--nooptimizationdata           Only include optimization information essential
//                               for implementing inlined constructs. Inhibits
//                               cross-module inlining but improves binary
//                               compatibility
//--nointerfacedata              Don't add a resource to the generated assembly
//                               containing F#-specific metadata
//--sig:<file>                   Print the inferred interface of the assembly to
//                               a file
//
//
//- INPUT FILES -
//--reference:<file>             Reference an assembly (Short form: -r)
//
//
//- RESOURCES -
//--win32res:<file>              Specify a Win32 resource file (.res)
//--win32manifest:<file>         Specify a Win32 manifest file
//--nowin32manifest              Do not include the default Win32 manifest
//--resource:<resinfo>           Embed the specified managed resource
//--linkresource:<resinfo>       Link the specified resource to this assembly
//                               where the resinfo format is     <file>[,<string
//                               name>[,public|private]]
//
//
//- CODE GENERATION -
//--debug[+|-]                   Emit debug information (Short form: -g)
//--debug:{full|pdbonly}         Specify debugging type: full, pdbonly. ('full'
//                               is the default and enables attaching a debugger
//                               to a running program)
//--optimize[+|-]                Enable optimizations (Short form: -O)
//--tailcalls[+|-]               Enable or disable tailcalls
//--crossoptimize[+|-]           Enable or disable cross-module optimizations
//
//
//- ERRORS AND WARNINGS -
//--warnaserror[+|-]             Report all warnings as errors
//--warnaserror[+|-]:<warn;...>  Report specific warnings as errors
//--warn:<n>                     Set a warning level (0-4)
//--nowarn:<warn;...>            Disable specific warning messages
//
//
//- LANGUAGE -
//--checked[+|-]                 Generate overflow checks
//--define:<string>              Define conditional compilation symbols (Short
//                               form: -d)
//--mlcompatibility              Ignore OCaml-compatibility warnings.
//
//
//- HTML -
//--generatehtml                 Generate HTML documentation
//--htmloutputdir:<file>         Output directory for HTML documentation
//--htmlcss:<string>             Set the name of the Cascading Style Sheet
//--htmlnamespacefile:<string>   Set the name of the master namespaces.html file
//                               assumed to be in the output directory
//--htmlnamespacefileappend      Append to the master namespace file when
//                               generating HTML documentation
//
//
//- MISCELLANEOUS -
//--nologo                       Suppress compiler copyright message
//--help                         Display this usage message (Short form: -?)
//
//
//- ADVANCED -
//--codepage:<n>                 Specify the codepage used to read source files
//--utf8output                   Output messages in UTF-8 encoding
//--fullpaths                    Output messages with fully qualified paths
//--lib:<dir;...>                Specify a directory for the include path which
//                               is used to resolve source files and assemblies
//                               (Short form: -I)
//--baseaddress:<address>        Base address for the library to be built
//--noframework                  Do not reference the .NET Framework assemblies
//                               by default
//--standalone                   Statically link the F# library and all
//                               referenced DLLs that depend on it into the
//                               assembly being generated.
//--staticlink:<file>            Statically link the given assembly and all
//                               referenced DLLs that depend on this assembly.
//                               Use an assembly name e.g. mylib, not a DLL name
//--pdb:<string>                 Name the output debug file
		
		[ItemProperty ("DefineSymbols", DefaultValue = "")]
		string defineSymbols = String.Empty;
		
		public string DefineSymbols {
			get { return defineSymbols; }
			set { defineSymbols = value ?? string.Empty; }
		}
		
		[ItemProperty ("NoWarnings", DefaultValue = "")]
		string noWarnings = String.Empty;

		public string NoWarnings {
			get { return noWarnings; }
			set { noWarnings = value; }
		}
		
		[ItemProperty ("WarningLevel")]
		int  warningLevel = 4;
		
		public int WarningLevel{
			get { return warningLevel; }
			set { warningLevel = value; }
		}
	}
}
