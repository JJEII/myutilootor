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

namespace myutilootor.src
{
	class UTLRule {
		internal string name;
		internal E.Action action;
		internal int keepN;
		internal List<UTLRequirement> requirements;
		internal bool disabled;
		internal UTLRule() {
			name = "";
			action = E.Action.Keep; // arbitrary default value
            keepN = 0;
			requirements = new List<UTLRequirement>();
			disabled = false;
		}
		internal UTLRule(UTLRule r) {
			name = r.name;
			action = r.action;
			keepN = r.keepN;
			disabled = r.disabled;
			requirements = new List<UTLRequirement>();
			foreach (UTLRequirement q in r.requirements)
				requirements.Add(new UTLRequirement(q));
		}
		internal UTLRule(string n, E.Action a, int c, bool d) {
			name = n;
			action = a;
			keepN = c;
			disabled = d;
			requirements = new List<UTLRequirement>();
		}
		internal void AddRequirement(string qstr) {
			requirements.Add(new UTLRequirement(qstr));
		}
		internal int Read(StreamReader sr) {
			int nLinesReadInRule = 0;
			int baCount, readCount, loff=0; // loff = line offset
			string tmp;
			string[] reqdef;
			UTLRequirement req;

			try {
				// Rule name
				nLinesReadInRule++;
				name = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??

				// Blank line
				nLinesReadInRule++;
				tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                if (tmp.Length != 0)
					throw new MyException("Expected blank line.");

				// Rule definition (semi-colon-separated line)
				nLinesReadInRule++;
				reqdef = (sr.ReadLine() ?? "").Split(';'); // [0] = 0;   [1] = action;   [2...] = requirements   // should never be null, but make VS happy with ??
                if (reqdef[0] != "0")
					throw new MyException("Expected 0 as first value in list.");
				action = (E.Action)int.Parse(reqdef[1]); // should throw exception if can't cast or index

				// Optional Keep# number
				if (action == E.Action.KeepN) {
					nLinesReadInRule++;
					keepN = int.Parse(sr.ReadLine() ?? ""); // should never be null, but make VS happy with ??
                }

				// Requirements
				for (int r = 2; r < reqdef.Length; r++) {
					req = new() { type = (E.Requirement)int.Parse(reqdef[r]) }; // new UTLRequirement
                    nLinesReadInRule++;
					baCount = int.Parse(sr.ReadLine() ?? ""); // should never be null, but make VS happy with ??
                    readCount = 0;
					int i = 0;
					while (readCount < baCount) {
						object tmpData;
						nLinesReadInRule++;
						tmp = sr.ReadLine() ?? ""; // should never be null, but make VS happy with ??
                        readCount += tmp.Length + 2;
						if (ReferenceEquals(UTLRequirement.argVector[(int)req.type][i].GetType(), ((string)"").GetType())) // Object.ReferenceEquals
                            tmpData = tmp; // string
						else if (ReferenceEquals(UTLRequirement.argVector[(int)req.type][i].GetType(), ((int)0).GetType()))
                            tmpData = int.Parse(tmp);
						else if (ReferenceEquals(UTLRequirement.argVector[(int)req.type][i].GetType(), ((double)0.0).GetType()))
                            tmpData = double.Parse(tmp);
						else if (ReferenceEquals(UTLRequirement.argVector[(int)req.type][i].GetType(), ((byte)0).GetType()))
                            tmpData = byte.Parse(tmp);
						else if (ReferenceEquals(UTLRequirement.argVector[(int)req.type][i].GetType(), ((bool)false).GetType()))
                            tmpData = bool.Parse(tmp);
						else
							throw new MyException($"Unrecognized data type. [{tmp}]");
						req.data.Add(tmpData);
						i++;
					}

                    // Additional Requirement argument constraints

                    switch (req.type) {
						case E.Requirement.MatchRx: // loff = 0
							try {
								// E.MatchActsOn t = (E.MatchActsOn)req.data[1];
                                if (!E.MatchActsOn.ContainsValue((int)req.data[1]))
                                    throw new Exception(); // populated below
                            }
                            catch (Exception) {
								throw new Exception($"Invalid {req.type.GetType().Name}.{req.type} 'Acts On' designation.");
							}
							break;
						case E.Requirement.LKeyE: // loff = 0
						case E.Requirement.LKeyFlags:
						case E.Requirement.LKeyGE:
						case E.Requirement.LKeyLE:
						case E.Requirement.LKeyNE:
						case E.Requirement.LKeyBuffedGE:
							try {
                                // E.LongActsOn t = (E.LongActsOn)req.data[1];
                                int t = (int)req.data[1];
                                if (!E.LongActsOn.ContainsValue(t))
                                    throw new Exception(); // populated below
                                if (req.type == E.Requirement.LKeyBuffedGE && !E.LKeyBuffedGEActsOn.ContainsValue(t)) // !(t == E.LongActsOn.L_ArmorLevel || t == E.LongActsOn.L_MaxDamage)
                                    throw new Exception(); // invalid 'acts on' (message filled below)
							} catch (Exception) {
								throw new Exception($"Invalid {req.type.GetType().Name}.{req.type} 'Acts On' designation.");
							}
							break;
						case E.Requirement.DKeyGE: // loff = 0
						case E.Requirement.DKeyLE:
						case E.Requirement.DKeyBuffedGE:
							try {
                                //E.DoubleActsOn t = (E.DoubleActsOn)req.data[1];
                                int t = (int)req.data[1];
                                if (!E.DoubleActsOn.ContainsValue(t))
                                    throw new Exception(); // populated below
                                if (req.type == E.Requirement.DKeyBuffedGE && !E.DKeyBuffedGEActsOn.ContainsValue(t)) //!(t == E.DoubleActsOn.VofK("D_AttackBonus") || t == E.DoubleActsOn.VofK("D_ElementalDamageVersus")
																														//|| t == E.DoubleActsOn.VofK("D_ManaCBonus") || t == E.DoubleActsOn.VofK("D_MeleeDefenseBonus")))
                                    throw new Exception(); // invalid 'acts on' (message filled below)
							} catch (Exception) {
								throw new Exception($"Invalid {req.type.GetType().Name}.{req.type} 'Acts On' designation.");
							}
							break;
						case E.Requirement.ObjClass: // loff = 0
							try {
                                //E.ObjectClass t = (E.ObjectClass)req.data[0];
                                if (!E.ObjectClass.ContainsValue((int)req.data[0]))
                                    throw new Exception(); // populated below
                            }
                            catch (Exception) {
								throw new Exception("Invalid Object Class designation.");
							}
							break;
						case E.Requirement.ColorLike: // loff = -1 or 0
						case E.Requirement.ArmorColorLike:
						case E.Requirement.SlotColorLike:
							if (((double)req.data[4] < 0.0 || (double)req.data[4] > 1.0)) { // S/V
								loff = -1;
								throw new Exception("S/V must be between 0.0 and 1.0, inclusive.");
							}
							if (req.type == E.Requirement.ArmorColorLike && !E.ArmorType.ContainsValue((string)req.data[5])) // Armor Type/Color Region  // !E.UtlString_To_A_Type.ContainsKey((string)req.data[5])
                                throw new Exception("Unrecognized Armor Type/Color Region."); // loff = 0 here still (as it needs to be)
							break;
						case E.Requirement.BuffedSkillGE: // loff = 0
							try {
                                //E.Skill t = (E.Skill)req.data[1];
								if (!E.Skill.ContainsValue((int)req.data[1]))
									throw new Exception(); // populated below
                            }
                            catch (Exception) {
								throw new Exception($"Invalid {req.type.GetType().Name}.{req.type} 'Acts On' designation.");
							}
							break;
						case E.Requirement.BaseSkillRange: // loff = -2
							try {
                                //E.Skill t = (E.Skill)req.data[0];
                                if (!E.Skill.ContainsValue((int)req.data[0]))
                                    throw new Exception(); // populated below
                            }
                            catch (Exception) {
								loff = -2;
								throw new Exception($"Invalid {req.type.GetType().Name}.{req.type} 'Acts On' designation.");
							}
							break;
					}
	
					loff = 0;

					if( req.type == E.Requirement.Disabled ) // loff = 0
						try {
							disabled |= (bool)req.data[0]; // DISabled = TRUE, ENabled = FALSE, true overrides false (if ANY disabled=true requirements are present, the rule is disabled)
                        } catch (Exception) {
							throw new Exception($"Invalid {req.type.GetType().Name}.{req.type} status designation.");
						}
					else
						requirements.Add(req);
				}
			} catch (Exception e) {
				throw new MyException(e.Message, nLinesReadInRule + loff); // loff is a negative offset index
			}

			return nLinesReadInRule;
		}
		internal int Write(StreamWriter sw ) {
			int nLinesWritten = 0;

			sw.WriteLine(name); // rule name
			nLinesWritten++;

			sw.WriteLine(); // blank line
			nLinesWritten++;

			// semi-colon-separated list line
			string tmp = "0;" + ((int)action).ToString();
			for (int q = 0; q < requirements.Count; q++)
				tmp += ";" + ((int)requirements[q].type).ToString();
			if (disabled)
				tmp += ";" + ((int)E.Requirement.Disabled).ToString();
			sw.WriteLine(tmp);
			nLinesWritten++;

			// Optional KeepN line
			if (action == E.Action.KeepN) {
				sw.WriteLine(keepN.ToString());
				nLinesWritten++;
			}

			// Requirements
			foreach (UTLRequirement q in requirements) {
				tmp = "";
				for (int d = 0; d < q.data.Count; d++)
					tmp += q.data[d].ToString() + "\r\n";
				tmp = tmp.Length + "\r\n" + tmp;
				sw.Write(tmp);
				nLinesWritten += 1 + q.data.Count;
			}

			// Optional disabled lines
			if (disabled) {
				sw.WriteLine("6");
				nLinesWritten++;
				sw.WriteLine("true"); // DISabled = true
				nLinesWritten++;
			}

			return nLinesWritten;
		}
		internal int WriteM(StreamWriter sw) {
			int nLinesWritten = 0;
			string ln;

			ln = disabled ? "NO: " : "ON: ";
			ln += "{" + RE.USetStr(name) + "} {";
			ln += (action != E.Action.KeepN) ? action.ToString() : "Keep " + keepN.ToString();
			ln += "} ~~ {";
			sw.WriteLine(ln); // rule name, etc.
			nLinesWritten++;

			// Requirements
			foreach (UTLRequirement q in requirements)
				nLinesWritten += q.WriteM(sw);

			sw.WriteLine("~~ }");
			nLinesWritten++;

			return nLinesWritten;
		}

	}
}
