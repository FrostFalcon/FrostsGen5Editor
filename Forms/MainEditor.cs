using NewEditor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NewEditor.Data.NARCTypes;
using System.Threading;
using System.Runtime.InteropServices;

namespace NewEditor.Forms
{
    public partial class MainEditor : Form
    {
        public string loadedRomPath = "";

        //Key data
        static int fileSizeLimit;

        public static int textNarcID = -1;
        public static int storyTextNarcID = -1;
        public static int mapModelsNarcID = -1;
        public static int pokemonSpritesNarcID = -1;
        public static int pokemonIconsNarcID = -1;
        public static int pokemonDataNarcID = -1;
        public static int levelUpMovesNarcID = -1;
        public static int evolutionNarcID = -1;
        public static int childPokemonNarcID = -1;
        public static int moveDataNarcID = -1;
        public static int itemDataNarcID = -1;
        public static int moveAnimationNarcID = -1;
        public static int moveAnimationExtraNarcID = -1;
        public static int zoneDataNarcID = -1;
        public static int mapMatrixNarcID = -1;
        public static int scriptNarcID = -1;
        public static int trTextEntriesNarcID = -1;
        public static int trTextIndicesNarcID = -1;
        public static int trainerDataNarcID = -1;
        public static int trainerPokeNarcID = -1;
        public static int overworldsNarcID = -1;
        public static int encounterNarcID = -1;
        public static int pokemartNarcID = -1;
        public static int keyboardNarcID = -1;

        //Rom Data
        public static NDSFileSystem fileSystem;
        private static string romType = "";
        public static RomType RomType => (romType == "pokemon b2" || romType == "pokemon w2") ? RomType.BW2 : (romType == "pokemon hg" || romType == "pokemon ss") ? RomType.HGSS : RomType.Other; 

        //Narcs
        public static TextNARC textNarc;
        public static TextNARC storyTextNarc;
        public static PokemonSpritesNARC pokemonSpritesNarc;
        public static PokemonIconNARC pokemonIconNarc;
        public static PokemonDataNARC pokemonDataNarc;
        public static LearnsetNARC learnsetNarc;
        public static EvolutionDataNARC evolutionsNarc;
        public static ChildPokemonNARC childPokemonNarc;
        public static MoveDataNARC moveDataNarc;
        public static ItemDataNARC itemDataNarc;
        public static MoveAnimationNARC moveAnimationNarc;
        public static MoveAnimationNARC moveAnimationExtraNarc;
        public static ZoneDataNARC zoneDataNarc;
        public static MapMatrixNARC mapMatrixNarc;
        public static MapModelsNARC mapModelsNarc;
        public static ScriptNARC scriptNarc;
        public static TrTextEntriesNARC trTextEntriesNarc;
        public static TrTextIndexNARC trTextIndicesNarc;
        public static TrainerDataNARC trainerNarc;
        public static TrainerPokeNARC trainerPokeNarc;
        public static OverworldObjectsNARC overworldsNarc;
        public static EncounterNARC encounterNarc;
        public static PokemartNARC pokemartNarc;
        public static KeyboardNARC keyboardNarc;

        //Forms
        public static TextViewer textViewer;
        public static PokemonEditor pokemonEditor;
        public static MoveEditor moveEditor;
        public static OverworldEditor overworldEditor;
        public static EncounterEditor encounterEditor;
        public static ScriptEditor scriptEditor;
        public static TrainerEditor trainerEditor;
        public static TypeSwapEditor typeSwapEditor;
        public static RandomMovesEditor presetMovesEditor;
        public static PokemartEditor pokemartEditor;
        public static OverlayEditor overlayEditor;
        public static TypeChartEditor typeChartEditor;
        public static Pokepatcher pokePatchEditor;

        public bool loadingNARCS = false;
        bool autoLoaded = false;
        public static string AutoLoad = "";

        public MainEditor()
        {
            InitializeComponent();
            TryAutoLoad();
        }

