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
	// raw text strings for communicating information to myutilootor users (both on the command line and inside MUT files)
    internal class TextForUsers {
		// info for user messages, for describing the basic data types
        internal static readonly Dictionary<string, string> typeInfo = new()
        {
            ["_D"] = "Doubles are decimal numbers.",
            ["_I"] = "Integers are whole numbers.",
            ["_S"] = "Strings must be enclosed in " + RE.oD + (RE.oD.CompareTo(RE.cD) != 0 ? " " + RE.cD : "") + @" delimiters; any inside them must be escaped by doubling, i.e., single " + RE.oD + @" is not allowed inside myutilootor strings, and " + RE.oD + RE.oD + @" in myutilootor results in " + RE.oD + @" in met" + (RE.oD.CompareTo(RE.cD) != 0 ? @" (same for " + RE.cD + ")" : "") + @". Different strings require at least one whitespace character between their delimiters, separating them.",
            //_H omitted
            ["_L"] = "A literal starts with a letter or underscore, followed by letters, digits, or underscores; no whitespace, and no string delimiters (" + RE.oD + (RE.oD.CompareTo(RE.cD) != 0 ? RE.cD : "") + ")!"
        };

        // info for user messages, for describing details about the requirement types, as well as info on ON/NO/SALVAGE line lead-ins, a generic error message, and salvage syntax guidance
        internal static Dictionary<string, string> getInfo = new() { // Dictionary<string, string>()
			["ON:"] = "Required: 'ON:' (or 'NO:') must be at the start of the line, followed by a string rule name and a string action to take if the rule evaluates true. The action may be Keep, Salvage, Sell, Read, or Keep #, where # is an integer amount, e.g., {Keep 3}. (" + typeInfo["_S"] + ") Every rule may contain zero or more requirements.",
//			["NO:"] = "Required: 'NO:' must be at the start of the line, followed by a string rule name. (" + typeInfo["_S"] + ") Every rule may contain zero or more requirements.",
			["SALVAGE:"] = "Required: 'SALVAGE:' must be at the start of the line, followed by a string defining the Default salvage combination rule. E.g., {1-6, 7-8, 9, 10}. (" + typeInfo["_S"] + ")",

			["Generic"] = "Syntax error. (General tips: double-check tabbing and rule structure, ensure there are no stray characters or typo'd requirement keywords, and that you're using " + RE.oD + (RE.oD.CompareTo(RE.cD) != 0 ? " " + RE.cD : "") + @" string delimiters properly.)",

			          ["ArmorColorLike"] = "'ArmorColorLike' requires four inputs: a literal armor-type/color-region, a six-digit hex color (000000-FFFFFF), an integer hue (0-255), and a double saturation/value ratio (0.0-1.0). (" + typeInfo["_D"] + ") The literal must be one of the following:\n" + A_ActsOnTable,
			          ["BaseSkillRange"] = "'BaseSkillRange' requires three inputs: a literal skill-type, an integer minimum skill-level, and an integer maximum skill-level. (" + typeInfo["_I"] + ") The literal must be one of the following:\n" + S_ActsOnTable,
			           ["BuffedSkillGE"] = "'BuffedSkillGE' requires two inputs: a literal skill-type and a double skill-level. (" + typeInfo["_D"] + ") The literal must be one of the following:\n" + S_ActsOnTable,
			 ["CalcedBuffedMedianDmgGE"] = "'CalcedBuffedMedianDmgGE' requires one input: a double median-damage. (" + typeInfo["_D"] + ")",
			["CalcedBuffedMissileDmgGE"] = "'CalcedBuffedMissileDmgGE' requires one input: a double missile-damage. (" + typeInfo["_D"] + ")",
			   ["CalcedBuffedTinkDmgGE"] = "'CalcedBuffedTinkDmgGE' requires one input: a double tinkered-damage. (" + typeInfo["_D"] + ")",
			  ["CalcedBuffedTinkTarget"] = "'CalcedBuffedTinkTarget' requires three inputs: a double target-calced-buffed, a double target-buffed-melee, and a double target-buffed-attack. (" + typeInfo["_D"] + ")",
			    ["CalcedTotalRatingsGE"] = "'CalcedTotalRatingsGE' requires one input: a double total-ratings. (" + typeInfo["_D"] + ")",
			             ["CharLevelGE"] = "'CharLevelGE' requires one input: an integer character-level.",
			             ["CharLevelLE"] = "'CharLevelLE' requires one input: an integer character-level.",
			               ["ColorLike"] = "'ColorLike' requires three inputs: a six-digit hex color (000000-FFFFFF), an integer hue (0-255), and a double saturation/value ratio (0.0-1.0). (" + typeInfo["_D"] + ")",
			            ["DKeyBuffedGE"] = "'DKeyBuffedGE' requires two inputs: a literal 'acts on' specifier, and a double value. (" + typeInfo["_D"] + ") The literal must be one of the following items that is marked with an asterisk:\n" + D_ActsOnTable,
			                  ["DKeyGE"] = "'DKeyGE' requires two inputs: a literal 'acts on' specifier, and a double value. (" + typeInfo["_D"] + ") The literal must be one of the following:\n" + D_ActsOnTable,
			                  ["DKeyLE"] = "'DKeyLE' requires two inputs: a literal 'acts on' specifier, and a double value. (" + typeInfo["_D"] + ") The literal must be one of the following:\n" + D_ActsOnTable,
			            ["DmgPercentGE"] = "'DmgPercentGE' requires one input: a double damage-percent. (" + typeInfo["_D"] + ")",
			    ["EmptyMainPackSlotsGE"] = "'EmptyMainPackSlotsGE' requires one input: an integer value.",
			            ["LKeyBuffedGE"] = "'LKeyBuffedGE' requires two inputs: a literal 'acts on' specifier, and an integer value. The literal must be one of the following items that is marked with an asterisk:\n" + L_ActsOnTable,
			                   ["LKeyE"] = "'LKeyE' requires two inputs: a literal 'acts on' specifier, and an integer value. The literal must be one of the following:\n" + L_ActsOnTable,
			               ["LKeyFlags"] = "'LKeyFlags' requires two inputs: a literal 'acts on' specifier, and an integer value. The literal must be one of the following:\n" + L_ActsOnTable,
			                  ["LKeyGE"] = "'LKeyGE' requires two inputs: a literal 'acts on' specifier, and an integer value. The literal must be one of the following:\n" + L_ActsOnTable,
			                  ["LKeyLE"] = "'LKeyLE' requires two inputs: a literal 'acts on' specifier, and an integer value. The literal must be one of the following:\n" + L_ActsOnTable,
			                  ["LKeyNE"] = "'LKeyNE' requires two inputs: a literal 'acts on' specifier, and an integer value. The literal must be one of the following:\n" + L_ActsOnTable,
			                 ["MatchRx"] = "'MatchRx' requires two inputs: a literal 'acts on' specifier, and a string regex. (" + typeInfo["_S"] + ") The literal must be one of the following:\n" + M_ActsOnTable,
			                ["MinDmgGE"] = "'MinDmgGE' requires one input: a double minimum-damage. (" + typeInfo["_D"] + ")",
			               ["NSpellsGE"] = "'NSpellsGE' requires one input: an integer spell-count.",
			             ["NSpellsRxGE"] = "'NSpellsRxGE' requires three inputs: an integer spell-count, a string regex, and another string regex (not to match). (" + typeInfo["_S"] + ")",
			                ["ObjClass"] = "'ObjClass' requires one input: a literal class-specifier. The literal must be one of the following:\n" + C_ActsOnTable,
			             ["SlotPalette"] = "'SlotPalette' requires two inputs: an integer palette-entry-number, and an integer palette-ID.",
			           ["SlotColorLike"] = "'SlotColorLike' requires four inputs: an integer palette-entry-number, a six-digit hex color (000000-FFFFFF), an integer hue (0-255), and a double saturation/value ratio (0.0-1.0). (" + typeInfo["_D"] + ")",
			                 ["SpellRx"] = "'SpellRx' requires one input: a string regex. (" + typeInfo["_S"] + ")",
			//["Disabled"] = //

			["V_Salvage"] = "Every salvage combination definition requires one input, with an optional second input: a string combination-rule, a string value-combination-rule. (" + typeInfo["_S"] + ") Salvage-types cannot be defined multiple times; they must be unique or else absent (thereby falling back on the Default combination rule). The Default combination rule has no value-based component."
		};

        #region MUT file header text
        internal const string header =
@"~~ {
~~ Salvage: V_Agate V_Alabaster V_Amber V_Amethyst V_Aquamarine V_ArmoredilloHide V_Azurite V_BlackGarnet V_BlackOpal V_Bloodstone V_Brass V_Bronze V_Carnelian V_Ceramic V_Citrine V_Copper V_Diamond V_Ebony V_Emerald V_FireOpal V_Gold V_Granite V_GreenGarnet V_GreenJade V_GromnieHide V_Hematite V_ImperialTopaz V_Iron V_Ivory V_Jet V_LapisLazuli V_LavenderJade V_Leather V_Linen V_Mahogany V_Malachite V_Marble V_Moonstone V_Oak V_Obsidian V_Onyx V_Opal V_Peridot V_Pine V_Porcelain V_Pyreal V_RedGarnet V_RedJade V_ReedSharkHide V_RoseQuartz V_Ruby V_Sandstone V_Sapphire V_Satin V_Serpentine V_Silk V_Silver V_SmokeyQuartz V_Steel V_Sunstone V_Teak V_TigerEye V_Tourmaline V_Turquoise V_Velvet V_WhiteJade V_WhiteQuartz V_WhiteSapphire V_Wool V_YellowGarnet V_YellowTopaz V_Zircon
~~
~~ ArmorColorLike ActsOn: A_None A_Amuli_Coat_Chest A_Amuli_Coat_CollarShoulder A_Amuli_Coat_ArmsTrim A_Amuli_Legs_Base A_Amuli_Legs_Trim A_Celdon_Base A_Celdon_Veins A_Chiran_Coat_BaseArms A_Chiran_Coat_Stripes A_Chiran_Legs_Girth A_Chiran_Legs_Legs A_Chiran_Legs_Trim A_Chiran_Helm_Horns A_Chiran_Helm_Base A_Haebrean_BP_Chest A_Haebrean_BP_Ornaments A_Haebrean_BP_Trim A_Haebrean_Girth_Base A_Haebrean_Girth_BeltScales A_Haebrean_Helm_Base A_Haebrean_Helm_Mask A_Haebrean_Pauldrons_Base A_Haebrean_Pauldrons_Ornaments A_Lorica_BP_Veins A_Lorica_BP_Base A_Lorica_BP_NeckTrim A_Lorica_Legs_Base A_Lorica_Legs_KneesBeltCrotch A_Lorica_Legs_Legs A_Nariyid_BP_CircleLines A_Nariyid_BP_Base A_Nariyid_BP_Shoulders A_Nariyid_Girth_Base A_Nariyid_Girth_BeltLines A_Nariyid_Girth_Ornaments A_Nariyid_Sleeves_Shoulders A_Nariyid_Sleeves_UpperArm A_Nariyid_Sleeves_LowerArm A_Olthoi_BP_Base A_Olthoi_BP_Veins A_Olthoi_Alduressa_Legs_GirthBase A_Olthoi_Alduressa_Legs_GirthLines A_Olthoi_Alduressa_Legs_LegsLines A_Olthoi_Amuli_Coat_Base A_Olthoi_Amuli_Coat_Trim A_Olthoi_Amuli_Coat_Shoulders A_Olthoi_Amuli_Legs_Trim A_Olthoi_Koujia_Kabuton_Base A_Olthoi_Koujia_Kabuton_Horns A_Olthoi_Koujia_Legs_Base A_Olthoi_Koujia_Legs_SidesShins A_Scalemail_Cuirass_Base A_Scalemail_Cuirass_Bumps A_Scalemail_Cuirass_Belt A_Tenassa_Legs_LineAtSide A_Tenassa_Legs_Base A_Tenassa_Legs_Highlight A_Tenassa_BP_Shoulders A_Tenassa_BP_Base A_Yoroi_Cuirass_Base A_Yoroi_Cuirass_Belt A_Yoroi_Girth_Base A_Yoroi_Girth_Belt
~~ 
~~ MatchRx ActsOn: M_DateBorn M_FellowshipName M_FullDescription M_ImbuedBy M_InscribedBy M_Inscription M_LastTinkeredBy M_MonarchName M_Name M_OnlyActivatedBy M_Patron M_PortalDestination M_SecondaryName M_SimpleDescription M_Title M_UsageInstructions
~~
~~ ObjClass ActsOn: C_Armor C_BaseAlchemy C_BaseCooking C_BaseFletching C_Book C_Bundle C_Clothing C_Container C_Corpse C_CraftedAlchemy C_CraftedCooking C_CraftedFletching C_Door C_Foci C_Food C_Gem C_HealingKit C_Housing C_Jewelry C_Journal C_Key C_Lifestone C_Lockpick C_ManaStone C_MeleeWeapon C_Misc C_MissileWeapon C_Money C_Monster C_Npc C_Plant C_Player C_Portal C_Salvage C_Scroll C_Services C_Sign C_SpellComponent C_TradeNote C_Unknown C_Ust C_Vendor C_WandStaffOrb
~~ 
~~ LKey* ActsOn: L_ActivationReqSkillId L_ActiveSpellCount L_AffectsVitalAmt L_AffectsVitalId L_Age L_ArmorLevel L_ArmorSetID L_AssociatedSpell L_Attuned L_Behavior L_Bonded L_Burden L_Category L_CloakChanceType L_Container L_CooldownSeconds L_Coverage L_CreateFlags1 L_CreateFlags2 L_CreatureLevel L_CritDamRating L_CritDamResistRating L_CritRating L_CritResistRating L_CurrentMana L_DamageType L_DamRating L_DamResistRating L_DateOfBirth L_Deaths L_DescriptionFormat L_ElementalDmgBonus L_EquipableSlots L_EquippedSlots L_EquipSkill L_EquipType L_FishingSkill L_Flags L_GemSettingQty L_GemSettingType L_Gender L_HealBoostRating L_Heritage L_HookMask L_HookType L_HouseOwner L_Icon L_IconOutline L_IconOverlay L_IconUnderlay L_Imbued L_ItemMaxLevel L_ItemSlots L_KeysHeld L_Landblock L_LockpickDifficulty L_LockpickSkillBonus L_LoreRequirement L_MagicDef L_ManaCost L_Material L_MaxDamage L_MaximumMana L_MaxLevelRestrict L_MinLevelRestrict L_MissileType L_Model L_Monarch L_NumberFollowers L_NumberItemsSalvaged L_NumberTimesTinkered L_PackSlots L_PagesTotal L_PagesUsed L_PhysicsDataFlags L_PortalRestrictions L_Rank L_RankRequirement L_RareId L_RestrictedToToD L_SkillCreditsAvail L_SkillLevelReq L_SlayerSpecies L_Slot L_SpecialProps L_Species L_SpellCount L_Spellcraft L_StackCount L_StackMax L_SummoningGemBuffed L_SummoningGemLevel L_TotalValue L_Type L_Unenchantable L_Unknown10 L_Unknown100000 L_Unknown800000 L_Unknown8000000 L_UsageMask L_UsesRemaining L_UsesTotal L_Value L_VitalityRating L_WandElemDmgType L_WeaponMasteryCategory L_WeapSpeed L_Wielder L_WieldingSlot L_WieldReqAttribute L_WieldReqType L_WieldReqValue L_Workmanship L_XPForVPReduction
~~ 
~~ DKey* ActsOn: D_AcidProt D_ApproachDistance D_AttackBonus D_BludgeonProt D_ColdProt D_DamageBonus D_ElementalDamageVersus D_FireProt D_Heading D_HealingKitRestoreBonus D_LightningProt D_MagicDBonus D_ManaCBonus D_ManaRateOfChange D_ManaStoneChanceDestruct D_ManaTransferEfficiency D_MeleeDefenseBonus D_MissileDBonus D_PierceProt D_Range D_SalvageWorkmanship D_Scale D_SlashProt D_Variance
~~
~~ *Skill* ActsOn: S_Alchemy S_ArcaneLore S_ArmorTinkering S_AssessCreature S_AssessPerson S_Axe S_Bow S_Cooking S_CreatureEnchantment S_Crossbow S_Dagger S_Deception S_DirtyFighting S_DualWield S_FinesseWeapons S_Fletching S_Gearcraft S_Healing S_HeavyWeapons S_ItemEnchantment S_ItemTinkering S_Jump S_Leadership S_LifeMagic S_LightWeapons S_Lockpick S_Loyalty S_Mace S_MagicDefense S_MagicItemTinkering S_ManaConversion S_MeleeDefense S_MissileDefense S_MissileWeapons S_Recklessness S_Run S_Salvaging S_Shield S_SneakAttack S_Spear S_Staff S_Summoning S_Sword S_ThrownWeapons S_TwoHandedCombat S_Unarmed S_VoidMagic S_WarMagic S_WeaponTinkering
~~ }
~~ {																																		
~~ File auto-generated by myutilootor, a program created by Eskarina of Morningthaw/Coldeve.												
~~																																			
~~ REQUIREMENT KEYWORDS:		(See myutilootorReference.mut for more.)																	
~~																																			
~~		MatchRx			LKeyBuffedGE		DKeyBuffedGE		CalcedBuffedMedianDmgGE			BaseSkillRange			ArmorColorLike		
~~		ObjClass		LKeyE				DKeyGE				CalcedBuffedMissileDmgGE		BuffedSkillGE			ColorLike			
~~						LKeyGE				DKeyLE				CalcedBuffedTinkDmgGE			CharLevelGE				SlotPalette			
~~		NSpellsGE		LKeyFlags								CalcedBuffedTinkTarget			CharLevelLE				SlotColorLike		
~~		NSpellsRxGE		LKeyLE				DmgPercentGE		CalcedTotalRatingsGE			EmptyMainPackSlotsGE						
~~		SpellRx			LKeyNE				MinDmgGE																						
~~ }																																		

