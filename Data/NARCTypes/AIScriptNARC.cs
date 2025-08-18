using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewEditor.Data.NARCTypes
{
    public class AIScriptNARC : NARC
    {
        public List<AIScript> scripts;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            scripts = new List<AIScript>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                AIScript s = new AIScript(bytes);
                scripts.Add(s);

                pos += 8;
            }
        }

        public override void WriteData()
        {
            List<byte> newByteData = new List<byte>();
            List<byte> oldByteData = new List<byte>(byteData);

            newByteData.AddRange(oldByteData.GetRange(0, pointerStartAddress));
            newByteData.AddRange(oldByteData.GetRange(BTNFPosition, FileEntryStart - BTNFPosition));

            //Write Files
            int totalSize = 0;
            int pPos = pointerStartAddress;
            foreach (AIScript i in scripts)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += i.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (AIScript i in scripts)
            {
                newByteData.AddRange(i.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(scripts.Count);

            base.WriteData();
        }
    }

    public class AIScript
    {
        public static Dictionary<int, AIScriptCommandDefinition> commandDictionary = new Dictionary<int, AIScriptCommandDefinition>()
        {
            {0x00, new AIScriptCommandDefinition("If_Rand_LT", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x01, new AIScriptCommandDefinition("If_Rand_GT", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x02, new AIScriptCommandDefinition("If_Rand_EQ", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x03, new AIScriptCommandDefinition("If_Rand_NE", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x04, new AIScriptCommandDefinition("Add_To_Score", -1, ParamAlias.Integer) },
            {0x05, new AIScriptCommandDefinition("If_Health_LT", 2, ParamAlias.Position, ParamAlias.Integer, ParamAlias.Address) },
            {0x06, new AIScriptCommandDefinition("If_Health_GT", 2, ParamAlias.Position, ParamAlias.Integer, ParamAlias.Address) },
            {0x07, new AIScriptCommandDefinition("If_Health_EQ", 2, ParamAlias.Position, ParamAlias.Integer, ParamAlias.Address) },
            {0x08, new AIScriptCommandDefinition("If_Health_NE", 2, ParamAlias.Position, ParamAlias.Integer, ParamAlias.Address) },
            {0x09, new AIScriptCommandDefinition("If_Has_Any_Status", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x0A, new AIScriptCommandDefinition("If_Has_No_Status", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x0B, new AIScriptCommandDefinition("If_Has_Condition", 2, ParamAlias.Position, ParamAlias.Condition, ParamAlias.Address) },
            {0x0C, new AIScriptCommandDefinition("If_Not_Condition", 2, ParamAlias.Position, ParamAlias.Condition, ParamAlias.Address) },
            {0x0D, new AIScriptCommandDefinition("If_Badly_Poisoned", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x0E, new AIScriptCommandDefinition("If_Not_Badly_Poisoned", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x0F, new AIScriptCommandDefinition("If_Has_Condition_Flag", 2, ParamAlias.Position, ParamAlias.ConditionFlag, ParamAlias.Address) },
            {0x10, new AIScriptCommandDefinition("If_Not_Condition_Flag", 2, ParamAlias.Position, ParamAlias.ConditionFlag, ParamAlias.Address) },
            {0x11, new AIScriptCommandDefinition("If_Has_Side_Condition", 2, ParamAlias.Position, ParamAlias.SideCondition, ParamAlias.Address) },
            {0x12, new AIScriptCommandDefinition("If_Not_Side_Condition", 2, ParamAlias.Position, ParamAlias.SideCondition, ParamAlias.Address) },
            {0x13, new AIScriptCommandDefinition("If_Stored_LT", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x14, new AIScriptCommandDefinition("If_Stored_GT", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x15, new AIScriptCommandDefinition("If_Stored_EQ", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x16, new AIScriptCommandDefinition("If_Stored_NE", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x17, new AIScriptCommandDefinition("If_Stored_AND", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x18, new AIScriptCommandDefinition("If_Stored_NAND", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x19, new AIScriptCommandDefinition("If_Move_EQ", 1, ParamAlias.Move, ParamAlias.Address) },
            {0x1A, new AIScriptCommandDefinition("If_Move_NE", 1, ParamAlias.Move, ParamAlias.Address) },
            {0x1B, new AIScriptCommandDefinition("If_Stored_In_List", 1, ParamAlias.Address, ParamAlias.Address) },
            {0x1C, new AIScriptCommandDefinition("If_Stored_Not_In_List", 1, ParamAlias.Address, ParamAlias.Address) },
            {0x1D, new AIScriptCommandDefinition("If_Has_Damaging_Move", 0, ParamAlias.Address) },
            {0x1E, new AIScriptCommandDefinition("If_No_Damaging_Move", 0, ParamAlias.Address) },
            {0x1F, new AIScriptCommandDefinition("Get_Turn_Count", -1) { storedValue = ParamAlias.Integer } },
            {0x20, new AIScriptCommandDefinition("Get_Type", -1, ParamAlias.TypeParam) { storedValue = ParamAlias.Type } },
            {0x21, new AIScriptCommandDefinition("Get_Move_Base_Power", -1) { storedValue = ParamAlias.Integer } },
            {0x22, new AIScriptCommandDefinition("Get_Move_Damage", -1, ParamAlias.Integer) },
            {0x23, new AIScriptCommandDefinition("Get_Previous_Move", -1, ParamAlias.Position) {storedValue = ParamAlias.Move } },
            {0x24, new AIScriptCommandDefinition("If_Stored_EQ_2", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x25, new AIScriptCommandDefinition("If_Stored_NE_2", 1, ParamAlias.Stored, ParamAlias.Address) },
            {0x26, new AIScriptCommandDefinition("If_Compare_Speed", 1, ParamAlias.Comparison, ParamAlias.Address) },
            {0x27, new AIScriptCommandDefinition("Get_Party_Count", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x28, new AIScriptCommandDefinition("Get_Move", -1) { storedValue = ParamAlias.Move } },
            {0x29, new AIScriptCommandDefinition("Get_Move_Effect", -1) { storedValue = ParamAlias.MoveEffect } },
            {0x2A, new AIScriptCommandDefinition("Get_Ability_Guess", -1, ParamAlias.Position) { storedValue = ParamAlias.Ability } },
            {0x2B, new AIScriptCommandDefinition("c0x2B", -1) },
            {0x2C, new AIScriptCommandDefinition("If_Effectiveness_Eq", 1, ParamAlias.TypeEffectiveness, ParamAlias.Address) },
            {0x2D, new AIScriptCommandDefinition("If_Party_Has_Status", 1, ParamAlias.Condition, ParamAlias.Address) },
            {0x2E, new AIScriptCommandDefinition("If_Party_No_Status", 1, ParamAlias.Condition, ParamAlias.Address) },
            {0x2F, new AIScriptCommandDefinition("Get_Weather", -1) { storedValue = ParamAlias.Weather } },
            {0x30, new AIScriptCommandDefinition("If_Move_Effect_EQ", 1, ParamAlias.MoveEffect, ParamAlias.Address) },
            {0x31, new AIScriptCommandDefinition("If_Move_Effect_NE", 1, ParamAlias.MoveEffect, ParamAlias.Address) },
            {0x32, new AIScriptCommandDefinition("If_Stat_Stage_LT", 3, ParamAlias.Position, ParamAlias.Stat, ParamAlias.StatStage, ParamAlias.Address) },
            {0x33, new AIScriptCommandDefinition("If_Stat_Stage_GT", 3, ParamAlias.Position, ParamAlias.Stat, ParamAlias.StatStage, ParamAlias.Address) },
            {0x34, new AIScriptCommandDefinition("If_Stat_Stage_EQ", 3, ParamAlias.Position, ParamAlias.Stat, ParamAlias.StatStage, ParamAlias.Address) },
            {0x35, new AIScriptCommandDefinition("If_Stat_Stage_NE", 3, ParamAlias.Position, ParamAlias.Stat, ParamAlias.StatStage, ParamAlias.Address) },
            {0x36, new AIScriptCommandDefinition("If_Target_Will_Faint", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x37, new AIScriptCommandDefinition("If_Target_Will_Not_Faint", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x38, new AIScriptCommandDefinition("If_Has_Move", 2, ParamAlias.Position, ParamAlias.Move, ParamAlias.Address) },
            {0x39, new AIScriptCommandDefinition("If_Does_Not_Have_Move", 2, ParamAlias.Position, ParamAlias.Move, ParamAlias.Address) },
            {0x3A, new AIScriptCommandDefinition("If_Has_Move_With_Effect", 2, ParamAlias.Position, ParamAlias.MoveEffect, ParamAlias.Address) },
            {0x3B, new AIScriptCommandDefinition("If_No_Move_With_Effect", 2, ParamAlias.Position, ParamAlias.MoveEffect, ParamAlias.Address) },
            {0x3C, new AIScriptCommandDefinition("c0x3C", -1) },
            {0x3D, new AIScriptCommandDefinition("Flee", -1) },
            {0x3E, new AIScriptCommandDefinition("c0x3E", -1) },
            {0x3F, new AIScriptCommandDefinition("c0x3F", -1) },
            {0x40, new AIScriptCommandDefinition("Get_Held_Item", -1, ParamAlias.Position) { storedValue = ParamAlias.HeldItem } },
            {0x41, new AIScriptCommandDefinition("Get_Held_Item_Effect", -1, ParamAlias.Position) { storedValue = ParamAlias.HeldItemEffect } },
            {0x42, new AIScriptCommandDefinition("Get_Gender", -1, ParamAlias.Position) { storedValue = ParamAlias.Gender } },
            {0x43, new AIScriptCommandDefinition("Get_Is_First_Turn", -1, ParamAlias.Position) { storedValue = ParamAlias.Boolean } },
            {0x44, new AIScriptCommandDefinition("Get_Stockpile_Count", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x45, new AIScriptCommandDefinition("Get_Battle_Style", -1) { storedValue = ParamAlias.BattleStyle } },
            {0x46, new AIScriptCommandDefinition("Get_Battle_Type", -1) { storedValue = ParamAlias.BattleType } },
            {0x47, new AIScriptCommandDefinition("Get_Used_Item", -1, ParamAlias.Position) { storedValue = ParamAlias.HeldItem } },
            {0x48, new AIScriptCommandDefinition("c0x48", -1) },
            {0x49, new AIScriptCommandDefinition("Get_Power_Of_Stored_Move", -1) { storedValue = ParamAlias.Integer } },
            {0x4A, new AIScriptCommandDefinition("Get_Effect_Of_Stored_Move", -1) { storedValue = ParamAlias.MoveEffect } },
            {0x4B, new AIScriptCommandDefinition("Get_Protect_Count", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x4C, new AIScriptCommandDefinition("Jump", 0, ParamAlias.Address) },
            {0x4D, new AIScriptCommandDefinition("End_Script", -1) },
            {0x4E, new AIScriptCommandDefinition("If_Compare_Level", 1, ParamAlias.Comparison, ParamAlias.Address) },
            {0x4F, new AIScriptCommandDefinition("If_Taunted", 0, ParamAlias.Address) },
            {0x50, new AIScriptCommandDefinition("If_Not_Taunted", 0, ParamAlias.Address) },
            {0x51, new AIScriptCommandDefinition("If_Target_Is_Ally", 0, ParamAlias.Address) },
            {0x52, new AIScriptCommandDefinition("Get_Poke_Has_Type", -1, ParamAlias.Position, ParamAlias.Type) { storedValue = ParamAlias.Boolean } },
            {0x53, new AIScriptCommandDefinition("Get_Poke_Has_Ability", -1, ParamAlias.Position, ParamAlias.Ability) { storedValue = ParamAlias.Boolean } },
            {0x54, new AIScriptCommandDefinition("If_Flash_Fire_Active", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x55, new AIScriptCommandDefinition("If_Held_Item_EQ", 2, ParamAlias.Position, ParamAlias.HeldItem, ParamAlias.Address) },
            {0x56, new AIScriptCommandDefinition("If_Field_Effect_EQ", 1, ParamAlias.FieldEffect, ParamAlias.Address) },
            {0x57, new AIScriptCommandDefinition("Get_Side_Status_Level", -1, ParamAlias.FieldEffect, ParamAlias.SideCondition) { storedValue = ParamAlias.Integer } },
            {0x58, new AIScriptCommandDefinition("If_Party_Has_Damage", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x59, new AIScriptCommandDefinition("If_Party_Has_Reduced_PP", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x5A, new AIScriptCommandDefinition("Get_Fling_Power", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x5B, new AIScriptCommandDefinition("Get_Move_PP", -1) { storedValue = ParamAlias.Integer } },
            {0x5C, new AIScriptCommandDefinition("If_Can_Use_Last_Resort", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x5D, new AIScriptCommandDefinition("Get_Move_Category", -1) { storedValue = ParamAlias.MoveCategory } },
            {0x5E, new AIScriptCommandDefinition("Get_Previous_Move_Category", -1) { storedValue = ParamAlias.MoveCategory } },
            {0x5F, new AIScriptCommandDefinition("Get_Position_In_Turn_Order", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x60, new AIScriptCommandDefinition("Get_Poke_Turn_Count", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x61, new AIScriptCommandDefinition("If_Reserved_Has_Stronger_Move", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x62, new AIScriptCommandDefinition("If_Has_Super_Effective_Move", 0, ParamAlias.Address) },
            {0x63, new AIScriptCommandDefinition("If_Last_Move_GT_Strongest", 2, ParamAlias.Position, ParamAlias.Integer, ParamAlias.Address) },
            {0x64, new AIScriptCommandDefinition("Get_Positive_Stat_Stage_Total", -1, ParamAlias.Position) { storedValue = ParamAlias.Integer } },
            {0x65, new AIScriptCommandDefinition("Get_Stat_Difference", -1, ParamAlias.Position, ParamAlias.Stat) { storedValue = ParamAlias.Integer } },
            {0x66, new AIScriptCommandDefinition("c0x66", -1) },
            {0x67, new AIScriptCommandDefinition("c0x67", -1) },
            {0x68, new AIScriptCommandDefinition("c0x68", -1) },
            {0x69, new AIScriptCommandDefinition("Get_Damage_With_Partner", -1, ParamAlias.Integer) { storedValue = ParamAlias.Integer } },
            {0x6A, new AIScriptCommandDefinition("If_Is_Fainted", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x6B, new AIScriptCommandDefinition("If_Is_Not_Fainted", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x6C, new AIScriptCommandDefinition("Get_Ability", -1, ParamAlias.Position) { storedValue = ParamAlias.Ability } },
            {0x6D, new AIScriptCommandDefinition("If_Has_Substitute", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x6E, new AIScriptCommandDefinition("Get_Species", -1, ParamAlias.Position) { storedValue = ParamAlias.Species } },
            {0x6F, new AIScriptCommandDefinition("If_Rand_Seed_LT", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x70, new AIScriptCommandDefinition("If_Rand_Seed_GT", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x71, new AIScriptCommandDefinition("If_Rand_Seed_EQ", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x72, new AIScriptCommandDefinition("If_Rand_Seed_NE", 1, ParamAlias.Integer, ParamAlias.Address) },
            {0x73, new AIScriptCommandDefinition("Jump_Table", -1, ParamAlias.Integer, ParamAlias.Integer, ParamAlias.Address) },
            {0x74, new AIScriptCommandDefinition("If_Attack_Foreseen", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x75, new AIScriptCommandDefinition("If_Attack_LT_SpAtt", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x76, new AIScriptCommandDefinition("If_Attack_GT_SpAtt", 1, ParamAlias.Position, ParamAlias.Address) },
            {0x77, new AIScriptCommandDefinition("If_Attack_EQ_SpAtt", 1, ParamAlias.Position, ParamAlias.Address) },
        };

        List<string> moveEffects = new List<string>()
        {
            "Dmg", "Target_slp", "Dmg_target_psn", "Dmg_target_absorb", "Dmg_target_brn", "Dmg_target_frz", "Dmg_target_par", "Explosion", "Dream_eater", "Mirror_move", "User_atk+1", "User_def+1", "User_spe+1", "User_spa+1", "User_spd+1", "User_acc+1", "User_eva+1", "No_miss", "Target_atk-1", "Target_def-1",
            "Target_spe-1", "Target_spa-1", "Target_spd-1", "Target_acc-1", "Target_eva-1", "Haze", "Bide", "Thrash", "Force_switch", "Multi-strike_2-5", "Conversion", "Dmg_flinch", "Heal_50", "Target_tox", "Dmg_money", "Light_screen", "Tri_attack", "Rest", "Ohko", "Razor_wind",
            "Direct_half", "Direct_40", "Dmg_trap", "Increased_crit", "Multi-strike_2", "Jump_kick", "Mist", "Focus_energy", "Dmg_recoil_25", "Target_confusion", "User_atk+2", "User_def+2", "User_spe+2", "User_spa+2", "User_spd+2", "User_acc+2", "User_eva+2", "Transform", "Target_atk-2", "Target_def-2",
            "Target_spe-2", "Target_spa-2", "Target_spd-2", "Target_acc-2", "Target_eva-2", "Reflect", "Target_psn", "Target_par", "Dmg_target_atk-1", "Dmg_target_def-1", "Dmg_target_spe-1", "Dmg_target_spa-1", "Dmg_target_spd-1", "Dmg_target_acc-1", "Dmg_target_eva-1", "Sky_attack", "Dmg_target_confuse", "Twineedle", "Vital_throw", "Substitute",
            "Recharge", "Rage", "Mimic", "Metronome", "Leech_seed", "Splash", "Disable", "Direct_level", "Psywave", "Counter", "Encore", "Pain_split", "Snore", "Conversion_2", "Mind_reader", "Sketch", "Unknown_0x0060", "Sleep_talk", "Destiny_bond", "Flail",
            "Spite", "False_swipe", "Team_heal", "Increased_priority", "Triple_kick", "Theif", "Spider_web", "Nightmare", "Minimize", "Curse", "Unknown_0x006e", "Protect", "Spikes", "Foresight", "Perish_song", "Sandstorm", "Endure", "Rollout", "Swagger", "Fury_cutter",
            "Attract", "Return", "Present", "Frustration", "Safeguard", "Flame_wheel", "Magnitude", "Baton_pass", "Pursuit", "Rapid_spin", "Direct_30", "Unknown_0x0083", "Morning_sun", "Unknown0x0085", "Unknown0x0086", "Hidden_power", "Rain_dance", "Sunny_day", "Dmg_user_def+1", "Dmg_user_atk+1",
            "Ancient_power", "Unknown_0x008d", "Belly_drum", "Psych_up", "Mirror_coat", "Skull_bash", "Twister", "Earthquake", "Future_sight", "Gust", "Stomp", "Solar_beam", "Thunder", "Teleport", "Beat_up", "Fly", "Defense_curl", "Unknown_0x009d", "Fake_out", "Uproar",
            "Stockpile", "Spit_up", "Swallow", "Unknown_0x00a3", "Hail", "Torment", "Flatter", "Will-o-wisp", "Memento", "Facade", "Focus_punch", "Smelling_salts", "Follow_me", "Nature_power", "Charge_electric", "Taunt", "Helping_hand", "Trick", "Roleplay", "Wish",
            "Assist", "Ingrain", "Dmg_user_atk_def-1", "Magic_coat", "Recycle", "Revenge", "Brick_break", "Yawn", "Knock_off", "Endeavor", "Dmg_user_hp_pct", "Skill_swap", "Imprison", "Refresh", "Grudge", "Snatch", "Low_kick", "Secret_power", "Dmg_recoil_33", "Teeter_dance",
            "Dmg_target_brn_highcrit", "Mud_sport", "Dmg_target_tox", "Weather_ball", "Dmg_user_spa-2", "Target_atk_def-1", "User_def_spd+1", "Sky_uppercut", "User_atk_def+1", "Dmg_target_psn_highcrit", "Water_sport", "User_spa_spd+1", "User_atk_spe+1", "Camouflage", "Roost", "Gravity", "Miracle_eye", "Wake-up_slap", "Dmg_user_spe-1", "Gyro_ball",
            "Healing_wish", "Brine", "Natural_gift", "Feint", "Pluck", "Tailwind", "Acupressure", "Metal_burst", "Dmg_user_switch", "Dmg_user_def_spd-1", "Payback", "Assurance", "Embargo", "Fling", "Psycho_shift", "Trump_card", "Heal_block", "Dmg_target_hp_pct", "Power_trick", "Gastro_acid",
            "Lucky_chant", "Me_first", "Copycat", "Power_swap", "Guard_swap", "Punishment", "Last_resort", "Worry_seed", "Sucker_punch", "Toxic_spikes", "Heart_swap", "Aqua_ring", "Magnet_rise", "Flare_blitz", "Struggle", "Dive", "Dig", "Surf", "Defog", "Trick_room",
            "Blizzard", "Whirlpool", "Volt_tackle", "Bounce", "Unknown_0x0108", "Captivate", "Stealth_rock", "Chatter", "Judgment", "Dmg_recoil_50", "Lunar_dance", "Dmg_target_spd-2", "Shadow_force", "Dmg_target_brn_flinch", "Dmg_target_frz_flinch", "Dmg_target_par_flinch", "Dmg_user_spa+1", "User_atk_acc+1", "Wide_guard", "Guard_split",
            "Power_split", "Wonder_room", "Dmg_physical", "Venoshock", "Autotomize", "Telekinesis", "Magic_room", "Smack_down", "Dmg_alwayscrit", "Flame_burst", "User_spa_spd_spe+2", "Heavy_slam", "Synchronoise", "Electro_ball", "Soak", "Dmg_user_spe+1", "Acid_spray", "Foul_play", "Simple_beam", "Entrainment",
            "After_you", "Round", "Echoed_voice", "Chip_away", "Clear_smog", "Stored_power", "Quick_guard", "Ally_switch", "Shell_smash", "Heal_pulse", "Hex", "Sky_drop", "User_spe+2_atk+1", "Dmg_force_switch", "Incinerate", "Quash", "Growth", "Acrobatics", "Reflect_type", "Retaliate",
            "Final_gambit", "User_spa+3", "User_atk_def_acc+1", "Bestow", "Water_pledge", "Fire_pledge", "Grass_pledge", "Work_up", "Cotton_guard", "Relic_song", "Glaciate", "Freeze_shock", "Ice_burn", "Unknown_0x014d", "V-create", "Fusion_flare", "Fusion_bolt", "Hurricane",
        };
        List<string> moveCategories = new List<string>()
        {
            "Status", "Physical", "Special"
        };
        List<string> positions = new List<string>()
        {
            "Target", "User", "TargetAlly", "UserAlly"
        };
        List<string> typeEffectiveness = new List<string>()
        {
            "NoEffect", "DoubleResisted", "Resisted", "Neutral", "SuperEffective", "DoubleSuperEffective"
        };
        List<string> typeParam = new List<string>()
        {
            "TargetType1", "UserType1", "TargetType2", "UserType2", "MoveType", "StoredMoveType", "AllyType1", "TargetAllyType1", "AllyType2", "TargetAllyType2"
        };
        List<string> conditions = new List<string>()
        {
            "None", "Paralysis", "Sleep", "Freeze", "Burn", "Poison", "Confusion", "Infatuation", "Bind", "Nightmare", "Curse", "Taunt", "Torment",
            "Disable", "Yawn", "Heal Block", "Gastro Acid", "Foresight", "Leech Seed", "Embargo", "Perish Song", "Ingrain", "Block", "Encore",
            "Roost", "Move Lock", "Charge Lock", "Choice Lock", "Must Hit", "Lock On", "Floating", "Knocked Down", "Telekinesis", "Sky Drop",
            "Accuracy Up", "Aqua Ring"
        };
        List<string> conditionFlags = new List<string>()
        {
            "Action Done", "No Switch", "Charge", "Fly", "Dive", "Dig", "Shadow Force", "Defense Curl", "Minimize", "Focus Energy", "Power Trick", "Micle Berry", "No Action",
            "Flash Fire", "Baton Pass", "Null"
        };
        List<string> sideConditions = new List<string>()
        {
            "Reflect", "Light Screen", "Safeguard", "Mist", "Tailwind", "Lucky Chant", "Spikes", "Toxic Spikes", "Stealth Rock", "Wide Guard", "Quick Guard", "Rainbow", "Sea of Fire",
            "Swamp"
        };
        List<string> fieldEffects = new List<string>()
        {
            "Unknown0", "Trick Room", "Gravity", "Imprison", "Water Sport", "Mud Sport", "Wonder Room", "Magic Room"
        };
        List<string> weathers = new List<string>()
        {
            "None", "Sun", "Rain", "Hail", "Sand"
        };
        List<string> damageResults = new List<string>()
        {
            "No Damage", "NotStrongest", "IsStrongest"
        };
        List<string> comparisons = new List<string>()
        {
            "LT", "GT", "EQ", "NE", "AND", "NAND", "LTE", "GTE"
        };
        List<string> stats = new List<string>()
        {
            "Hp", "Att", "Def", "SpAtt", "SpDef", "Speed", "Acc", "Eva"
        };
        List<string> statStage = new List<string>()
        {
            "Minus6", "Minus5", "Minus4", "Minus3", "Minus2", "Minus1", "Neutral", "Plus1", "Plus2", "Plus3", "Plus4", "Plus5", "Plus6"
        };
        List<string> itemEffects = new List<string>()
        {
            "None", "Restore HP", "Griseous Orb", "Drive_Adamant Orb", "Lustrous Orb", "Cheri Berry", "Chesto Berry", "Pecha Berry", "Rawst Berry",
            "Aspear Berry", "Leppa Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry", "Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry",
            "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry", "Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry",
            "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry", "Chilan Berry", "Liechi Berry", "Ganlon Berry",
            "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry", "Custap Berry", "Jaboca Berry",
            "Rowap Berry", "Increase Evasion", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb", "Choice Band",
            "Increase Flinch", "Increase Bug", "Increase Money", "Cleanse Tag", "Pure Incense", "Soul Dew", "DeepSeaTooth", "DeepSeaScale", "Smoke Ball",
            "Everstone", "Focus Band", "Increase Crit", "Increase Steel", "Leftovers", "Dragon Scale", "Light Ball", "Increase Ground", "Increase Rock",
            "Increase Grass", "Increase Dark", "Increase Fighting", "Increase Electric", "Increase Water", "Increase Flying", "Increase Poison", "Increase Ice",
            "Increase Ghost", "Increase Psychic", "Increase Fire", "Increase Dragon", "Increase Normal", "Up-Grade", "Shell Bell", "Lucky Punch", "Metal Powder",
            "Thick Club", "Stick", "Increase Accuracy", "Increase Physical", "Increase Special", "Expert Belt", "Extend Screen", "Life Orb", "Power Herb",
            "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Move Last", "Destiny Knot", "Black Sludge", "Icy Rock",
            "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer", "Power Belt", "Power Lens", "Power Band",
            "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate", "Zap Plate", "Meadow Plate", "Icicle Plate",
            "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate", "Spooky Plate", "Draco Plate", "Dread Plate",
            "Iron Plate", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Other"
        };
        List<string> genders = new List<string>()
        {
            "Male", "Female", "Unknown"
        };
        List<string> battleStyles = new List<string>()
        {
            "Single", "Double", "Triple", "Rotation"
        };
        List<string> battleTypes = new List<string>()
        {
            "Standard", "NetMulti", "NetMultiVsAI", "AIMulti", "AI1v2", "AIMultiVsWild"
        };

        public byte[] bytes;

        public AIScript(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public void Export(string file)
        {
            string output = "#include \"AIScriptCommands.h\"\n//AI Script " + MainEditor.AIScriptNarc.scripts.IndexOf(this) + "\n\nvoid Main()\n{";
            string mainScript = "";

            int pos = 0;
            ParamAlias storedParam = ParamAlias.Integer;
            Dictionary<int, string> labels = new Dictionary<int, string>();
            List<(List<string>, ParamAlias)> jumpTables = new List<(List<string>, ParamAlias)>();
            List<(List<int>, ParamAlias)> itemLists = new List<(List<int>, ParamAlias)>();
            Dictionary<int, string> unusedLabels = new Dictionary<int, string>();
            while (pos < bytes.Length)
            {
                if (labels.ContainsKey(pos))
                {
                    if (unusedLabels.ContainsKey(pos)) unusedLabels.Remove(pos);
                    mainScript += "\n\n" + labels[pos] + ":";
                }

                AIScriptCommandDefinition def = commandDictionary[HelperFunctions.ReadShort(bytes, pos)];
                pos += 2;

                int endPos = pos + def.paramAliases.Length * 4;

                string command = "\n\t" + def.name + "(";
                for (int i = 0; i < def.paramAliases.Length; i++)
                {
                    if (i == def.jumpParam && !labels.ContainsKey(endPos + HelperFunctions.ReadInt(bytes, pos)))
                    {
                        unusedLabels.Add(endPos + HelperFunctions.ReadInt(bytes, pos), "Label_" + labels.Count);
                        labels.Add(endPos + HelperFunctions.ReadInt(bytes, pos), "Label_" + labels.Count);
                    }

                    //Special case for list checks
                    if ((def.name == "If_Stored_In_List" || def.name == "If_Stored_Not_In_List") && i == 0)
                    {
                        command += "itemList" + itemLists.Count;
                        pos += 4;
                        (List<int>, ParamAlias) list = (new List<int>(), storedParam);
                        itemLists.Add(list);
                        int start = pos + HelperFunctions.ReadInt(bytes, pos - 4);
                        while (start < bytes.Length)
                        {
                            if (HelperFunctions.ReadInt(bytes, start) == -1) break;
                            list.Item1.Add(HelperFunctions.ReadInt(bytes, start));
                            start += 4;
                        }
                        continue;
                    }

                    if (i != 0) command += ", ";

                    if (def.paramAliases[i] == ParamAlias.Address && i == def.jumpParam)
                        command += "\"" + labels[endPos + HelperFunctions.ReadInt(bytes, pos)] + "\"";
                    else if (def.paramAliases[i] == ParamAlias.Stored)
                        command += ParamToString(storedParam, HelperFunctions.ReadInt(bytes, pos));
                    else
                        command += ParamToString(def.paramAliases[i], HelperFunctions.ReadInt(bytes, pos));
                    pos += 4;

                    //Special case for jump tables
                    if (def.name == "Jump_Table")
                    {
                        command += ", jumpTable" + jumpTables.Count;
                        break;
                    }
                }
                command += ");";

                mainScript += command;

                if (def.name == "Jump_Table")
                {
                    int count = HelperFunctions.ReadInt(bytes, pos);
                    int start = endPos + HelperFunctions.ReadInt(bytes, pos + 4);
                    List<string> table = new List<string>();
                    jumpTables.Add((table, ParamAlias.MoveEffect));

                    for (int i = 0; i < count; i++)
                    {
                        int addr = start + HelperFunctions.ReadInt(bytes, start + i * 4);
                        if (!labels.ContainsKey(addr))
                        {
                            unusedLabels.Add(addr, "Label_" + labels.Count);
                            labels.Add(addr, "Label_" + labels.Count);
                        }

                        table.Add(labels[addr]);
                    }
                    pos += 8;
                }

                if (def.name == "End_Script" || def.name == "Jump")
                {
                    int jumpTo = -1;
                    foreach (int i in unusedLabels.Keys)
                    {
                        if (i < jumpTo || jumpTo == -1) jumpTo = i;
                    }
                    if (jumpTo != -1) pos = jumpTo;
                    else break;
                }
                if (def.storedValue != ParamAlias.Stored)
                    storedParam = def.storedValue;
            }

            mainScript += "\n}";

            foreach (var list in jumpTables)
            {
                output += "\n\tchar *jumpTable" + jumpTables.IndexOf(list) + "[] =\n\t{";
                for (int i = 0; i < list.Item1.Count; i++)
                {
                    output += "\n\t\t\"" + list.Item1[i] + "\",\t\t";
                    if (list.Item1[i].Length < 9) output += "\t";
                    output += "//" + ParamToString(list.Item2, i);
                }
                output += "\n\t};\n";
            }

            foreach (var list in itemLists)
            {
                output += "\n\tint itemList" + itemLists.IndexOf(list) + "[] =\n\t{";
                for (int i = 0; i < list.Item1.Count; i++)
                {
                    output += "\n\t\t" + ParamToString(list.Item2, list.Item1[i]) + ",";
                }
                output += "\n\t};\n";
            }

            output += mainScript;

            //TextFile t = MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID];
            //for (int i = 0; i < moveEffects.Count; i++)
            //{
            //    string str = "const int MoveEffect_" + moveEffects[i].Replace(" ", "").Replace("-", "").Replace(".", "") + " = " + i + ";";
            //    Debug.WriteLine(str);
            //}

            File.WriteAllText(file, output);
        }

        public void Import(string file)
        {
            string[] lines = File.ReadAllLines(file);
            List<byte> newBytes = new List<byte>();

            Dictionary<string, List<string>> jumpTables = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> itemLists = new Dictionary<string, List<string>>();
            Dictionary<string, int> labels = new Dictionary<string, int>();
            Dictionary<int, string> jumpInstructions = new Dictionary<int, string>();
            Dictionary<int, string> jumpTableInstructions = new Dictionary<int, string>();
            Dictionary<int, string> itemListInstructions = new Dictionary<int, string>();

            ParamAlias storedParam = ParamAlias.Integer;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Replace("\t", "");
                if (line.Length == 0) continue;
                if (line.StartsWith("#") || line.StartsWith("//") || line.StartsWith("{") || line.StartsWith("}")) continue;
                if (line.StartsWith("void")) continue;

                //Jump Table
                if (line.StartsWith("char"))
                {
                    string name = line.Substring(line.IndexOf("*") + 1, line.IndexOf("[") - line.IndexOf("*") - 1);
                    if (!line.Contains("*") || !line.Contains("["))
                        throw new Exception("Error on line " + (i + 1) + ":\n\nImproper jump table formatting\nFormat should be:\n\nchar *tableName[] =\n{\n   \"Label\",\n   ...\n};");
                    if (jumpTables.ContainsKey(name))
                        throw new Exception("Error on line " + (i + 1) + ":\n\nDuplicate jump table name \"" + name + "\"");
                    List<string> entries = new List<string>();
                    jumpTables.Add(name, entries);

                    while (i < lines.Length)
                    {
                        i++;
                        line = lines[i].Replace("\t", "");
                        if (line.StartsWith("}")) break;
                        if (line.StartsWith("{") || line.StartsWith("//") || line.Length == 0) continue;

                        string[] quotes = line.Split('\"');
                        if (quotes.Length == 3 && quotes[1].Length > 0)
                        {
                            entries.Add(quotes[1]);
                        }
                        else
                            throw new Exception("Error on line " + (i + 1) + ":\n\nImproper jump table formatting\nFormat should be:\n\nchar *tableName[] =\n{\n   \"Label\",\n   ...\n};");
                    }
                    continue;
                }

                //Item List
                if (line.StartsWith("int"))
                {
                    string name = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("[", "").Replace("]", "");
                    if (line.Contains("*") || !line.Contains("["))
                        throw new Exception("Error on line " + (i + 1) + ":\n\nImproper list formatting\nFormat should be:\n\nint listName[] =\n{\n   Item,\n   ...\n};");
                    if (itemLists.ContainsKey(name))
                        throw new Exception("Error on line " + (i + 1) + ":\n\nDuplicate list name \"" + name + "\"");
                    List<string> entries = new List<string>();
                    itemLists.Add(name, entries);
                    while (i < lines.Length)
                    {
                        i++;
                        line = lines[i].Replace("\t", "").Replace(",", "");
                        if (line.StartsWith("}")) break;
                        else if (line.StartsWith("{") || line.StartsWith("//")) continue;
                        else entries.Add(line);
                    }
                    continue;
                }

                //Labels
                if (line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].EndsWith(":"))
                {
                    string name = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].Replace(":", "");
                    if (!labels.ContainsKey(name)) labels.Add(name, newBytes.Count);
                    continue;
                }

                //Commands
                if (line.Contains("("))
                {
                    string com = line.Substring(0, line.IndexOf("(")).Replace(" ", "");
                    var def = commandDictionary.FirstOrDefault(c => c.Value.name == com);
                    if (def.Value == null)
                        throw new Exception("Error on line " + (i + 1) + ":\n\nInvalid command \"" + com + "\"");
                    int start = newBytes.Count;
                    newBytes.AddRange(BitConverter.GetBytes((short)def.Key));

                    string[] pars = line.Substring(line.IndexOf("(") + 1, line.IndexOf(")") - line.IndexOf("(") - 1).Replace(" ", "").Split(',');
                    if (pars[0].Length == 0) pars = new string[0];
                    if (def.Value.name == "Jump_Table")
                    {
                        jumpTableInstructions.Add(start, pars[1]);
                        newBytes.AddRange(BitConverter.GetBytes(int.Parse(pars[0])));
                        newBytes.AddRange(BitConverter.GetBytes(0));
                        newBytes.AddRange(BitConverter.GetBytes(0));
                    }
                    else
                    {
                        if (pars.Length != def.Value.paramAliases.Length)
                            throw new Exception("Error on line " + (i + 1) + ":\n\nIncorrect parameter count:\n\nExpected " + def.Value.paramAliases.Length + "\nFound " + pars.Length);
                        for (int p = 0; p < def.Value.paramAliases.Length; p++)
                        {
                            if (p == def.Value.jumpParam)
                            {
                                jumpInstructions.Add(start, pars[p].Replace("\"", ""));
                                newBytes.AddRange(BitConverter.GetBytes(0));
                            }
                            else if (p == 0 && (def.Value.name == "If_Stored_Not_In_List" || def.Value.name == "If_Stored_In_List"))
                            {
                                itemListInstructions.Add(start, pars[p]);
                                newBytes.AddRange(BitConverter.GetBytes(0));
                            }
                            else
                            {
                                int val = ParamFromString(def.Value.paramAliases[p] == ParamAlias.Stored ? storedParam : def.Value.paramAliases[p], pars[p]);
                                if (val == -1000)
                                    throw new Exception("Error on line " + (i + 1) + ":\n\nCould not parse value \"" + pars[p] + "\"");
                                newBytes.AddRange(BitConverter.GetBytes(val));
                            }
                        }
                    }
                    if (def.Value.storedValue != ParamAlias.Stored)
                        storedParam = def.Value.storedValue;
                }
                else
                {
                    throw new Exception("Error on line " + (i + 1) + ":\n\nUnknown instruction");
                }
            }

            int end = newBytes.Count;

            foreach (var table in jumpTableInstructions)
            {
                if (!jumpTables.ContainsKey(table.Value))
                    throw new Exception("Error: undefined jump table pointer \"" + table.Value + "\"");
                int offset = newBytes.Count - (table.Key + 14);
                HelperFunctions.WriteInt(newBytes, (table.Key + 6), jumpTables[table.Value].Count);
                HelperFunctions.WriteInt(newBytes, (table.Key + 10), offset);
                int tabStart = newBytes.Count;
                foreach (var tableEntry in jumpTables[table.Value])
                {
                    if (!labels.ContainsKey(tableEntry))
                        throw new Exception("Error: undefined label \"" + tableEntry + "\"");
                    newBytes.AddRange(BitConverter.GetBytes(labels[tableEntry] - tabStart));
                }
            }

            int pos = 0;
            storedParam = ParamAlias.Integer;
            while (pos < end)
            {
                AIScriptCommandDefinition def = commandDictionary[HelperFunctions.ReadShort(newBytes, pos)];

                if (jumpInstructions.ContainsKey(pos))
                {
                    if (!labels.ContainsKey(jumpInstructions[pos]))
                        throw new Exception("Error: undefined label \"" + jumpInstructions[pos] + "\"");
                    HelperFunctions.WriteInt(newBytes, pos + 2 + 4 * def.jumpParam, labels[jumpInstructions[pos]] - (pos + def.paramAliases.Length * 4 + 2));
                }
                if (def.name == "If_Stored_Not_In_List" || def.name == "If_Stored_In_List")
                {
                    int offset = newBytes.Count - (pos + 6);
                    HelperFunctions.WriteInt(newBytes, (pos + 2), offset);
                    List<string> list = itemLists[itemListInstructions[pos]];
                    foreach (string str in list)
                    {
                        newBytes.AddRange(BitConverter.GetBytes(ParamFromString(storedParam, str)));
                    }
                    newBytes.AddRange(BitConverter.GetBytes(-1));
                }

                if (def.storedValue != ParamAlias.Stored)
                    storedParam = def.storedValue;

                pos += def.paramAliases.Length * 4 + 2;
            }

            bytes = newBytes.ToArray();
        }

        string ParamToString(ParamAlias alias, int value)
        {
            try
            {
                switch (alias)
                {
                    case ParamAlias.Ability: return "Ability_" + MainEditor.textNarc.textFiles[VersionConstants.AbilityNameTextFileID].text[value].Replace(" ", "").Replace("-", "");
                    case ParamAlias.Move: return "Move_" + MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text[value].Replace(" ", "").Replace("-", "");
                    case ParamAlias.MoveEffect: return "MoveEffect_" + moveEffects[value].Replace(" ", "").Replace("-", "").Replace("+", "");
                    case ParamAlias.MoveCategory: return "MoveCat_" + moveCategories[value].Replace(" ", "");
                    case ParamAlias.Position: return positions[value];
                    case ParamAlias.Type: return "Type_" + MainEditor.textNarc.textFiles[VersionConstants.TypeNameTextFileID].text[value].Replace(" ", "").Replace("-", "");
                    case ParamAlias.TypeEffectiveness: return typeEffectiveness[value];
                    case ParamAlias.TypeParam: return typeParam[value];
                    case ParamAlias.Condition: return "Condition_" + conditions[value].Replace(" ", "");
                    case ParamAlias.ConditionFlag: return "ConditionFlag_" + conditionFlags[value].Replace(" ", "");
                    case ParamAlias.SideCondition: return "SideCondition_" + sideConditions[value].Replace(" ", "");
                    case ParamAlias.FieldEffect: return "Field_" + fieldEffects[value].Replace(" ", "");
                    case ParamAlias.Weather: return "Weather_" + weathers[value].Replace(" ", "");
                    case ParamAlias.DamageCalcResult: return damageResults[value].Replace(" ", "");
                    case ParamAlias.Comparison: return comparisons[value].Replace(" ", "");
                    case ParamAlias.Stat: return "Stat_" + stats[value].Replace(" ", "");
                    case ParamAlias.StatStage: return "StatStage_" + statStage[value].Replace(" ", "");
                    case ParamAlias.HeldItem: return "Item_" + MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text[value].Replace(" ", "").Replace("-", "").Replace(".", "").Replace("'", "");
                    case ParamAlias.HeldItemEffect: return "ItemEffect_" + itemEffects[value].Replace(" ", "").Replace(".", "").Replace("-", "");
                    case ParamAlias.Gender: return "Gender_" + genders[value].Replace(" ", "");
                    case ParamAlias.BattleStyle: return "Battle_" + battleStyles[value].Replace(" ", "");
                    case ParamAlias.BattleType: return "BattleType_" + battleTypes[value].Replace(" ", "");
                    case ParamAlias.Boolean: return value == 1 ? "True" : "False";
                    case ParamAlias.Species: return "Poke_" + MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[value].Replace(" ", "").Replace("-", "").Replace(".", "").Replace("'", ""); ;
                    default: return value.ToString();
                }
            }
            catch
            {
                return value.ToString();
            }
        }

        int ParamFromString(ParamAlias alias, string value)
        {
            if (value.StartsWith("0x") && int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out int hex)) return hex;
            if (int.TryParse(value, out int num)) return num;
            if (value == "True") return 1;
            if (value == "False") return 0;

            List<string> list = new List<string>();

            switch (alias)
            {
                case ParamAlias.Ability: list = MainEditor.textNarc.textFiles[VersionConstants.AbilityNameTextFileID].text; break;
                case ParamAlias.Move: list = MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text; break;
                case ParamAlias.MoveEffect: list = moveEffects; break;
                case ParamAlias.MoveCategory: list = moveCategories; break;
                case ParamAlias.Position: list = positions; break;
                case ParamAlias.Type: list = MainEditor.textNarc.textFiles[VersionConstants.TypeNameTextFileID].text; break;
                case ParamAlias.TypeEffectiveness: list = typeEffectiveness; break;
                case ParamAlias.TypeParam: list = typeParam; break;
                case ParamAlias.Condition: list = conditions; break;
                case ParamAlias.ConditionFlag: list = conditionFlags; break;
                case ParamAlias.SideCondition: list = sideConditions; break;
                case ParamAlias.FieldEffect: list = fieldEffects; break;
                case ParamAlias.Weather: list = weathers; break;
                case ParamAlias.DamageCalcResult: list = damageResults; break;
                case ParamAlias.Comparison: list = comparisons; break;
                case ParamAlias.Stat: list = stats; break;
                case ParamAlias.StatStage: list = statStage; break;
                case ParamAlias.HeldItem: list = MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text; break;
                case ParamAlias.HeldItemEffect: list = itemEffects; break;
                case ParamAlias.Gender: list = genders; break;
                case ParamAlias.BattleStyle: list = battleStyles; break;
                case ParamAlias.BattleType: list = battleTypes; break;
                case ParamAlias.Species: list = MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text; break;
            }
            
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string str = list[i].Replace(" ", "").Replace("-", "").Replace(".", "").Replace("'", "").Replace("+", "");
                    if (value == str || (value.Contains("_") && value.Substring(value.IndexOf("_") + 1) == str))
                    {
                        return i;
                    }
                }
                return -1000;
            }
            else return -1000;
        }
    }

    public class AIScriptCommandDefinition
    {
        public string name;
        public ParamAlias[] paramAliases;
        public int jumpParam = -1;
        public ParamAlias storedValue = ParamAlias.Stored;

        public AIScriptCommandDefinition(string name, int jumpParam = -1, params ParamAlias[] paramAliases)
        {
            this.name = name;
            this.paramAliases = paramAliases;
            this.jumpParam = jumpParam;
        }
    }

    public enum ParamAlias
    {
        Integer,
        Address,
        Stored,
        Ability,
        Move,
        MoveEffect,
        MoveCategory,
        Position,
        Type,
        TypeEffectiveness,
        TypeParam,
        Condition,
        ConditionFlag,
        SideCondition,
        FieldEffect,
        Weather,
        DamageCalcResult,
        Comparison,
        Stat,
        StatStage,
        HeldItem,
        HeldItemEffect,
        Gender,
        BattleStyle,
        BattleType,
        Boolean,
        Species,
    }
}
