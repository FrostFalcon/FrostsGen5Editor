using NewEditor.Data.NARCTypes;
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
        public RomType RomType => (romType == "pokemon b2" || romType == "pokemon w2") ? RomType.BW2 : (romType == "pokemon b" || romType == "pokemon w") ? RomType.BW1 : (romType == "pokemon hg" || romType == "pokemon ss") ? RomType.HGSS : RomType.Other;

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
        public Y9Table y9;
        public List<byte> banner;
        public List<byte> romEnder;

        public List<byte> ctrCameraData;
        public List<byte> ctrRailData;

        List<byte> fontData;
        List<byte> skb;
        List<byte> soundStatus;
        public SoundData soundData;
        List<byte> titleDemo;

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
        public EggMoveNARC eggMoveNarc;
        public EvolutionDataNARC evolutionsNarc;
        public ChildPokemonNARC childPokemonNarc;
        public MoveDataNARC moveDataNarc;
        public ItemDataNARC itemDataNarc;
        public MoveAnimationNARC moveAnimationNarc;
        public MoveAnimationNARC moveAnimationExtraNarc;
        public ZoneDataNARC zoneDataNarc;
        public MapMatrixNARC mapMatrixNarc;
        public MapFilesNARC mapFilesNarc;
        public ScriptNARC scriptNarc;
        public TrTextEntriesNARC trTextEntriesNarc;
        public TrTextIndexNARC trTextIndicesNarc;
        public TrainerDataNARC trainerNarc;
        public TrainerPokeNARC trainerPokeNarc;
        public OverworldObjectsNARC overworldsNarc;
        public EncounterNARC encounterNarc;
        public HabitatListNARC habitatListNarc;
        public PokemartNARC pokemartNarc;
        public PokemartItemCountNARC pokemartItemCountNarc;
        public KeyboardNARC keyboardNarc;
        public XPCurveNARC xpCurveNarc;
        public HiddenGrottoNARC hiddenGrottoNarc;

        //Read from rom file
        public static NDSFileSystem FromRom(FileStream fs, bool setEditorNarcs = false)
        {
            NDSFileSystem result = new NDSFileSystem();
            byte[] romTypeBytes = new byte[16];
            fs.Read(romTypeBytes, 0, 16);
            result.romType = Encoding.ASCII.GetString(romTypeBytes).ToLower();
            result.romType = result.romType.Remove(result.romType.IndexOf((char)0));
            while (result.romType[result.romType.Length - 1] == (char)0x20) result.romType = result.romType.Remove(result.romType.Length - 1, 1);

            MainEditor.GetVersionConstants(result.romType);
            bool ctrRom = false;

            //Populate static fields
            if (result.RomType == RomType.BW2)
            {
                result.NARCCount = 308;
                result.MiscCount = 7;
            }
            if (result.RomType == RomType.BW1)
            {
                result.NARCCount = 235;
                result.MiscCount = 7;
            }
            result.narcIDs = new Dictionary<int, Type>()
            {
                { MainEditor.childPokemonNarcID, typeof(ChildPokemonNARC) },
                { MainEditor.encounterNarcID, typeof(EncounterNARC) },
                { MainEditor.eggMovesNarcID, typeof(EggMoveNARC) },
                { MainEditor.evolutionNarcID, typeof(EvolutionDataNARC) },
                { MainEditor.habitatListNarcID, typeof(HabitatListNARC) },
                { MainEditor.hiddenGrottoNarcID, typeof(HiddenGrottoNARC) },
                { MainEditor.keyboardNarcID, typeof(KeyboardNARC) },
                { MainEditor.levelUpMovesNarcID, typeof(LearnsetNARC) },
                { MainEditor.mapMatrixNarcID, typeof(MapMatrixNARC) },
                { MainEditor.mapModelsNarcID, typeof(MapFilesNARC) },
                { MainEditor.moveAnimationNarcID, typeof(MoveAnimationNARC) },
                { MainEditor.moveAnimationExtraNarcID, typeof(MoveAnimationNARC) },
                { MainEditor.moveDataNarcID, typeof(MoveDataNARC) },
                { MainEditor.itemDataNarcID, typeof(ItemDataNARC) },
                { MainEditor.overworldsNarcID, typeof(OverworldObjectsNARC) },
                { MainEditor.pokemartNarcID, typeof(PokemartNARC) },
                { MainEditor.pokemartItemCountNarcID, typeof(PokemartItemCountNARC) },
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
                { MainEditor.xpCurveNarcID, typeof(XPCurveNARC) },
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

            if (result.RomType == RomType.BW2 && result.fat.Count > 5296) ctrRom = true;
            if (result.RomType == RomType.BW1 && result.fat.Count > 3872) ctrRom = true;

            b = new byte[HelperFunctions.ReadInt(result.romHeader, Y9_SizeLocation)];
            fs.Position = HelperFunctions.ReadInt(result.romHeader, Y9_PointerLocation);
            fs.Read(b, 0, b.Length);
            result.y9 = new Y9Table(b);
            result.OverlayCount = result.y9.entries.Count;

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
                result.overlays.Add(new List<byte>(b));
                pos += 8;
            }

            int start;
            int end;

            if (ctrRom)
            {
                start = HelperFunctions.ReadInt(result.fat, pos);
                end = HelperFunctions.ReadInt(result.fat, pos + 4);
                b = new byte[end - start];
                fs.Position = start;
                fs.Read(b, 0, b.Length);
                result.ctrCameraData = new List<byte>(b);
                pos += 8;

                if (result.RomType == RomType.BW2)
                {
                    start = HelperFunctions.ReadInt(result.fat, pos);
                    end = HelperFunctions.ReadInt(result.fat, pos + 4);
                    b = new byte[end - start];
                    fs.Position = start;
                    fs.Read(b, 0, b.Length);
                    result.ctrRailData = new List<byte>(b);
                    pos += 8;
                }
            }

            if (result.RomType == RomType.BW1)
            {
                start = HelperFunctions.ReadInt(result.fat, pos);
                end = HelperFunctions.ReadInt(result.fat, pos + 4);
                b = new byte[end - start];
                fs.Position = start;
                fs.Read(b, 0, b.Length);
                result.fontData = new List<byte>(b);
                pos += 8;

                if (ctrRom)
                {
                    start = HelperFunctions.ReadInt(result.fat, pos);
                    end = HelperFunctions.ReadInt(result.fat, pos + 4);
                    b = new byte[end - start];
                    fs.Position = start;
                    fs.Read(b, 0, b.Length);
                    result.ctrRailData = new List<byte>(b);
                    pos += 8;
                }
            }

            start = HelperFunctions.ReadInt(result.fat, pos);
            end = HelperFunctions.ReadInt(result.fat, pos + 4);
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

            if (result.RomType == RomType.BW1)
            {
                int start2 = HelperFunctions.ReadInt(result.fat, pos);
                int end2 = HelperFunctions.ReadInt(result.fat, pos + 4);
                b = new byte[end2 - start2];
                fs.Position = start2;
                fs.Read(b, 0, b.Length);
                result.titleDemo = new List<byte>(b);
                pos += 8;
            }

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
            result.eggMoveNarc = result.narcs[MainEditor.eggMovesNarcID] as EggMoveNARC;
            result.evolutionsNarc = result.narcs[MainEditor.evolutionNarcID] as EvolutionDataNARC;
            result.childPokemonNarc = result.narcs[MainEditor.childPokemonNarcID] as ChildPokemonNARC;
            result.moveDataNarc = result.narcs[MainEditor.moveDataNarcID] as MoveDataNARC;
            result.itemDataNarc = result.narcs[MainEditor.itemDataNarcID] as ItemDataNARC;
            result.moveAnimationNarc = result.narcs[MainEditor.moveAnimationNarcID] as MoveAnimationNARC;
            result.moveAnimationExtraNarc = result.narcs[MainEditor.moveAnimationExtraNarcID] as MoveAnimationNARC;
            result.zoneDataNarc = result.narcs[MainEditor.zoneDataNarcID] as ZoneDataNARC;
            result.mapMatrixNarc = result.narcs[MainEditor.mapMatrixNarcID] as MapMatrixNARC;
            result.mapFilesNarc = result.narcs[MainEditor.mapModelsNarcID] as MapFilesNARC;
            result.scriptNarc = result.narcs[MainEditor.scriptNarcID] as ScriptNARC;
            result.trTextEntriesNarc = result.narcs[MainEditor.trTextEntriesNarcID] as TrTextEntriesNARC;
            result.trTextIndicesNarc = result.narcs[MainEditor.trTextIndicesNarcID] as TrTextIndexNARC;
            result.trainerNarc = result.narcs[MainEditor.trainerDataNarcID] as TrainerDataNARC;
            result.trainerPokeNarc = result.narcs[MainEditor.trainerPokeNarcID] as TrainerPokeNARC;
            result.overworldsNarc = result.narcs[MainEditor.overworldsNarcID] as OverworldObjectsNARC;
            result.encounterNarc = result.narcs[MainEditor.encounterNarcID] as EncounterNARC;
            result.xpCurveNarc = result.narcs[MainEditor.xpCurveNarcID] as XPCurveNARC;
            if (result.RomType == RomType.BW2)
            {
                result.pokemartNarc = result.narcs[MainEditor.pokemartNarcID] as PokemartNARC;
                result.pokemartItemCountNarc = result.narcs[MainEditor.pokemartItemCountNarcID] as PokemartItemCountNARC;
                result.keyboardNarc = result.narcs[MainEditor.keyboardNarcID] as KeyboardNARC;
                result.habitatListNarc = result.narcs[MainEditor.habitatListNarcID] as HabitatListNARC;
                result.hiddenGrottoNarc = result.narcs[MainEditor.hiddenGrottoNarcID] as HiddenGrottoNARC;
            }

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
            int ptr = HelperFunctions.ReadInt(result.romHeader, End_PointerLocation);
            b = new byte[(ptr == 0 || ptr > fs.Length) ? 0 : Math.Min(fs.Length, 0x12000000) - ptr];
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
            while (result.romType[result.romType.Length - 1] == (char)0x20) result.romType = result.romType.Remove(result.romType.Length - 2, 1);

            MainEditor.GetVersionConstants(result.romType);

            //Populate static fields
            if (result.RomType == RomType.BW2)
            {
                result.OverlayCount = 344;
                result.NARCCount = 308;
                result.MiscCount = 7;
            }
            if (result.RomType == RomType.BW1)
            {
                result.OverlayCount = 237;
                result.NARCCount = 235;
                result.MiscCount = 7;
            }
            result.narcIDs = new Dictionary<int, Type>()
            {
                { MainEditor.childPokemonNarcID, typeof(ChildPokemonNARC) },
                { MainEditor.encounterNarcID, typeof(EncounterNARC) },
                { MainEditor.eggMovesNarcID, typeof(EggMoveNARC) },
                { MainEditor.evolutionNarcID, typeof(EvolutionDataNARC) },
                { MainEditor.habitatListNarcID, typeof(HabitatListNARC) },
                { MainEditor.hiddenGrottoNarcID, typeof(HiddenGrottoNARC) },
                { MainEditor.keyboardNarcID, typeof(KeyboardNARC) },
                { MainEditor.levelUpMovesNarcID, typeof(LearnsetNARC) },
                { MainEditor.mapMatrixNarcID, typeof(MapMatrixNARC) },
                { MainEditor.mapModelsNarcID, typeof(MapFilesNARC) },
                { MainEditor.moveAnimationNarcID, typeof(MoveAnimationNARC) },
                { MainEditor.moveAnimationExtraNarcID, typeof(MoveAnimationNARC) },
                { MainEditor.moveDataNarcID, typeof(MoveDataNARC) },
                { MainEditor.itemDataNarcID, typeof(ItemDataNARC) },
                { MainEditor.overworldsNarcID, typeof(OverworldObjectsNARC) },
                { MainEditor.pokemartNarcID, typeof(PokemartNARC) },
                { MainEditor.pokemartItemCountNarcID, typeof(PokemartItemCountNARC) },
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
                { MainEditor.xpCurveNarcID, typeof(XPCurveNARC) },
            };
            result.overlays = new List<List<byte>>();
            result.narcs = new List<NARC>();
            result.misc = new List<List<byte>>();

            try
            {
                result.arm9 = new List<byte>(File.ReadAllBytes(rootFolder + "/arm9.bin"));
                result.arm7 = new List<byte>(File.ReadAllBytes(rootFolder + "/arm7.bin"));
                result.y9 = new Y9Table(File.ReadAllBytes(rootFolder + "/y9.bin"));
                result.OverlayCount = result.y9.entries.Count;
                result.fat = new List<byte>(File.ReadAllBytes(rootFolder + "/fat.bin"));
                result.fnt = new List<byte>(File.ReadAllBytes(rootFolder + "/fnt.bin"));
                result.banner = new List<byte>(File.ReadAllBytes(rootFolder + "/banner.bin"));
                if (File.Exists(rootFolder + "/data/ctrCameraData.bin")) result.ctrCameraData = new List<byte>(File.ReadAllBytes(rootFolder + "/data/ctrCameraData.bin"));
                if (File.Exists(rootFolder + "/data/ctrRailData.bin")) result.ctrRailData = new List<byte>(File.ReadAllBytes(rootFolder + "/data/ctrRailData.bin"));
                if (result.RomType == RomType.BW1) result.fontData = new List<byte>(File.ReadAllBytes(rootFolder + "/data/glf_font.bin"));
                result.skb = new List<byte>(File.ReadAllBytes(rootFolder + "/data/skb.bin"));
                result.soundStatus = new List<byte>(File.ReadAllBytes(rootFolder + "/data/SoundStatus.bin"));
                result.soundData = new SoundData(File.ReadAllBytes(rootFolder + "/data/SoundData.sdat"));
                if (result.RomType == RomType.BW1) result.titleDemo = new List<byte>(File.ReadAllBytes(rootFolder + "/data/titledemo.bin"));
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
            result.eggMoveNarc = result.narcs[MainEditor.eggMovesNarcID] as EggMoveNARC;
            result.evolutionsNarc = result.narcs[MainEditor.evolutionNarcID] as EvolutionDataNARC;
            result.childPokemonNarc = result.narcs[MainEditor.childPokemonNarcID] as ChildPokemonNARC;
            result.moveDataNarc = result.narcs[MainEditor.moveDataNarcID] as MoveDataNARC;
            result.itemDataNarc = result.narcs[MainEditor.itemDataNarcID] as ItemDataNARC;
            result.moveAnimationNarc = result.narcs[MainEditor.moveAnimationNarcID] as MoveAnimationNARC;
            result.moveAnimationExtraNarc = result.narcs[MainEditor.moveAnimationExtraNarcID] as MoveAnimationNARC;
            result.zoneDataNarc = result.narcs[MainEditor.zoneDataNarcID] as ZoneDataNARC;
            result.mapMatrixNarc = result.narcs[MainEditor.mapMatrixNarcID] as MapMatrixNARC;
            result.mapFilesNarc = result.narcs[MainEditor.mapModelsNarcID] as MapFilesNARC;
            result.scriptNarc = result.narcs[MainEditor.scriptNarcID] as ScriptNARC;
            result.trTextEntriesNarc = result.narcs[MainEditor.trTextEntriesNarcID] as TrTextEntriesNARC;
            result.trTextIndicesNarc = result.narcs[MainEditor.trTextIndicesNarcID] as TrTextIndexNARC;
            result.trainerNarc = result.narcs[MainEditor.trainerDataNarcID] as TrainerDataNARC;
            result.trainerPokeNarc = result.narcs[MainEditor.trainerPokeNarcID] as TrainerPokeNARC;
            result.overworldsNarc = result.narcs[MainEditor.overworldsNarcID] as OverworldObjectsNARC;
            result.encounterNarc = result.narcs[MainEditor.encounterNarcID] as EncounterNARC;
            result.xpCurveNarc = result.narcs[MainEditor.xpCurveNarcID] as XPCurveNARC;
            if (result.RomType == RomType.BW2)
            {
                result.pokemartNarc = result.narcs[MainEditor.pokemartNarcID] as PokemartNARC;
                result.pokemartItemCountNarc = result.narcs[MainEditor.pokemartItemCountNarcID] as PokemartItemCountNARC;
                result.keyboardNarc = result.narcs[MainEditor.keyboardNarcID] as KeyboardNARC;
                result.habitatListNarc = result.narcs[MainEditor.habitatListNarcID] as HabitatListNARC;
                result.hiddenGrottoNarc = result.narcs[MainEditor.hiddenGrottoNarcID] as HiddenGrottoNARC;
            }
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
            HelperFunctions.WriteInt(romHeader, Y9_SizeLocation, y9.bytes.Length);
            AddSection(romBytes, y9.bytes);

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

            int pos = OverlayCount * 8;

            if (RomType == RomType.BW1)
            {
                if (ctrCameraData != null)
                {
                    HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                    HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + ctrCameraData.Count);
                    AddSection(romBytes, ctrCameraData);
                    pos += 8;
                }

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + fontData.Count);
                AddSection(romBytes, fontData);
                pos += 8;

                if (ctrRailData != null)
                {
                    HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                    HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + ctrRailData.Count);
                    AddSection(romBytes, ctrRailData);
                    pos += 8;
                }

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + skb.Count);
                AddSection(romBytes, skb);
                pos += 8;

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + soundStatus.Count);
                AddSection(romBytes, soundStatus);
                pos += 8;

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + titleDemo.Count);
                AddSection(romBytes, titleDemo);
                pos += 8;

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + soundData.bytes.Count);
                AddSection(romBytes, soundData.bytes);
                pos += 8;

                for (int i = 0; i < NARCCount; i++)
                {
                    narcs[i].WriteData();
                    HelperFunctions.WriteInt(fat, pos + i * 8, romBytes.Count);
                    HelperFunctions.WriteInt(fat, pos + 4 + i * 8, romBytes.Count + narcs[i].byteData.Length);
                    AddSection(romBytes, narcs[i].byteData);
                }
            }
            else
            {
                if (ctrCameraData != null && ctrRailData != null)
                {
                    HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                    HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + ctrCameraData.Count);
                    AddSection(romBytes, ctrCameraData);
                    pos += 8;

                    HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                    HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + ctrRailData.Count);
                    AddSection(romBytes, ctrRailData);
                    pos += 8;
                }

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + skb.Count);
                AddSection(romBytes, skb);
                pos += 8;

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + soundStatus.Count);
                AddSection(romBytes, soundStatus);
                pos += 8;

                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + soundData.bytes.Count);
                AddSection(romBytes, soundData.bytes);
                pos += 8;

                for (int i = 0; i < NARCCount; i++)
                {
                    narcs[i].WriteData();
                    HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                    HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + narcs[i].byteData.Length);
                    AddSection(romBytes, narcs[i].byteData);
                    pos += 8;
                }
            }
            

            for (int i = 0; i < misc.Count; i++)
            {
                HelperFunctions.WriteInt(fat, pos, romBytes.Count);
                HelperFunctions.WriteInt(fat, pos + 4, romBytes.Count + misc[i].Count);
                AddSection(romBytes, misc[i]);
                pos += 8;
            }

            HelperFunctions.WriteInt(romHeader, End_PointerLocation, romEnder.Count == 0 ? 0 : romBytes.Count);
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
            File.WriteAllBytes(path + "/y9.bin", y9.bytes);
            File.WriteAllBytes(path + "/fat.bin", fat.ToArray());
            File.WriteAllBytes(path + "/fnt.bin", fnt.ToArray());
            File.WriteAllBytes(path + "/banner.bin", banner.ToArray());
            if (ctrCameraData != null) File.WriteAllBytes(path + "/data/ctrCameraData.bin", ctrCameraData.ToArray());
            if (ctrRailData != null) File.WriteAllBytes(path + "/data/ctrRailData.bin", ctrRailData.ToArray());
            if (RomType == RomType.BW1) File.WriteAllBytes(path + "/data/gfl_font.bin", fontData.ToArray());
            File.WriteAllBytes(path + "/data/skb.bin", skb.ToArray());
            File.WriteAllBytes(path + "/data/SoundStatus.bin", soundStatus.ToArray());
            if (RomType == RomType.BW1) File.WriteAllBytes(path + "/data/titledemo.bin", titleDemo.ToArray());
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