";
        #endregion
        #region armor "acts on" table
        internal const string A_ActsOnTable =
@"    A_None                       A_Haebrean_BP_Ornaments         A_Nariyid_BP_Shoulders              A_Olthoi_Koujia_Kabuton_Base
    A_Amuli_Coat_Chest           A_Haebrean_BP_Trim              A_Nariyid_Girth_Base                A_Olthoi_Koujia_Kabuton_Horns
    A_Amuli_Coat_CollarShoulder  A_Haebrean_Girth_Base           A_Nariyid_Girth_BeltLines           A_Olthoi_Koujia_Legs_Base
    A_Amuli_Coat_ArmsTrim        A_Haebrean_Girth_BeltScales     A_Nariyid_Girth_Ornaments           A_Olthoi_Koujia_Legs_SidesShins
    A_Amuli_Legs_Base            A_Haebrean_Helm_Base            A_Nariyid_Sleeves_Shoulders         A_Scalemail_Cuirass_Base
    A_Amuli_Legs_Trim            A_Haebrean_Helm_Mask            A_Nariyid_Sleeves_UpperArm          A_Scalemail_Cuirass_Bumps
    A_Celdon_Base                A_Haebrean_Pauldrons_Base       A_Nariyid_Sleeves_LowerArm          A_Scalemail_Cuirass_Belt
    A_Celdon_Veins               A_Haebrean_Pauldrons_Ornaments  A_Olthoi_BP_Base                    A_Tenassa_Legs_LineAtSide
    A_Chiran_Coat_BaseArms       A_Lorica_BP_Veins               A_Olthoi_BP_Veins                   A_Tenassa_Legs_Base
    A_Chiran_Coat_Stripes        A_Lorica_BP_Base                A_Olthoi_Alduressa_Legs_GirthBase   A_Tenassa_Legs_Highlight
    A_Chiran_Legs_Girth          A_Lorica_BP_NeckTrim            A_Olthoi_Alduressa_Legs_GirthLines  A_Tenassa_BP_Shoulders
    A_Chiran_Legs_Legs           A_Lorica_Legs_Base              A_Olthoi_Alduressa_Legs_LegsLines   A_Tenassa_BP_Base
    A_Chiran_Legs_Trim           A_Lorica_Legs_KneesBeltCrotch   A_Olthoi_Amuli_Coat_Base            A_Yoroi_Cuirass_Base
    A_Chiran_Helm_Horns          A_Lorica_Legs_Legs              A_Olthoi_Amuli_Coat_Trim            A_Yoroi_Cuirass_Belt
    A_Chiran_Helm_Base           A_Nariyid_BP_CircleLines        A_Olthoi_Amuli_Coat_Shoulders       A_Yoroi_Girth_Base
    A_Haebrean_BP_Chest          A_Nariyid_BP_Base               A_Olthoi_Amuli_Legs_Trim            A_Yoroi_Girth_Belt
