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
    internal class RE
    {
		//RULES:
		//	1. INTERNALLY STORE A VALID myutilootor STRING (minus enclosing delimiters).
		//	2. UTL Import/Export
		//		a. set: expand, set
		//		b. get: shrink, return
		//	3. MUT Import/Export
		//		a. set: enforce {set | throw} <<< this is taken care of by the regex itself matching, or not
		//		b. get: return
		internal static string MSetStr(string s) { // MUT set: enforce the rules, but... the regex should already be doing that
			return s;
		}
		internal static string MGetStr(string s) { // MUT get: just return the string
			return s;
		}
		internal static string USetStr(string s) { // UTL set: grow the string (oD --> oDoD; cD --> cDcD)
			string t = new Regex(@"\" + oD).Replace(s, oD + oD);
			if (oD.CompareTo(cD) != 0)
				t = new Regex(@"\" + cD).Replace(t, cD + cD);
			return t;
		}
		internal static string UGetStr(string s) { // UTL get: shrink the string (oDoD --> oD; cDcD --> cD)
			string t = new Regex(@"\" + oD + @"\" + oD).Replace(s, oD);
			if (oD.CompareTo(cD) != 0)
				t = new Regex(@"\" + cD + @"\" + cD).Replace(t, cD);
			return t;
		}

		internal const string oD = "{"; // opening string delimiter
		internal const string cD = "}"; // closing string delimiter

		private const string __2EOL = @"\s*(~~.*)?$";//new Regex( , RegexOptions.Compiled);
		internal static Regex R__2EOL = new(__2EOL, RegexOptions.Compiled);
		internal static Regex R__LN = new(@"^\s*(~~.*)?$", RegexOptions.Compiled);
		internal static Regex R_Empty = new(@"^$", RegexOptions.Compiled);

        // "Core" regexes: double, int, hexInt, mutString, literal
        private const string _D = @"[+\-]?(([1-9][0-9]*\.|[0-9]?\.)([0-9]+([eE][+\-]?[0-9]+)|[0-9]+)|([1-9][0-9]*|0))";
        private const string _I = @"[+\-]?([1-9][0-9]*|0)";
        private const string _H = @"[A-F0-9]{6}";
        private const string _S = @"[\" + oD + @"]([^\" + oD + @"\" + cD + @"]|\" + oD + @"\" + oD + @"|\" + cD + @"\" + cD + @")*[\" + cD + @"]"; // [o]([^oc]|[oo]|[cc])*[c]
																																				   //private const string _L = @"[a-zA-Z_][a-zA-Z0-9_]*"; // literal

		// recognized literals for the various field values (enums, as well as "A_ dictionaries")
		//private const string _L_A_ = @"A_(None|Amuli_(Coat_Chest|Coat_CollarShoulder|Coat_ArmsTrim|Legs_Base|Legs_Trim)|Celdon_(Base|Veins)|Chiran_(Coat_BaseArms|Coat_Stripes|Legs_Girth|Legs_Legs|Legs_Trim|Helm_Horns|Helm_Base)|Haebrean_(BP_Chest|BP_Ornaments|BP_Trim|Girth_Base|Girth_BeltScales|Helm_Base|Helm_Mask|Pauldrons_Base|Pauldrons_Ornaments)|Lorica_(BP_Veins|BP_Base|BP_NeckTrim|Legs_Base|Legs_KneesBeltCrotch|Legs_Legs)|Nariyid_(BP_CircleLines|BP_Base|BP_Shoulders|Girth_Base|Girth_BeltLines|Girth_Ornaments|Sleeves_Shoulders|Sleeves_UpperArm|Sleeves_LowerArm)|Olthoi_(BP_Base|BP_Veins|Alduressa_(Legs_GirthBase|Legs_GirthLines|Legs_LegsLines)|Amuli_(Coat_Base|Coat_Trim|Coat_Shoulders|Legs_Trim)|Koujia_(Kabuton_Base|Kabuton_Horns|Legs_Base|Legs_SidesShins))|Scalemail_(Cuirass_Base|Cuirass_Bumps|Cuirass_Belt)|Tenassa_(Legs_LineAtSide|Legs_Base|Legs_Highlight|BP_Shoulders|BP_Base)|Yoroi_(Cuirass|Girth)_(Base|Belt))";
		//private const string _L_D_ = @"D_(AcidProt|ApproachDistance|AttackBonus|BludgeonProt|ColdProt|DamageBonus|ElementalDamageVersus|FireProt|Heading|HealingKitRestoreBonus|LightningProt|MagicDBonus|ManaCBonus|ManaRateOfChange|ManaStoneChanceDestruct|ManaTransferEfficiency|MeleeDefenseBonus|MissileDBonus|PierceProt|Range|SalvageWorkmanship|Scale|SlashProt|Variance)";
		//private const string _L_L_ = @"L_(ActivationReqSkillId|ActiveSpellCount|AffectsVitalAmt|AffectsVitalId|Age|ArmorLevel|ArmorSetID|AssociatedSpell|Attuned|Behavior|Bonded|Burden|Category|CloakChanceType|Container|CooldownSeconds|Coverage|CreateFlags1|CreateFlags2|CreatureLevel|CritDamRating|CritDamResistRating|CritRating|CritResistRating|CurrentMana|DamageType|DamRating|DamResistRating|DateOfBirth|Deaths|DescriptionFormat|ElementalDmgBonus|EquipableSlots|EquippedSlots|EquipSkill|EquipType|FishingSkill|Flags|GemSettingQty|GemSettingType|Gender|HealBoostRating|Heritage|HookMask|HookType|HouseOwner|Icon|IconOutline|IconOverlay|IconUnderlay|Imbued|ItemMaxLevel|ItemSlots|KeysHeld|Landblock|LockpickDifficulty|LockpickSkillBonus|LoreRequirement|MagicDef|ManaCost|Material|MaxDamage|MaximumMana|MaxLevelRestrict|MinLevelRestrict|MissileType|Model|Monarch|NumberFollowers|NumberItemsSalvaged|NumberTimesTinkered|PackSlots|PagesTotal|PagesUsed|PhysicsDataFlags|PortalRestrictions|Rank|RankRequirement|RareId|RestrictedToToD|SkillCreditsAvail|SkillLevelReq|SlayerSpecies|Slot|SpecialProps|Species|SpellCount|Spellcraft|StackCount|StackMax|SummoningGemBuffed|SummoningGemLevel|TotalValue|Type|Unenchantable|Unknown10|Unknown100000|Unknown800000|Unknown8000000|UsageMask|UsesRemaining|UsesTotal|Value|VitalityRating|WandElemDmgType|WeaponMasteryCategory|WeapSpeed|Wielder|WieldingSlot|WieldReqAttribute|WieldReqType|WieldReqValue|Workmanship|XPForVPReduction)";
		//private const string _L_M_ = @"M_(DateBorn|FellowshipName|FullDescription|ImbuedBy|InscribedBy|Inscription|LastTinkeredBy|MonarchName|Name|OnlyActivatedBy|Patron|PortalDestination|SecondaryName|SimpleDescription|Title|UsageInstructions)";
		//private const string _L_C_ = @"C_(Armor|BaseAlchemy|BaseCooking|BaseFletching|Book|Bundle|Clothing|Container|Corpse|CraftedAlchemy|CraftedCooking|CraftedFletching|Door|Foci|Food|Gem|HealingKit|Housing|Jewelry|Journal|Key|Lifestone|Lockpick|ManaStone|MeleeWeapon|Misc|MissileWeapon|Money|Monster|Npc|Plant|Player|Portal|Salvage|Scroll|Services|Sign|SpellComponent|TradeNote|Unknown|Ust|Vendor|WandStaffOrb)";
		//private const string _L_S_ = @"S_(Alchemy|ArcaneLore|ArmorTinkering|AssessCreature|AssessPerson|Axe|Bow|Cooking|CreatureEnchantment|Crossbow|Dagger|Deception|DirtyFighting|DualWield|FinesseWeapons|Fletching|Gearcraft|Healing|HeavyWeapons|ItemEnchantment|ItemTinkering|Jump|Leadership|LifeMagic|LightWeapons|Lockpick|Loyalty|Mace|MagicDefense|MagicItemTinkering|ManaConversion|MeleeDefense|MissileDefense|MissileWeapons|Recklessness|Run|Salvaging|Shield|SneakAttack|Spear|Staff|Summoning|Sword|ThrownWeapons|TwoHandedCombat|Unarmed|VoidMagic|WarMagic|WeaponTinkering)";

		private static string _L_A_ = String.Join("|", E.ArmorType.Keys);
        private static string _L_D_ = String.Join("|", E.DoubleActsOn.Keys);
        private static string _L_L_ = String.Join("|", E.LongActsOn.Keys);
        private static string _L_M_ = String.Join("|", E.MatchActsOn.Keys);
        private static string _L_C_ = String.Join("|", E.ObjectClass.Keys);
        private static string _L_S_ = String.Join("|", E.Skill.Keys);

        // regex patterns for different parts of the MUT grammar (line lead-in tags / requirement types / salvage types)
        internal static Dictionary<string, Regex> getLeadIn = new() {
			   ["OnNoSalvage"] = new Regex(@"^(?<type>ON\:|NO\:|SALVAGE\:)", RegexOptions.Compiled),
            ["AnyRequirement"] = new Regex(@"^\t(?<req>" + String.Join("|", Enum.GetNames(typeof(E.Requirement)).Where(nm => nm != "Disabled")) + ")", RegexOptions.Compiled),
                ["AnySalvage"] = new Regex(@"^\t(?<salv>" + String.Join("|", E.Salvage.Keys) + ")", RegexOptions.Compiled)
            //["AnyRequirement"] = new Regex(@"^\t(?<req>ArmorColorLike|BaseSkillRange|BuffedSkillGE|CalcedBuffedMedianDmgGE|CalcedBuffedMissileDmgGE|CalcedBuffedTinkDmgGE|CalcedBuffedTinkTarget|CalcedTotalRatingsGE|CharLevelGE|CharLevelLE|ColorLike|DKeyBuffedGE|DKeyGE|DKeyLE|DmgPercentGE|EmptyMainPackSlotsGE|LKeyBuffedGE|LKeyE|LKeyFlags|LKeyGE|LKeyLE|LKeyNE|MatchRx|MinDmgGE|NSpellsGE|NSpellsRxGE|ObjClass|SlotColorLike|SlotPalette|SpellRx)", RegexOptions.Compiled),
			    //["AnySalvage"] = new Regex(@"^\t(?<salv>V_(Agate|Alabaster|Amber|Amethyst|Aquamarine|ArmoredilloHide|Azurite|BlackGarnet|BlackOpal|Bloodstone|Brass|Bronze|Carnelian|Ceramic|Citrine|Copper|Diamond|Ebony|Emerald|FireOpal|Gold|Granite|GreenGarnet|GreenJade|GromnieHide|Hematite|ImperialTopaz|Iron|Ivory|Jet|LapisLazuli|LavenderJade|Leather|Linen|Mahogany|Malachite|Marble|Moonstone|Oak|Obsidian|Onyx|Opal|Peridot|Pine|Porcelain|Pyreal|RedGarnet|RedJade|ReedSharkHide|RoseQuartz|Ruby|Sandstone|Satin|Serpentine|Silk|Silver|SmokeyQuartz|Steel|Sunstone|Teak|TigerEye|Tourmaline|Turquoise|Velvet|WhiteJade|WhiteQuartz|WhiteSapphire|Sapphire|Wool|YellowGarnet|YellowTopaz|Zircon))", RegexOptions.Compiled)
        };

        // regex patterns for specific grammar elements, to pull out their arguments
        internal static Dictionary<string, Regex> getArgs = new() {
			["ON:"] = new Regex(@"^\s+(?<s>" + _S + @")\s+(?<s2>" + _S + @")$", RegexOptions.Compiled),
			//["NO:"] = new Regex(@"^\s+(?<s>" + _S + @")\s+(?<s2>" + _S + @")$", RegexOptions.Compiled),
			     //["RuleAction"] = new Regex(@"^(?<s>Keep|Salvage|Sell|Read)$", RegexOptions.Compiled),
			     ["RuleAction"] = new Regex(@"^(?<s>" + String.Join("|", Enum.GetNames(typeof(E.Action)).Where(nm => nm != "KeepN")) + ")$", RegexOptions.Compiled),
			["RuleActionKeepN"] = new Regex(@"^Keep\s+(?<i>" + _I + @")$", RegexOptions.Compiled), // maps to KeepN
			       ["SALVAGE:"] = new Regex(@"^\s+(?<s>" + _S + ")$", RegexOptions.Compiled),

			          ["ArmorColorLike"] = new Regex(@"^\s+(?<l>" + _L_A_ + @")\s+(?<h>" + _H + @")\s+(?<i>" + _I + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			          ["BaseSkillRange"] = new Regex(@"^\s+(?<l>" + _L_S_ + @")\s+(?<i>" + _I + @")\s+(?<i2>" + _I + @")$", RegexOptions.Compiled),
			           ["BuffedSkillGE"] = new Regex(@"^\s+(?<l>" + _L_S_ + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			 ["CalcedBuffedMedianDmgGE"] = new Regex(@"^\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			["CalcedBuffedMissileDmgGE"] = new Regex(@"^\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			   ["CalcedBuffedTinkDmgGE"] = new Regex(@"^\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			  ["CalcedBuffedTinkTarget"] = new Regex(@"^\s+(?<d>" + _D + @")\s+(?<d2>" + _D + @")\s+(?<d3>" + _D + @")$", RegexOptions.Compiled),
			    ["CalcedTotalRatingsGE"] = new Regex(@"^\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			             ["CharLevelGE"] = new Regex(@"^\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			             ["CharLevelLE"] = new Regex(@"^\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			               ["ColorLike"] = new Regex(@"^\s+(?<h>" + _H + @")\s+(?<i>" + _I + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			            ["DKeyBuffedGE"] = new Regex(@"^\s+(?<l>" + _L_D_ + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			                  ["DKeyGE"] = new Regex(@"^\s+(?<l>" + _L_D_ + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			                  ["DKeyLE"] = new Regex(@"^\s+(?<l>" + _L_D_ + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			            ["DmgPercentGE"] = new Regex(@"^\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			    ["EmptyMainPackSlotsGE"] = new Regex(@"^\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			            ["LKeyBuffedGE"] = new Regex(@"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			                   ["LKeyE"] = new Regex(@"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			               ["LKeyFlags"] = new Regex(@"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			                  ["LKeyGE"] = new Regex(@"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			                  ["LKeyLE"] = new Regex(@"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			                  ["LKeyNE"] = new Regex(@"^\s+(?<l>" + _L_L_ + @")\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			                 ["MatchRx"] = new Regex(@"^\s+(?<l>" + _L_M_ + @")\s+(?<s>" + _S + @")$", RegexOptions.Compiled),
			                ["MinDmgGE"] = new Regex(@"^\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			               ["NSpellsGE"] = new Regex(@"^\s+(?<i>" + _I + @")$", RegexOptions.Compiled),
			             ["NSpellsRxGE"] = new Regex(@"^\s+(?<i>" + _I + @")\s+(?<s>" + _S + @")\s+(?<s2>" + _S + @")$", RegexOptions.Compiled),
			                ["ObjClass"] = new Regex(@"^\s+(?<l>" + _L_C_ + @")$", RegexOptions.Compiled),
			             ["SlotPalette"] = new Regex(@"^\s+(?<i>" + _I + @")\s+(?<i2>" + _I + @")$", RegexOptions.Compiled),
			           ["SlotColorLike"] = new Regex(@"^\s+(?<i>" + _I + @")\s+(?<h>" + _H + @")\s+(?<i2>" + _I + @")\s+(?<d>" + _D + @")$", RegexOptions.Compiled),
			                 ["SpellRx"] = new Regex(@"^\s+(?<s>" + _S + @")$", RegexOptions.Compiled),
			//["Disabled"] = //

			["V_Salvage"] = new Regex(@"^\s+(?<s>" + _S + @")(\s+(?<v>" + _S + @"))?$", RegexOptions.Compiled)
		};
    }
}
