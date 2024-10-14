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
	class UTLRequirement {
        // requirement argument-vector definitions
        internal static Dictionary<int, object[]> argVector = new() {
			/*                  SpellRx */    [0] = new object[] { (string)"" },
			/*                  MatchRx */    [1] = new object[] { (string)"", (int)0 },
			/*                   LKeyLE */    [2] = new object[] { (int)0, (int)0 },
			/*                   LKeyGE */    [3] = new object[] { (int)0, (int)0 },
			/*                   DKeyLE */    [4] = new object[] { (double)0.0, (int)0 },
			/*                   DKeyGE */    [5] = new object[] { (double)0.0, (int)0 },
			/*             DmgPercentGE */    [6] = new object[] { (double)0.0 },
			/*                 ObjClass */    [7] = new object[] { (int)0 },
			/*                NSpellsGE */    [8] = new object[] { (int)0 },
			/*              NSpellsRxGE */    [9] = new object[] { (string)"", (string)"", (int)0 },
			/*                 MinDmgGE */   [10] = new object[] { (double)0.0 },
			/*                LKeyFlags */   [11] = new object[] { (int)0, (int)0 },
			/*                    LKeyE */   [12] = new object[] { (int)0, (int)0 },
			/*                   LKeyNE */   [13] = new object[] { (int)0, (int)0 },
			/*                ColorLike */   [14] = new object[] { (byte)0, (byte)0, (byte)0, (byte)0, (double)0.0 },
			/*           ArmorColorLike */   [15] = new object[] { (byte)0, (byte)0, (byte)0, (byte)0, (double)0.0, (string)"" },
			/*            SlotColorLike */   [16] = new object[] { (byte)0, (byte)0, (byte)0, (byte)0, (double)0.0, (int)0 },
			/*              SlotPalette */   [17] = new object[] { (int)0, (int)0 },
			/*            BuffedSkillGE */ [1000] = new object[] { (double)0.0, (int)0 },
			/*     EmptyMainPackSlotsGE */ [1001] = new object[] { (int)0 },
			/*              CharLevelGE */ [1002] = new object[] { (int)0 },
			/*              CharLevelLE */ [1003] = new object[] { (int)0 },
			/*           BaseSkillRange */ [1004] = new object[] { (int)0, (int)0, (int)0 },
			/*  CalcedBuffedMedianDmgGE */ [2000] = new object[] { (double)0.0 },
			/* CalcedBuffedMissileDmgGE */ [2001] = new object[] { (double)0.0 },
			/*             LKeyBuffedGE */ [2003] = new object[] { (int)0, (int)0 },
			/*             DKeyBuffedGE */ [2005] = new object[] { (double)0.0, (int)0 },
			/*    CalcedBuffedTinkDmgGE */ [2006] = new object[] { (double)0.0 },
			/*     CalcedTotalRatingsGE */ [2007] = new object[] { (double)0.0 },
			/*   CalcedBuffedTinkTarget */ [2008] = new object[] { (double)0.0, (double)0.0, (double)0.0 },
			/*                 Disabled */ [9999] = new object[] { (bool)false }
		};

        internal E.Requirement type;
		internal List<object> data = []; // data in each req
		internal UTLRequirement() { data = new List<object>(); }
		internal UTLRequirement(UTLRequirement q) {
			//data = new List<object>();
			type = q.type;
			foreach (object d in q.data)
				data.Add(d);
		}
		internal UTLRequirement(string qstr) {
			const uint m = 0x000000FF;
			double d;
			uint ui;

			Match match;
			string tmp;

			data = new List<object>();

			match = RE.getLeadIn["AnyRequirement"].Match(qstr);
			if (!match.Success)
				throw new MyException(TextForUsers.getInfo["Generic"]);
			tmp = match.Groups["req"].Value;
			match = RE.getArgs[tmp].Match(qstr[(1 + tmp.Length)..]);
			if (!match.Success)
				throw new MyException(TextForUsers.getInfo[tmp]);
			try {
				type = (E.Requirement)Enum.Parse(typeof(E.Requirement), tmp);
				switch (type) {
					case E.Requirement.SpellRx:      //  string         @"^\s+(?<s>" + _S + @")$"
						data.Add(RE.UGetStr(match.Groups["s"].Value[1..^1]));
						break;
					case E.Requirement.MatchRx:      //  E.MatchActsOn   string      @"^\s+(?<l>" + _L_M_ + @")\s+(?<s>" + _S + @")$"
						data.Add(RE.UGetStr(match.Groups["s"].Value[1..^1]));
						data.Add(E.MatchActsOn.VofK(match.Groups["l"].Value)); // (int)Enum.Parse(typeof(E.MatchActsOn),match.Groups["l"].Value)
						break;
                    case E.Requirement.LKeyE:        //  E.LongActsOn   int
					case E.Requirement.LKeyFlags:
					case E.Requirement.LKeyGE:
					case E.Requirement.LKeyLE:
					case E.Requirement.LKeyNE:
					case E.Requirement.LKeyBuffedGE:     // limited       @"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$"
						int eL = E.LongActsOn.VofK(match.Groups["l"].Value); // (E.LongActsOn)Enum.Parse(typeof(E.LongActsOn), match.Groups["l"].Value);
                        if (type == E.Requirement.LKeyBuffedGE && !E.LKeyBuffedGEActsOn.ContainsValue(eL)) // eL != E.LongActsOn.VofK("L_ArmorLevel") && eL != E.LongActsOn.VofK("L_MaxDamage"))
							throw new Exception();
						data.Add(int.Parse(match.Groups["i"].Value));
						data.Add((int)eL);
						break;
					case E.Requirement.DKeyGE:       //  E.DoubleActsOn   dbl
					case E.Requirement.DKeyLE:
					case E.Requirement.DKeyBuffedGE: // @"^\s+(?<l>" + _L_D_ + @")\s+(?<d>" + _D + @")$"
						int eD = E.DoubleActsOn.VofK(match.Groups["l"].Value); // (E.DoubleActsOn)Enum.Parse(typeof(E.DoubleActsOn), match.Groups["l"].Value);
						if (type == E.Requirement.DKeyBuffedGE && !E.DKeyBuffedGEActsOn.ContainsValue(eD))
								//eD != E.DoubleActsOn.VofK("D_AttackBonus") && eD != E.DoubleActsOn.VofK("D_ElementalDamageVersus") &&
								//eD != E.DoubleActsOn.VofK("D_ManaCBonus") && eD != E.DoubleActsOn.VofK("D_MeleeDefenseBonus")
                            throw new Exception();
						data.Add(double.Parse(match.Groups["d"].Value));
						data.Add((int)eD);
						break;
					case E.Requirement.DmgPercentGE: //  dbl
					case E.Requirement.MinDmgGE:
					case E.Requirement.CalcedBuffedMedianDmgGE:
					case E.Requirement.CalcedBuffedMissileDmgGE:
					case E.Requirement.CalcedBuffedTinkDmgGE:
					case E.Requirement.CalcedTotalRatingsGE:   // @"^\s+(?<d>" + _D + @")$"
						data.Add(double.Parse(match.Groups["d"].Value));
						break;
					case E.Requirement.ObjClass:     //  E.ObjectClass      @"^\s+(?<l>" + _L_C_ + @")$"
                        data.Add(E.ObjectClass.VofK(match.Groups["l"].Value)); // (int)Enum.Parse(typeof(E.ObjectClass), match.Groups["l"].Value)
						break;
					case E.Requirement.NSpellsGE:    //  int
					case E.Requirement.EmptyMainPackSlotsGE:
					case E.Requirement.CharLevelGE:
					case E.Requirement.CharLevelLE: //  @"^\s+(?<i>" + _I + @")$"
						data.Add(int.Parse(match.Groups["i"].Value));
						break;
					case E.Requirement.NSpellsRxGE:  //  int   string   string    @"^\s+(?<i>" + _I + @")\s+(?<s>" + _S + @")\s+(?<s2>" + _S + @")$"
						data.Add(RE.UGetStr(match.Groups["s"].Value[1..^1]));
                        data.Add(RE.UGetStr(match.Groups["s2"].Value[1..^1]));
                        data.Add(int.Parse(match.Groups["i"].Value));
						break;
					case E.Requirement.ColorLike:    //  hex6   byte   dbl    @"^\s+(?<h>" + _H + @")\s+(?<i>" + _I + @")\s+(?<d>" + _D + @")$"
						ui = uint.Parse(match.Groups["h"].Value, System.Globalization.NumberStyles.HexNumber);
						data.Add((byte)((ui >> 16) & m));
						data.Add((byte)((ui >> 8) & m));
						data.Add((byte)(ui & m));
						data.Add(byte.Parse(match.Groups["i"].Value));
						d = double.Parse(match.Groups["d"].Value);
						if (d < 0.0 || d > 1.0)
							throw new Exception();
						data.Add(d);
						break;
					case E.Requirement.ArmorColorLike:// eA_Type   hex6   byte   dbl    @"^\s+(?<l>" + _L_A_ + @")\s+(?<h>" + _H + @")\s+(?<i>" + _I + @")\s+(?<d>" + _D + @")$"
						ui = uint.Parse(match.Groups["h"].Value, System.Globalization.NumberStyles.HexNumber);
						data.Add((byte)((ui >> 16) & m));
						data.Add((byte)((ui >> 8) & m));
						data.Add((byte)(ui & m));
						data.Add(byte.Parse(match.Groups["i"].Value));
						d = double.Parse(match.Groups["d"].Value);
						if (d < 0.0 || d > 1.0)
							throw new Exception();
						data.Add(d);
						data.Add(E.ArmorType.VofK(match.Groups["l"].Value)); // E.A_Type_To_UtlString[match.Groups["l"].Value]
						break;
					case E.Requirement.SlotColorLike://  int   hex6   byte   dbl    @"^\s+(?<i>" + _I + @")\s+(?<h>" + _H + @")\s+(?<i2>" + _I + @")\s+(?<d>" + _D + @")$"
						ui = uint.Parse(match.Groups["h"].Value, System.Globalization.NumberStyles.HexNumber);
						data.Add((byte)((ui >> 16) & m));
						data.Add((byte)((ui >> 8) & m));
						data.Add((byte)(ui & m));
						data.Add(byte.Parse(match.Groups["i2"].Value));
						d = double.Parse(match.Groups["d"].Value);
						if (d < 0.0 || d > 1.0)
							throw new Exception();
						data.Add(d);
						data.Add(int.Parse(match.Groups["i"].Value));
						break;
					case E.Requirement.SlotPalette:    //  int   int    @"^\s+(?<i>" + _I + @")\s+(?<i2>" + _I + @")$"
						data.Add(int.Parse(match.Groups["i"].Value));
						data.Add(int.Parse(match.Groups["i2"].Value));
						break;
					case E.Requirement.BuffedSkillGE://  E.Skill   dbl   @"^\s+(?<l>" + _L_S_ + @")\s+(?<d>" + _D + @")$"
						data.Add(double.Parse(match.Groups["d"].Value));
						data.Add(E.Skill.VofK(match.Groups["l"].Value)); // (int)Enum.Parse(typeof(E.Skill), match.Groups["l"].Value)
                        break;
					case E.Requirement.BaseSkillRange://  E.Skill   int   int   @"^\s+(?<l>" + _L_S_ + @")\s+(?<i>" + _I + @")\s+(?<i2>" + _I + @")$"
						data.Add(E.Skill.VofK(match.Groups["l"].Value)); //  (int)Enum.Parse(typeof(E.Skill),match.Groups["l"].Value)
						data.Add(int.Parse(match.Groups["i"].Value));
						data.Add(int.Parse(match.Groups["i2"].Value));
						break;
					case E.Requirement.CalcedBuffedTinkTarget: //  dbl   dbl   dbl    @"^\s+(?<d>" + _D + @")\s+(?<d2>" + _D + @")\s+(?<d3>" + _D + @")$"
						data.Add(double.Parse(match.Groups["d"].Value));
						data.Add(double.Parse(match.Groups["d2"].Value));
						data.Add(double.Parse(match.Groups["d3"].Value));
						break;
					//case E.Requirement.Disabled:     // skip disable/enable commands since they're placed elsewhere
					//	return 0;
					default:
						throw new MyException($"Unknown Requirement Type {type}!");
				}
			} catch (Exception) {
				throw new MyException(TextForUsers.getInfo[tmp]);
			}

		}
		private static string GetStr_HexHueSV(byte r, byte g, byte b, byte hue, double sv) {
			uint c = (((uint)r) << 16) | (((uint)g) << 8) | ((uint)b);
			return c.ToString("X6") + " " + ((uint)hue).ToString() + " " + sv.ToString();
        }
		internal int WriteM(StreamWriter sw) {
			int nLinesWritten = 0;
			string ln;

			// Requirement type
			ln = "\t" + type.ToString();
			
			// Requirement arguments
			switch( type ) {
				case E.Requirement.SpellRx:		//  string
					ln += " {" + RE.USetStr(((string)data[0]).ToString()) + "}";
					break;
				case E.Requirement.MatchRx:     //  E.MatchActsOn   string
                    ln += " " + E.MatchActsOn.KofV((int)data[1]) + " {" + RE.USetStr(((string)data[0]).ToString()) + "}"; // ((E.MatchActsOn)data[1]).ToString()
                    break;
				case E.Requirement.LKeyE:		//  E.LongActsOn   int
				case E.Requirement.LKeyFlags:
				case E.Requirement.LKeyGE:
				case E.Requirement.LKeyLE:
				case E.Requirement.LKeyNE:
				case E.Requirement.LKeyBuffedGE:        // limited
					string commentStr = "";
					if (type != E.Requirement.LKeyBuffedGE && (int)data[1] == E.LongActsOn.VofK("L_ArmorSetID")) // (int)E.LongActsOn.L_ArmorSetID
						commentStr = AutoComment.GetEquipmentSetNameById( (int)data[0] );
                    ln += " " + E.LongActsOn.KofV((int)data[1]) + " " + ((int)data[0]).ToString() + commentStr; // ((E.LongActsOn)data[1]).ToString()
                    break;
				case E.Requirement.DKeyGE:		//  E.DoubleActsOn   dbl
				case E.Requirement.DKeyLE:
				case E.Requirement.DKeyBuffedGE:
					ln += " " + E.DoubleActsOn.KofV((int)data[1]) + " " + ((double)data[0]).ToString(); // ((E.DoubleActsOn)data[1]).ToString()
                    break;
				case E.Requirement.DmgPercentGE: //  dbl
				case E.Requirement.MinDmgGE:
				case E.Requirement.CalcedBuffedMedianDmgGE:
				case E.Requirement.CalcedBuffedMissileDmgGE:
				case E.Requirement.CalcedBuffedTinkDmgGE:
				case E.Requirement.CalcedTotalRatingsGE:
					ln += " " + ((double)data[0]).ToString();
					break;
				case E.Requirement.ObjClass:     //  E.ObjectClass
					ln += " " + E.ObjectClass.KofV((int)data[0]); // ((E.ObjectClass)data[0]).ToString()
					break;
				case E.Requirement.NSpellsGE:	//  int
				case E.Requirement.EmptyMainPackSlotsGE:
				case E.Requirement.CharLevelGE:
				case E.Requirement.CharLevelLE:
					ln += " " + ((int)data[0]).ToString();
					break;
				case E.Requirement.NSpellsRxGE:  //  int   string   string
					ln += " " + ((int)data[2]).ToString() + " {" + RE.USetStr((string)data[0]) + "}" + " {" + RE.USetStr((string)data[1]) + "}";
					break;
				case E.Requirement.ColorLike:    //  hex6   byte   dbl
					ln += " " + GetStr_HexHueSV((byte)data[0], (byte)data[1], (byte)data[2], (byte)data[3], (double)data[4]);
					break;
				case E.Requirement.ArmorColorLike:// eA_Type   hex6   byte   dbl
					ln += " " + E.ArmorType.KofV((string)data[5]) + " " + GetStr_HexHueSV((byte)data[0], (byte)data[1], (byte)data[2], (byte)data[3], (double)data[4]); // E.UtlString_To_A_Type[((string)data[5])]
                    break;
				case E.Requirement.SlotColorLike://  int   hex6   byte   dbl
					ln += " " + ((int)data[5]).ToString() + " " + GetStr_HexHueSV((byte)data[0], (byte)data[1], (byte)data[2], (byte)data[3], (double)data[4]);
					break;
				case E.Requirement.SlotPalette:    //  int   int
					ln += " " + ((int)data[0]).ToString() + " " + ((int)data[1]).ToString();
					break;
				case E.Requirement.BuffedSkillGE://  E.Skill   dbl
                    //ln += " " + ((E.Skill)data[1]).ToString() + " " + ((double)data[0]).ToString();
                    ln += " " + E.Skill.KofV((int)data[1]) + " " + ((double)data[0]).ToString();
                    break;
				case E.Requirement.BaseSkillRange://  E.Skill   int   int
                    //ln += " " + ((E.Skill)data[0]).ToString() + " " + ((int)data[1]).ToString() + " " + ((int)data[2]).ToString();
                    ln += " " + E.Skill.KofV((int)data[0]) + " " + ((int)data[1]).ToString() + " " + ((int)data[2]).ToString();
                    break;
				case E.Requirement.CalcedBuffedTinkTarget: //  dbl   dbl   dbl
					ln += " " + ((double)data[0]).ToString() + " " + ((double)data[1]).ToString() + " " + ((double)data[2]).ToString();
					break;
				case E.Requirement.Disabled:		// skip disable/enable commands since they're placed elsewhere
					return 0;
				default:
					throw new MyException($"Unknown Requirement Type {type}!");
			}

			sw.WriteLine(ln);
			nLinesWritten++;

			return nLinesWritten;
        }
	}
}