";
        #endregion
        #region double "acts on" table
        internal const string D_ActsOnTable =
@"    D_AcidProt             D_ElementalDamageVersus *     D_ManaCBonus *                D_PierceProt
    D_ApproachDistance     D_FireProt                    D_ManaRateOfChange            D_Range
    D_AttackBonus *        D_Heading                     D_ManaStoneChanceDestruct     D_SalvageWorkmanship
    D_BludgeonProt         D_HealingKitRestoreBonus      D_ManaTransferEfficiency      D_Scale
    D_ColdProt             D_LightningProt               D_MeleeDefenseBonus *         D_SlashProt
    D_DamageBonus          D_MagicDBonus                 D_MissileDBonus               D_Variance
";
        #endregion
        #region long "acts on" table
        internal const string L_ActsOnTable =
@"    L_ActivationReqSkillId     L_Deaths                 L_MagicDef                L_Spellcraft
    L_ActiveSpellCount         L_DescriptionFormat      L_ManaCost                L_StackCount
    L_AffectsVitalAmt          L_ElementalDmgBonus      L_Material                L_StackMax
    L_AffectsVitalId           L_EquipableSlots         L_MaxDamage *             L_SummoningGemBuffed
    L_Age                      L_EquippedSlots          L_MaximumMana             L_SummoningGemLevel
    L_ArmorLevel *             L_EquipSkill             L_MaxLevelRestrict        L_TotalValue
    L_ArmorSetID               L_EquipType              L_MinLevelRestrict        L_Type
    L_AssociatedSpell          L_FishingSkill           L_MissileType             L_Unenchantable
    L_Attuned                  L_Flags                  L_Model                   L_Unknown10
    L_Behavior                 L_GemSettingQty          L_Monarch                 L_Unknown100000
    L_Bonded                   L_GemSettingType         L_NumberFollowers         L_Unknown800000
    L_Burden                   L_Gender                 L_NumberItemsSalvaged     L_Unknown8000000
    L_Category                 L_HealBoostRating        L_NumberTimesTinkered     L_UsageMask
    L_CloakChanceType          L_Heritage               L_PackSlots               L_UsesRemaining
    L_Container                L_HookMask               L_PagesTotal              L_UsesTotal
    L_CooldownSeconds          L_HookType               L_PagesUsed               L_Value
    L_Coverage                 L_HouseOwner             L_PhysicsDataFlags        L_VitalityRating
    L_CreateFlags1             L_Icon                   L_PortalRestrictions      L_WandElemDmgType
    L_CreateFlags2             L_IconOutline            L_Rank                    L_WeaponMasteryCategory
    L_CreatureLevel            L_IconOverlay            L_RankRequirement         L_WeapSpeed
    L_CritDamRating            L_IconUnderlay           L_RareId                  L_Wielder
    L_CritDamResistRating      L_Imbued                 L_RestrictedToToD         L_WieldingSlot
    L_CritRating               L_ItemMaxLevel           L_SkillCreditsAvail       L_WieldReqAttribute
    L_CritResistRating         L_ItemSlots              L_SkillLevelReq           L_WieldReqType
    L_CurrentMana              L_KeysHeld               L_SlayerSpecies           L_WieldReqValue
    L_DamageType               L_Landblock              L_Slot                    L_Workmanship
    L_DamRating                L_LockpickDifficulty     L_SpecialProps            L_XPForVPReduction
    L_DamResistRating          L_LockpickSkillBonus     L_Species
    L_DateOfBirth              L_LoreRequirement        L_SpellCount
