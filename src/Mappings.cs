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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace myutilootor.src
{
	// Repeated values disappear in reverse-mapping; only the first is used for that.
	// Repeated AddAll calls that try to overwrite already-existing keys throw exceptions.
    internal class BiDict<T> where T : notnull
    {
		private readonly Dictionary<string, T> _kvDict = []; // new Dictionary<string, T>();
        private readonly Dictionary<T, string> _vkDict = []; // new Dictionary<T, string>();

        internal BiDict(Dictionary<string, T> dict, string keyPrefix)
        {
			if (!(typeof(T) == typeof(string) || typeof(T) == typeof(int)))
				throw new Exception($"Bad mapping type \"{typeof(T)}\".");
			AddAll(dict, keyPrefix);
        }
        internal T VofK(string key)
		{
            if (!ContainsKey(key))
				throw new Exception($"Mapping not found for key \"{key}\".");
			return _kvDict[key];
		}
        internal string KofV(T val)
		{
            if (!ContainsValue(val))
				throw new Exception($"Mapping not found for value \"{val}\".");
            return _vkDict[val];
		}
		internal void AddAll(Dictionary<string, T> dict, string keyPrefix)
		{
            foreach (var key in dict.Keys)
				Add(keyPrefix + key, dict[key]);
        }
		internal void Add(string key, T val)
		{
            if (_kvDict.ContainsKey(key))
                throw new Exception($"Cannot overwrite mapped key \"{key}\".");
            _kvDict[key] = val;
            if (!_vkDict.ContainsKey(val))
                _vkDict[val] = key; //_vkDict.Add(val, key);
            else if (key != "L_Unknown8000000") // special exception: "L_Unknown8000000" has same value as "L_Unknown800000" (= 218103845)
                Console.WriteLine($"WARNING: Value \"{val}\" is already mapped-to and will be mapped back to \"{_vkDict[val]}\" instead of \"{key}\".");
        }
        internal bool ContainsKey(string key)
        {
            return _kvDict.ContainsKey(key);
        }
        internal bool ContainsValue(T val)
        {
            return _vkDict.ContainsKey(val);
        }
		internal Dictionary<string, T>.KeyCollection Keys {
			get { return _kvDict.Keys; }
		}
    }

    internal static class E { // enums / enum-likes
		internal static readonly BiDict<int> Salvage, MatchActsOn, LongActsOn, DoubleActsOn, Skill, ObjectClass, LKeyBuffedGEActsOn, DKeyBuffedGEActsOn;
		internal static readonly BiDict<string> ArmorType; // A_Type_To_UtlString / UtlString_To_A_Type
        static E() {
			Salvage = new(new() {
                ["Agate"] = 10,
                ["Alabaster"] = 66,
                ["Amber"] = 11,
                ["Amethyst"] = 12,
                ["Aquamarine"] = 13,
                ["ArmoredilloHide"] = 53,
                ["Azurite"] = 14,
                ["BlackGarnet"] = 15,
                ["BlackOpal"] = 16,
                ["Bloodstone"] = 17,
                ["Brass"] = 57,
                ["Bronze"] = 58,
                ["Carnelian"] = 18,
                ["Ceramic"] = 1,
                ["Citrine"] = 19,
                ["Copper"] = 59,
                ["Diamond"] = 20,
                ["Ebony"] = 73,
                ["Emerald"] = 21,
                ["FireOpal"] = 22,
                ["Gold"] = 60,
                ["Granite"] = 67,
                ["GreenGarnet"] = 23,
                ["GreenJade"] = 24,
                ["GromnieHide"] = 54,
                ["Hematite"] = 25,
                ["ImperialTopaz"] = 26,
                ["Iron"] = 61,
                ["Ivory"] = 51,
                ["Jet"] = 27,
                ["LapisLazuli"] = 28,
                ["LavenderJade"] = 29,
                ["Leather"] = 52,
                ["Linen"] = 4,
                ["Mahogany"] = 74,
                ["Malachite"] = 30,
                ["Marble"] = 68,
                ["Moonstone"] = 31,
                ["Oak"] = 75,
                ["Obsidian"] = 69,
                ["Onyx"] = 32,
                ["Opal"] = 33,
                ["Peridot"] = 34,
                ["Pine"] = 76,
                ["Porcelain"] = 2,
                ["Pyreal"] = 62,
                ["RedGarnet"] = 35,
                ["RedJade"] = 36,
                ["ReedSharkHide"] = 55,
                ["RoseQuartz"] = 37,
                ["Ruby"] = 38,
                ["Sandstone"] = 70,
                ["Sapphire"] = 39,
                ["Satin"] = 5,
                ["Serpentine"] = 71,
                ["Silk"] = 6,
                ["Silver"] = 63,
                ["SmokeyQuartz"] = 40,
                ["Steel"] = 64,
                ["Sunstone"] = 41,
                ["Teak"] = 77,
                ["TigerEye"] = 42,
                ["Tourmaline"] = 43,
                ["Turquoise"] = 44,
                ["Velvet"] = 7,
                ["WhiteJade"] = 45,
                ["WhiteQuartz"] = 46,
                ["WhiteSapphire"] = 47,
                ["Wool"] = 8,
                ["YellowGarnet"] = 48,
                ["YellowTopaz"] = 49,
                ["Zircon"] = 50
            }, "V_");
            MatchActsOn = new(new() {
                ["DateBorn"] = 43,
                ["FellowshipName"] = 10,
                ["FullDescription"] = 16,
                ["ImbuedBy"] = 40,
                ["InscribedBy"] = 8,
                ["Inscription"] = 7,
                ["LastTinkeredBy"] = 39,
                ["MonarchName"] = 21,
                ["Name"] = 1,
                ["OnlyActivatedBy"] = 25,
                ["Patron"] = 35,
                ["PortalDestination"] = 38,
                ["SecondaryName"] = 184549376,
                ["SimpleDescription"] = 15,
                ["Title"] = 5,
                ["UsageInstructions"] = 14
            }, "M_");
            LongActsOn = new(new() {
                ["ActivationReqSkillId"] = 176,
                ["ActiveSpellCount"] = 218103848,
                ["AffectsVitalAmt"] = 90,
                ["AffectsVitalId"] = 89,
                ["Age"] = 125,
                ["ArmorLevel"] = 28,                  // *
                ["ArmorSetID"] = 265,
                ["AssociatedSpell"] = 218103816,
                ["Attuned"] = 114,
                ["Behavior"] = 218103835,
                ["Bonded"] = 33,
                ["Burden"] = 5,
                ["Category"] = 218103834,
                ["CloakChanceType"] = 352,
                ["Container"] = 218103810,
                ["CooldownSeconds"] = 167,
                ["Coverage"] = 218103821,
                ["CreateFlags1"] = 218103832,
                ["CreateFlags2"] = 218103833,
                ["CreatureLevel"] = 25,
                ["CritDamRating"] = 374,
                ["CritDamResistRating"] = 375,
                ["CritRating"] = 372,
                ["CritResistRating"] = 373,
                ["CurrentMana"] = 107,
                ["DamageType"] = 218103841,
                ["DamRating"] = 370,
                ["DamResistRating"] = 371,
                ["DateOfBirth"] = 98,
                ["Deaths"] = 43,
                ["DescriptionFormat"] = 172,
                ["ElementalDmgBonus"] = 204,
                ["EquipableSlots"] = 218103822,
                ["EquippedSlots"] = 10,
                ["EquipSkill"] = 218103840,
                ["EquipType"] = 218103823,
                ["FishingSkill"] = 192,
                ["Flags"] = 218103831,
                ["GemSettingQty"] = 177,
                ["GemSettingType"] = 178,
                ["Gender"] = 113,
                ["HealBoostRating"] = 376,
                ["Heritage"] = 188,
                ["HookMask"] = 218103828,
                ["HookType"] = 218103829,
                ["HouseOwner"] = 218103827,
                ["Icon"] = 218103809,
                ["IconOutline"] = 218103824,
                ["IconOverlay"] = 218103849,
                ["IconUnderlay"] = 218103850,
                ["Imbued"] = 179,
                ["ItemMaxLevel"] = 319,
                ["ItemSlots"] = 218103812,
                ["KeysHeld"] = 193,
                ["Landblock"] = 218103811,
                ["LockpickDifficulty"] = 38,
                ["LockpickSkillBonus"] = 88,
                ["LoreRequirement"] = 109,
                ["MagicDef"] = 218103836,
                ["ManaCost"] = 117,
                ["Material"] = 131,
                ["MaxDamage"] = 218103842,            // *
                ["MaximumMana"] = 108,
                ["MaxLevelRestrict"] = 87,
                ["MinLevelRestrict"] = 86,
                ["MissileType"] = 218103825,
                ["Model"] = 218103830,
                ["Monarch"] = 218103820,
                ["NumberFollowers"] = 35,
                ["NumberItemsSalvaged"] = 170,
                ["NumberTimesTinkered"] = 171,
                ["PackSlots"] = 218103813,
                ["PagesTotal"] = 175,
                ["PagesUsed"] = 174,
                ["PhysicsDataFlags"] = 218103847,
                ["PortalRestrictions"] = 111,
                ["Rank"] = 30,
                ["RankRequirement"] = 110,
                ["RareId"] = 17,
                ["RestrictedToToD"] = 26,
                ["SkillCreditsAvail"] = 24,
                ["SkillLevelReq"] = 115,
                ["SlayerSpecies"] = 166,
                ["Slot"] = 218103817,
                ["SpecialProps"] = 218103837,
                ["Species"] = 2,
                ["SpellCount"] = 218103838,
                ["Spellcraft"] = 106,
                ["StackCount"] = 218103814,
                ["StackMax"] = 218103815,
                ["SummoningGemBuffed"] = 367,
                ["SummoningGemLevel"] = 369,
                ["TotalValue"] = 20,
                ["Type"] = 218103808,
                ["Unenchantable"] = 36,
                ["Unknown10"] = 218103843,
                ["Unknown100000"] = 218103844,
                ["Unknown800000"] = 218103845,
                ["Unknown8000000"] = 218103845, // = Unknown800000, // 218103845,
                ["UsageMask"] = 218103826,
                ["UsesRemaining"] = 92,
                ["UsesTotal"] = 91,
                ["Value"] = 19,
                ["VitalityRating"] = 379,
                ["WandElemDmgType"] = 45,
                ["WeaponMasteryCategory"] = 353,
                ["WeapSpeed"] = 218103839,
                ["Wielder"] = 218103818,
                ["WieldingSlot"] = 218103819,
                ["WieldReqAttribute"] = 159,
                ["WieldReqType"] = 158,
                ["WieldReqValue"] = 160,
                ["Workmanship"] = 105,
                ["XPForVPReduction"] = 129
            }, "L_");
            DoubleActsOn = new(new() {
                ["AcidProt"] = 167772163,
                ["ApproachDistance"] = 167772168,
                ["AttackBonus"] = 167772172,          // *
                ["BludgeonProt"] = 167772162,
                ["ColdProt"] = 167772166,
                ["DamageBonus"] = 167772174,
                ["ElementalDamageVersus"] = 152,      // *
                ["FireProt"] = 167772165,
                ["Heading"] = 167772167,
                ["HealingKitRestoreBonus"] = 100,
                ["LightningProt"] = 167772164,
                ["MagicDBonus"] = 150,
                ["ManaCBonus"] = 144,                 // *
                ["ManaRateOfChange"] = 5,
                ["ManaStoneChanceDestruct"] = 137,
                ["ManaTransferEfficiency"] = 87,
                ["MeleeDefenseBonus"] = 29,           // *
                ["MissileDBonus"] = 149,
                ["PierceProt"] = 167772161,
                ["Range"] = 167772173,
                ["SalvageWorkmanship"] = 167772169,
                ["Scale"] = 167772170,
                ["SlashProt"] = 167772160,
                ["Variance"] = 167772171
            }, "D_");
            Skill = new(new() {
                ["Alchemy"] = 38,
                ["ArcaneLore"] = 14,
                ["ArmorTinkering"] = 29,
                ["AssessCreature"] = 27,
                ["AssessPerson"] = 19,
                ["Axe"] = 1,
                ["Bow"] = 2,
                ["Cooking"] = 39,
                ["CreatureEnchantment"] = 31,
                ["Crossbow"] = 3,
                ["Dagger"] = 4,
                ["Deception"] = 20,
                ["DirtyFighting"] = 52,
                ["DualWield"] = 49,
                ["FinesseWeapons"] = 46,
                ["Fletching"] = 37,
                ["Gearcraft"] = 42,
                ["Healing"] = 21,
                ["HeavyWeapons"] = 44,
                ["ItemEnchantment"] = 32,
                ["ItemTinkering"] = 18,
                ["Jump"] = 22,
                ["Leadership"] = 35,
                ["LifeMagic"] = 33,
                ["LightWeapons"] = 45,
                ["Lockpick"] = 23,
                ["Loyalty"] = 36,
                ["Mace"] = 5,
                ["MagicDefense"] = 15,
                ["MagicItemTinkering"] = 30,
                ["ManaConversion"] = 16,
                ["MeleeDefense"] = 6,
                ["MissileDefense"] = 7,
                ["MissileWeapons"] = 47,
                ["Recklessness"] = 50,
                ["Run"] = 24,
                ["Salvaging"] = 40,
                ["Shield"] = 48,
                ["SneakAttack"] = 51,
                ["Spear"] = 9,
                ["Staff"] = 10,
                ["Summoning"] = 54,
                ["Sword"] = 11,
                ["ThrownWeapons"] = 12,
                ["TwoHandedCombat"] = 41,
                ["Unarmed"] = 13,
                ["VoidMagic"] = 43,
                ["WarMagic"] = 34,
                ["WeaponTinkering"] = 28
            }, "S_");
            ObjectClass = new(new() {
                ["Armor"] = 2,
                ["BaseAlchemy"] = 19,
                ["BaseCooking"] = 18,
                ["BaseFletching"] = 20,
                ["Book"] = 33,
                ["Bundle"] = 32,
                ["Clothing"] = 3,
                ["Container"] = 10,
                ["Corpse"] = 27,
                ["CraftedAlchemy"] = 22,
                ["CraftedCooking"] = 21,
                ["CraftedFletching"] = 23,
                ["Door"] = 26,
                ["Foci"] = 38,
                ["Food"] = 6,
                ["Gem"] = 11,
                ["HealingKit"] = 29,
                ["Housing"] = 36,
                ["Jewelry"] = 4,
                ["Journal"] = 34,
                ["Key"] = 13,
                ["Lifestone"] = 28,
                ["Lockpick"] = 30,
                ["ManaStone"] = 16,
                ["MeleeWeapon"] = 1,
                ["Misc"] = 8,
                ["MissileWeapon"] = 9,
                ["Money"] = 7,
                ["Monster"] = 5,
                ["Npc"] = 37,
                ["Plant"] = 17,
                ["Player"] = 24,
                ["Portal"] = 14,
                ["Salvage"] = 39,
                ["Scroll"] = 42,
                ["Services"] = 41,
                ["Sign"] = 35,
                ["SpellComponent"] = 12,
                ["TradeNote"] = 15,
                ["Unknown"] = 0,
                ["Ust"] = 40,
                ["Vendor"] = 25,
                ["WandStaffOrb"] = 31
            }, "C_");
            LKeyBuffedGEActsOn = new(new() {
				["ArmorLevel"] = 28,
				["MaxDamage"] = 218103842
			}, "L_");
            DKeyBuffedGEActsOn = new(new() {
				["AttackBonus"] = 167772172,
				["ElementalDamageVersus"] = 152,
				["ManaCBonus"] = 144,
				["MeleeDefenseBonus"] = 29
            }, "D_");
            ArmorType = new(new() {
                ["None"] = "None",
                ["Amuli_Coat_Chest"] = "Amuli Coat (Chest)",
                ["Amuli_Coat_CollarShoulder"] = "Amuli Coat (Collar/Shoulder)",
                ["Amuli_Coat_ArmsTrim"] = "Amuli Coat (Arms/Trim)",
                ["Amuli_Legs_Base"] = "Amuli Legs (Base)",
                ["Amuli_Legs_Trim"] = "Amuli Legs (Trim)",
                ["Celdon_Base"] = "Celdon (Base)",
                ["Celdon_Veins"] = "Celdon (Veins)",
                ["Chiran_Coat_BaseArms"] = "Chiran Coat (Base/Arms)",
                ["Chiran_Coat_Stripes"] = "Chiran Coat (Stripes)",
                ["Chiran_Legs_Girth"] = "Chiran Legs (Girth)",
                ["Chiran_Legs_Legs"] = "Chiran Legs (Legs)",
                ["Chiran_Legs_Trim"] = "Chiran Legs (Trim)",
                ["Chiran_Helm_Horns"] = "Chiran Helm (Horns)",
                ["Chiran_Helm_Base"] = "Chiran Helm (Base)",
                ["Haebrean_BP_Chest"] = "Haebrean BP (Chest) *",
                ["Haebrean_BP_Ornaments"] = "Haebrean BP (Ornaments)",
                ["Haebrean_BP_Trim"] = "Haebrean BP (Trim)",
                ["Haebrean_Girth_Base"] = "Haebrean Girth (Base) *",
                ["Haebrean_Girth_BeltScales"] = "Haebrean Girth (Belt/Scales)",
                ["Haebrean_Helm_Base"] = "Haebrean Helm (Base)",
                ["Haebrean_Helm_Mask"] = "Haebrean Helm (Mask)",
                ["Haebrean_Pauldrons_Base"] = "Haebrean Pauldrons (Base) *",
                ["Haebrean_Pauldrons_Ornaments"] = "Haebrean Pauldrons (Ornaments)",
                ["Lorica_BP_Veins"] = "Lorica BP (Veins)",
                ["Lorica_BP_Base"] = "Lorica BP (Base)",
                ["Lorica_BP_NeckTrim"] = "Lorica BP (Neck/Trim) *",
                ["Lorica_Legs_Base"] = "Lorica Legs (Base)",
                ["Lorica_Legs_KneesBeltCrotch"] = "Lorica Legs (Knees/Belt/Crotch) *",
                ["Lorica_Legs_Legs"] = "Lorica Legs (Legs) *",
                ["Nariyid_BP_CircleLines"] = "Nariyid BP (Circle/Lines)",
                ["Nariyid_BP_Base"] = "Nariyid BP (Base)",
                ["Nariyid_BP_Shoulders"] = "Nariyid BP (Shoulders)",
                ["Nariyid_Girth_Base"] = "Nariyid Girth (Base) *",
                ["Nariyid_Girth_BeltLines"] = "Nariyid Girth (Belt/Lines)",
                ["Nariyid_Girth_Ornaments"] = "Nariyid Girth (Ornaments)",
                ["Nariyid_Sleeves_Shoulders"] = "Nariyid Sleeves (Shoulders)",
                ["Nariyid_Sleeves_UpperArm"] = "Nariyid Sleeves (Upper Arm)",
                ["Nariyid_Sleeves_LowerArm"] = "Nariyid Sleeves (Lower Arm)",
                ["Olthoi_BP_Base"] = "Olthoi BP (Base)",
                ["Olthoi_BP_Veins"] = "Olthoi BP (Veins)",
                ["Olthoi_Alduressa_Legs_GirthBase"] = "Olthoi Alduressa Legs (Girth: Base)",
                ["Olthoi_Alduressa_Legs_GirthLines"] = "Olthoi Alduressa Legs (Girth: Lines)",
                ["Olthoi_Alduressa_Legs_LegsLines"] = "Olthoi Alduressa Legs (Legs: Lines)",
                ["Olthoi_Amuli_Coat_Base"] = "Olthoi Amuli Coat (Base) *",
                ["Olthoi_Amuli_Coat_Trim"] = "Olthoi Amuli Coat (Trim)",
                ["Olthoi_Amuli_Coat_Shoulders"] = "Olthoi Amuli Coat (Shoulders)",
                ["Olthoi_Amuli_Legs_Trim"] = "Olthoi Amuli Legs (Trim)",
                ["Olthoi_Koujia_Kabuton_Base"] = "Olthoi Koujia Kabuton (Base)",
                ["Olthoi_Koujia_Kabuton_Horns"] = "Olthoi Koujia Kabuton (Horns)",
                ["Olthoi_Koujia_Legs_Base"] = "Olthoi Koujia Legs (Base)",
                ["Olthoi_Koujia_Legs_SidesShins"] = "Olthoi Koujia Legs (Sides/Shins)",
                ["Scalemail_Cuirass_Base"] = "Scalemail Cuirass (Base)",
                ["Scalemail_Cuirass_Bumps"] = "Scalemail Cuirass (Bumps)",
                ["Scalemail_Cuirass_Belt"] = "Scalemail Cuirass (Belt)",
                ["Tenassa_Legs_LineAtSide"] = "Tenassa Legs (Line at Side)",
                ["Tenassa_Legs_Base"] = "Tenassa Legs (Base)",
                ["Tenassa_Legs_Highlight"] = "Tenassa Legs (Hilight)",
                ["Tenassa_BP_Shoulders"] = "Tenassa BP (Shoulders)",
                ["Tenassa_BP_Base"] = "Tenassa BP (Base)",
                ["Yoroi_Cuirass_Base"] = "Yoroi Cuirass (Base)",
                ["Yoroi_Cuirass_Belt"] = "Yoroi Cuirass (Belt)",
                ["Yoroi_Girth_Base"] = "Yoroi Girth (Base)",
                ["Yoroi_Girth_Belt"] = "Yoroi Girth (Belt)"
            }, "A_");

            const string mappings_file = "mappings.jsonc";
            string mappings_file_fullpath = AppDomain.CurrentDomain.BaseDirectory + mappings_file;
            if (File.Exists(mappings_file_fullpath))
			{
				string jsonc = File.ReadAllText(mappings_file_fullpath);

				var minifier = new DouglasCrockford.JsMin.JsMinifier();
				var sb = new StringBuilder();
				minifier.Minify(jsonc, sb);
				var minJson = sb.ToString();


				var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(minJson);
				if (data != null)
				{
					// Salvage, MatchActsOn, LongActsOn, DoubleActsOn, Skill, ObjectClass, Armor
					Dictionary<string, int> si = [];
					Dictionary<string, string> ss = [];
					int importCount = 0;
					foreach (var kv in data)
					{
						si.Clear();
						ss.Clear();
						switch (kv.Key)
						{
							case "Salvage":
								foreach (var ikv in kv.Value)
									si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? ""); // si.Add(ikv.Key, (int)ikv.Value);
								Salvage.AddAll(si, "V_x");
								importCount += si.Count;
								break;
							case "MatchActsOn":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                MatchActsOn.AddAll(si, "M_x");
								importCount += si.Count;
								break;
							case "LongActsOn":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                LongActsOn.AddAll(si, "L_x");
								importCount += si.Count;
								break;
							case "DoubleActsOn":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                DoubleActsOn.AddAll(si, "D_x");
								importCount += si.Count;
								break;
							case "Skill":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                Skill.AddAll(si, "S_x");
								importCount += si.Count;
								break;
							case "ObjectClass":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                ObjectClass.AddAll(si, "C_x");
								importCount += si.Count;
								break;

							case "LKeyBuffedGEActsOn":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                LKeyBuffedGEActsOn.AddAll(si, "L_x");
								//importCount += si.Count; // omit for this case
								break;
							case "DKeyBuffedGEActsOn":
								foreach (var ikv in kv.Value)
                                    si[ikv.Key] = int.Parse(ikv.Value.ToString() ?? "");
                                DKeyBuffedGEActsOn.AddAll(si, "D_x");
								//importCount += si.Count; // omit for this case
								break;
							case "ArmorType":
								foreach (var ikv in kv.Value)
                                    ss[ikv.Key] = ikv.Value.ToString() ?? "";
                                //ss[ikv.Key] = (string)ikv.Value; // ss.Add(ikv.Key, (string)ikv.Value);
								ArmorType.AddAll(ss, "A_x");
								importCount += ss.Count;
								break;
							default:
								break;
						}
					}

					foreach (var k in LKeyBuffedGEActsOn.Keys)
						if (!LongActsOn.ContainsKey(k))
						{
							LongActsOn.Add(k, LKeyBuffedGEActsOn.VofK(k));
							importCount++;
						}
                    foreach (var k in DKeyBuffedGEActsOn.Keys)
                        if (!DoubleActsOn.ContainsKey(k))
						{
                            DoubleActsOn.Add(k, DKeyBuffedGEActsOn.VofK(k));
                            importCount++;
                        }

                    if (importCount > 0)
                        Console.WriteLine($"Using {importCount} custom mapping{(importCount > 1 ? "s" : "")}.");
                }
            } else
                Console.WriteLine($"No \"{mappings_file}\" file found, so using no custom mappings.");
        }

        internal enum Requirement { // all the various requirements that can appear in a loot rule
            ArmorColorLike = 15,
			BaseSkillRange = 1004,
			BuffedSkillGE = 1000,
			CalcedBuffedMedianDmgGE = 2000,
			CalcedBuffedMissileDmgGE = 2001,
			CalcedBuffedTinkDmgGE = 2006,
			CalcedBuffedTinkTarget = 2008,
			CalcedTotalRatingsGE = 2007,
			CharLevelGE = 1002,
			CharLevelLE = 1003,
			ColorLike = 14,
			DKeyBuffedGE = 2005,
			DKeyGE = 5,
			DKeyLE = 4,
			DmgPercentGE = 6,
			EmptyMainPackSlotsGE = 1001,
			LKeyBuffedGE = 2003,
			LKeyE = 12,
			LKeyFlags = 11,
			LKeyGE = 3,
			LKeyLE = 2,
			LKeyNE = 13,
			MatchRx = 1,
			MinDmgGE = 10,
			NSpellsGE = 8,
			NSpellsRxGE = 9,
			ObjClass = 7,
			SlotColorLike = 16,
			SlotPalette = 17,
			SpellRx = 0,
			Disabled = 9999         // special one (ON: and NO:)
		};
		internal enum Action { // all the available actions that can be taken in response to a loot rule matching
			Keep = 1,
			Salvage = 2,
			Sell = 3,
			Read = 4,
			KeepN = 10      // special case
		};
	}
}
