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
using DarkModeForms;
using System.Drawing.Imaging;

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
        public static int eggMovesNarcID = -1;
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
        public static int pokemartItemCountNarcID = -1;
        public static int AIScriptNarcID = -1;
        public static int keyboardNarcID = -1;
        public static int xpCurveNarcID = -1;
        public static int habitatListNarcID = -1;
        public static int hiddenGrottoNarcID = -1;

        //Rom Data
        public static NDSFileSystem fileSystem;
        private static string romType = "";
        public static RomType RomType => (romType == "pokemon b2" || romType == "pokemon w2") ? RomType.BW2 : (romType == "pokemon b" || romType == "pokemon w") ? RomType.BW1 : (romType == "pokemon hg" || romType == "pokemon ss") ? RomType.HGSS : RomType.Other;
        public static bool BlackVersion => romType.IndexOf('b') != -1;

        //Narcs
        public static TextNARC textNarc;
        public static TextNARC storyTextNarc;
        public static PokemonSpritesNARC pokemonSpritesNarc;
        public static PokemonIconNARC pokemonIconNarc;
        public static PokemonDataNARC pokemonDataNarc;
        public static LearnsetNARC learnsetNarc;
        public static EggMoveNARC eggMoveNarc;
        public static EvolutionDataNARC evolutionsNarc;
        public static ChildPokemonNARC childPokemonNarc;
        public static MoveDataNARC moveDataNarc;
        public static ItemDataNARC itemDataNarc;
        public static MoveAnimationNARC moveAnimationNarc;
        public static MoveAnimationNARC moveAnimationExtraNarc;
        public static ZoneDataNARC zoneDataNarc;
        public static MapMatrixNARC mapMatrixNarc;
        public static MapFilesNARC mapFilesNarc;
        public static ScriptNARC scriptNarc;
        public static TrTextEntriesNARC trTextEntriesNarc;
        public static TrTextIndexNARC trTextIndicesNarc;
        public static TrainerDataNARC trainerNarc;
        public static TrainerPokeNARC trainerPokeNarc;
        public static OverworldObjectsNARC overworldsNarc;
        public static EncounterNARC encounterNarc;
        public static PokemartNARC pokemartNarc;
        public static PokemartItemCountNARC pokemartItemCountNarc;
        public static AIScriptNARC AIScriptNarc;
        public static KeyboardNARC keyboardNarc;
        public static XPCurveNARC xpCurveNarc;
        public static HabitatListNARC habitatListNarc;
        public static HiddenGrottoNARC hiddenGrottoNarc;

        //Forms
        public static TextViewer textViewer;
        public static PokemonEditor pokemonEditor;
        public static MoveEditor moveEditor;
        public static OverworldEditor overworldEditor;
        public static EncounterEditor encounterEditor;
        public static NewScriptEditor scriptEditor;
        public static TrainerEditor trainerEditor;
        public static TypeSwapEditor typeSwapEditor;
        public static RandomMovesEditor presetMovesEditor;
        public static PokemartEditor pokemartEditor;
        public static GrottoEditor grottoEditor;
        public static ExpCurveEditor xpCurveEditor;
        public static OverlayEditor overlayEditor;
        public static TypeChartEditor typeChartEditor;
        public static Pokepatcher pokePatchEditor;
        public static FileExplorer fileExplorer;
        public static List<Form> extraForms = new List<Form>();

        public bool loadingNARCS = false;
        bool autoLoaded = false;
        public static string AutoLoad = "";

        public static DarkModeCS darkMode = null;

        public static MainEditor instance;

        public MainEditor()
        {
            instance = this;
            InitializeComponent();

            TryAutoLoad();
        }

        public void ChangeTheme(object sender, EventArgs e)
        {
            if (themeDropdown.SelectedIndex < 0 || themeDropdown.SelectedIndex >= themeDropdown.Items.Count) return;
            if (themeDropdown.SelectedIndex == 0 && darkMode == null) return;

            if (darkMode == null) darkMode = new DarkModeCS(this)
            {
                ColorMode = themeDropdown.SelectedIndex == 1 ? DarkModeCS.DisplayMode.DarkMode : DarkModeCS.DisplayMode.ClearMode
            };
            darkMode.ApplyTheme(themeDropdown.SelectedIndex == 1);
            FileFunctions.WriteFileSection("Preferences.txt", "DarkMode", new byte[] { (byte)themeDropdown.SelectedIndex });
        }

        public static List<Form> GetAllForms()
        {
            List<Form> list = new List<Form>();
            list.Add(instance);
            if (textViewer != null && !textViewer.IsDisposed) list.Add(textViewer);
            if (pokemonEditor != null && !pokemonEditor.IsDisposed) list.Add(pokemonEditor);
            if (moveEditor != null && !moveEditor.IsDisposed) list.Add(moveEditor);
            if (overworldEditor != null && !overworldEditor.IsDisposed) list.Add(overworldEditor);
            if (encounterEditor != null && !encounterEditor.IsDisposed) list.Add(encounterEditor);
            if (xpCurveEditor != null && !xpCurveEditor.IsDisposed) list.Add(xpCurveEditor);
            if (scriptEditor != null && !scriptEditor.IsDisposed) list.Add(scriptEditor);
            if (trainerEditor != null && !trainerEditor.IsDisposed) list.Add(trainerEditor);
            if (pokemartEditor != null && !pokemartEditor.IsDisposed) list.Add(pokemartEditor);
            if (grottoEditor != null && !grottoEditor.IsDisposed) list.Add(grottoEditor);
            if (overlayEditor != null && !overlayEditor.IsDisposed) list.Add(overlayEditor);
            if (typeSwapEditor != null && !typeSwapEditor.IsDisposed) list.Add(typeSwapEditor);
            if (presetMovesEditor != null && !presetMovesEditor.IsDisposed) list.Add(presetMovesEditor);
            if (pokePatchEditor != null && !pokePatchEditor.IsDisposed) list.Add(pokePatchEditor);
            if (fileExplorer != null && !fileExplorer.IsDisposed) list.Add(fileExplorer);
            if (typeChartEditor != null && !typeChartEditor.IsDisposed) list.Add(typeChartEditor);
            for (int i = 0; i < extraForms.Count; i++)
            {
                if (extraForms[i].IsDisposed)
                {
                    extraForms.RemoveAt(i);
                    i--;
                    continue;
                }
                list.Add(extraForms[i]);
            }
            return list;
        }

        public static void GetVersionConstants(string romType)
        {
            if (romType == "pokemon b2" || romType == "pokemon w2")
            {
                //FirstNARCPointerLocation = VersionConstants.BW2_FirstNarcPointerLocation;
                //FirstNARCPointerLocation = VersionConstants.FindFirstNARCPointerLocation(romFile);
                //sDatLocation = VersionConstants.FindSDat(romFile);
                //LastNarc = VersionConstants.BW2_LastNarc;
                //NARCsToSkip = VersionConstants.BW2_NARCsToSkip;
                fileSizeLimit = VersionConstants.BW2_FileSizeLimit;

                textNarcID = VersionConstants.BW2_TextNARCID;
                storyTextNarcID = VersionConstants.BW2_StoryTextNARCID;
                mapModelsNarcID = VersionConstants.BW2_MapFilesNARCID;
                mapMatrixNarcID = VersionConstants.BW2_MapMatriciesNARCID;
                pokemonSpritesNarcID = VersionConstants.BW2_PokemonSpritesNARCID;
                pokemonIconsNarcID = VersionConstants.BW2_PokemonIconsNARCID;
                pokemonDataNarcID = VersionConstants.BW2_PokemonDataNARCID;
                levelUpMovesNarcID = VersionConstants.BW2_LevelUpMovesNARCID;
                eggMovesNarcID = VersionConstants.BW2_EggMoveNARCID;
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
                pokemartItemCountNarcID = VersionConstants.BW2_PokemartItemCountNARCID;
                AIScriptNarcID = VersionConstants.BW2_AIScriptNARCID;
                keyboardNarcID = VersionConstants.BW2_KeyboardLayoutNARCID;
                xpCurveNarcID = VersionConstants.BW2_XPCurveNARCID;
                habitatListNarcID = VersionConstants.BW2_HabitatListNARCID;
                hiddenGrottoNarcID = VersionConstants.BW2_HiddenGrottoNARCID;
                return;
            }
            else if (romType == "pokemon b" || romType == "pokemon w")
            {
                //FirstNARCPointerLocation = VersionConstants.BW2_FirstNarcPointerLocation;
                //FirstNARCPointerLocation = VersionConstants.FindFirstNARCPointerLocation(romFile);
                //sDatLocation = VersionConstants.FindSDat(romFile);
                //LastNarc = VersionConstants.BW2_LastNarc;
                //NARCsToSkip = VersionConstants.BW2_NARCsToSkip;
                fileSizeLimit = VersionConstants.BW2_FileSizeLimit;

                textNarcID = VersionConstants.BW1_TextNARCID;
                storyTextNarcID = VersionConstants.BW1_StoryTextNARCID;
                mapModelsNarcID = VersionConstants.BW1_MapFilesNARCID;
                mapMatrixNarcID = VersionConstants.BW1_MapMatriciesNARCID;
                pokemonSpritesNarcID = VersionConstants.BW1_PokemonSpritesNARCID;
                pokemonIconsNarcID = VersionConstants.BW1_PokemonIconsNARCID;
                pokemonDataNarcID = VersionConstants.BW1_PokemonDataNARCID;
                levelUpMovesNarcID = VersionConstants.BW1_LevelUpMovesNARCID;
                eggMovesNarcID = VersionConstants.BW1_EggMovesNARCID;
                evolutionNarcID = VersionConstants.BW1_EvolutionsNARCID;
                childPokemonNarcID = VersionConstants.BW1_ChildPokemonNARCID;
                moveDataNarcID = VersionConstants.BW1_MoveDataNARCID;
                itemDataNarcID = VersionConstants.BW1_ItemDataNARCID;
                moveAnimationNarcID = VersionConstants.BW1_MoveAnimationNARCID;
                moveAnimationExtraNarcID = VersionConstants.BW1_MoveAnimationExtraNARCID;
                zoneDataNarcID = VersionConstants.BW1_ZoneDataNARCID;
                scriptNarcID = VersionConstants.BW1_ScriptNARCID;
                trTextEntriesNarcID = VersionConstants.BW1_TrTextEntriesNARCID;
                trTextIndicesNarcID = VersionConstants.BW1_TrTextIndicesNARCID;
                trainerDataNarcID = VersionConstants.BW1_TrainerDataNARCID;
                trainerPokeNarcID = VersionConstants.BW1_TrainerPokemonNARCID;
                overworldsNarcID = VersionConstants.BW1_OverworldsNARCID;
                encounterNarcID = VersionConstants.BW1_EncountersNARCID;
                pokemartNarcID = VersionConstants.BW1_PokemartNARCID;
                pokemartItemCountNarcID = VersionConstants.BW1_PokemartItemCountNARCID;
                AIScriptNarcID = VersionConstants.BW1_AIScriptNARCID;
                keyboardNarcID = VersionConstants.BW1_KeyboardLayoutNARCID;
                xpCurveNarcID = VersionConstants.BW1_XPCurveNARCID;
                habitatListNarcID = VersionConstants.BW1_HabitatListNARCID;
                hiddenGrottoNarcID = VersionConstants.BW1_HiddenGrottoNARCID;
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

            MessageBox.Show("Invalid file type.\nExpected a pokemon black, white, black 2, or white 2 rom");
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

            string prevStatus = statusText.Text;
            statusText.Text = "Saving rom";
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                taskProgressBar.Value = 0;
                loadedRomPath = prompt.FileName;
                FileStream fileStream = null;
                try
                {
                    fileStream = File.OpenWrite(loadedRomPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save rom");

                    statusText.Text = "Failed to save rom - " + DateTime.Now.StatusText();
                    return;
                }
                fileStream.SetLength(0);
                byte[] data = fileSystem.BuildRom();
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
                taskProgressBar.Value = taskProgressBar.Maximum;
                statusText.Text = "Saved rom to " + prompt.FileName + " - " + DateTime.Now.StatusText();
                MessageBox.Show("Rom saved to " + prompt.FileName);
            }
            else
            {
                statusText.Text = prevStatus;
            }
        }

        private void dumpRomButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog prompt = new FolderBrowserDialog();
            string prevStatus = statusText.Text;
            statusText.Text = "Dumping rom";
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                fileSystem.DumpFileSystem(prompt.SelectedPath);
                statusText.Text = "Dumped rom to " + prompt.SelectedPath + " - " + DateTime.Now.StatusText();
                MessageBox.Show("Rom dumped successfully");
            }
            else
            {
                statusText.Text = prevStatus;
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
            if (xpCurveEditor != null && !xpCurveEditor.IsDisposed) xpCurveEditor.Close();
            if (scriptEditor != null && !scriptEditor.IsDisposed) scriptEditor.Close();
            if (trainerEditor != null && !trainerEditor.IsDisposed) trainerEditor.Close();
            if (pokemartEditor != null && !pokemartEditor.IsDisposed) pokemartEditor.Close();
            if (grottoEditor != null && !grottoEditor.IsDisposed) grottoEditor.Close();
            if (overlayEditor != null && !overlayEditor.IsDisposed) overlayEditor.Close();
            if (typeSwapEditor != null && !typeSwapEditor.IsDisposed) typeSwapEditor.Close();
            if (presetMovesEditor != null && !presetMovesEditor.IsDisposed) presetMovesEditor.Close();
            if (pokePatchEditor != null && !pokePatchEditor.IsDisposed) pokePatchEditor.Close();
            if (fileExplorer != null && !fileExplorer.IsDisposed) fileExplorer.Close();
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
            mapFilesNarc = null;
            scriptNarc = null;
            trainerNarc = null;
            trainerPokeNarc = null;
            overworldsNarc = null;
            encounterNarc = null;
            pokemartEditor = null;
            grottoEditor = null;
            overlayEditor = null;
            loadingNARCS = true;
            taskProgressBar.Value = 0;
            statusText.Text = "Loading rom from " + (fromFolder ? "folder" : "file");

            bool fail = false;
            await Task.Run(() =>
            {
                try
                {
                    if (fromFolder)
                    {
                        NDSFileSystem fs = NDSFileSystem.FromFileSystem(loadedRomPath, true);
                        if (fs == null)
                        {
                            fail = true;
                            return;
                        }
                        fileSystem = fs;
                    }
                    else
                    {
                        FileStream fileStream = File.OpenRead(loadedRomPath);
                        fileSystem = NDSFileSystem.FromRom(fileStream, true);

                        fileStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    if (autoLoaded)
                    {
                        DisableAutoLoad(null, null);
                        MessageBox.Show("Auto load has been disabled due to an error with the rom file.\nPlease restart the application.");
                    }
                    throw ex;
                }
            });
            if (fail)
            {
                openRomButton.Enabled = true;
                loadFromFolderButton.Enabled = true;
                autoLoadButton.Enabled = true;
                statusText.Text = "Failed to load rom - " + DateTime.Now.StatusText();
                return;
            }

            //Setup Editor
            saveRomButton.Enabled = true;
            dumpRomButton.Enabled = true;
            openRomButton.Enabled = true;
            loadFromFolderButton.Enabled = true;
            autoLoadButton.Enabled = true;
            romNameText.Text = "Rom: " + loadedRomPath.Substring(loadedRomPath.LastIndexOf('\\') + 1, loadedRomPath.Length - (loadedRomPath.LastIndexOf('\\') + 1));
            romTypeText.Text = "Rom Type: " + romType;
            taskProgressBar.Value = taskProgressBar.Maximum;
            statusText.Text = "Loaded rom - " + DateTime.Now.StatusText();

            MessageBox.Show("Rom Loaded");
            
            loadingNARCS = false;
            autoLoaded = false;
        }

        public static void SetNARCVars(NDSFileSystem fileSystem)
        {
            romType = fileSystem.romType;
            textNarc = fileSystem.narcs[textNarcID] as TextNARC;
            storyTextNarc = fileSystem.narcs[storyTextNarcID] as TextNARC;
            pokemonSpritesNarc = fileSystem.narcs[pokemonSpritesNarcID] as PokemonSpritesNARC;
            pokemonIconNarc = fileSystem.narcs[pokemonIconsNarcID] as PokemonIconNARC;
            pokemonDataNarc = fileSystem.narcs[pokemonDataNarcID] as PokemonDataNARC;
            learnsetNarc = fileSystem.narcs[levelUpMovesNarcID] as LearnsetNARC;
            eggMoveNarc = fileSystem.narcs[eggMovesNarcID] as EggMoveNARC;
            evolutionsNarc = fileSystem.narcs[evolutionNarcID] as EvolutionDataNARC;
            moveDataNarc = fileSystem.narcs[moveDataNarcID] as MoveDataNARC;
            itemDataNarc = fileSystem.narcs[itemDataNarcID] as ItemDataNARC;
            childPokemonNarc = fileSystem.narcs[childPokemonNarcID] as ChildPokemonNARC;
            moveAnimationNarc = fileSystem.narcs[moveAnimationNarcID] as MoveAnimationNARC;
            moveAnimationExtraNarc = fileSystem.narcs[moveAnimationExtraNarcID] as MoveAnimationNARC;
            zoneDataNarc = fileSystem.narcs[zoneDataNarcID] as ZoneDataNARC;
            mapMatrixNarc = fileSystem.narcs[mapMatrixNarcID] as MapMatrixNARC;
            mapFilesNarc = fileSystem.narcs[mapModelsNarcID] as MapFilesNARC;
            scriptNarc = fileSystem.narcs[scriptNarcID] as ScriptNARC;
            trTextEntriesNarc = fileSystem.narcs[trTextEntriesNarcID] as TrTextEntriesNARC;
            trTextIndicesNarc = fileSystem.narcs[trTextIndicesNarcID] as TrTextIndexNARC;
            trainerNarc = fileSystem.narcs[trainerDataNarcID] as TrainerDataNARC;
            trainerPokeNarc = fileSystem.narcs[trainerPokeNarcID] as TrainerPokeNARC;
            overworldsNarc = fileSystem.narcs[overworldsNarcID] as OverworldObjectsNARC;
            encounterNarc = fileSystem.narcs[encounterNarcID] as EncounterNARC;
            xpCurveNarc = fileSystem.narcs[xpCurveNarcID] as XPCurveNARC;
            AIScriptNarc = fileSystem.narcs[AIScriptNarcID] as AIScriptNARC;
            if (RomType == RomType.BW2)
            {
                pokemartNarc = fileSystem.narcs[pokemartNarcID] as PokemartNARC;
                pokemartItemCountNarc = fileSystem.narcs[pokemartItemCountNarcID] as PokemartItemCountNARC;
                keyboardNarc = fileSystem.narcs[keyboardNarcID] as KeyboardNARC;
                habitatListNarc = fileSystem.narcs[habitatListNarcID] as HabitatListNARC;
                hiddenGrottoNarc = fileSystem.narcs[hiddenGrottoNarcID] as HiddenGrottoNARC;
            }
        }

        public void TryAutoLoad()
        {
            List<byte> dark = FileFunctions.ReadFileSection("Preferences.txt", "DarkMode");
            List<byte> enabled = FileFunctions.ReadFileSection("Preferences.txt", "AutoLoad");
            List<byte> value = FileFunctions.ReadFileSection("Preferences.txt", "AutoLoadPath");
            if (dark == null || dark.Count == 0)
            {
                FileFunctions.WriteFileSection("Preferences.txt", "DarkMode", new byte[] { 0 });
                themeDropdown.SelectedIndex = 0;
                ChangeTheme(null, null);
            }
            else
            {
                themeDropdown.SelectedIndex = dark[0];
                ChangeTheme(null, null);
            }

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
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
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

            if (scriptEditor == null || scriptEditor.IsDisposed) scriptEditor = new NewScriptEditor();
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
            encounterEditor.Show();
            encounterEditor.BringToFront();
        }

        private void OpenHiddenGrottoEditor(object sender, EventArgs e)
        {
            if (RomType != RomType.BW2)
            {
                MessageBox.Show("Only available for Black 2 and White 2 roms");
                return;
            }
            if (hiddenGrottoNarc == null || loadingNARCS)
            {
                MessageBox.Show("Hidden Grotto data files have not been loaded");
                return;
            }

            if (grottoEditor == null || grottoEditor.IsDisposed) grottoEditor = new GrottoEditor();
            ChangeTheme(null, null);
            grottoEditor.Show();
            grottoEditor.BringToFront();
        }

        private void OpenShopEditor(object sender, EventArgs e)
        {
            if (RomType != RomType.BW2)
            {
                MessageBox.Show("Only available for Black 2 and White 2 roms");
                return;
            }
            if (pokemartNarc == null || pokemartItemCountNarc == null || itemDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Pokemart data files have not been loaded");
                return;
            }

            if (pokemartEditor == null || pokemartEditor.IsDisposed) pokemartEditor = new PokemartEditor();
            ChangeTheme(null, null);
            pokemartEditor.Show();
            pokemartEditor.BringToFront();
        }

        public void OpenFileExplorer(object sender, EventArgs e)
        {
            if (fileSystem == null || loadingNARCS)
            {
                MessageBox.Show("Data files have not been loaded");
                return;
            }

            if (fileExplorer == null || fileExplorer.IsDisposed) fileExplorer = new FileExplorer();
            ChangeTheme(null, null);
            fileExplorer.Show();
            fileExplorer.BringToFront();
        }

        private void openOverlayEditorButton_Click(object sender, EventArgs e)
        {
            if (loadingNARCS)
            {
                MessageBox.Show("Overlay files have not been loaded");
                return;
            }

            if (overlayEditor == null || overlayEditor.IsDisposed) overlayEditor = new OverlayEditor(fileSystem);
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
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
            ChangeTheme(null, null);
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

                    if (other.romType != romType)
                    {
                        MessageBox.Show("The provided game does not match the loaded rom.");
                        return;
                    }

                    var patch = PatchingSystem.MakePatch(fileSystem, other);

                    if (xpCurveNarc == null || loadingNARCS)
                    {
                        MessageBox.Show("Necessary data files have not been loaded");
                        return;
                    }

                    if (patch.Count == 0)
                    {
                        MessageBox.Show("No changes found in from the provided file.");
                        return;
                    }

                    PatchMaker maker = new PatchMaker(patch);
                    maker.ShowDialog();

                    //SaveFileDialog save = new SaveFileDialog();
                    //save.Filter = RomType == RomType.BW1 ? "Black White 1 Patch File|*.bw1Patch" : "Black White 2 Patch File|*.bw2Patch";
                    //save.Title = "Select where to save the patch file";

                    //if (save.ShowDialog() == DialogResult.OK)
                    //{
                    //    Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();

                    //    if (!fileSystem.arm9.SequenceEqual(other.arm9))
                    //    {
                    //        data.Add("arm9_", fileSystem.arm9);
                    //    }
                    //    if (!fileSystem.y9.bytes.SequenceEqual(other.y9.bytes))
                    //    {
                    //        data.Add("y9_", fileSystem.y9.bytes);
                    //    }

                    //    for (int i = 0; i < fileSystem.narcs.Count; i++)
                    //    {
                    //        fileSystem.narcs[i].WriteData();
                    //        if (!fileSystem.narcs[i].byteData.SequenceEqual(other.narcs[i].byteData))
                    //        {
                    //            data.Add("id_" + i, fileSystem.narcs[i].GetPatchBytes(other.narcs[i]));
                    //        }
                    //    }

                    //    //for (int i = 0; i < 700/*textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count*/; i++)
                    //    //    if (!fileSystem.soundData.GetSoundBytes(i).SequenceEqual(other.soundData.GetSoundBytes(i)))
                    //    //    {
                    //    //        data.Add("sound" + (i + 1), fileSystem.soundData.GetSoundBytes(i));
                    //    //    }

                    //    for (int i = 0; i < fileSystem.overlays.Count; i++)
                    //        if (!fileSystem.overlays[i].SequenceEqual(other.overlays[i]))
                    //        {
                    //            data.Add("ov_" + i, fileSystem.overlays[i]);
                    //        }

                    //    FileFunctions.WriteAllSections(save.FileName, data, true);
                    //    MessageBox.Show("Patch Created");
                    //}
                }
            }
            else
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

                    SaveFileDialog prompt2 = new SaveFileDialog();
                    prompt2.Filter = RomType == RomType.BW1 ? "Black White 1 Patch File|*.bw1Patch" : "Black White 2 Patch File|*.bw2Patch";
                    if (prompt2.ShowDialog() == DialogResult.OK)
                    {
                        Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();
                        for (int i = 0; i < fileSystem.narcs.Count; i++)
                        {
                            if (fileSystem.narcs[i].GetType().Name != "NARC" && selectedNARCs.Contains(i))
                            {
                                fileSystem.narcs[i].WriteData();
                                data.Add("id_" + i, fileSystem.narcs[i].GetPatchBytes(other.narcs[i]));
                            }
                        }

                        FileFunctions.WriteAllSections(prompt2.FileName, data, true);
                        MessageBox.Show("Patch Created");
                    }
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
            prompt.Filter = "Gen 5 Patch File|*.Vpatch";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, IEnumerable<byte>> data = FileFunctions.ReadAllSections(prompt.FileName, true);

                if (data.ContainsKey("version"))
                {
                    string version = Encoding.ASCII.GetString(data["version"].ToArray());
                    if (version != romType)
                    {
                        MessageBox.Show("This patch's game version does not match the current rom.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("This patch's game version does not match the current rom.");
                    return;
                }

                PatchingSystem.ApplyPatch(fileSystem, data);

                MessageBox.Show("Patch Applied");
                return;

                //foreach (KeyValuePair<string, IEnumerable<byte>> entry in data)
                //{
                //    if (entry.Key.StartsWith("sound"))
                //    {
                //        //if (int.TryParse(entry.Key.Substring(5), out int id) && id >= 0)
                //        //{
                //        //    fileSystem.soundData.WriteToSwavFromPatch(id - 1, entry.Value.ToList());
                //        //}
                //    }
                //    else if (entry.Key.StartsWith("ov_"))
                //    {
                //        if (int.TryParse(entry.Key.Substring(3), out int id) && id >= 0 && id < fileSystem.overlays.Count)
                //        {
                //            fileSystem.overlays[id] = entry.Value.ToList();
                //        }
                //    }
                //    else if (entry.Key.StartsWith("arm9_"))
                //    {
                //        fileSystem.arm9 = entry.Value.ToList();
                //    }
                //    else if (entry.Key.StartsWith("y9_"))
                //    {
                //        fileSystem.y9 = new Y9Table(entry.Value.ToArray());
                //    }
                //    else if (int.TryParse(entry.Key.Substring(3), out int id) && id >= 0 && id < fileSystem.narcs.Count)
                //    {
                //        fileSystem.narcs[id].ReadPatchBytes(entry.Value.ToArray());
                //    }
                //}
                //
                //MessageBox.Show("Patch Applied");
            }
        }

        public void rogueModeButton_Click(object sender, EventArgs e)
        {
            if (RomType != RomType.BW2)
            {
                MessageBox.Show("Only available for Black 2 and White 2 roms");
                return;
            }
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
            //fileSystem.soundData.WriteToSwav((int)replaceSoundID.Value - 1);
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
                pokemonIconNarc.files[(int)replaceIconID.Value] = bytes;

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
                mapFilesNarc.files[(int)replaceMapID.Value].bytes = bytes;

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
            ChangeTheme(null, null);
            pokePatchEditor.Show();
            pokePatchEditor.BringToFront();
        }

        private void typeChartEditorButton_Click(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (typeChartEditor == null || typeChartEditor.IsDisposed) typeChartEditor = new TypeChartEditor();
            ChangeTheme(null, null);
            typeChartEditor.Show();
            typeChartEditor.BringToFront();
        }

        private void littleCupButton_Click(object sender, EventArgs e)
        {
            if (pokemonDataNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }
            //Normalize level rates
            foreach (PokemonEntry pk in pokemonDataNarc.pokemon)
            {
                pk.xpYield = 0;
                pk.levelRate = 0;
                pk.ApplyData();
            }
            //Edit movesets (first 4 moves are known at level 5, the rest can be relearned)
            foreach (LevelUpMoveset ls in learnsetNarc.learnsets)
            {
                LevelUpMoveSlot[] first4 = new LevelUpMoveSlot[4];
                if (ls.moves.Count > 4)
                {
                    for (int i = 0; i < 4; i++) first4[i] = new LevelUpMoveSlot(ls.moves[i].moveID, 2);
                    for (int i = 4; i < ls.moves.Count; i++) ls.moves[i - 4] = new LevelUpMoveSlot(ls.moves[i].moveID, 1);
                    for (int i = 0; i < 4; i++) ls.moves[ls.moves.Count - 4 + i] = first4[i];
                }
                else for (int i = 0; i < ls.moves.Count; i++) ls.moves[i] = new LevelUpMoveSlot(ls.moves[i].moveID, 1);
                ls.ApplyData();
            }
            //Add move relearner to every pokemon center
            foreach (OverworldObjectsEntry ow in overworldsNarc.objects)
            {
                if (ow != null && ow.NPCs != null && ow.NPCs.Exists(npc => npc.xPosition == 7 && npc.yPosition == 10 && npc.scriptUsed == 2100))
                {
                    ow.NPCs.Add(new OverworldNPC() { xPosition = 1, yPosition = 12, defaultDirection = 3, sprite = 14, scriptUsed = 2261 });
                    ow.ApplyData();
                }
            }
            //Edit trainer pokemon
            foreach (TrainerEntry tr in trainerNarc.trainers)
            {
                foreach (TrainerPokemon trpoke in tr.pokemon.pokemon)
                {
                    trpoke.level = 5;
                    trpoke.pokemonID = childPokemonNarc.ids[trpoke.pokemonID];
                }
                tr.pokemon.ApplyData();
            }
            //Edit encounters
            foreach (EncounterEntry enc in encounterNarc.encounterPools)
            {
                foreach (List<EncounterSlot> slots in enc.groupedLandSlots)
                {
                    foreach (EncounterSlot slot in slots)
                    {
                        slot.minLevel = 5;
                        slot.maxLevel = 5;
                        slot.pokemonID = childPokemonNarc.ids[slot.pokemonID];
                    }
                }
                foreach (List<EncounterSlot> slots in enc.groupedWaterSlots)
                {
                    foreach (EncounterSlot slot in slots)
                    {
                        slot.minLevel = 5;
                        slot.maxLevel = 5;
                        slot.pokemonID = childPokemonNarc.ids[slot.pokemonID];
                    }
                }
                enc.ApplyData();
            }

            for (int i = 0; i < 100; i++)
            {
                xpCurveNarc.curves[0].SetXPAtLevel(i, (i - 1) * 999999);
            }

            MessageBox.Show("Little Cup Game Mode Applied");
        }

        private void openXPCurveEditorButton_Click(object sender, EventArgs e)
        {
            if (xpCurveNarc == null || loadingNARCS)
            {
                MessageBox.Show("Necessary data files have not been loaded");
                return;
            }

            if (xpCurveEditor == null || xpCurveEditor.IsDisposed) xpCurveEditor = new ExpCurveEditor();
            ChangeTheme(null, null);
            xpCurveEditor.Show();
            xpCurveEditor.BringToFront();
        }

        private void ReadCommandHeaders()
        {
            //string[] lines = File.ReadAllLines("ScriptCommands.h");
            //string[] lines2 = File.ReadAllLines("ScriptCommands2.h");
            //string[] lines3 = File.ReadAllLines("ScriptCommands3.h");
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    string[] parts = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            //    string[] parts2 = lines2[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            //    int num = int.Parse(parts[0], System.Globalization.NumberStyles.HexNumber);
            //    string name = parts[1];
            //    int[] par = new int[parts.Length - 2];
            //    for (int j = 0; j < par.Length; j++)
            //    {
            //        if (parts[j + 2] == "i") par[j] = 4;
            //        else if (parts[j + 2] == "s") par[j] = 2;
            //        else if (parts[j + 2] == "b") par[j] = 1;
            //        else throw new Exception();
            //    }
            //    Debug.WriteLine("//" + lines3[i]);
            //    string output = "void " + name + "(";
            //    for (int j = 0; j < par.Length; j++)
            //    {
            //        output += par[j] == 1 ? "char " : par[j] == 2 ? "short " : "int ";
            //        if (j + 2 < parts2.Length) output += parts2[j + 2];
            //        else output += "p" + j.ToString();
            //        if (j < par.Length - 1) output += ", ";
            //    }
            //    output += ");";
            //    Debug.WriteLine(output);
            //}

            string[] lines = File.ReadAllLines("ScriptCommands.h");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(parts[0], System.Globalization.NumberStyles.HexNumber);
                string name = parts[1];
                int[] par = new int[parts.Length - 2];
                for (int j = 0; j < par.Length; j++)
                {
                    if (parts[j + 2] == "i") par[j] = 4;
                    else if (parts[j + 2] == "s") par[j] = 2;
                    else if (parts[j + 2] == "b") par[j] = 1;
                    else throw new Exception();
                }
                string output = "{0x" + parts[0].ToUpper() + ", new CommandType(\"" + parts[1] + "\", " + par.Length;
                for (int j = 0; j < par.Length; j++)
                {
                    output += ", " + par[j];
                }
                output += ")},";
                Debug.WriteLine(output);
            }
        }

        private void FindChanges()
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "Nds Roms|*.nds";
            prompt.Title = "Select the base version of the rom";
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = File.OpenRead(prompt.FileName);
                NDSFileSystem other = NDSFileSystem.FromRom(fileStream);
                fileStream.Close();

                Debug.WriteLine("Moves:");
                for (int i = 0; i < moveDataNarc.moves.Count; i++)
                {
                    if (i >= other.moveDataNarc.moves.Count || !other.moveDataNarc.moves[i].bytes.SequenceEqual(moveDataNarc.moves[i].bytes) ||
                        textNarc.textFiles[VersionConstants.MoveDescriptionTextFileID].text[i] != other.textNarc.textFiles[VersionConstants.MoveDescriptionTextFileID].text[i])
                    {
                        Debug.WriteLine(moveDataNarc.moves[i].ToString());
                    }
                }
                Debug.WriteLine("Trainers:");
                for (int i = 0; i < trainerNarc.trainers.Count; i++)
                {
                    if (i >= other.trainerNarc.trainers.Count || !other.trainerNarc.trainers[i].bytes.SequenceEqual(trainerNarc.trainers[i].bytes) ||
                        !other.trainerNarc.trainers[i].pokemon.bytes.SequenceEqual(trainerNarc.trainers[i].pokemon.bytes))
                    {
                        Debug.WriteLine(trainerNarc.trainers[i].ToString());
                    }
                }

                Debug.WriteLine("Pokemon:");
                for (int i = 0; i < pokemonDataNarc.pokemon.Count; i++)
                {
                    if (i < other.pokemonDataNarc.pokemon.Count) other.pokemonDataNarc.pokemon[i].bytes[9] = pokemonDataNarc.pokemon[i].bytes[9];
                    if (i >= other.pokemonDataNarc.pokemon.Count || !other.pokemonDataNarc.pokemon[i].bytes.SequenceEqual(pokemonDataNarc.pokemon[i].bytes) ||
                        !other.pokemonDataNarc.pokemon[i].evolutions.bytes.SequenceEqual(pokemonDataNarc.pokemon[i].evolutions.bytes) ||
                        !other.pokemonDataNarc.pokemon[i].levelUpMoves.bytes.SequenceEqual(pokemonDataNarc.pokemon[i].levelUpMoves.bytes))
                    {
                        Debug.WriteLine(pokemonDataNarc.pokemon[i].Name);
                    }
                }

                Debug.WriteLine("Egg moves:");
                for (int i = 0; i < eggMoveNarc.entries.Count; i++)
                {
                    if (i >= other.eggMoveNarc.entries.Count || !other.eggMoveNarc.entries[i].bytes.SequenceEqual(eggMoveNarc.entries[i].bytes))
                    {
                        Debug.WriteLine(pokemonDataNarc.pokemon[i].Name);
                    }
                }

                Debug.WriteLine("Encounters:");
                for (int i = 0; i < encounterNarc.encounterPools.Count; i++)
                {
                    if (i >= other.encounterNarc.encounterPools.Count || !other.encounterNarc.encounterPools[i].bytes.SequenceEqual(encounterNarc.encounterPools[i].bytes))
                    {
                        Debug.WriteLine(encounterNarc.encounterPools[i].ToString());
                    }
                }
            }
        }
    }

    public enum RomType
    {
        Other,
        BW1,
        BW2,
        HGSS
    }
}