";
        #endregion
        #region string-match "acts on" table
        internal const string M_ActsOnTable =
@"    M_Name            M_FellowshipName        M_MonarchName           M_LastTinkeredBy
    M_Title           M_UsageInstructions     M_OnlyActivatedBy       M_ImbuedBy
    M_Inscription     M_SimpleDescription     M_Patron                M_DateBorn
    M_InscribedBy     M_FullDescription       M_PortalDestination     M_SecondaryName
";
        #endregion
        #region object-class "acts on" table
        internal const string C_ActsOnTable =
@"    C_Armor              C_CraftedFletching     C_Lockpick          C_Salvage
    C_BaseAlchemy        C_Door                 C_ManaStone         C_Scroll
    C_BaseCooking        C_Foci                 C_MeleeWeapon       C_Services
    C_BaseFletching      C_Food                 C_Misc              C_Sign
    C_Book               C_Gem                  C_MissileWeapon     C_SpellComponent
    C_Bundle             C_HealingKit           C_Money             C_TradeNote
    C_Clothing           C_Housing              C_Monster           C_Unknown
    C_Container          C_Jewelry              C_Npc               C_Ust
    C_Corpse             C_Journal              C_Plant             C_Vendor
    C_CraftedAlchemy     C_Key                  C_Player            C_WandStaffOrb
    C_CraftedCooking     C_Lifestone            C_Portal