        public static void GetVersionConstants(string romType)
        {
            MainEditor.romType = romType;
            if (RomType == RomType.BW2)
            {
                //FirstNARCPointerLocation = VersionConstants.BW2_FirstNarcPointerLocation;
                //FirstNARCPointerLocation = VersionConstants.FindFirstNARCPointerLocation(romFile);
                //sDatLocation = VersionConstants.FindSDat(romFile);
                //LastNarc = VersionConstants.BW2_LastNarc;
                //NARCsToSkip = VersionConstants.BW2_NARCsToSkip;
                fileSizeLimit = VersionConstants.BW2_FileSizeLimit;

                textNarcID = VersionConstants.BW2_TextNARCID;
                storyTextNarcID = VersionConstants.BW2_StoryTextNARCID;
                mapModelsNarcID = VersionConstants.BW2_MapModelsNARCID;
                mapMatrixNarcID = VersionConstants.BW2_MapMatriciesNARCID;
                pokemonSpritesNarcID = VersionConstants.BW2_PokemonSpritesNARCID;
                pokemonIconsNarcID = VersionConstants.BW2_PokemonIconsNARCID;
                pokemonDataNarcID = VersionConstants.BW2_PokemonDataNARCID;
                levelUpMovesNarcID = VersionConstants.BW2_LevelUpMovesNARCID;
                evolutionNarcID = VersionConstants.BW2_EvolutionsNARCID;
                childPokemonNarcID = VersionConstants.BW2_ChildPokemonNARCID;
                moveDataNarcID = VersionConstants.BW2_MoveDataNARCID;
                itemDataNarcID = VersionConstants.BW2_ItemDataNARCID;
                moveAnimationNarcID = VersionConstants.BW2_MoveAnimationNARCID;
                moveAnimationExtraNarcID = VersionConstants.BW2_MoveAnimationExtraNARCID;
                zoneDataNarcID = VersionConstants.BW2_ZoneDataNARCID;
                scriptNarcID = VersionConstants.BW2_ScriptNARCID;
                trTextEntriesNarcID = VersionConstants.BW2_TrTextEntriesNARCID;
                trTextIndicesNarcID = VersionConstants.BW2_TrTextIndicesNARCID;
                trainerDataNarcID = VersionConstants.BW2_TrainerDataNARCID;
                trainerPokeNarcID = VersionConstants.BW2_TrainerPokemonNARCID;
                overworldsNarcID = VersionConstants.BW2_OverworldsNARCID;
                encounterNarcID = VersionConstants.BW2_EncountersNARCID;
                pokemartNarcID = VersionConstants.BW2_PokemartNARCID;
                keyboardNarcID = VersionConstants.BW2_KeyboardLayoutNARCID;
                return;
            }

            //if (RomType == RomType.HGSS)
            //{
            //    //FirstNARCPointerLocation = VersionConstants.HGSS_FirstNarcPointerLocation;
            //    //FirstNARCPointerLocation = VersionConstants.FindFirstNARCPointerLocation(romFile);
            //    //LastNarc = VersionConstants.HGSS_LastNarc;
            //    //NARCsToSkip = VersionConstants.HGSS_NARCsToSkip;
            //    fileSizeLimit = VersionConstants.HGSS_FileSizeLimit;

            //    mapModelsNarcID = -1;
            //    mapMatrixNarcID = -1;
            //    pokemonSpritesNarcID = -1;
            //    pokemonIconsNarcID = -1;
            //    moveAnimationNarcID = -1;
            //    itemDataNarcID = -1;
            //    trainerDataNarcID = -1;
            //    trainerPokeNarcID = -1;
            //    overworldsNarcID = -1;
            //    encounterNarcID = -1;
            //    pokemartNarcID = -1;
            //    keyboardNarcID = -1;

            //    textNarcID = VersionConstants.HGSS_TextNARCID;
            //    storyTextNarcID = VersionConstants.HGSS_StoryTextNARCID;
            //    pokemonDataNarcID = VersionConstants.HGSS_PokemonDataNARCID;
            //    levelUpMovesNarcID = VersionConstants.HGSS_LevelUpMovesNARCID;
            //    evolutionNarcID = VersionConstants.HGSS_EvolutionsNARCID;
            //    moveDataNarcID = VersionConstants.HGSS_MoveDataNARCID;
            //    zoneDataNarcID = VersionConstants.HGSS_ZoneDataNARCID;
            //    mapMatrixNarcID = VersionConstants.HGSS_MapMatriciesNARCID;
            //    scriptNarcID = VersionConstants.HGSS_ScriptNARCID;
            //    return;
            //}

            MessageBox.Show("Invalid file type.\nExpected a pokemon black 2 or white 2 rom");
            ActiveForm.Close();
        }

