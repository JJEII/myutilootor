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
*/

using System.Text.RegularExpressions;

namespace myutilootor.src
{
	class MUT {
		internal List<UTLRule> rules;
		internal List<UTLSalvage> salvage;
		internal string salvageDefaultCombo;

		internal MUT() {
			rules = new List<UTLRule>();
			salvage = new List<UTLSalvage>();
			salvageDefaultCombo = string.Empty; // give it some sort of default value
        }
        internal MUT(UTL u) {
			rules = new List<UTLRule>();
			foreach (UTLRule r in u.rules)
				rules.Add(new UTLRule(r));

			salvage = new List<UTLSalvage>();
			foreach (UTLSalvage s in u.salvage)
				salvage.Add(new UTLSalvage(s));

			salvageDefaultCombo = u.salvageDefaultCombo ?? ""; // should never be null, but make VS happy with ??
		}
		internal int Write(StreamWriter sw) {
			int nLinesWritten = 0;

			sw.Write(TextForUsers.header);

			// Rules
			foreach (UTLRule r in rules)
				nLinesWritten += r.WriteM(sw);

			// Blank line
			sw.WriteLine();
			nLinesWritten++;

			// Salvage
			sw.WriteLine("SALVAGE: {" + RE.USetStr(salvageDefaultCombo) + "} ~~ {");
			nLinesWritten++;

			foreach (UTLSalvage s in salvage) {
				if(s.value == null)
                    //sw.WriteLine("\t" + s.type.ToString() + " {" + RE.USetStr(s.combo) + "}");
	                sw.WriteLine("\t" + E.Salvage.KofV(s.type) + " {" + RE.USetStr(s.combo) + "}");
                else
                    //sw.WriteLine("\t" + s.type.ToString() + " {" + RE.USetStr(s.combo) + "} {" + RE.USetStr(s.value) + "}");
					sw.WriteLine("\t" + E.Salvage.KofV(s.type) + " {" + RE.USetStr(s.combo) + "} {" + RE.USetStr(s.value) + "}");
                nLinesWritten++;
			}

			sw.WriteLine("~~ }");
			nLinesWritten++;

			return nLinesWritten;
        }
		internal int Read(StreamReader sr) {
			int nLinesRead = 0;
			string ln;
			Match match;
			int mode = -1; // -1 pre-rules, 0 in rules, 1 in salvage
			while (!sr.EndOfStream) {

				do { // find next non-"blank" line, or EOF
					ln = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    nLinesRead++;
				} while (!sr.EndOfStream && (match = RE.R__LN.Match(ln)).Success);

				// EOF
				if (sr.EndOfStream) {
					break;
				}


				// Found first non-"blank" line... done reading Rules for this loot set ? ("SALVAGE:" line ?)
				ln = RE.R__2EOL.Replace(ln, ""); // purge any trailing whitespace and comments from the line
				match = RE.getLeadIn["OnNoSalvage"].Match(ln);

				// if found something-else, but haven't previously found the first ON: or NO: (or SALVAGE:)
				if (mode < 0 && !match.Success)
					throw new MyException($"[LINE {nLinesRead}]: Syntax error. Must define 'ON:', 'NO:' or 'SALVAGE:' before other data. {TextForUsers.getInfo["ON:"]} {TextForUsers.getInfo["SALVAGE:"]}");

				// if found SALVAGE:
				if (match.Success && match.Groups["type"].Value.CompareTo("SALVAGE:") == 0) {
					if (mode == 1)
						throw new MyException($"[LINE {nLinesRead}]: Syntax error. Only one 'SALVAGE:' section allowed. {TextForUsers.getInfo["SALVAGE:"]}");
					mode = 1;
					match = RE.getArgs["SALVAGE:"].Match(ln[8..]); // start just after "SALVAGE:"
					if (!match.Success)
						throw new MyException($"[LINE {nLinesRead}]: Syntax error. {TextForUsers.getInfo["SALVAGE:"]}");
					salvageDefaultCombo = RE.UGetStr(match.Groups["s"].Value[1..^1]); // length is at least 2; remove delimiters
																						// now, just go finish up the file in the salvage section...
					break;
				} else if (match.Success) { // if found an ON: or NO:
					mode = 0;
					match = RE.getArgs["ON:"].Match(ln[3..]); // start just after "ON:" (or NO:)
					if (!match.Success)
						throw new MyException($"[LINE {nLinesRead}]: Syntax error. {TextForUsers.getInfo["ON:"]}");
					string nam = RE.UGetStr(match.Groups["s"].Value[1..^1]); // length is at least 2; remove delimiters
					string act = RE.UGetStr(match.Groups["s2"].Value[1..^1]); // length is at least 2; remove delimiters
					match = RE.getArgs["RuleAction"].Match(act);
					int n = 0;
					E.Action eAct;
					if (!match.Success) {
						match = RE.getArgs["RuleActionKeepN"].Match(act);
						if (!match.Success)
							throw new MyException($"[LINE {nLinesRead}]: Syntax error. {TextForUsers.getInfo["ON:"]}");
						try {
							n = int.Parse(match.Groups["i"].Value);
						} catch (Exception) {
							throw new MyException($"[LINE {nLinesRead}]: Syntax error. {TextForUsers.getInfo["ON:"]} ({TextForUsers.typeInfo["_S"]})");
						}
						eAct = E.Action.KeepN;
					}
					else {
						try {
							eAct = (E.Action)Enum.Parse(typeof(E.Action), act);
						} catch (Exception) {
							throw new MyException($"[LINE {nLinesRead}]: Syntax error. {TextForUsers.getInfo["ON:"]} ({TextForUsers.typeInfo["_S"]})");
						}
					}
					this.rules.Add(new UTLRule(nam, eAct, n, ln[0] == 'N')); // == determines if it's ON: or NO:
				} else { // ! match.Success (inside of a rule somewhere, presumably, so read the requirement)
					try {
						rules[^1].AddRequirement(ln); // [rules.Count - 1]
					} catch (MyException e) {
						throw new MyException($"[LINE {nLinesRead}]: {e.Message}");
					}
				}

			}

			// Read in the specific salvage combination rules
			while (!sr.EndOfStream) {
				do { // find next non-"blank" line, or EOF
					ln = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    nLinesRead++;
				} while (!sr.EndOfStream && (match = RE.R__LN.Match(ln)).Success);

				// EOF
				if (sr.EndOfStream) {
					break;
				}

				// Found first non-"blank" line... done reading Rules for this loot set ? ("SALVAGE:" line ?)
				ln = RE.R__2EOL.Replace(ln, ""); // purge any trailing whitespace and comments from the line
				match = RE.getLeadIn["AnySalvage"].Match(ln);

				// if found something-else, wrong
				if (!match.Success)
					throw new MyException($"[LINE {nLinesRead}]: Syntax error. {TextForUsers.getInfo["V_Salvage"]}");

                //E.Salvage eV = (E.Salvage)Enum.Parse(typeof(E.Salvage), match.Groups["salv"].Value);
                int eV = E.Salvage.VofK(match.Groups["salv"].Value);
                if (0 <= salvage.FindIndex(f => f.type == eV))
					throw new MyException($"[LINE {nLinesRead}]: Each salvage type may have no more than one combination-rule entry. Duplicate detected.{TextForUsers.getInfo["V_Salvage"]}");
				match = RE.getArgs["V_Salvage"].Match(ln[(1+match.Groups["salv"].Value.Length)..]);
				if( !match.Success )
					throw new MyException($"[LINE {nLinesRead}]: Each salvage type entered requires a combination-rule.{TextForUsers.getInfo["V_Salvage"]}");
				salvage.Add(new UTLSalvage(eV,
					RE.UGetStr(match.Groups["s"].Value[1..^1]),
					match.Groups["v"].Value.Length>=2 ? RE.UGetStr(match.Groups["v"].Value[1..^1]) : null
				));
			}

			return nLinesRead;
        }
	}
}