";
        #endregion
        #region skill "acts on" table
        internal const string S_ActsOnTable =
@"    S_Alchemy                 S_DualWield           S_Lockpick               S_Shield
    S_ArcaneLore              S_FinesseWeapons      S_Loyalty                S_SneakAttack
    S_ArmorTinkering          S_Fletching           S_Mace                   S_Spear
    S_AssessCreature          S_Gearcraft           S_MagicDefense           S_Staff
    S_AssessPerson            S_Healing             S_MagicItemTinkering     S_Summoning
    S_Axe                     S_HeavyWeapons        S_ManaConversion         S_Sword
    S_Bow                     S_ItemEnchantment     S_MeleeDefense           S_ThrownWeapons
    S_Cooking                 S_ItemTinkering       S_MissileDefense         S_TwoHandedCombat
    S_CreatureEnchantment     S_Jump                S_MissileWeapons         S_Unarmed
    S_Crossbow                S_Leadership          S_Recklessness           S_VoidMagic
    S_Dagger                  S_LifeMagic           S_Run                    S_WarMagic
    S_Deception               S_LightWeapons        S_Salvaging              S_WeaponTinkering
    S_DirtyFighting
";
        #endregion
        #region myutilootor reference file text
        internal const string reference =
@"~~																																								
~~		myutilootor																																	Created by	
~~		 REFERENCE																																	 Eskarina	
~~																																								


