/*
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
==================================================================================================================================
VirindiTank is a third party add-on to the game Asheron's Call; it has a "ClassicLooter" GUI component that works with data
files stored in a custom text-based format. Myutilootor is a C# Console Application developed to translate from/to that format
and a much more human-friendly text-based format that can be directly edited more easily in text editors.
Original creation by J. Edwards in 2021.
*/

//#define _DBG_

using System.Text.RegularExpressions;
using System.Globalization; // Needed to force .NET to behave properly in other countries, with decimal numbers.

using myutilootor.src;

namespace Myutilootor
{
#if (_DBG_)
	class myDebug {
		internal static string[] args = { "zLootSnobV4~0.mut", "-keep-inactive" }; //{ "utl_mut/zLootSnobV4.utl" };
	}
#endif
    internal static class CmdLnParms {
		internal const string     version = "myutilootor, v.0.0.0.4     GPLv3 Copyright (C) 2021     Created by J. Edwards";
		internal const string newFileName = "__NEW__.mut";
		internal const string refFileName = "myutilootorReference.mut";
	}

	class Myutilootor {
		private static string GetOutputFileName(string inFileName, string outExt) {
            FileInfo fInfo = new(inFileName); // System.IO.FileInfo
            string baseFileName = fInfo.Name[..^fInfo.Extension.Length]; // Name.Substring(0, fInfo.Name.Length - fInfo.Extension.Length);
			string cutName = new Regex(@"~[0-9]*$").Replace(baseFileName, "");
			int i = 0;
			while (File.Exists(cutName + "~" + i.ToString() + outExt)) // System.IO.File.Exists
                i++;
			return cutName + "~" + i.ToString() + outExt;
		}

		static void Main(string[] args) {
#if (_DBG_)
			args = myDebug.args;
#endif
			if (args.Length > 0) {
                bool doSmartOmit = true;
                if (args.Length>1 && args[^1].CompareTo("-keep-inactive") == 0)
                {
                    doSmartOmit = false;
					args = args.SkipLast(1).ToArray();
                }
                if (args[0].CompareTo("-version") == 0) {
					Console.WriteLine($"\n{CmdLnParms.version}");
					Environment.Exit(0);
				}
				if (args[0].CompareTo("-new") == 0) {
                    StreamWriter fOut = new(CmdLnParms.newFileName); // System.IO.StreamWriter
                    fOut.WriteLine(TextForUsers.header);
					fOut.Close();
					Console.Write($"\n\tOutput file: {CmdLnParms.newFileName}\n");
					Environment.Exit(0);
				}
				if (args[0].CompareTo("-help") == 0) {
                    StreamWriter fOut = new(CmdLnParms.refFileName); // System.IO.StreamWriter
					fOut.WriteLine(TextForUsers.reference);
					fOut.Close();
					Console.Write($"\n\tOutput file: {CmdLnParms.refFileName}\n");

					Environment.Exit(0);
				}

				string inFileName = args[0];

				// Check if input file exists; if not, exit immediately ... can't continue
				if (!System.IO.File.Exists(inFileName)) {
					Console.WriteLine($"{inFileName} does not exist.");
					Environment.Exit(0);
				}

				// Kinda kludgey, but needed for some other countries' handling of doubles (periods vs commas), and easier than editing every
				// to/from string of a double, throughout the code (Parse and Format and CultureInfo.InvariantCulture...)
				// For what was happening, exactly: https://ayulin.net/blog/2015/the-invariant-culture/
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // CultureInfo.CreateSpecificCulture("en-US");


				bool isUtl = true;

				// Read in the file
				string[] utlIntro = { "UTL", "1" };
                StreamReader fileIn = new(inFileName); // System.IO.StreamReader
                foreach (string s in utlIntro) {
					string tmp = fileIn.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    if ( fileIn.EndOfStream )
						throw new MyException("Data-deficient file!");
					if (s != tmp) {
						isUtl = false;
						break;
                    }
                }
				fileIn.Close();

				string outFileName;
				UTL u;
				MUT m;

				// Set the output file name
				if (args.Length > 1)
					outFileName = args[1];
				else
					outFileName = GetOutputFileName(inFileName, isUtl ? ".mut" : ".utl");

				// Read, translate, output
				fileIn = new(inFileName); // System.IO.StreamReader
				StreamWriter fileOut = new(outFileName); // System.IO.StreamWriter
                if ( isUtl ) {
#if (!_DBG_)
					try {
#endif
						u = new UTL();
						u.Read(fileIn);
						m = new MUT(u);
						m.Write(fileOut);
#if (!_DBG_)
					} catch (MyException e) {
						Console.WriteLine($"[LINE {e.line}]: {e.Message}\nPress ENTER.");
						System.Console.ReadLine();
					} catch (Exception e) {
						Console.WriteLine($"{e.Message}\nPress ENTER.");
						System.Console.ReadLine();
					}
#endif
				} else { // assume is .mut
#if (!_DBG_)
					try {
#endif
						m = new MUT();
						m.Read(fileIn);
						u = new UTL(m);
						u.Write(fileOut, doSmartOmit);
#if (!_DBG_)
					} catch (Exception e) {
						Console.WriteLine($"{e.Message}\nPress ENTER.");
						System.Console.ReadLine();
					}
#endif
				}
				fileIn.Close();
				fileOut.Close();
				Console.Write($"\n\tOutput file: {outFileName}\n");
			}
			else // no command-line arguments
				Console.WriteLine("\n\t       USAGE: myutilootor InputFileName [OutputFileName] [-keep-inactive]\n\n\t        Help: myutilootor -help\n\t    New file: myutilootor -new\n\t     Version: myutilootor -version");
		}
	}
}