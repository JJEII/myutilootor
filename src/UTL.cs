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

using myutilootor.src;

namespace myutilootor.src
{
	class UTL {
		internal List<UTLRule> rules;
		internal List<UTLSalvage> salvage;
		internal string? salvageDefaultCombo;

		internal UTL() {
			rules = new List<UTLRule>();
			salvage = new List<UTLSalvage>();
		}
		internal UTL(MUT my) {
			rules = new List<UTLRule>();
			foreach (UTLRule r in my.rules)
				rules.Add(new UTLRule(r));

			salvage = new List<UTLSalvage>();
			foreach (UTLSalvage s in my.salvage)
				salvage.Add(new UTLSalvage(s));

			salvageDefaultCombo = my.salvageDefaultCombo;
		}

		internal int Read(StreamReader sr) {
			UTLRule rule;
			UTLSalvage salv;
			int ruleCount, salvCount, readCount, nLinesRead = 0;
			string tmp;

			try {
				// UTL
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                if (tmp.CompareTo("UTL") != 0)
					throw new MyException("Expected line to read: UTL");

				// 1
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                if (tmp.CompareTo("1") != 0)
					throw new MyException("Expected line to read: 1");

				// Rule Count
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                ruleCount = int.Parse(tmp);

				// Rules
				for (int r = 0; r < ruleCount; r++) {
					rule = new UTLRule();
					nLinesRead += rule.Read(sr);
					rules.Add(rule);
				}



				// START OF Salvage section

				// SalvageCombine
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                if (tmp.CompareTo("SalvageCombine") != 0)
					throw new MyException("Expected line to read: SalvageCombine");

				// baCount
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                int baCount = int.Parse(tmp);

				readCount = 0;

				// 1
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                if (tmp.CompareTo("1") != 0)
					throw new MyException("Expected line to read: 1");
				readCount += tmp.Length + 2;

				// Default combination rule
				nLinesRead++;
				salvageDefaultCombo = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                readCount += salvageDefaultCombo.Length + 2;

				// Salvage combo count
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                salvCount = int.Parse(tmp);
				readCount += tmp.Length + 2;

				// Salvage combinations
				for (int s = 0; s < salvCount; s++) {
					salv = new UTLSalvage();

					// Salvage type
					nLinesRead++;
					tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    salv.type = E.Salvage.VofK( E.Salvage.KofV(int.Parse(tmp)) ); // enforces that int is in mapping
					readCount += tmp.Length + 2;

					// Ensure uniqueness
					foreach (UTLSalvage sv in salvage)
						if (sv.type == salv.type)
							throw new MyException("Each salvage type may have no more than one specific combination rule defined. Duplicate rule identified.");

					// Salvage combination rule
					nLinesRead++;
					salv.combo = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    readCount += salv.combo.Length + 2;

					salvage.Add(salv);
				}

				// Salvage value-combo count
				nLinesRead++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                salvCount = int.Parse(tmp);
				readCount += tmp.Length + 2;

				// Salvage value-combinations

				List<int> temptypes = new();
				for (int s = 0; s < salvCount; s++) {
					salv = new UTLSalvage();
					
					// Salvage type
					nLinesRead++;
					tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    salv.type = E.Salvage.VofK(E.Salvage.KofV(int.Parse(tmp))); // enforces that int is in mapping   // (E.Salvage)int.Parse(tmp);
                    readCount += tmp.Length + 2;

					// Ensure uniqueness
					foreach (int st in temptypes)
						if (st == salv.type)
							throw new MyException("Each salvage type may have no more than one specific value-based combination rule defined. Duplicate rule identified.");

					// Ensure corresponding workmanship-based combination rule exists
					int si = 0;
					for(si=0; si<salvage.Count; si++)// UTLSalvage sv in salvage)
						if (salvage[si].type == salv.type)
							break;

					if (si >= salvage.Count)
						Console.WriteLine($"WARNING: [LINE {nLinesRead}]: Ignoring value-based salvage combination rule with no corresponding workmanship-based rule.");

//					throw new MyException("Each salvage type may have no more than one specific combination rule defined. Duplicate rule identified.");

					// Salvage combination rule
					nLinesRead++;
					salv.value = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                    readCount += salv.value.Length + 2;

					temptypes.Add(salv.type);

					if ( si < salvage.Count )
						salvage[si].value = salv.value;
				}
				temptypes.Clear();

				if (readCount != baCount)
					throw new MyException("Salvage Error: Specified ByteArrayCount does not match number of bytes read.");

				if (!sr.EndOfStream)
					throw new MyException("Expected end of file.");
			} catch (MyException e) {
				throw new MyException(e.Message, nLinesRead + e.line);
			} catch (Exception e) {
				throw new MyException(e.Message, nLinesRead);
			}

			return nLinesRead;
		}
		internal int Write(StreamWriter sw, bool doSmartOmit = true) {
			int nLinesWritten = 0;
			string tmp;

			// UTL
			sw.WriteLine("UTL");
			nLinesWritten++;

			// 1
			sw.WriteLine("1");
			nLinesWritten++;

			if (doSmartOmit) { // omit disabled rules and rules that have only a MatchRx {do not edit, etc.} requirement
				List<string> omit = new() { "do not edit", "Do not edit this", "leave this disabled" };
				int ruleCount = 0;

				foreach (UTLRule r in rules)
					if (r.disabled == false && r.requirements.Count > 0 && !(r.requirements.Count == 1 && r.requirements[0].type == E.Requirement.MatchRx && omit.Contains((string)r.requirements[0].data[0])))
						ruleCount++;

				// Rule Count
				sw.WriteLine(ruleCount.ToString());
				nLinesWritten++;

				// Rules
				foreach (UTLRule r in rules)
					if (r.disabled == false && r.requirements.Count > 0 && !(r.requirements.Count == 1 && r.requirements[0].type == E.Requirement.MatchRx && omit.Contains((string)r.requirements[0].data[0])))
						nLinesWritten += r.Write(sw);
			} else {
				// Rule Count
				sw.WriteLine(rules.Count.ToString());
				nLinesWritten++;

				// Rules
				foreach (UTLRule r in rules)
					nLinesWritten += r.Write(sw);
			}
			// SalvageCombine
			sw.WriteLine("SalvageCombine");
			nLinesWritten++;

			// Salvage: build ByteArray block containing '1', the default combination rule, all the specific salvage rules, and all the specific value-based salvage rules
			tmp = "1\r\n" + salvageDefaultCombo + "\r\n" + salvage.Count.ToString() + "\r\n";
			int valueCount = 0;
			foreach (UTLSalvage s in salvage) {
				tmp += ((int)s.type).ToString() + "\r\n" + s.combo + "\r\n";
				if (s.value != null)
					valueCount++;
			}
			tmp += valueCount.ToString() + "\r\n";
			foreach (UTLSalvage s in salvage)
				if (s.value != null)
					tmp += ((int)s.type).ToString() + "\r\n" + s.value + "\r\n";
			tmp = tmp.Length.ToString() + "\r\n" + tmp; // insert 4th pre-line (ByteArray Count)
			sw.Write(tmp);
			nLinesWritten += 1 + 2 + 2 * (1 + salvage.Count + valueCount);

			return nLinesWritten;
		}
	}
}