PRONOUNCE 'myutilootor' like 'mutilator' except with the A like OO in 'loot'. It's a play on words, where 'my util lootor' is a loot-rule utility.


~~ GENERAL FORMAT																																				

Zero or more loot RULES are present first, followed by a SALVAGE combination-rule section. Each RULE starts with either an ON: or NO: label, where an ON: label
indicates a rule is currently active, and a NO: label that it is not. Following this start (on the same line) is a rule name inside curly braces, and then an
action specifier inside another set of curly braces, to indicate what to do if the rule evaluates to true. This second input may be any of the following:
    Keep        Salvage        Sell        Read       Keep #
That right-most option (Keep #) is just Keep and an integer separated by whitespace, all still inside the second set of curly braces.

Each rule is comprised of zero or more REQUIREMENTS, each tabbed in exactly once and occupying exactly one line (following the rule name line).
(See REQUIREMENTS DEFINITIONS below.)

The SALVAGE: label appears after all RULES have been fully defined, and it is followed (on the same line) by the default combination rule, inside curly braces,
then any combination rules for specific salvage types follow, with each being unique (cannot repeat), tabbed in exactly once, occupying its own line, and
followed by its combination rule within curly braces. An optional additional set of curly braces may be included to specify a value-based combination rule.

Note that if a rule requirement contains any highlighted words, that requirement necessitates a server check. (Highlighted word examples include D_FireProt and
CalcedBuffedMedianDmgGE.) Those requirements containing no highlighted words are evaluated client-side.

Example loot profile:

ON: {    (T) Aetheria (DFDdrp 5)} {Keep 3} ~~ {
	MatchRx M_Name {Aetheria}
	LKeyE L_IconOverlay 27704
	LKeyGE L_ArmorSetID 35
	LKeyLE L_ArmorSetID 37
	SpellRx {Surge of (Destruction|Regeneration|Protection)}
~~ }
ON: {    (T) Deck of Cards} {Keep} ~~ {
	ObjClass C_Misc
	MatchRx M_Name {of Eyes|of Hands|Jester's Token|The Jester}
~~ }
NO: {    (T) Level 8 Glyphs} {Sell} ~~ {
	MatchRx M_Name {Glyph of}
~~ }
ON: {(A - my) Armor (SolidRed)} {Keep} ~~ {
	MatchRx M_Name {Yoroi}
	SlotColorLike 0 B60000 10 0.1
	MatchRx M_Name {^((?!Alduressa).)*$}
	MatchRx M_Name {^((?!Amuli).)*$}
	MatchRx M_Name {^((?!Celdon).)*$}
	MatchRx M_Name {^((?!Covenant).)*$}
	MatchRx M_Name {^((?!Diforsa).)*$}
	MatchRx M_Name {^((?!Koujia).)*$}
	MatchRx M_Name {^((?!Basinet).)*$}
	MatchRx M_Name {^((?!Coif).)*$}
	MatchRx M_Name {^((?!Chainmail Gauntlets).)*$}
~~ }

SALVAGE: {1-6, 7-8, 9, 10} ~~ {
	V_RoseQuartz {1-10}
	V_BlackOpal {1-10}
	V_Zircon {1-10} {85000} ~~ includes a value-based combination rule
	V_Leather {1-10}
~~ }


~~ REQUIREMENTS DEFINITIONS																																		


	~~ Data-type abbreviations:			
		i - Integer. A whole number.
		h - Integer expressed in Hexidecimal.
		d - Double. A decimal number.
		r - RegEx. Same as S, but expected to be a Regular Expression.
		l - Literal. A chain of characters beginning with a letter or underscore, followed by zero or more letters,
			underscores, or digits. Note: cannot contain any whitespace.
		p - Operation (Condition or Action).


~~ Calced									

	CalcedTotalRatingsGE   d Value
		DETAILS: One input. True if calculated total ratings >= Value.
		EXAMPLE: CalcedTotalRatingsGE 42.2

	CalcedBuffedMedianDmgGE   d Value
		DETAILS: One input. True if calculated buffed median damage >= Value.
		EXAMPLE: CalcedBuffedMedianDmgGE 37.4

	CalcedBuffedMissileDmgGE   d Value
		DETAILS: One input. True if calculated buffed missile damage >= Value.
		EXAMPLE: CalcedBuffedMissileDmgGE 52.5

	CalcedBuffedTinkDmgGE   d Value
		DETAILS: One input. True if calculated buffed tinkered damage >= Value.
		EXAMPLE: CalcedBuffedTinkDmgGE 61.4

	CalcedBuffedTinkTarget   d CalcBuff   d BuffMelee   d BuffAttack
???		DETAILS: Three inputs. True if calculated buffed ?????.
		EXAMPLE: CalcedBuffedTinkTarget 39.1 428 513


~~ Character / Skills						

	BaseSkillRange   l Target   i Min   i Max
		DETAILS: Three inputs. True if character's Target skill lies between Min and Max. Valid Target values listed in Skills table in TARGET TABLES section.
		EXAMPLE: BaseSkillRange S_MeleeDefense 120 170

	BuffedSkillGE   l Target   d Value
		DETAILS: Two inputs. True if character's buffed Target skill >= Value. Valid Target values listed in Skills table in TARGET TABLES section.
		EXAMPLE: BuffedSkillGE S_HeavyWeapons 500

	CharLevelGE   i Value
		DETAILS: One input. True if character's level >= Value.
		EXAMPLE: CharLevelGE 100

	CharLevelLE   i Value
		DETAILS: One input. True if character's level <= Value.
		EXAMPLE: CharLevelLE 200

	EmptyMainPackSlotsGE   i Value
		DETAILS: One input. True if number of empty slots in character's main pack >= Value.
		EXAMPLE: EmptyMainPackSlotsGE 8


~~ Colors									

	ArmorColorLike   l Target   h HexColor   i Hue   d SV
		DETAILS: Four inputs. True if inspected item matches Target and is of a color within the parameters described below. Valid Target values listed in
			Armor Type/Color Region table in TARGET TABLES section.
			HexColor is a six-digit hexidecimal number representing a color (similar to HTML code).
			Hue is an integer from 0 to 255 that specifies an amount by which the inspected item's color may vary from HexColor and still match.
			SV is the Saturation/Value ratio between 0.0 and 1.0 within which the color may vary from HexColor and still match.
		EXAMPLE: ArmorColorLike A_Lorica_BP_Base 0FC36B 14 0.1

	ColorLike   h HexColor   i Hue   d SV
		DETAILS: Three inputs. True if inspected item is of a color within the parameters described below.
			HexColor is a six-digit hexidecimal number representing a color (similar to HTML code).
			Hue is an integer from 0 to 255 that specifies an amount by which the inspected item's color may vary from HexColor and still match.
			SV is the Saturation/Value ratio between 0.0 and 1.0 within which the color may vary from HexColor and still match.
		EXAMPLE: ColorLike 0FC36B 14 0.1

	SlotPalette   i PaletteN   i PaletteID
		DETAILS: Two inputs. True if inspected item is of a color within the parameters described below.
			PaletteN is the palette entry number.
			PaletteID is the palette ID.
		EXAMPLE: SlotPalette 4012 0FC36B

	SlotColorLike   i PaletteN   h HexColor   i Hue   d SV
		DETAILS: Four inputs. True if inspected item is of a color within the parameters described below.
			PaletteN is the palette entry number.
			HexColor is a six-digit hexidecimal number representing a color (similar to HTML code).
			Hue is an integer from 0 to 255 that specifies an amount by which the inspected item's color may vary from HexColor and still match.
			SV is the Saturation/Value ratio between 0.0 and 1.0 within which the color may vary from HexColor and still match.
		EXAMPLE: SlotColorLike 4012 0FC36B 14 0.1


~~ Double Value Key							

	DKeyBuffedGE   l Target   d Value
		DETAILS: Two inputs. True if buffed double-value-key Target >= Value. Valid Target values asterisked in Double Key Values table in TARGET TABLES section.
???		EXAMPLE: DKeyBuffedGE D_AttackBonus 0.25

	DKeyGE   l Target   d Value
		DETAILS: Two inputs. True if double-value-key Target >= Value. Valid Target values listed in Double Key Values table in TARGET TABLES section.
???		EXAMPLE: DKeyGE D_ManaCBonus 0.2

	DKeyLE   l Target   d Value
		DETAILS: Two inputs. True if double-value-key Target <= Value. Valid Target values listed in Double Key Values table in TARGET TABLES section.
???		EXAMPLE: DKeyLE D_Variance 0.4


~~ Long Value Key							

	LKeyBuffedGE   l Target   i Value
		DETAILS: Two inputs. True if buffed long-value-key Target >= Value. Valid Target values asterisked in Long Key Values table in TARGET TABLES section.
		EXAMPLE: LKeyBuffedGE L_ArmorLevel 400

	LKeyE   l Target   i Value
		DETAILS: Two inputs. True if long-value-key Target == Value. Valid Target values listed in Long Key Values table in TARGET TABLES section.
		EXAMPLE: LKeyE L_EquipSkill 80

???	LKeyFlags   l Target   i Value
		DETAILS: Two inputs. True if long-value-key Target has Value flags. Valid Target values listed in Long Key Values table in TARGET TABLES section.
		EXAMPLE: LKeyFlags L_EquippedSlots 4

	LKeyGE   l Target   i Value
		DETAILS: Two inputs. True if long-value-key Target >= Value. Valid Target values listed in Long Key Values table in TARGET TABLES section.
		EXAMPLE: LKeyGE L_MaximumMana 450

	LKeyLE   l Target   i Value
		DETAILS: Two inputs. True if long-value-key Target <= Value. Valid Target values listed in Long Key Values table in TARGET TABLES section.
		EXAMPLE: LKeyLE L_TotalValue 80000

	LKeyNE   l Target   i Value
		DETAILS: Two inputs. True if long-value-key Target != Value. Valid Target values listed in Long Key Values table in TARGET TABLES section.
		EXAMPLE: LKeyNE L_MinLevelRestrict 100


~~ Miscellaneous							

	DmgPercentGE   d Value
		DETAILS: One input. True if inspected item's DmgPercent >= Value.
		EXAMPLE: DmgPercentGE 17

	MinDmgGE   d Value
		DETAILS: One input. True if inspected item's minimum damage >= Value.
		EXAMPLE: MinDmgGE 18.4

	ObjClass   l Class
		DETAILS: One input. True if inspected item is of object class Class. Valid Class values listed in Object Classes table in TARGET TABLES section.
		EXAMPLE: ObjClass C_Plant


~~ Spells									

???	NSpellsGE   i Value
		DETAILS: One input. True if number of spells on inspected item >= Value.
		EXAMPLE: NSpellsGE 4

	NSpellsRxGE   i Value   {r Match}   {r NoMatch}
		DETAILS: Three inputs. Count how many spells on inspected item match regex Match while NOT matching regex NoMatch. True if that count >= Value.
		EXAMPLE: NSpellsRxGE 2 {Acid|Lightning|Fire} {Bane}

???	SpellRx   {r Match}
		DETAILS: One input. True if regex Match matches a spell's name on the inspected item.
		EXAMPLE: SpellRx {.* Bane}


~~ String Match								

	MatchRx   l Target   {r Match}
		DETAILS: Two inputs. True if Target matches regex Match. Valid Target values listed in Match Strings table in TARGET TABLES section.
		EXAMPLE: MatchRx M_ImbuedBy {^Machine.*$}


~~ TARGET TABLES																																				


~~ Armor Type/Color Region					

	Acted on by requirements: ArmorColorLike

" + A_ActsOnTable + @"

~~ Double Key Values						

	Acted on by requirements: DKeyGE, DKeyLE; also DKeyBuffedGE (*)

" + D_ActsOnTable + @"

~~ Long Key Values							

	Acted on by requirements: LKeyE, LKeyFlags, LKeyGE, LKeyLE, LKeyNE; also LKeyBuffedGE (*)

" + L_ActsOnTable + @"

~~ Match Strings 							

	Acted on by requirements: MatchRx

" + M_ActsOnTable + @"

~~ Object Classes							

	Acted on by requirements: ObjClass

" + C_ActsOnTable + @"

~~ Skills									

	Acted on by requirements: BaseSkillRange, BuffedSkillGE

" + S_ActsOnTable + @"
~~																																								
~~		myutilootor																																	Created by	
~~		 REFERENCE																																	 Eskarina	
~~																																								
";
		#endregion
    }
}
