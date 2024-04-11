﻿using NewEditor.Data.NARCTypes;
using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Data
{
    public class NDSFileSystem
    {
        private string romType = "";
        public RomType RomType => (romType == "pokemon b2" || romType == "pokemon w2") ? RomType.BW2 : (romType == "pokemon hg" || romType == "pokemon ss") ? RomType.HGSS : RomType.Other;

        int OverlayCount = 0;
        int NARCCount = 0;
        int MiscCount = 0;

        const int ARM9_PointerLocation = 0x20;
        const int ARM9_SizeLocation = 0x2C;
        const int ARM7_PointerLocation = 0x30;
        const int ARM7_SizeLocation = 0x3C;
        const int FNT_PointerLocation = 0x40;
        const int FNT_SizeLocation = 0x44;
        const int FAT_PointerLocation = 0x48;
        const int FAT_SizeLocation = 0x4C;
        const int Y9_PointerLocation = 0x50;
        const int Y9_SizeLocation = 0x54;
        const int Banner_PointerLocation = 0x68;
        const int End_PointerLocation = 0x1F0;

        /* Order
            - romHeader
            - arm9
            - y9
            - overlays
            - arm7
            - fnt
            - fat
            - banner
            - skb
            - soundStatus
            - soundData
            - narcs
            - misc
            - romEnder
        */

        public List<byte> romHeader;
        public List<byte> arm9;
        public List<byte> arm7;
        public List<byte> fnt;
        public List<byte> fat;
        public List<byte> y9;
        public List<byte> banner;
        public List<byte> romEnder;

        List<byte> skb;
        List<byte> soundStatus;
        public SoundData soundData;

        public List<List<byte>> overlays;
        public List<NARC> narcs;

        public List<List<byte>> misc;

        Dictionary<int, Type> narcIDs;
        public TextNARC textNarc;
        public TextNARC storyTextNarc;
        public PokemonSpritesNARC pokemonSpritesNarc;
        public PokemonIconNARC pokemonIconNarc;
        public PokemonDataNARC pokemonDataNarc;
        public LearnsetNARC learnsetNarc;
        public EvolutionDataNARC evolutionsNarc;
        public MoveDataNARC moveDataNarc;
        public ItemDataNARC itemDataNarc;
        public MoveAnimationNARC moveAnimationNarc;
        public ZoneDataNARC zoneDataNarc;
        public MapMatrixNARC mapMatrixNarc;
        public MapModelsNARC mapModelsNarc;
        public ScriptNARC scriptNarc;
        public TrTextEntriesNARC trTextEntriesNarc;
        public TrTextIndexNARC trTextIndicesNarc;
        public TrainerDataNARC trainerNarc;
        public TrainerPokeNARC trainerPokeNarc;
        public OverworldObjectsNARC overworldsNarc;
        public EncounterNARC encounterNarc;
        public PokemartNARC pokemartNarc;
        public KeyboardNARC keyboardNarc;

        //Read from rom file
        public static NDSFileSystem FromRom(FileStream fs, bool setEditorNarcs = false)
        {
            NDSFileSystem result = new NDSFileSystem();
            byte[] romTypeBytes = new byte[16];
            fs.Read(romTypeBytes, 0, 16);
            result.romType = Encoding.ASCII.GetString(romTypeBytes).ToLower();
            result.romType = result.romType.Remove(result.romType.IndexOf((char)0));

            MainEditor.GetVersionConstants(result.romType);

            //Populate static fields
            if (result.RomType == RomType.BW2)
            {
                result.OverlayCount = 344;
                result.NARCCount = 308;
                result.MiscCount = 7;
            }
            result.narcIDs = new Dictionary<int, Type>()
            {
                { MainEditor.encounterNarcID, typeof(EncounterNARC) },
                { MainEditor.evolutionNarcID, typeof(EvolutionDataNARC) },
                { MainEditor.keyboardNarcID, typeof(KeyboardNARC) },
                { MainEditor.levelUpMovesNarcID, typeof(LearnsetNARC) },
                { MainEditor.mapMatrixNarcID, typeof(MapMatrixNARC) },
                { MainEditor.mapModelsNarcID, typeof(MapModelsNARC) },
                { MainEditor.moveAnimationNarcID, typeof(MoveAnimationNARC) },
                { MainEditor.moveDataNarcID, typeof(MoveDataNARC) },
                { MainEditor.itemDataNarcID, typeof(ItemDataNARC) },
                { MainEditor.overworldsNarcID, typeof(OverworldObjectsNARC) },
                { MainEditor.pokemartNarcID, typeof(PokemartNARC) },
                { MainEditor.pokemonDataNarcID, typeof(PokemonDataNARC) },
                { MainEditor.pokemonIconsNarcID, typeof(PokemonIconNARC) },
                { MainEditor.pokemonSpritesNarcID, typeof(PokemonSpritesNARC) },
                { MainEditor.scriptNarcID, typeof(ScriptNARC) },
                { MainEditor.textNarcID, typeof(TextNARC) },
                { MainEditor.storyTextNarcID, typeof(TextNARC) },
                { MainEditor.trTextEntriesNarcID, typeof(TrTextEntriesNARC) },
                { MainEditor.trTextIndicesNarcID, typeof(TrTextIndexNARC) },
                { MainEditor.trainerDataNarcID, typeof(TrainerDataNARC) },
                { MainEditor.trainerPokeNarcID, typeof(TrainerPokeNARC) },
                { MainEditor.zoneDataNarcID, typeof(ZoneDataNARC) },
            };
            result.overlays = new List<List<byte>>();
            result.narcs = new List<NARC>();
            result.misc = new List<List<byte>>();

            //Get Rom Header
            fs.Position = ARM9_PointerLocation;
            int arm9Loc = fs.ReadInt();

            byte[] b = new byte[arm9Loc];
            fs.Position = 0;
            fs.Read(b, 0, b.Length);
            result.romHeader = new List<byte>(b);



            //Get arms and tables
            b = new byte[HelperFunctions.ReadInt(result.romHeader, ARM9_SizeLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, ARM9_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.arm9 = new List<byte>(b);

            b = new byte[HelperFunctions.ReadInt(result.romHeader, ARM7_SizeLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, ARM7_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.arm7 = new List<byte>(b);

            b = new byte[HelperFunctions.ReadInt(result.romHeader, FNT_SizeLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, FNT_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.fnt = new List<byte>(b);

            b = new byte[HelperFunctions.ReadInt(result.romHeader, FAT_SizeLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, FAT_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.fat = new List<byte>(b);

            b = new byte[HelperFunctions.ReadInt(result.romHeader, Y9_SizeLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, Y9_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.y9 = new List<byte>(b);

            b = new byte[HelperFunctions.ReadInt(result.fat, result.OverlayCount * 8) - HelperFunctions.ReadInt(result.romHeader, Banner_PointerLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, Banner_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.banner = new List<byte>(b);



            //Read FAT data for overlays, narcs, etc
            int pos = 0;
            for (int i = 0; i < result.OverlayCount; i++)
            {
                int oStart = HelperFunctions.ReadInt(result.fat, pos);
                int oEnd = HelperFunctions.ReadInt(result.fat, pos + 4);
                b = new byte[oEnd - oStart];
                fs.Position = oStart;
                fs.Read(b, 0, b.Length);
                bool compressed = result.y9[i * 32 + 31] == 3;
                if (compressed)
                {
                    byte[] ov = BLZDecoder.BLZ_DecodePub(b);
                    if (ov == null) result.overlays.Add(new List<byte>(b));
                    else
                    {
                        result.overlays.Add(new List<byte>(ov));
                        result.y9[i * 32 + 31] = 2;
                    }
                }
                else
                {
                    result.overlays.Add(new List<byte>(b));
                }
                pos += 8;
            }

            int start = HelperFunctions.ReadInt(result.fat, pos);
            int end = HelperFunctions.ReadInt(result.fat, pos + 4);
            b = new byte[end - start];
            fs.Position = start;
            fs.Read(b, 0, b.Length);
            result.skb = new List<byte>(b);
            pos += 8;

            start = HelperFunctions.ReadInt(result.fat, pos);
            end = HelperFunctions.ReadInt(result.fat, pos + 4);
            b = new byte[end - start];
            fs.Position = start;
            fs.Read(b, 0, b.Length);
            result.soundStatus = new List<byte>(b);
            pos += 8;

            start = HelperFunctions.ReadInt(result.fat, pos);
            end = HelperFunctions.ReadInt(result.fat, pos + 4);
            result.soundData = new SoundData(fs, start, end);
            pos += 8;

            for (int i = 0; i < result.NARCCount; i++)
            {
                int oStart = HelperFunctions.ReadInt(result.fat, pos);
                int oEnd = HelperFunctions.ReadInt(result.fat, pos + 4);
                b = new byte[oEnd - oStart];
                fs.Position = oStart;
                fs.Read(b, 0, b.Length);
                if (result.narcIDs.ContainsKey(i)) result.narcs.Add((NARC)Activator.CreateInstance(result.narcIDs[i]));
                else result.narcs.Add(new NARC());
                result.narcs[i].fileSystem = result;
                result.narcs[i].ID = (short)i;
                result.narcs[i].byteData = b;
                pos += 8;
            }

            if (setEditorNarcs) MainEditor.SetNARCVars(result);
            result.textNarc = result.narcs[MainEditor.textNarcID] as TextNARC;
            result.storyTextNarc = result.narcs[MainEditor.storyTextNarcID] as TextNARC;
            result.pokemonSpritesNarc = result.narcs[MainEditor.pokemonSpritesNarcID] as PokemonSpritesNARC;
            result.pokemonIconNarc = result.narcs[MainEditor.pokemonIconsNarcID] as PokemonIconNARC;
            result.pokemonDataNarc = result.narcs[MainEditor.pokemonDataNarcID] as PokemonDataNARC;
            result.learnsetNarc = result.narcs[MainEditor.levelUpMovesNarcID] as LearnsetNARC;
            result.evolutionsNarc = result.narcs[MainEditor.evolutionNarcID] as EvolutionDataNARC;
            result.moveDataNarc = result.narcs[MainEditor.moveDataNarcID] as MoveDataNARC;
            result.itemDataNarc = result.narcs[MainEditor.itemDataNarcID] as ItemDataNARC;
            result.moveAnimationNarc = result.narcs[MainEditor.moveAnimationNarcID] as MoveAnimationNARC;
            result.zoneDataNarc = result.narcs[MainEditor.zoneDataNarcID] as ZoneDataNARC;
            result.mapMatrixNarc = result.narcs[MainEditor.mapMatrixNarcID] as MapMatrixNARC;
            result.mapModelsNarc = result.narcs[MainEditor.mapModelsNarcID] as MapModelsNARC;
            result.scriptNarc = result.narcs[MainEditor.scriptNarcID] as ScriptNARC;
            result.trTextEntriesNarc = result.narcs[MainEditor.trTextEntriesNarcID] as TrTextEntriesNARC;
            result.trTextIndicesNarc = result.narcs[MainEditor.trTextIndicesNarcID] as TrTextIndexNARC;
            result.trainerNarc = result.narcs[MainEditor.trainerDataNarcID] as TrainerDataNARC;
            result.trainerPokeNarc = result.narcs[MainEditor.trainerPokeNarcID] as TrainerPokeNARC;
            result.overworldsNarc = result.narcs[MainEditor.overworldsNarcID] as OverworldObjectsNARC;
            result.encounterNarc = result.narcs[MainEditor.encounterNarcID] as EncounterNARC;
            result.pokemartNarc = result.narcs[MainEditor.pokemartNarcID] as PokemartNARC;
            result.keyboardNarc = result.narcs[MainEditor.keyboardNarcID] as KeyboardNARC;

            for (int i = 0; i < result.NARCCount; i++)
            {
                result.narcs[i].ReadData();
            }

            while (pos < result.fat.Count)
            {
                int oStart = HelperFunctions.ReadInt(result.fat, pos);
                int oEnd = HelperFunctions.ReadInt(result.fat, pos + 4);
                b = new byte[oEnd - oStart];
                fs.Position = oStart;
                fs.Read(b, 0, b.Length);
                result.misc.Add(new List<byte>(b));
                pos += 8;
            }

            //Get Rom Ender
            b = new byte[fs.Length - HelperFunctions.ReadInt(result.romHeader, End_PointerLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, End_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.romEnder = new List<byte>(b);
            return result;
        }

        public static NDSFileSystem FromFileSystem(string rootFolder, bool setEditorNarcs = false)
        {
            NDSFileSystem result = new NDSFileSystem();

            try
            {
                result.romHeader = new List<byte>(File.ReadAllBytes(rootFolder + "/header.bin"));
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not access header.bin file.\n" + e.Message);
                return null;
            }

            byte[] romTypeBytes = result.romHeader.GetRange(0, 16).ToArray();
            result.romType = Encoding.ASCII.GetString(romTypeBytes).ToLower();
            result.romType = result.romType.Remove(result.romType.IndexOf((char)0));

            MainEditor.GetVersionConstants(result.romType);

            //Populate static fields
            if (result.RomType == RomType.BW2)
            {
                result.OverlayCount = 344;
                result.NARCCount = 308;
                result.MiscCount = 7;
            }
            result.narcIDs = new Dictionary<int, Type>()
            {
                { MainEditor.encounterNarcID, typeof(EncounterNARC) },
                { MainEditor.evolutionNarcID, typeof(EvolutionDataNARC) },
                { MainEditor.keyboardNarcID, typeof(KeyboardNARC) },
                { MainEditor.levelUpMovesNarcID, typeof(LearnsetNARC) },
                { MainEditor.mapMatrixNarcID, typeof(MapMatrixNARC) },
                { MainEditor.mapModelsNarcID, typeof(MapModelsNARC) },
                { MainEditor.moveAnimationNarcID, typeof(MoveAnimationNARC) },
                { MainEditor.moveDataNarcID, typeof(MoveDataNARC) },
                { MainEditor.itemDataNarcID, typeof(ItemDataNARC) },
                { MainEditor.overworldsNarcID, typeof(OverworldObjectsNARC) },
                { MainEditor.pokemartNarcID, typeof(PokemartNARC) },
                { MainEditor.pokemonDataNarcID, typeof(PokemonDataNARC) },
                { MainEditor.pokemonIconsNarcID, typeof(PokemonIconNARC) },
                { MainEditor.pokemonSpritesNarcID, typeof(PokemonSpritesNARC) },
                { MainEditor.scriptNarcID, typeof(ScriptNARC) },
                { MainEditor.textNarcID, typeof(TextNARC) },
                { MainEditor.storyTextNarcID, typeof(TextNARC) },
                { MainEditor.trTextEntriesNarcID, typeof(TrTextEntriesNARC) },
                { MainEditor.trTextIndicesNarcID, typeof(TrTextIndexNARC) },
                { MainEditor.trainerDataNarcID, typeof(TrainerDataNARC) },
                { MainEditor.trainerPokeNarcID, typeof(TrainerPokeNARC) },
                { MainEditor.zoneDataNarcID, typeof(ZoneDataNARC) },
            };
            result.overlays = new List<List<byte>>();
            result.narcs = new List<NARC>();
            result.misc = new List<List<byte>>();

            try
            {
                result.arm9 = new List<byte>(File.ReadAllBytes(rootFolder + "/arm9.bin"));
                result.arm7 = new List<byte>(File.ReadAllBytes(rootFolder + "/arm7.bin"));
                result.y9 = new List<byte>(File.ReadAllBytes(rootFolder + "/y9.bin"));
                result.fat = new List<byte>(File.ReadAllBytes(rootFolder + "/fat.bin"));
                result.fnt = new List<byte>(File.ReadAllBytes(rootFolder + "/fnt.bin"));
                result.banner = new List<byte>(File.ReadAllBytes(rootFolder + "/banner.bin"));
                result.skb = new List<byte>(File.ReadAllBytes(rootFolder + "/data/skb.bin"));
                result.soundStatus = new List<byte>(File.ReadAllBytes(rootFolder + "/data/SoundStatus.bin"));
                result.soundData = new SoundData(File.ReadAllBytes(rootFolder + "/data/SoundData.sdat"));
                result.romEnder = new List<byte>(File.ReadAllBytes(rootFolder + "/data/RomEnder.bin"));
                for (int i = 0; i < result.OverlayCount; i++) result.overlays.Add(new List<byte>(File.ReadAllBytes(rootFolder + $"/overlay/ov_{i:D3}.bin")));
                for (int i = 0; i < result.MiscCount; i++) result.misc.Add(new List<byte>(File.ReadAllBytes(rootFolder + $"/data/misc/{i}.bin")));
                for (int i = 0; i < result.NARCCount; i++)
                {
                    string narcPath = rootFolder + "/data/a/";
                    narcPath += (i / 100).ToString() + "/";
                    narcPath += ((i % 100) / 10).ToString() + "/";
                    if (result.narcIDs.ContainsKey(i)) result.narcs.Add((NARC)Activator.CreateInstance(result.narcIDs[i]));
                    else result.narcs.Add(new NARC());
                    result.narcs[i].fileSystem = result;
                    result.narcs[i].ID = (short)i;
                    result.narcs[i].ReadNarcDump(narcPath + (i % 10).ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not access a data file.\n" + e.Message);
                return null;
            }

            if (setEditorNarcs) MainEditor.SetNARCVars(result);
            result.textNarc = result.narcs[MainEditor.textNarcID] as TextNARC;
            result.storyTextNarc = result.narcs[MainEditor.storyTextNarcID] as TextNARC;
            result.pokemonSpritesNarc = result.narcs[MainEditor.pokemonSpritesNarcID] as PokemonSpritesNARC;
            result.pokemonIconNarc = result.narcs[MainEditor.pokemonIconsNarcID] as PokemonIconNARC;
            result.pokemonDataNarc = result.narcs[MainEditor.pokemonDataNarcID] as PokemonDataNARC;
            result.learnsetNarc = result.narcs[MainEditor.levelUpMovesNarcID] as LearnsetNARC;
            result.evolutionsNarc = result.narcs[MainEditor.evolutionNarcID] as EvolutionDataNARC;
            result.moveDataNarc = result.narcs[MainEditor.moveDataNarcID] as MoveDataNARC;
            result.itemDataNarc = result.narcs[MainEditor.itemDataNarcID] as ItemDataNARC;
            result.moveAnimationNarc = result.narcs[MainEditor.moveAnimationNarcID] as MoveAnimationNARC;
            result.zoneDataNarc = result.narcs[MainEditor.zoneDataNarcID] as ZoneDataNARC;
            result.mapMatrixNarc = result.narcs[MainEditor.mapMatrixNarcID] as MapMatrixNARC;
            result.mapModelsNarc = result.narcs[MainEditor.mapModelsNarcID] as MapModelsNARC;
            result.scriptNarc = result.narcs[MainEditor.scriptNarcID] as ScriptNARC;
            result.trTextEntriesNarc = result.narcs[MainEditor.trTextEntriesNarcID] as TrTextEntriesNARC;
            result.trTextIndicesNarc = result.narcs[MainEditor.trTextIndicesNarcID] as TrTextIndexNARC;
            result.trainerNarc = result.narcs[MainEditor.trainerDataNarcID] as TrainerDataNARC;
            result.trainerPokeNarc = result.narcs[MainEditor.trainerPokeNarcID] as TrainerPokeNARC;
            result.overworldsNarc = result.narcs[MainEditor.overworldsNarcID] as OverworldObjectsNARC;
            result.encounterNarc = result.narcs[MainEditor.encounterNarcID] as EncounterNARC;
            result.pokemartNarc = result.narcs[MainEditor.pokemartNarcID] as PokemartNARC;
            result.keyboardNarc = result.narcs[MainEditor.keyboardNarcID] as KeyboardNARC;
            for (int i = 0; i < result.NARCCount; i++)
            {
                result.narcs[i].ReadData();
            }

            return result;
        }

        public byte[] BuildRom()
        {
            List<byte> romBytes = new List<byte>();

            //Add rom sections
            AddSection(romBytes, romHeader);

            HelperFunctions.WriteInt(romHeader, ARM9_PointerLocation, romBytes.Count);
            HelperFunctions.WriteInt(romHeader, ARM9_SizeLocation, arm9.Count);
            AddSection(romBytes, arm9);

            HelperFunctions.WriteInt(romHeader, Y9_PointerLocation, romBytes.Count);
            HelperFunctions.WriteInt(romHeader, Y9_SizeLocation, y9.Count);
            AddSection(romBytes, y9);

            for (int i = 0; i < OverlayCount; i++)
            {
                HelperFunctions.WriteInt(fat, i * 8, romBytes.Count);
                HelperFunctions.WriteInt(fat, i * 8 + 4, romBytes.Count + overlays[i].Count);
                AddSection(romBytes, overlays[i]);
            }

            HelperFunctions.WriteInt(romHeader, ARM7_PointerLocation, romBytes.Count);
            HelperFunctions.WriteInt(romHeader, ARM7_SizeLocation, arm7.Count);
            AddSection(romBytes, arm7);

            HelperFunctions.WriteInt(romHeader, FNT_PointerLocation, romBytes.Count);
            HelperFunctions.WriteInt(romHeader, FNT_SizeLocation, fnt.Count);
            AddSection(romBytes, fnt);

            HelperFunctions.WriteInt(romHeader, FAT_PointerLocation, romBytes.Count);
            HelperFunctions.WriteInt(romHeader, FAT_SizeLocation, fat.Count);
            AddSection(romBytes, fat);

            HelperFunctions.WriteInt(romHeader, Banner_PointerLocation, romBytes.Count);
            AddSection(romBytes, banner);

            HelperFunctions.WriteInt(fat, OverlayCount * 8, romBytes.Count);
            HelperFunctions.WriteInt(fat, OverlayCount * 8 + 4, romBytes.Count + skb.Count);
            AddSection(romBytes, skb);

            HelperFunctions.WriteInt(fat, OverlayCount * 8 + 8, romBytes.Count);
            HelperFunctions.WriteInt(fat, OverlayCount * 8 + 12, romBytes.Count + soundStatus.Count);
            AddSection(romBytes, soundStatus);

            HelperFunctions.WriteInt(fat, OverlayCount * 8 + 16, romBytes.Count);
            HelperFunctions.WriteInt(fat, OverlayCount * 8 + 20, romBytes.Count + soundData.bytes.Count);
            AddSection(romBytes, soundData.bytes);

            for (int i = 0; i < NARCCount; i++)
            {
                narcs[i].WriteData();
                HelperFunctions.WriteInt(fat, OverlayCount * 8 + 24 + i * 8, romBytes.Count);
                HelperFunctions.WriteInt(fat, OverlayCount * 8 + 28 + i * 8, romBytes.Count + narcs[i].byteData.Length);
                AddSection(romBytes, narcs[i].byteData);
            }

            for (int i = 0; i < misc.Count; i++)
            {
                narcs[i].WriteData();
                HelperFunctions.WriteInt(fat, OverlayCount * 8 + 24 + NARCCount * 8 + i * 8, romBytes.Count);
                HelperFunctions.WriteInt(fat, OverlayCount * 8 + 28 + NARCCount * 8 + i * 8, romBytes.Count + misc[i].Count);
                AddSection(romBytes, misc[i]);
            }

            HelperFunctions.WriteInt(romHeader, End_PointerLocation, romBytes.Count);
            AddSection(romBytes, romEnder);

            //Rewrite header and FAT after updating values
            romBytes.RemoveRange(0, romHeader.Count);
            romBytes.InsertRange(0, romHeader);

            romBytes.RemoveRange(HelperFunctions.ReadInt(romHeader, FAT_PointerLocation), fat.Count);
            romBytes.InsertRange(HelperFunctions.ReadInt(romHeader, FAT_PointerLocation), fat);

            return romBytes.Count > 0x12000000 ? romBytes.GetRange(0, 0x12000000).ToArray() : romBytes.ToArray();
        }

        public void DumpFileSystem(string path)
        {
            Directory.CreateDirectory(path + "/overlay");
            Directory.CreateDirectory(path + "/data");
            Directory.CreateDirectory(path + "/data/misc");
            Directory.CreateDirectory(path + "/data/a");

            File.WriteAllBytes(path + "/header.bin", romHeader.ToArray());
            File.WriteAllBytes(path + "/arm9.bin", arm9.ToArray());
            File.WriteAllBytes(path + "/arm7.bin", arm7.ToArray());
            File.WriteAllBytes(path + "/y9.bin", y9.ToArray());
            File.WriteAllBytes(path + "/fat.bin", fat.ToArray());
            File.WriteAllBytes(path + "/fnt.bin", fnt.ToArray());
            File.WriteAllBytes(path + "/banner.bin", banner.ToArray());
            File.WriteAllBytes(path + "/data/skb.bin", skb.ToArray());
            File.WriteAllBytes(path + "/data/SoundStatus.bin", soundStatus.ToArray());
            File.WriteAllBytes(path + "/data/SoundData.sdat", soundData.bytes.ToArray());
            File.WriteAllBytes(path + "/data/RomEnder.bin", romEnder.ToArray());
            for (int i = 0; i < overlays.Count; i++) File.WriteAllBytes(path + $"/overlay/ov_{i:D3}.bin", overlays[i].ToArray());
            for (int i = 0; i < misc.Count; i++) File.WriteAllBytes(path + $"/data/misc/{i}.bin", misc[i].ToArray());
            for (int i = 0; i < narcs.Count; i++)
            {
                string narcPath = path + "/data/a/";
                narcPath += (i / 100).ToString() + "/";
                narcPath += ((i % 100) / 10).ToString() + "/";
                Directory.CreateDirectory(narcPath);
                narcs[i].DumpNarc(narcPath + (i % 10).ToString());
            }
        }

        void AddSection(List<byte> romBytes, IEnumerable<byte> section)
        {
            romBytes.AddRange(section);
            while (romBytes.Count % 0x200 != 0) romBytes.Add(0xFF);
        }
    }
}