        public void OpenRomButton(object sender, EventArgs e)
        {
            BringToFront();
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "Nds Roms|*.nds";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                loadedRomPath = prompt.FileName;

                LoadRom();
            }
        }

        public void SaveRomButton(object sender, EventArgs e)
        {
            SaveFileDialog prompt = new SaveFileDialog();
            prompt.Filter = "Nds Roms|*.nds";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                taskProgressBar.Value = 0;
                //Compile data into target file
                loadedRomPath = prompt.FileName;
                FileStream fileStream = null;
                try
                {
                    fileStream = File.OpenWrite(loadedRomPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save rom");
                    return;
                }
                fileStream.SetLength(0);
                byte[] data = fileSystem.BuildRom();
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
                taskProgressBar.Value = taskProgressBar.Maximum;

                MessageBox.Show("Rom saved to " + prompt.FileName);
            }
        }

        private void dumpRomButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog prompt = new FolderBrowserDialog();
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                fileSystem.DumpFileSystem(prompt.SelectedPath);
                MessageBox.Show("Rom dumped successfully");
            }
        }

        private void LoadRomFromFolder(object sender, EventArgs e)
        {
            BringToFront();
            FolderBrowserDialog prompt = new FolderBrowserDialog();

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                loadedRomPath = prompt.SelectedPath;

                LoadRom(true);
            }
        }

        public async void LoadRom(bool fromFolder = false)
        {
            if (loadedRomPath == "") return;

            openRomButton.Enabled = false;
            loadFromFolderButton.Enabled = false;
            autoLoadButton.Enabled = false;

            //Reset narc variables and editors
            if (textViewer != null && !textViewer.IsDisposed) textViewer.Close();
            if (pokemonEditor != null && !pokemonEditor.IsDisposed) pokemonEditor.Close();
            if (moveEditor != null && !moveEditor.IsDisposed) moveEditor.Close();
            if (overworldEditor != null && !overworldEditor.IsDisposed) overworldEditor.Close();
            if (encounterEditor != null && !encounterEditor.IsDisposed) encounterEditor.Close();
            if (scriptEditor != null && !scriptEditor.IsDisposed) scriptEditor.Close();
            if (trainerEditor != null && !trainerEditor.IsDisposed) trainerEditor.Close();
            if (pokemartEditor != null && !pokemartEditor.IsDisposed) pokemartEditor.Close();
            if (overlayEditor != null && !overlayEditor.IsDisposed) overlayEditor.Close();
            if (typeSwapEditor != null && !typeSwapEditor.IsDisposed) typeSwapEditor.Close();
            if (presetMovesEditor != null && !presetMovesEditor.IsDisposed) presetMovesEditor.Close();
            if (pokePatchEditor != null && !pokePatchEditor.IsDisposed) pokePatchEditor.Close();
            if (typeChartEditor != null && !typeChartEditor.IsDisposed) typeChartEditor.Close();
            textNarc = null;
            storyTextNarc = null;
            pokemonSpritesNarc = null;
            pokemonDataNarc = null;
            learnsetNarc = null;
            evolutionsNarc = null;
            moveDataNarc = null;
            itemDataNarc = null;
            childPokemonNarc = null;
            moveAnimationNarc = null;
            moveAnimationExtraNarc = null;
            zoneDataNarc = null;
            mapMatrixNarc = null;
            mapModelsNarc = null;
            scriptNarc = null;
            trainerNarc = null;
            trainerPokeNarc = null;
            overworldsNarc = null;
            encounterNarc = null;
            pokemartEditor = null;
            overlayEditor = null;
            loadingNARCS = true;
            taskProgressBar.Value = 0;

            await Task.Run(() =>
            {
                if (fromFolder)
                {
                    fileSystem = NDSFileSystem.FromFileSystem(loadedRomPath, true);
                }
                else
                {
                    FileStream fileStream = File.OpenRead(loadedRomPath);
                    fileSystem = NDSFileSystem.FromRom(fileStream, true);

                    fileStream.Close();
                }
            });

            //Setup Editor
            saveRomButton.Enabled = true;
            dumpRomButton.Enabled = true;
            openRomButton.Enabled = true;
            loadFromFolderButton.Enabled = true;
            autoLoadButton.Enabled = true;
            romNameText.Text = "Rom: " + loadedRomPath.Substring(loadedRomPath.LastIndexOf('\\') + 1, loadedRomPath.Length - (loadedRomPath.LastIndexOf('\\') + 1));
            romTypeText.Text = "Rom Type: " + romType;
            taskProgressBar.Value = taskProgressBar.Maximum;

            MessageBox.Show("Rom Loaded");

            loadingNARCS = false;
            autoLoaded = false;
        }

        public static void SetNARCVars(NDSFileSystem fileSystem)
        {
            textNarc = fileSystem.narcs[textNarcID] as TextNARC;
            storyTextNarc = fileSystem.narcs[storyTextNarcID] as TextNARC;
            pokemonSpritesNarc = fileSystem.narcs[pokemonSpritesNarcID] as PokemonSpritesNARC;
            pokemonIconNarc = fileSystem.narcs[pokemonIconsNarcID] as PokemonIconNARC;
            pokemonDataNarc = fileSystem.narcs[pokemonDataNarcID] as PokemonDataNARC;
            learnsetNarc = fileSystem.narcs[levelUpMovesNarcID] as LearnsetNARC;
            evolutionsNarc = fileSystem.narcs[evolutionNarcID] as EvolutionDataNARC;
            moveDataNarc = fileSystem.narcs[moveDataNarcID] as MoveDataNARC;
            itemDataNarc = fileSystem.narcs[itemDataNarcID] as ItemDataNARC;
            childPokemonNarc = fileSystem.narcs[childPokemonNarcID] as ChildPokemonNARC;
            moveAnimationNarc = fileSystem.narcs[moveAnimationNarcID] as MoveAnimationNARC;
            moveAnimationExtraNarc = fileSystem.narcs[moveAnimationExtraNarcID] as MoveAnimationNARC;
            zoneDataNarc = fileSystem.narcs[zoneDataNarcID] as ZoneDataNARC;
            mapMatrixNarc = fileSystem.narcs[mapMatrixNarcID] as MapMatrixNARC;
            mapModelsNarc = fileSystem.narcs[mapModelsNarcID] as MapModelsNARC;
            scriptNarc = fileSystem.narcs[scriptNarcID] as ScriptNARC;
            trTextEntriesNarc = fileSystem.narcs[trTextEntriesNarcID] as TrTextEntriesNARC;
            trTextIndicesNarc = fileSystem.narcs[trTextIndicesNarcID] as TrTextIndexNARC;
            trainerNarc = fileSystem.narcs[trainerDataNarcID] as TrainerDataNARC;
            trainerPokeNarc = fileSystem.narcs[trainerPokeNarcID] as TrainerPokeNARC;
            overworldsNarc = fileSystem.narcs[overworldsNarcID] as OverworldObjectsNARC;
            encounterNarc = fileSystem.narcs[encounterNarcID] as EncounterNARC;
            pokemartNarc = fileSystem.narcs[pokemartNarcID] as PokemartNARC;
            keyboardNarc = fileSystem.narcs[keyboardNarcID] as KeyboardNARC;
        }

        public void TryAutoLoad()
        {
            List<byte> enabled = FileFunctions.ReadFileSection("Preferences.txt", "AutoLoad");
            List<byte> value = FileFunctions.ReadFileSection("Preferences.txt", "AutoLoadPath");
            if (enabled == null || value == null)
            {
                FileFunctions.WriteFileSection("Preferences.txt", "AutoLoad", ASCIIEncoding.ASCII.GetBytes("Off"));
                FileFunctions.WriteFileSection("Preferences.txt", "AutoLoadPath", new byte[0]);
                return;
            }
            if (ASCIIEncoding.ASCII.GetString(enabled.ToArray()) == "Off") return;

            autoLoaded = true;
            loadedRomPath = ASCIIEncoding.ASCII.GetString(value.ToArray());
            LoadRom();

            autoLoadButton.Text = "Disable Auto Load";
            autoLoadButton.Click -= EnableAutoLoad;
            autoLoadButton.Click += DisableAutoLoad;
        }

        private void EnableAutoLoad(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "Nds Roms|*.nds";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileFunctions.WriteFileSection("Preferences.txt", "AutoLoad", ASCIIEncoding.ASCII.GetBytes("On"));
                FileFunctions.WriteFileSection("Preferences.txt", "AutoLoadPath", ASCIIEncoding.ASCII.GetBytes(prompt.FileName));

                autoLoadButton.Text = "Disable Auto Load";
                autoLoadButton.Click -= EnableAutoLoad;
                autoLoadButton.Click += DisableAutoLoad;
            }
        }
        private void DisableAutoLoad(object sender, EventArgs e)
        {
            FileFunctions.WriteFileSection("Preferences.txt", "AutoLoad", ASCIIEncoding.ASCII.GetBytes("Off"));
            FileFunctions.WriteFileSection("Preferences.txt", "AutoLoadPath", new byte[0]);

            autoLoadButton.Text = "Enable Auto Load";
            autoLoadButton.Click -= DisableAutoLoad;
            autoLoadButton.Click += EnableAutoLoad;
        }

        #region Editor Buttons
        public void OpenTextViewer(object sender, EventArgs e)
        {
            if (textNarc == null || (storyTextNarc == null && RomType == RomType.BW2) || loadingNARCS)
            {
                MessageBox.Show("Text files have not been loaded");
                return;
            }

            if (textViewer == null || textViewer.IsDisposed) textViewer = new TextViewer();
            textViewer.Show();
            textViewer.BringToFront();
        }

        public void OpenPokemonEditor(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Pokemon data files have not been loaded");
                return;
            }

            if (pokemonEditor == null || pokemonEditor.IsDisposed) pokemonEditor = new PokemonEditor();
            pokemonEditor.Show();
            pokemonEditor.BringToFront();
        }

        public void OpenOverworldEditor(object sender, EventArgs e)
        {
            if (zoneDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Zone data files have not been loaded");
                return;
            }

            if (overworldEditor == null || overworldEditor.IsDisposed) overworldEditor = new OverworldEditor();
            overworldEditor.Show();
            overworldEditor.BringToFront();
        }

        private void OpenMoveEditor(object sender, EventArgs e)
        {
            if (moveDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Move data files have not been loaded");
                return;
            }

            if (moveEditor == null || moveEditor.IsDisposed) moveEditor = new MoveEditor();
            moveEditor.Show();
            moveEditor.BringToFront();
        }

        public void OpenScriptEditor(object sender, EventArgs e)
        {
            if (scriptNarc == null || loadingNARCS)
            {
                MessageBox.Show("Script data files have not been loaded");
                return;
            }

            if (scriptEditor == null || scriptEditor.IsDisposed) scriptEditor = new ScriptEditor();
            scriptEditor.Show();
            scriptEditor.BringToFront();
        }

        private void OpenTrainerEditor(object sender, EventArgs e)
        {
            if (trainerNarc == null || loadingNARCS)
            {
                MessageBox.Show("Trainer data files have not been loaded");
                return;
            }

            if (trainerEditor == null || trainerEditor.IsDisposed) trainerEditor = new TrainerEditor();
            trainerEditor.Show();
            trainerEditor.BringToFront();
        }

        public void OpenEncounterEditor(object sender, EventArgs e)
        {
            if (encounterNarc == null || loadingNARCS)
            {
                MessageBox.Show("Encounter data files have not been loaded");
                return;
            }

            if (encounterEditor == null || encounterEditor.IsDisposed) encounterEditor = new EncounterEditor();
            encounterEditor.Show();
            encounterEditor.BringToFront();
        }

        private void OpenShopEditor(object sender, EventArgs e)
        {
            if (pokemartNarc == null || itemDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Pokemart data files have not been loaded");
                return;
            }

            if (pokemartEditor == null || pokemartEditor.IsDisposed) pokemartEditor = new PokemartEditor();
            pokemartEditor.Show();
            pokemartEditor.BringToFront();
        }

        private void openOverlayEditorButton_Click(object sender, EventArgs e)
        {
            if (loadingNARCS)
            {
                MessageBox.Show("Overlay files have not been loaded");
                return;
            }

            if (overlayEditor == null || overlayEditor.IsDisposed) overlayEditor = new OverlayEditor(fileSystem);
            overlayEditor.Show();
            overlayEditor.BringToFront();
        }

        public void OpenTypeSwapEditor(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || moveDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (typeSwapEditor == null || typeSwapEditor.IsDisposed) typeSwapEditor = new TypeSwapEditor();
            typeSwapEditor.Show();
            typeSwapEditor.BringToFront();
        }

        private void ApplyTypeShuffle(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || moveDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            TypeShuffler.Shuffle();
        }

        public void OpenPresetMoveEditor(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || moveDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (presetMovesEditor == null || presetMovesEditor.IsDisposed) presetMovesEditor = new RandomMovesEditor();
            presetMovesEditor.Show();
            presetMovesEditor.BringToFront();
        }

        private void testGameModeButton_Click(object sender, EventArgs e)
        {
            CustomGameModeManager.NoStoryMode();
        }
        #endregion
        private void replaceNarcButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "Nds Roms|*.nds";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = File.OpenRead(prompt.FileName);
                NDSFileSystem other = NDSFileSystem.FromRom(fileStream);
                fileStream.Close();

                fileSystem.narcs[(int)narcToReplaceNumberBox.Value] = other.narcs[(int)narcToReplaceNumberBox.Value];

                MessageBox.Show("NARC replace complete");
            }
        }

        public void createPatchButton_Click(object sender, EventArgs e)
        {
            if (loadedRomPath == "" || loadingNARCS)
            {
                MessageBox.Show("No Rom Loaded");
                return;
            }

            List<int> selectedNARCs = new List<int>();
            string[] vals = selectivePatchTextBox.Text.Split(' ');
            for (int i = 0; i < vals.Length; i++)
            {
                if (int.TryParse(vals[i], out int num))
                {
                    selectedNARCs.Add(num);
                }
            }

            if (selectedNARCs.Count == 0)
            {
                OpenFileDialog prompt = new OpenFileDialog();
                prompt.Filter = "Nds Roms|*.nds";
                prompt.Title = "Select the base version of the rom";
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    FileStream fileStream = File.OpenRead(prompt.FileName);
                    NDSFileSystem other = NDSFileSystem.FromRom(fileStream);
                    fileStream.Close();

                    if (other.RomType != RomType)
                    {
                        MessageBox.Show("The provided game does not match the loaded rom.");
                        return;
                    }

                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "Black White 2 Patch File|*.bw2Patch";
                    save.Title = "Select where to save the patch file";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();

                        if (!fileSystem.arm9.SequenceEqual(other.arm9))
                        {
                            data.Add("arm9_", fileSystem.arm9);
                        }
                        if (!fileSystem.y9.SequenceEqual(other.y9))
                        {
                            data.Add("y9_", fileSystem.y9);
                        }

                        for (int i = 0; i < fileSystem.narcs.Count; i++)
                            if (!fileSystem.narcs[i].byteData.SequenceEqual(other.narcs[i].byteData))
                            {
                                fileSystem.narcs[i].WriteData();
                                data.Add("id_" + i, fileSystem.narcs[i].GetPatchBytes(other.narcs[i]));
                            }

                        for (int i = 0; i < 700/*textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count*/; i++)
                            if (!fileSystem.soundData.GetSoundBytes(i).SequenceEqual(other.soundData.GetSoundBytes(i)))
                            {
                                data.Add("sound" + (i + 1), fileSystem.soundData.GetSoundBytes(i));
                            }

                        for (int i = 0; i < fileSystem.overlays.Count; i++)
                            if (!fileSystem.overlays[i].SequenceEqual(other.overlays[i]))
                            {
                                data.Add("ov_" + i, fileSystem.overlays[i]);
                            }

                        FileFunctions.WriteAllSections(save.FileName, data, true);
                        MessageBox.Show("Patch Created");
                    }
                }
            }
            else
            {
                SaveFileDialog prompt = new SaveFileDialog();
                prompt.Filter = "Black White 2 Patch File|*.bw2Patch";

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();
                    for (int i = 0; i < fileSystem.narcs.Count; i++)
                    {
                        if (fileSystem.narcs[i].GetType().Name != "NARC" && selectedNARCs.Contains(i))
                        {
                            fileSystem.narcs[i].WriteData();
                            data.Add("id_" + i, fileSystem.narcs[i].byteData);
                        }
                    }

                    FileFunctions.WriteAllSections(prompt.FileName, data, true);
                    MessageBox.Show("Patch Created");
                }
            }
        }

        public void applyPatchButton_Click(object sender, EventArgs e)
        {
            if (loadedRomPath == "" || loadingNARCS)
            {
                MessageBox.Show("No Rom Loaded");
                return;
            }

            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "Black White 2 Patch File|*.bw2Patch";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, IEnumerable<byte>> data = FileFunctions.ReadAllSections(prompt.FileName, true);

                foreach (KeyValuePair<string, IEnumerable<byte>> entry in data)
                {
                    if (entry.Key.StartsWith("sound"))
                    {
                        if (int.TryParse(entry.Key.Substring(5), out int id) && id >= 0)
                        {
                            fileSystem.soundData.WriteToSwavFromPatch(id - 1, entry.Value.ToList());
                        }
                    }
                    else if (entry.Key.StartsWith("ov_"))
                    {
                        if (int.TryParse(entry.Key.Substring(3), out int id) && id >= 0 && id < fileSystem.overlays.Count)
                        {
                            fileSystem.overlays[id] = entry.Value.ToList();
                        }
                    }
                    else if (entry.Key.StartsWith("arm9_"))
                    {
                        fileSystem.arm9 = entry.Value.ToList();
                    }
                    else if (entry.Key.StartsWith("y9_"))
                    {
                        fileSystem.y9 = entry.Value.ToList();
                    }
                    else if (int.TryParse(entry.Key.Substring(3), out int id) && id >= 0 && id < fileSystem.narcs.Count)
                    {
                        fileSystem.narcs[id].ReadPatchBytes(entry.Value.ToArray());
                    }
                }

                MessageBox.Show("Patch Applied");
            }
        }

        public void rogueModeButton_Click(object sender, EventArgs e)
        {
            if (loadedRomPath == "" || loadingNARCS)
            {
                MessageBox.Show("The rom has not been fully loaded");
                return;
            }

            RogueGameModeManager.ApplyMode(stsFormatDropdown.SelectedIndex >= 0 ? stsFormatDropdown.SelectedIndex : 0);
            MessageBox.Show("Roguelike mode applied");
        }

        private void doubleBattleButton_Click(object sender, EventArgs e)
        {
            if (trainerNarc == null || trainerPokeNarc == null || overworldsNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (loadedRomPath == "" || loadingNARCS)
            {
                MessageBox.Show("The rom has not been fully loaded");
                return;
            }

            for (int i = 0; i < trainerNarc.trainers.Count; i++)
            {
                //Exclude trainers with only one pokemon or that are already multi battles
                if (trainerNarc.trainers[i].pokemon.pokemon.Count == 1 || trainerNarc.trainers[i].battleType != 0) continue;

                trainerNarc.trainers[i].battleType = 1;
                trainerNarc.trainers[i].AI = (byte)(trainerNarc.trainers[i].AI | 128);
                trainerNarc.trainers[i].ApplyData();
            }

            //Fix overworld scripts for double battle trainers
            //foreach (OverworldObjectsEntry ow in overworldsNarc.objects)
            //{
            //    if (ow.NPCs == null || ow.NPCs.Count == 0) continue;
            //    foreach (OverworldNPC npc in ow.NPCs)
            //    {
            //        if (npc.scriptUsed >= 3000 && npc.scriptUsed <= 4000)
            //        {
            //            int trainer = npc.scriptUsed - 3000;
            //            if (trainerNarc.trainers[trainer].battleType == 1) npc.scriptUsed += 2000;
            //        }
            //        ow.ApplyData();
            //    }
            //}

            MessageBox.Show("Double battle mode applied");
        }

        public void tripleBattleButton_Click(object sender, EventArgs e)
        {
            if (trainerNarc == null || trainerPokeNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (loadedRomPath == "" || loadingNARCS)
            {
                MessageBox.Show("The rom has not been fully loaded");
                return;
            }

            for (int i = 0; i < trainerNarc.trainers.Count; i++)
            {
                //Exclude trainers with only one pokemon or that are already multi battles
                if (trainerNarc.trainers[i].pokemon.pokemon.Count == 1 || trainerNarc.trainers[i].battleType != 0) continue;

                trainerNarc.trainers[i].battleType = 2;
                trainerNarc.trainers[i].AI = (byte)(trainerNarc.trainers[i].AI | 128);
                if (trainerNarc.trainers[i].pokemon.pokemon.Count == 2)
                {
                    TrainerPokemon p1 = trainerNarc.trainers[i].pokemon.pokemon[0];
                    trainerNarc.trainers[i].pokemon.pokemon.Add(p1.Clone());
                    trainerNarc.trainers[i].numPokemon++;
                }
                trainerNarc.trainers[i].ApplyData();
                trainerNarc.trainers[i].pokemon.ApplyData();
            }

            MessageBox.Show("Triple battle mode applied");
        }

        private void rotationBattleButton_Click(object sender, EventArgs e)
        {
            if (trainerNarc == null || trainerPokeNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (loadedRomPath == "" || loadingNARCS)
            {
                MessageBox.Show("The rom has not been fully loaded");
                return;
            }

            for (int i = 0; i < trainerNarc.trainers.Count; i++)
            {
                //Exclude trainers with only one pokemon or that are already multi battles
                if (trainerNarc.trainers[i].pokemon.pokemon.Count == 1 || trainerNarc.trainers[i].battleType != 0) continue;

                trainerNarc.trainers[i].battleType = 3;
                if (trainerNarc.trainers[i].pokemon.pokemon.Count == 2)
                {
                    TrainerPokemon p1 = trainerNarc.trainers[i].pokemon.pokemon[1];
                    trainerNarc.trainers[i].pokemon.pokemon.Add(p1.Clone());
                    trainerNarc.trainers[i].numPokemon++;
                }
                trainerNarc.trainers[i].ApplyData();
                trainerNarc.trainers[i].pokemon.ApplyData();
            }

            MessageBox.Show("Rotation battle mode applied");
        }

        private void replaceSoundButton_Click(object sender, EventArgs e)
        {
            fileSystem.soundData.WriteToSwav((int)replaceSoundID.Value - 1);
        }

        private void replaceIconButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "pokemon icon file|*.pkicon";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenRead(prompt.FileName);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                pokemonIconNarc.files[(int)replaceIconID.Value].bytes = bytes;

                MessageBox.Show("File Replace Complete");
            }
        }

        private void replaceOverlayButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "binary file|*.bin";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenRead(prompt.FileName);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                fileSystem.overlays[(int)replaceOverlayID.Value] = new List<byte>(bytes);

                MessageBox.Show("File Replace Complete");
            }
        }

        private void replaceMapButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "binary file|*.bin";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenRead(prompt.FileName);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                mapModelsNarc.models[(int)replaceMapID.Value].bytes = bytes;

                MessageBox.Show("File Replace Complete");
            }
        }

        private void pokepatherButton_Click(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (pokePatchEditor == null || pokePatchEditor.IsDisposed) pokePatchEditor = new Pokepatcher();
            pokePatchEditor.Show();
            pokePatchEditor.BringToFront();
        }

        private void typeChartEditorButton_Click(object sender, EventArgs e)
        {
            if (loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (typeChartEditor == null || typeChartEditor.IsDisposed) typeChartEditor = new TypeChartEditor();
            typeChartEditor.Show();
            typeChartEditor.BringToFront();
        }
    }

    public enum RomType
    {
        Other,
        BW2,
        HGSS
    }
}
