using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public static class VersionConstants
    {
        //File Data
        public const int BW2_FirstNarcPointerLocation = 0x3644C0;
        public const int BW2_LastNarc = 307;
        public const int BW2_NARCsToSkip = 3;
        public const int BW2_FileSizeLimit = 0x12000000;

        //Narc Data
        public const int BW2_TextNARCID = 2;
        public const int BW2_StoryTextNARCID = 3;
        public const int BW2_PokemonSpritesNARCID = 4;
        public const int BW2_PokemonIconsNARCID = 7;
        public const int BW2_MapFilesNARCID = 8;
        public const int BW2_MapMatriciesNARCID = 9;
        public const int BW2_ZoneDataNARCID = 12;
        public const int BW2_PokemonDataNARCID = 16;
        public const int BW2_XPCurveNARCID = 17;
        public const int BW2_LevelUpMovesNARCID = 18;
        public const int BW2_EvolutionsNARCID = 19;
        public const int BW2_ChildPokemonNARCID = 20;
        public const int BW2_MoveDataNARCID = 21;
        public const int BW2_ItemDataNARCID = 24;
        public const int BW2_ScriptNARCID = 56;
        public const int BW2_MoveAnimationNARCID = 65;
        public const int BW2_MoveAnimationExtraNARCID = 66;
        public const int BW2_TrTextEntriesNARCID = 89;
        public const int BW2_TrTextIndicesNARCID = 90;
        public const int BW2_TrainerDataNARCID = 91;
        public const int BW2_TrainerPokemonNARCID = 92;
        public const int BW2_KeyboardLayoutNARCID = 120;
        public const int BW2_EggMoveNARCID = 124;
        public const int BW2_OverworldsNARCID = 126;
        public const int BW2_EncountersNARCID = 127;
        public const int BW2_HabitatListNARCID = 296;
        public const int BW2_PokemartNARCID = 282;
        public const int BW2_PokemartItemCountNARCID = 283;

        public const int BW1_TextNARCID = 2;
        public const int BW1_StoryTextNARCID = 3;
        public const int BW1_PokemonSpritesNARCID = 4;
        public const int BW1_PokemonIconsNARCID = 7;
        public const int BW1_MapFilesNARCID = 8;
        public const int BW1_MapMatriciesNARCID = 9;
        public const int BW1_ZoneDataNARCID = 12;
        public const int BW1_PokemonDataNARCID = 16;
        public const int BW1_XPCurveNARCID = 17;
        public const int BW1_LevelUpMovesNARCID = 18;
        public const int BW1_EvolutionsNARCID = 19;
        public const int BW1_ChildPokemonNARCID = 20;
        public const int BW1_MoveDataNARCID = 21;
        public const int BW1_ItemDataNARCID = 24;
        public const int BW1_ScriptNARCID = 57;
        public const int BW1_MoveAnimationNARCID = 66;
        public const int BW1_MoveAnimationExtraNARCID = 67;
        public const int BW1_TrTextEntriesNARCID = 90;
        public const int BW1_TrTextIndicesNARCID = 91;
        public const int BW1_TrainerDataNARCID = 92;
        public const int BW1_TrainerPokemonNARCID = 93;
        public const int BW1_EggMovesNARCID = 123;
        public const int BW1_OverworldsNARCID = 125;
        public const int BW1_EncountersNARCID = 126;
        public const int BW1_KeyboardLayoutNARCID = -1;
        public const int BW1_PokemartNARCID = -2;
        public const int BW1_PokemartItemCountNARCID = -3;
        public const int BW1_HabitatListNARCID = -4;

        //Text Data
        public static int PokemonNameTextFileID => MainEditor.RomType == RomType.BW2 ? 90 : MainEditor.RomType == RomType.BW1 ? 70 : 237;
        public static int PokemonName2TextFileID => MainEditor.RomType == RomType.BW2 ? 483 : MainEditor.RomType == RomType.BW1 ? 281 : -1;
        public static int AbilityNameTextFileID => MainEditor.RomType == RomType.BW2 ? 374 : MainEditor.RomType == RomType.BW1 ? 182 : 720;
        public static int TypeNameTextFileID => MainEditor.RomType == RomType.BW2 ? 398 : MainEditor.RomType == RomType.BW1 ? 199 : 735;
        public static int ItemNameTextFileID => MainEditor.RomType == RomType.BW2 ? 64 : MainEditor.RomType == RomType.BW1 ? 54 : 222;
        public static int MoveNameTextFileID => MainEditor.RomType == RomType.BW2 ? 403 : MainEditor.RomType == RomType.BW1 ? 203 : 750;
        public static int MoveDescriptionTextFileID => MainEditor.RomType == RomType.BW2 ? 402 : MainEditor.RomType == RomType.BW1 ? 202 : 749;
        public static int MoveUsageTextFileID => MainEditor.RomType == RomType.BW2 ? 16 : MainEditor.RomType == RomType.BW1 ? 13 : 3;
        public static int ZoneNameTextFileID => MainEditor.RomType == RomType.BW2 ? 109 : MainEditor.RomType == RomType.BW1 ? 89 : 279;
        public static int TrainerNameTextFileID => MainEditor.RomType == RomType.BW2 ? 382 : MainEditor.RomType == RomType.BW1 ? 190 : 729;
        public static int TrainerDialogueTextFileID => MainEditor.RomType == RomType.BW2 ? 381 : MainEditor.RomType == RomType.BW1 ? 189 : -1;
        public static int PokedexEntryTextFileID => MainEditor.RomType == RomType.BW2 ? 442 : MainEditor.RomType == RomType.BW1 ? 235 : -1;
        public static int PokedexClassificationTextFileID => MainEditor.RomType == RomType.BW2 ? 464 : MainEditor.RomType == RomType.BW1 ? 260 : -1;
        public static int[] PokedexImpericalHeightTextFileID => MainEditor.RomType == RomType.BW2 ? new int[]
		{
			451, 452
		} : MainEditor.RomType == RomType.BW1 ? new int[]
        {
            245, 246
        } : new int[0];
        public static int[] PokedexMetricCommaHeightTextFileID => MainEditor.RomType == RomType.BW2 ? new int[]
        {
            453, 454, 455, 457
        } : MainEditor.RomType == RomType.BW1 ? new int[]
        {
            247, 248, 249, 252
        } : new int[0];
        public static int[] PokedexMetricHeightTextFileID => MainEditor.RomType == RomType.BW2 ? new int[]
        {
            456
        } : MainEditor.RomType == RomType.BW1 ? new int[]
        {
            251
        } : new int[0];
        public static int[] PokedexImpericalWeightTextFileID => MainEditor.RomType == RomType.BW2 ? new int[]
        {
            471, 472
        } : MainEditor.RomType == RomType.BW1 ? new int[]
        {
            268, 269
        } : new int[0];
        public static int[] PokedexMetricCommaWeightTextFileID => MainEditor.RomType == RomType.BW2 ? new int[]
        {
            473, 474, 475, 477
        } : MainEditor.RomType == RomType.BW1 ? new int[]
        {
            270, 270, 272, 275
        } : new int[0];
        public static int[] PokedexMetricWeightTextFileID => MainEditor.RomType == RomType.BW2 ? new int[]
        {
            476
        } : MainEditor.RomType == RomType.BW1 ? new int[]
        {
            274
        } : new int[0];

        //File Data
        public const int HGSS_FirstNarcPointerLocation = 0x31FC08;
		public const int HGSS_LastNarc = 264;
		public const int HGSS_NARCsToSkip = 0;
		public const int HGSS_FileSizeLimit = 0x8000000;

		//Narc Data
		public const int HGSS_TextNARCID = 27;
		public const int HGSS_StoryTextNARCID = -1;
		public const int HGSS_MapMatriciesNARCID = -1;
		public const int HGSS_ZoneDataNARCID = -1;
		public const int HGSS_PokemonDataNARCID = 2;
		public const int HGSS_LevelUpMovesNARCID = 33;
		public const int HGSS_EvolutionsNARCID = -1;
		public const int HGSS_MoveDataNARCID = -1;
		public const int HGSS_ScriptNARCID = -1;

		public static List<string> BW2_TMNames = new List<string>()
        {
			"TM01 Hone Claws",
			"TM02 Dragon Claw",
			"TM03 Psyshock",
			"TM04 Calm Mind",
			"TM05 Roar",
			"TM06 Toxic",
			"TM07 Hail",
			"TM08 Bulk Up",
			"TM09 Venoshock",
			"TM10 Hidden Power",
			"TM11 Sunny Day",
			"TM12 Taunt",
			"TM13 Ice Beam",
			"TM14 Blizzard",
			"TM15 Hyper Beam",
			"TM16 Light Screen",
			"TM17 Protect",
			"TM18 Rain Dance",
			"TM19 Telekinesis",
			"TM20 Safeguard",
			"TM21 Frustration",
			"TM22 SolarBeam",
			"TM23 Smack Down",
			"TM24 Thunderbolt",
			"TM25 Thunder",
			"TM26 Earthquake",
			"TM27 Return",
			"TM28 Dig",
			"TM29 Psychic",
			"TM30 Shadow Ball",
			"TM31 Brick Break",
			"TM32 Double Team",
			"TM33 Reflect",
			"TM34 Sludge Wave",
			"TM35 Flamethrower",
			"TM36 Sludge Bomb",
			"TM37 Sandstorm",
			"TM38 Fire Blast",
			"TM39 Rock Tomb",
			"TM40 Aerial Ace",
			"TM41 Torment",
			"TM42 Facade",
			"TM43 Flame Charge",
			"TM44 Rest",
			"TM45 Attract",
			"TM46 Thief",
			"TM47 Low Sweep",
			"TM48 Round",
			"TM49 Echoed Voice",
			"TM50 Overheat",
			"TM51 Ally Switch",
			"TM52 Focus Blast",
			"TM53 Energy Ball",
			"TM54 False Swipe",
			"TM55 Scald",
			"TM56 Fling",
			"TM57 Charge Beam",
			"TM58 Sky Drop",
			"TM59 Incinerate",
			"TM60 Quash",
			"TM61 Will-O-Wisp",
			"TM62 Acrobatics",
			"TM63 Embargo",
			"TM64 Explosion",
			"TM65 Shadow Claw",
			"TM66 Payback",
			"TM67 Retaliate",
			"TM68 Giga Impact",
			"TM69 Rock Polish",
			"TM70 Flash",
			"TM71 Stone Edge",
			"TM72 Volt Switch",
			"TM73 Thunder Wave",
			"TM74 Gyro Ball",
			"TM75 Swords Dance",
			"TM76 Struggle Bug",
			"TM77 Psych Up",
			"TM78 Bulldoze",
			"TM79 Frost Breath",
			"TM80 Rock Slide",
			"TM81 X-Scissor",
			"TM82 Dragon Tail",
			"TM83 Work Up",
			"TM84 Poison Jab",
			"TM85 Dream Eater",
			"TM86 Grass Knot",
			"TM87 Swagger",
			"TM88 Pluck",
			"TM89 U-turn",
			"TM90 Substitute",
			"TM91 Flash Cannon",
			"TM92 Trick Room",
			"TM93 Wild Charge",
			"TM94 Rock Smash",
			"TM95 Snarl",
			"HM01 Cut",
			"HM02 Fly",
			"HM03 Surf",
			"HM04 Strength",
			"HM05 Waterfall",
			"HM06 Dive"
		};

        public static List<string> BW2_TutorMoves = new List<string>()
        {
            "Grass Pledge",
            "Fire Pledge",
            "Water Pledge",
            "Frenzy Plant",
            "Blast Burn",
            "Hydro Cannon",
            "Draco Meteor",
            "Bug Bite",
            "Covet",
            "Super Fang",
            "Dual Chop",
            "Signal Beam",
            "Iron Head",
            "Seed Bomb",
            "Drill Run",
            "Bounce",
            "Low Kick",
            "Gunk Shot",
            "Uproar",
            "Thunder Punch",
            "Fire Punch",
            "Ice Punch",
            "Magic Coat",
            "Block",
            "Earth Power",
            "Foul Play",
            "Gravity",
            "Magnet Rise",
            "Iron Defense",
            "Last Resort",
            "Super Power",
            "Electroweb",
            "Icy Wind",
            "Aqua Tail",
            "Dark Pulse",
            "Zen Headbutt",
            "Dragon Pulse",
            "Hyper Voice",
            "Iron Tail",
            "Bind",
            "Snore",
            "Knock Off",
            "Synthesis",
            "Heat Wave",
            "Role Play",
            "Heal Bell",
            "Tailwind",
            "Sky Attack",
            "Pain Split",
            "Giga Drain",
            "Drain Punch",
            "Roost",
            "Gastro Acid",
            "Worry Seed",
            "Spite",
            "After You",
            "Helping Hand",
            "Trick",
            "Magic Room",
            "Wonder Room",
            "Endeavor",
            "Outrage",
            "Recycle",
            "Snatch",
            "Stealth Rock",
            "Sleep Talk",
            "Skill Swap"
        };

        public static List<string> BW2_EvolutionMethodNames = new List<string>()
		{
			"None",
			"Level up with high friendship",
			"Friendship during the day",
			"Friendship at night",
			"Level up",
			"Trade",
			"Trade with held item",
			"Trade for Karrablast / Shelmet",
			"Evolution stone / Held Item",
			"Level up with attack > defense",
			"Level up with attack = defense",
			"Level up with attack < defense",
			"Level up with personality < 5 (Silcoon)",
			"Level up with personality > 5 (Cascoon)",
			"Level up (Ninjask)",
			"Level up (Shedinja)",
			"Level up with high beauty",
			"Evolution stone (Male)",
			"Evolution stone (Female)",
			"Evolution stone (Day)",
			"Evolution stone (Night)",
			"Level up with a certain move",
			"Level up with a certain pokemon",
			"Level up (Male)",
			"Level up (Female)",
			"Level up in electric cave",
			"Level up near mossy rock",
			"Level up near ice rock"
		};

		public static List<string> BW2_RouteEncounterPoolNames = new List<string>()
		{
			"Striaton City",
			"Castelia City",
			"Icirrus City",
			"Aspertia City",
			"Virbank City",
			"Humilau City",
			"Dreamyard 1",
			"Dreamyard 2",
			"Pinwheel Forest 1",
			"Pinwheel Forest 2",
			"Desert Resort 1",
			"Desert Resort 2",
			"Relic Castle 1",
			"Relic Castle 2",
			"Relic Castle 3",
			"Relic Castle 4",
			"Relic Castle 5",
			"Relic Castle 6",
			"Relic Castle 7",
			"Relic Castle 8",
			"Chargestone Cave 1",
			"Chargestone Cave 2",
			"Chargestone Cave 3",
			"Twist Mountain 1",
			"Twist Mountain 2",
			"Twist Mountain 3",
			"Twist Mountain 4",
			"Dragonspiral Tower 1",
			"Dragonspiral Tower 2",
			"Dragonspiral Tower 3",
			"Dragonspiral Tower 4",
			"Victory Road",
			"Giant Chasm 1",
			"Giant Chasm 2",
			"Giant Chasm 3",
			"Giant Chasm 4",
			"Giant Chasm 5",
			"Castelia Sewers 5",
			"Castelia Sewers 4",
			"Castelia Sewers 3",
			"Castelia Sewers 2",
			"Castelia Sewers 1",
			"P2 Laboratory",
			"Undella Bay",
			"Floccesy Ranch 1",
			"Floccesy Ranch 2",
			"Virbank Complex 1",
			"Virbank Complex 2",
			"Reverse Mountain 1",
			"Reverse Mountain 2",
			"Reverse Mountain 3",
			"Reverse Mountain 4",
			"Reverse Mountain 5",
			"Reverse Mountain 6",
			"Reverse Mountain 7",
			"Reverse Mountain 8",
			"Reverse Mountain 9",
			"Reverse Mountain 10",
			"Reverse Mountain 11",
			"Reverse Mountain 12",
			"Reverse Mountain 13",
			"Stranger's House 1",
			"Stranger's House 2",
			"Stranger's House 3",
			"Stranger's House 4",
			"Stranger's House 5",
			"Stranger's House 6",
			"Stranger's House 7",
			"Stranger's House 8",
			"Stranger's House 9",
			"Stranger's House 10",
			"Victory Road 1",
			"Victory Road 2",
			"Victory Road 3",
			"Victory Road 4",
			"Victory Road 5",
			"Victory Road 6",
			"Victory Road 7",
			"Victory Road 8",
			"Victory Road 9",
			"Victory Road 10",
			"Relic Passage 1",
			"Relic Passage 2",
			"Relic Passage 3",
			"Clay Road 1",
			"Clay Road 2",
			"Clay Road 3",
			"Underground Ruins 1",
			"Underground Ruins 2",
			"Underground Ruins 3",
			"Rocky Mountain Room",
			"Glacier Room",
			"Iron Room",
			"Seaside Grotto 1",
			"Seaside Grotto 2",
			"Nature Sanctuary",
			"Driftveil Drawbridge",
			"Village Bridge",
			"Marvelous Bridge",
			"Route 1",
			"Route 2",
			"Route 3",
			"Wellspring Cave 1",
			"Wellspring Cave 2",
			"Route 4 1",
			"Route 4 2",
			"Route 5",
			"Route 6",
			"Mistralton Cave 1",
			"Mistralton Cave 2",
			"Guidance Chamber",
			"Route 7",
			"Celestial Tower 1",
			"Celestial Tower 2",
			"Celestial Tower 3",
			"Celestial Tower 4",
			"Route 8",
			"Moor of Icirrus",
			"Route 9",
			"Route 11",
			"Route 12",
			"Route 13",
			"Route 14",
			"Abundant Shrine",
			"Route 15",
			"Route 16",
			"Lostlorn Forest",
			"Route 18",
			"Route 19",
			"Route 20",
			"Route 22",
			"Route 23",
			"Undella Town",
			"Route 17",
			"Route 21"
		};

		public static List<string> BW1_RouteEncounterPoolNames = new List<string>()
		{
			"Striaton City",
			"Driftveil City",
			"Icirrus City",
			"Dreamyard 1",
			"Dreamyard 2",
			"Pinwheel Forest 1",
			"Pinwheel Forest 2",
			"Desert Resort 1",
			"Desert Resort 2",
			"Relic Castle 1",
			"Relic Castle 2",
			"Relic Castle 3",
			"Relic Castle 4",
			"Relic Castle 5",
			"Relic Castle 6",
			"Relic Castle 7",
			"Relic Castle 8",
			"Relic Castle 9",
			"Relic Castle 10",
			"Relic Castle 11",
			"Relic Castle 12",
			"Relic Castle 13",
			"Relic Castle 14",
			"Relic Castle 15",
			"Relic Castle 16",
			"Relic Castle 17",
			"Relic Castle 18",
			"Relic Castle 19",
			"Relic Castle 20",
			"Relic Castle 21",
			"Relic Castle 22",
			"Relic Castle 23",
			"Relic Castle 24",
			"Relic Castle 25",
			"Relic Castle 26",
			"Relic Castle 27",
			"Relic Castle 28",
			"Relic Castle 29",
			"Relic Castle 30",
			"Relic Castle 31",
			"Cold Storage",
			"Chargestone Cave 1",
			"Chargestone Cave 2",
			"Chargestone Cave 3",
			"Twist Mountain 1",
			"Twist Mountain 2",
			"Twist Mountain 3",
			"Twist Mountain 4",
			"Dragonspiral Tower 1",
			"Dragonspiral Tower 2",
			"Dragonspiral Tower 3",
			"Dragonspiral Tower 4",
			"Victory Road 1",
			"Victory Road 2",
			"Victory Road 3",
			"Victory Road 4",
			"Victory Road 5",
			"Victory Road 6",
			"Victory Road 7",
			"Victory Road 8",
			"Victory Road 9",
			"Victory Road 10",
			"Victory Road 11",
			"Victory Road 12",
			"Victory Road 13",
			"Victory Road 14",
			"Victory Road 15",
			"Trial Chamber",
			"Giant Chasm 1",
			"Giant Chasm 2",
			"Giant Chasm 3",
			"Giant Chasm 4",
			"P2 Laboratory",
			"Undella Bay",
			"Driftveil Drawbridge",
			"Village Bridge",
			"Marvelous Bridge",
			"Route 1",
			"Route 2",
			"Route 3",
			"Wellspring Cave 1",
			"Wellspring Cave 2",
			"Route 4",
			"Route 5",
			"Route 6",
			"Mistralton Cave 1",
			"Mistralton Cave 2",
			"Guidance Chamber",
			"Route 7",
			"Celestial Tower 1",
			"Celestial Tower 2",
			"Celestial Tower 3",
			"Celestial Tower 4",
			"Route 8",
			"Moor of Icirrus",
			"Route 9",
			"Challenger's Cave 1",
			"Challenger's Cave 2",
			"Challenger's Cave 3",
			"Route 10 1",
			"Route 10 2",
			"Route 11",
			"Route 12",
			"Route 13",
			"Route 14",
			"Abundant Shrine",
			"Route 15",
			"Route 16",
			"Lostlorn Forest",
			"Route 18",
			"Undella Town",
			"Route 17"
		};

        public static List<int[]> habitatListEntries = new List<int[]>()
		{
			new int[] { 104, 105, 10 }, // Route 4
			new int[] { 124 }, // Route 15
			new int[] { 134 }, // Route 21
			new int[] { 84, 85, 86 }, // Clay Tunnel
			new int[] { 23, 24, 25, 26 }, // Twist Mountain
			new int[] { 97 }, // Village Bridge
			new int[] { 27, 28, 29, 30 }, // Dragonspiral Tower
			new int[] { 81, 82, 83 }, // Relic Passage
			new int[] { 106 }, // Route 5*
			new int[] { 125 }, // Route 16*
			new int[] { 98 }, // Marvelous Bridge
			new int[] { 123 }, // Abundant Shrine
			new int[] { 132 }, // Undella Town
			new int[] { 107 }, // Route 6
			new int[] { 43 }, // Undella Bay
			new int[] { 102, 103 }, // Wellspring Cave
			new int[] { 95 }, // Nature Preserve
			new int[] { 127 }, // Route 18
			new int[] { 32, 33, 34, 35, 36 }, // Giant Chasm
			new int[] { 111 }, // Route 7
			new int[] { 31, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 }, // Victory Road
			new int[] { 12, 13, 14, 15, 16, 17, 18, 19 }, // Relic Castle
			new int[] { 0 }, // Striation City
			new int[] { 128 }, // Route 19
			new int[] { 3 }, // Aspertia City
			new int[] { 116 }, // Route 8*
			new int[] { 44, 45 }, // Floccesy Ranch
			new int[] { 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 }, // Strange House
			new int[] { 129 }, // Route 20
			new int[] { 4 }, // Virbank City
			new int[] { 37, 38, 39, 40, 41 }, // Castelia Sewers
			new int[] { 118 }, // Route 9
			new int[] { 46, 47 }, // Virbank Complex
			new int[] { 42 }, // P2 Laboratory
			new int[] { 1 }, // Castelia City
			new int[] { 8, 9 }, // Pinwheel Forest
			new int[] { 5 }, // Humilau City
			new int[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 }, // Reversal Mountain
			new int[] { 6, 7 }, // Dreamyard
			new int[] { 112, 113, 114, 115 }, // Celestial Tower
			new int[] { 130 }, // Route 22
			new int[] { 11 }, // Desert Resort
			new int[] { 119 }, // Route 11
			new int[] { 133 }, // Route 17
			new int[] { 99 }, // Route 1
			new int[] { 131 }, // Route 23
			new int[] { 2 }, // Icirrus City*
			new int[] { 120 }, // Route 12
			new int[] { 100 }, // Route 2
			new int[] { 108, 109 }, // Mistralton Cave
			new int[] { 121 }, // Route 13
			new int[] { 101 }, // Route 3
			new int[] { 117 }, // Moor of Icirrus*
			new int[] { 96 }, // Driftveil Drawbridge
			new int[] { 93, 94 }, // Seaside Cave
			new int[] { 126 }, // Lostlorn Forest
			new int[] { 122 }, // Route 14
			new int[] { 20, 21, 22 }, // Chargestone Cave
		};
    }
}
