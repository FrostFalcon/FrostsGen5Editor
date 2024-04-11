using NewEditor.Data.NARCTypes;
using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace NewEditor.Data
{
    public static class CustomGameModeManager
    {
        public static Random random;

        public static List<RefByte> StarterHouseGivesStarterScriptBytes = new List<RefByte>()
        {
            0x42, 0x00, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0xD2, 0x00, 0x00, 0x00, 0xF2, 0x00, 0x00, 0x00, 0x2E, 0x03, 0x00, 0x00, 0x2C, 0x03, 0x00, 0x00, 0x2A, 0x03, 0x00, 0x00, 0x28, 0x03, 0x00, 0x00, 0x26, 0x03, 0x00, 0x00, 0x24, 0x03, 0x00, 0x00, 0x22, 0x03, 0x00, 0x00, 0x20, 0x03, 0x00, 0x00, 0x1E, 0x03, 0x00, 0x00, 0x1C, 0x03, 0x00, 0x00, 0x1A, 0x03, 0x00, 0x00, 0x18, 0x03, 0x00, 0x00, 0x16, 0x03, 0x00, 0x00, 0x13, 0xFD, 0x2E, 0x00, 0x10, 0x00, 0x66, 0x09, 0x08, 0x00, 0x01, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x1A, 0x00, 0x00, 0x00, 0xA6, 0x00, 0x47, 0x05, 0x74, 0x00, 0x3D, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x00, 0x3E, 0x00, 0x1E, 0x00, 0x14, 0x00, 0x00, 0x00, 0xA6, 0x00, 0x47, 0x05, 0x74, 0x00, 0x3D, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x00, 0x3E, 0x00, 0x30, 0x00, 0x2F, 0x00, 0x02, 0x00, 0x2E, 0x00, 0xD5, 0x00, 0x10, 0x80, 0x00, 0x00, 0x09, 0x00, 0x10, 0x80, 0x08, 0x00, 0x01, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x1A, 0x00, 0x00, 0x00, 0xA6, 0x00, 0x47, 0x05, 0x74, 0x00, 0x3D, 0x00, 0x00, 0x04, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x00, 0x3E, 0x00, 0x1E, 0x00, 0x14, 0x00, 0x00, 0x00, 0xA6, 0x00, 0x47, 0x05, 0x74, 0x00, 0x3D, 0x00, 0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x00, 0x3E, 0x00, 0x30, 0x00, 0x2F, 0x00, 0x02, 0x00, 0x2E, 0x00, 0xA6, 0x00, 0x47, 0x05, 0x74, 0x00, 0xAB, 0x00, 0xFA, 0x01, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x04, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0xAC, 0x00, 0x32, 0x00, 0x3E, 0x00, 0x30, 0x00, 0x2F, 0x00, 0x02, 0x00, 0x2E, 0x00, 0x74, 0x00, 0x10, 0x00, 0x61, 0x09, 0x08, 0x00, 0x01, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x18, 0x00, 0x00, 0x00, 0xA6, 0x00, 0x47, 0x05, 0x3D, 0x00, 0x00, 0x04, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x00, 0x3F, 0x00, 0x30, 0x00, 0x2F, 0x00, 0x02, 0x00, 0xCB, 0x00, 0x25, 0x80, 0x0F, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x00, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x01, 0x00, 0x1E, 0x00, 0xB2, 0x01, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x01, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x04, 0x00, 0x1E, 0x00, 0x93, 0x01, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x02, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x07, 0x00, 0x1E, 0x00, 0x74, 0x01, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x03, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x98, 0x00, 0x1E, 0x00, 0x55, 0x01, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x04, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x9B, 0x00, 0x1E, 0x00, 0x36, 0x01, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x05, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x9E, 0x00, 0x1E, 0x00, 0x17, 0x01, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x06, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0xFC, 0x00, 0x1E, 0x00, 0xF8, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x07, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0xFF, 0x00, 0x1E, 0x00, 0xD9, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x08, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x02, 0x01, 0x1E, 0x00, 0xBA, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x09, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x83, 0x01, 0x1E, 0x00, 0x9B, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x0A, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x86, 0x01, 0x1E, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x0B, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0x89, 0x01, 0x1E, 0x00, 0x5D, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x0C, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0xEF, 0x01, 0x1E, 0x00, 0x3E, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x0D, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0xF2, 0x01, 0x1E, 0x00, 0x1F, 0x00, 0x00, 0x00, 0x09, 0x00, 0x25, 0x80, 0x08, 0x00, 0x0E, 0x00, 0x11, 0x00, 0x01, 0x00, 0x1F, 0x00, 0xFF, 0x0C, 0x00, 0x00, 0x00, 0x28, 0x00, 0x25, 0x80, 0xF5, 0x01, 0x1E, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA9, 0x00, 0x18, 0x05, 0x34, 0x00, 0x02, 0x00, 0x01, 0x00, 0xAA, 0x00, 0x4B, 0x00, 0x36, 0x00, 0x0C, 0x01, 0x10, 0x80, 0x25, 0x80, 0x00, 0x00, 0x05, 0x00, 0x23, 0x00, 0x61, 0x09, 0x23, 0x00, 0x62, 0x09, 0xE3, 0x00, 0x28, 0x00, 0x25, 0x80, 0x00, 0x00, 0x04, 0x00, 0x80, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x30, 0x00, 0x2F, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x02, 0x00, 0x2E, 0x00, 0x3F, 0x01, 0x41, 0x01, 0x43, 0x01, 0xD8, 0x25, 0x00, 0x00, 0x00, 0xD0, 0x0E, 0x00, 0x00, 0x80, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x0A, 0x00, 0x01, 0x00, 0x45, 0x01, 0x24, 0x00, 0xE4, 0x02, 0x6B, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0xA3, 0x01, 0xA7, 0x01, 0xA6, 0x00, 0x59, 0x05, 0xA8, 0x00, 0x4C, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x44, 0x01, 0x28, 0x00, 0x45, 0x01, 0x42, 0x01, 0x40, 0x01, 0x30, 0x00, 0x2F, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00
        };

        public static List<int> verticalGates = new List<int>()
        {
            438, 51, 372
        };

        public static List<int> horizontalGates = new List<int>()
        {
            447, 91, 92, 131, 133, 366, 369, 27, 320
        };

        public static List<int> GymZones = new List<int>()
        {
            489, 502, 29, 488, 35, 63, 97, 98, 108, 121, 465, 140, 141, 142, 143
        };

        public static List<short> bannedPokemon = new List<short>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 144, 145, 146, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 243, 244, 245, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 500, 501, 502, 503, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649
        };

        public static void NoStoryMode()
        {
            Program.main.taskProgressBar.Minimum = 1;
            Program.main.taskProgressBar.Maximum = 3;
            Program.main.taskProgressBar.Value = 1;
            Program.main.taskProgressBar.Step = 1;

            random = new Random();
            foreach (OverworldObjectsEntry o in MainEditor.overworldsNarc.objects)
            {
                StandardizeXpYeilds();

                int i = MainEditor.overworldsNarc.objects.IndexOf(o);
                if (i != 167 && !GymZones.Exists(ow => i == MainEditor.zoneDataNarc.zones[ow].mapId) &&
                    MainEditor.zoneDataNarc.zones.Exists(z => z.mapId == i) && MainEditor.zoneDataNarc.zones.FirstOrDefault(z => z.mapId == i).matrix != 13) o.NPCs?.Clear();
                if (!GymZones.Contains(MainEditor.overworldsNarc.objects.IndexOf(o))) o.triggers?.Clear();
            }

            Program.main.taskProgressBar.PerformStep();

            MainEditor.scriptNarc.scriptFiles[856].bytes = StarterHouseGivesStarterScriptBytes.ToArray();
            ModifyStarterHouseText();

            SwapWarps(453, 1, 252, 0);

            Program.main.taskProgressBar.PerformStep();

            //Routes 19, 20, Floccesy ranch
            RandomizeEncounters(164, 2, 4);
            RandomizeEncounters(165, 3, 5);
            for (int n = 166; n <= 168; n++) CopyEncounters(165, n);
            RandomizeEncounters(69, 4, 7);

            //Virbank complex
            RandomizeEncounters(70, 10, 13);
            RandomizeEncounters(71, 11, 14);

            //Castelia and route 4
            RandomizeEncounters(1, 16, 20);
            RandomizeEncounters(58, 14, 18);
            for (int n = 59; n <= 62; n++) CopyEncounters(58, n);
            RandomizeEncounters(105, 16, 20);
            RandomizeEncounters(128, 14, 18);
            CopyEncounters(128, 129);

            //Desert Resort and Nimbasa
            RandomizeEncounters(13, 20, 22);
            CopyEncounters(13, 14);
            RandomizeEncounters(15, 22, 24);
            CopyEncounters(15, 16);
            RandomizeEncounters(130, 22, 24);
            RandomizeEncounters(161, 22, 24);
            RandomizeEncounters(162, 24, 26);

            //Relic Passage and Castle
            RandomizeEncounters(106, 24, 26);
            CopyEncounters(106, 107);
            RandomizeEncounters(17, 26, 28);
            for (int n = 18; n <= 22; n++) CopyEncounters(17, n);

            //Clay Tunnel and Axew Cave
            RandomizeEncounters(108, 24, 26);
            for (int n = 109; n <= 116; n++) CopyEncounters(108, n);
            RandomizeEncounters(135, 28, 30);
            CopyEncounters(135, 136);
            CopyEncounters(135, 137);

            //Route 6 and Chargestone Cave
            RandomizeEncounters(131, 24, 26);
            for (int n = 132; n <= 134; n++) CopyEncounters(131, n);
            RandomizeEncounters(23, 26, 28);
            for (int n = 24; n <= 25; n++) CopyEncounters(23, n);

            //Route 7 and Celestial Tower
            RandomizeEncounters(138, 30, 32);
            for (int n = 139; n <= 141; n++) CopyEncounters(138, n);
            RandomizeEncounters(142, 32, 34);
            for (int n = 143; n <= 145; n++) CopyEncounters(142, n);

            //Twist Mountain
            RandomizeEncounters(26, 36, 38);
            for (int n = 27; n <= 41; n++) CopyEncounters(26, n);

            //Icirrus City
            RandomizeEncounters(2, 40, 42);
            for (int n = 3; n <= 5; n++) CopyEncounters(2, n);
            RandomizeEncounters(42, 42, 44);
            for (int n = 43; n <= 51; n++) CopyEncounters(42, n);
            RandomizeEncounters(146, 41, 43);
            for (int n = 147; n <= 149; n++) CopyEncounters(146, n);
            RandomizeEncounters(150, 42, 44);
            for (int n = 151; n <= 153; n++) CopyEncounters(150, n);

            //Opelucid
            RandomizeEncounters(154, 43, 45);
            RandomizeEncounters(155, 43, 45);

            //Village Bridge to Undella
            RandomizeEncounters(121, 40, 42);
            RandomizeEncounters(156, 37, 39);
            RandomizeEncounters(157, 36, 38);

            //Revesal Mountain
            RandomizeEncounters(73, 32, 34);
            for (int n = 74; n <= 84; n++) CopyEncounters(73, n);
            RandomizeEncounters(72, 34, 36);
            RandomizeEncounters(85, 36, 38);
            for (int n = 86; n <= 94; n++) CopyEncounters(85, n);

            //Route 14 to Nimbasa
            RandomizeEncounters(158, 30, 32);
            RandomizeEncounters(160, 29, 31);

            //Humilau and Giant Chasm
            RandomizeEncounters(169, 46, 49);
            RandomizeEncounters(53, 48, 50);
            for (int n = 54; n <= 57; n++) CopyEncounters(53, n);

            //Victory Road
            RandomizeEncounters(170, 50, 52);
            RandomizeEncounters(95, 52, 54);
            for (int n = 96; n <= 104; n++) CopyEncounters(95, n);

            Program.main.taskProgressBar.PerformStep();

            MessageBox.Show("No Story Mode Loaded");
        }

        public static void RandomizeEncounters(int encounterFile, byte minLv, byte maxLv)
        {
            EncounterEntry route = MainEditor.encounterNarc.encounterPools[encounterFile];

            int minBST = 180 + 5 * minLv;
            int maxBST = 280 + 5 * maxLv;

            List<PokemonEntry> options = MainEditor.pokemonDataNarc.pokemon.Where(p => p.baseStatTotal >= minBST && p.baseStatTotal <= maxBST && !bannedPokemon.Contains((short)p.nameID) && MainEditor.pokemonDataNarc.pokemon.IndexOf(p) <= 640
            && !MainEditor.pokemonDataNarc.pokemon.Exists(p2 => p2.evolutions != null && new List<EvolutionMethod>(p2.evolutions.methods).Exists(e => e.newPokemonID == p.nameID && e.method == 4 && e.condition >= maxLv))).ToList();
            
            
            
            if (options.Count < 4) return;
            for (int j = 0; j < 3; j++) route.groupedLandSlots[j].Clear();

            for (int i = 0; i < 4; i++)
            {
                int r = random.Next(options.Count);
                for (int j = 0; j < 3; j++)
                {
                    route.groupedLandSlots[j].Add(new EncounterSlot((short)MainEditor.pokemonDataNarc.pokemon.IndexOf(options[r]), 0, minLv, maxLv, 25));
                }
                options.RemoveAt(r);
            }
            route.ApplyData();
        }

        public static void CopyEncounters(int from, int to)
        {
            EncounterEntry fromRoute = MainEditor.encounterNarc.encounterPools[from];
            EncounterEntry toRoute = MainEditor.encounterNarc.encounterPools[to];

            for (int j = 0; j < 3; j++)
            {
                toRoute.groupedLandSlots[j] = new List<EncounterSlot>(fromRoute.groupedLandSlots[j].ToArray());
            }
            toRoute.ApplyData();
        }

        public static void StandardizeXpYeilds()
        {
            foreach (PokemonEntry p in MainEditor.pokemonDataNarc.pokemon)
            {
                p.xpYield = (short)(p.baseStatTotal / 1.5f);
                p.ApplyData();
            }
        }

        public static void RandomizeWarpPoints()
        {
            List<OverworldWarp> topWarps = new List<OverworldWarp>();
            List<OverworldWarp> bottomWarps = new List<OverworldWarp>();
            List<OverworldWarp> leftWarps = new List<OverworldWarp>();
            List<OverworldWarp> rightWarps = new List<OverworldWarp>();

            List<(short, short)> topDests = new List<(short, short)>();
            List<(short, short)> bottomDests = new List<(short, short)>();
            List<(short, short)> leftDests = new List<(short, short)>();
            List<(short, short)> rightDests = new List<(short, short)>();

            foreach (int i in verticalGates)
            {
                int mapID = MainEditor.zoneDataNarc.zones[i].mapId;
                int flip = MainEditor.overworldsNarc.objects[mapID].warps[0].exitY == 15204352 ? 1 : 0;
                bottomWarps.Add(MainEditor.overworldsNarc.objects[mapID].warps[flip]);
                bottomDests.Add((MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationMap, MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationWarp));
                flip = (flip + 1) % 2;
                topWarps.Add(MainEditor.overworldsNarc.objects[mapID].warps[flip]);
                topDests.Add((MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationMap, MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationWarp));
            }
            foreach (int i in horizontalGates)
            {
                int mapID = MainEditor.zoneDataNarc.zones[i].mapId;
                int flip = MainEditor.overworldsNarc.objects[mapID].warps[0].exitX == 16252928 ? 0 : 1;
                leftWarps.Add(MainEditor.overworldsNarc.objects[mapID].warps[flip]);
                leftDests.Add((MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationMap, MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationWarp));
                flip = (flip + 1) % 2;
                rightWarps.Add(MainEditor.overworldsNarc.objects[mapID].warps[flip]);
                rightDests.Add((MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationMap, MainEditor.overworldsNarc.objects[mapID].warps[flip].destinationWarp));
            }

            for (int i = 0; i < topWarps.Count; i++)
            {
                int r = random.Next(topDests.Count);
                topWarps[i].destinationMap = topDests[r].Item1;
                topWarps[i].destinationWarp = topDests[r].Item2;
                OverworldWarp opposite = MainEditor.overworldsNarc.objects[MainEditor.zoneDataNarc.zones[topDests[r].Item1].mapId].warps[topDests[r].Item2];
                opposite.destinationMap = (short)verticalGates[i];
                opposite.destinationWarp = (short)(topWarps[i].exitX == 15204352 ? 1 : 0);
                topDests.RemoveAt(r);

                r = random.Next(bottomDests.Count);
                bottomWarps[i].destinationMap = bottomDests[r].Item1;
                bottomWarps[i].destinationWarp = bottomDests[r].Item2;
                opposite = MainEditor.overworldsNarc.objects[MainEditor.zoneDataNarc.zones[bottomDests[r].Item1].mapId].warps[bottomDests[r].Item2];
                opposite.destinationMap = (short)verticalGates[i];
                opposite.destinationWarp = (short)(topWarps[i].exitX == 15204352 ? 0 : 1);
                bottomDests.RemoveAt(r);
            }

            for (int i = 0; i < leftWarps.Count; i++)
            {
                int r = random.Next(leftDests.Count);
                leftWarps[i].destinationMap = leftDests[r].Item1;
                leftWarps[i].destinationWarp = leftDests[r].Item2;
                OverworldWarp opposite = MainEditor.overworldsNarc.objects[MainEditor.zoneDataNarc.zones[leftDests[r].Item1].mapId].warps[leftDests[r].Item2];
                opposite.destinationMap = (short)horizontalGates[i];
                opposite.destinationWarp = (short)(leftWarps[i].exitX == 16252928 ? 1 : 0);
                leftDests.RemoveAt(r);

                r = random.Next(rightDests.Count);
                rightWarps[i].destinationMap = rightDests[r].Item1;
                rightWarps[i].destinationWarp = rightDests[r].Item2;
                opposite = MainEditor.overworldsNarc.objects[MainEditor.zoneDataNarc.zones[rightDests[r].Item1].mapId].warps[rightDests[r].Item2];
                opposite.destinationMap = (short)horizontalGates[i];
                opposite.destinationWarp = (short)(leftWarps[i].exitX == 16252928 ? 0 : 1);
                rightDests.RemoveAt(r);
            }

            foreach (int i in verticalGates) MainEditor.overworldsNarc.objects[i].ApplyData();
            foreach (int i in horizontalGates) MainEditor.overworldsNarc.objects[i].ApplyData();
        }

        public static void ModifyStarterHouseText()
        {
            MainEditor.storyTextNarc.textFiles[173].text[2] = "You received a starter!";
            MainEditor.storyTextNarc.textFiles[173].text[6] = "Good Luck!";
            MainEditor.storyTextNarc.textFiles[173].CompressData();
        }

        public static void SwapWarps(short map1, short warp1, short map2, short warp2)
        {
            int mapID = MainEditor.zoneDataNarc.zones[map1].mapId;
            OverworldWarp w = MainEditor.overworldsNarc.objects[mapID].warps[warp1];
            w.destinationMap = map2;
            w.destinationWarp = warp2;
            MainEditor.overworldsNarc.objects[mapID].ApplyData();

            mapID = MainEditor.zoneDataNarc.zones[map2].mapId;
            w = MainEditor.overworldsNarc.objects[mapID].warps[warp2];
            w.destinationMap = map1;
            w.destinationWarp = warp1;
            MainEditor.overworldsNarc.objects[mapID].ApplyData();
        }

        public static void RogueMode()
        {
            random = new Random();
            foreach (OverworldObjectsEntry o in MainEditor.overworldsNarc.objects)
            {
                StandardizeXpYeilds();

                int i = MainEditor.overworldsNarc.objects.IndexOf(o);
                if (i != 167 && !GymZones.Exists(ow => i == MainEditor.zoneDataNarc.zones[ow].mapId) &&
                    MainEditor.zoneDataNarc.zones.Exists(z => z.mapId == i) && MainEditor.zoneDataNarc.zones.FirstOrDefault(z => z.mapId == i).matrix != 13) o.NPCs?.Clear();
                if (!GymZones.Contains(MainEditor.overworldsNarc.objects.IndexOf(o))) o.triggers?.Clear();
            }

            Program.main.taskProgressBar.PerformStep();

            MainEditor.scriptNarc.scriptFiles[856].bytes = StarterHouseGivesStarterScriptBytes.ToArray();
            ModifyStarterHouseText();
        }
    }
}
