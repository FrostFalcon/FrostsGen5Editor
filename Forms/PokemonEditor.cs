using NewEditor.Data;
using NewEditor.Data.NARCTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListBox;

namespace NewEditor.Forms
{
    public partial class PokemonEditor : Form
    {
        static TextNARC textNARC => MainEditor.textNarc;
        static PokemonDataNARC pokemonNARC => MainEditor.pokemonDataNarc;
        static ChildPokemonNARC babyPokemonNARC => MainEditor.childPokemonNarc;
        static LearnsetNARC learnsetNarc => MainEditor.learnsetNarc;
        static EggMoveNARC eggMoveNarc => MainEditor.eggMoveNarc;

        static List<TextValue> levelRates = new List<TextValue>()
        {
            new TextValue(1, "Erratic"),
            new TextValue(4, "Fast"),
            new TextValue(0, "Medium Fast"),
            new TextValue(3, "Medium Slow"),
            new TextValue(5, "Slow"),
            new TextValue(2, "Fluctuating"),
        };
        static List<TextValue> genderRatios = new List<TextValue>()
        {
            new TextValue(0, "100% male"),
            new TextValue(0x1F, "87.5% M / 12.5% F"),
            new TextValue(0x3F, "75% M / 25% F"),
            new TextValue(0x7F, "50% M / 50% F"),
            new TextValue(0xBF, "25% M / 75% F"),
            new TextValue(0xFE, "100% female"),
            new TextValue(0xFF, "Genderless"),
        };

        static List<string> eggGroups = new List<string>()
        {
            "-----",
            "Monster",
            "Water 1",
            "Bug",
            "Flying",
            "Field",
            "Fairy",
            "Grass",
            "Human-Like",
            "Water 3",
            "Mineral",
            "Amorphous",
            "Water 2",
            "Ditto",
            "Dragon",
            "Unknown",
        };

        public List<LevelUpMoveSlot> learnsetClipboard;

        public PokemonEditor()
        {
            InitializeComponent();

            List<PokemonEntry> pk = new List<PokemonEntry>(pokemonNARC.pokemon);
            pk.Sort((p1, p2) => p2.nameID == p1.nameID ? Math.Sign(pokemonNARC.pokemon.IndexOf(p1) - pokemonNARC.pokemon.IndexOf(p2)) : p1.nameID - p2.nameID);

            pokemonNameDropdown.Items.AddRange(pk.ToArray());
            babyPokemonDropdown.Items.AddRange(pokemonNARC.pokemon.GetRange(0, babyPokemonNARC.ids.Count).ToArray());

            pokemonTypeDropdown1.Items.AddRange(textNARC.textFiles[VersionConstants.TypeNameTextFileID].text.ToArray());
            pokemonTypeDropdown2.Items.AddRange(textNARC.textFiles[VersionConstants.TypeNameTextFileID].text.ToArray());

            pokeAbilityDropdown1.Items.AddRange(textNARC.textFiles[VersionConstants.AbilityNameTextFileID].text.ToArray());
            pokeAbilityDropdown2.Items.AddRange(textNARC.textFiles[VersionConstants.AbilityNameTextFileID].text.ToArray());
            pokeAbilityDropdown3.Items.AddRange(textNARC.textFiles[VersionConstants.AbilityNameTextFileID].text.ToArray());

            pkLevelRateDropdown.Items.AddRange(levelRates.ToArray());
            pkGenderRatioDropdown.Items.AddRange(genderRatios.ToArray());

            heldItem1Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            heldItem2Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            heldItem3Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            heldItem1Dropdown.Items[0] = "---";
            heldItem2Dropdown.Items[0] = "---";
            heldItem3Dropdown.Items[0] = "---";

            learnsetMoveDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.GetRange(0, MainEditor.moveDataNarc != null ? MainEditor.moveDataNarc.moves.Count : textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.Count).ToArray());
            eggMoveDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.GetRange(0, MainEditor.moveDataNarc != null ? MainEditor.moveDataNarc.moves.Count : textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.Count).ToArray());
            eggMoveDropdown.SelectedIndex = 0;
            eggGroup1Dropdown.Items.AddRange(eggGroups.ToArray());
            eggGroup2Dropdown.Items.AddRange(eggGroups.ToArray());

            tmMovesListBox.Items.AddRange(VersionConstants.BW2_TMNames.ToArray());
            miscTutorsListBox.Items.AddRange(VersionConstants.BW2_TutorMoves.GetRange(0, 7).ToArray());
            driftveilTutorsListBox.Items.AddRange(VersionConstants.BW2_TutorMoves.GetRange(7, 15).ToArray());
            lentimasTutorsListBox.Items.AddRange(VersionConstants.BW2_TutorMoves.GetRange(22, 17).ToArray());
            humilauTutorsListBox.Items.AddRange(VersionConstants.BW2_TutorMoves.GetRange(39, 13).ToArray());
            nacreneTutorsListBox.Items.AddRange(VersionConstants.BW2_TutorMoves.GetRange(52, 15).ToArray());

            evolutionIntoDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.ToArray());
            evolutionMethodDropdown.Items.AddRange(VersionConstants.BW2_EvolutionMethodNames.ToArray());

            //Universal xp increase
            //foreach (PokemonEntry p in pokemonNARC.pokemon)
            //{
            //    List<string> excludePokemon = new List<string>()
            //    {
            //        "Snivy - 495",
            //        "Tepig - 498",
            //        "Oshawott - 501",
            //        "Chansey - 113",
            //        "Blissey - 242",
            //        "Audino - 531"
            //    };
            //    if (!excludePokemon.Contains(p.ToString()))
            //    {
            //        p.xpYield = (short)(p.xpYield * 1.3f);
            //        p.ApplyData();
            //    }
            //}
        }

        private void LoadPokemonIntoEditor(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p && p.nameID > 0 && p.bytes.Length == (MainEditor.RomType == RomType.BW2 ? 76 : MainEditor.RomType == RomType.BW1 ? 60 : 44))
            {
                pokemonBaseHpNumberBox.Value = p.baseHP;
                pokemonBaseAttackNumberBox.Value = p.baseAttack;
                pokemonBaseDefenseNumberBox.Value = p.baseDefense;
                pokemonBaseSpAttNumberBox.Value = p.baseSpAtt;
                pokemonBaseSpDefNumberBox.Value = p.baseSpDef;
                pokemonBaseSpeedNumberBox.Value = p.baseSpeed;

                pokemonTypeDropdown1.SelectedIndex = p.type1;
                pokemonTypeDropdown2.SelectedIndex = p.type2;

                pokeAbilityDropdown1.SelectedIndex = p.ability1;
                pokeAbilityDropdown2.SelectedIndex = p.ability2;
                pokeAbilityDropdown3.SelectedIndex = p.ability3;

                pkLevelRateDropdown.SelectedItem = levelRates.First(l => l.hexID == p.levelRate);
                pkGenderRatioDropdown.SelectedItem = genderRatios.First(g => g.hexID == p.genderRatio);
                xpYieldNumberBox.Value = p.xpYield;
                friendshipNumberBox.Value = p.baseHappiness;
                catchRateNumberBox.Value = p.catchRate;
                heightNumberBox.Value = p.height;
                weightNumberBox.Value = p.weight;
                babyPokemonDropdown.SelectedIndex = babyPokemonNARC.ids[p.nameID];

                heldItem1Dropdown.SelectedIndex = p.heldItem1;
                heldItem2Dropdown.SelectedIndex = p.heldItem2;
                heldItem3Dropdown.SelectedIndex = p.heldItem3;
                try
                {
                    p.RetrieveSprite();
                    if (p.sprite != null) pokemonSpriteBox.Image = p.sprite;
                    else pokemonSpriteBox.Image = null;
                }
                catch
                {
                    pokemonSpriteBox.Image = null;
                }
                formSpriteIDNumberBox.Value = 0;
                formSpriteIDNumberBox.Maximum = p.formID == 0 ? p.numberOfForms - 1 : 0;

                SetupPokemonLearnsetList();

                eggGroup1Dropdown.SelectedIndex = p.eggGroup1;
                eggGroup2Dropdown.SelectedIndex = p.eggGroup2;
                if (p.nameID < eggMoveNarc.entries.Count)
                {
                    eggMoveListBox.Items.Clear();
                    foreach (short move in eggMoveNarc.entries[p.nameID].moves)
                    {
                        eggMoveListBox.Items.Add(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[move]);
                    }
                    if (eggMoveListBox.Items.Count > 0) eggMoveListBox.SelectedIndex = 0;
                }

                baseStatsGroup.Enabled = true;
                miscStatsGroup.Enabled = true;
                applyPokemonButton.Enabled = true;
                levelUpMovesGroup.Enabled = true;
                tmTutorsGroupBox.Enabled = true;
                evolutionsGroupBox.Enabled = true;
                hexDataGroupBox.Enabled = true;
                pokedexDataGroupBox.Enabled = true;
                eggMovesGroupBox.Enabled = p.nameID < eggMoveNarc.entries.Count;

                string text = "";
                foreach (byte b in p.bytes) text += b.ToString("X2") + " ";
                hexDataTextBox.Text = text;

                if (MainEditor.RomType == RomType.BW2)
                {
                    for (int i = 0; i < p.TMs.Length; i++) tmMovesListBox.SetItemChecked(i, p.TMs[i]);
                    for (int i = 0; i < p.miscTutors.Length; i++) miscTutorsListBox.SetItemChecked(i, p.miscTutors[i]);
                    for (int i = 0; i < p.driftveilTutors.Length; i++) driftveilTutorsListBox.SetItemChecked(i, p.driftveilTutors[i]);
                    for (int i = 0; i < p.lentimasTutors.Length; i++) lentimasTutorsListBox.SetItemChecked(i, p.lentimasTutors[i]);
                    for (int i = 0; i < p.humilauTutors.Length; i++) humilauTutorsListBox.SetItemChecked(i, p.humilauTutors[i]);
                    for (int i = 0; i < p.nacreneTutors.Length; i++) nacreneTutorsListBox.SetItemChecked(i, p.nacreneTutors[i]);

                    evolutionsListBox.Items.Clear();
                    for (int i = 0; i < 7; i++) evolutionsListBox.Items.Add(p.evolutions.methods[i]);
                    evolutionsListBox.SelectedIndex = 0;
                }
                if (MainEditor.RomType == RomType.BW1)
                {
                    for (int i = 0; i < p.TMs.Length; i++) tmMovesListBox.SetItemChecked(i, p.TMs[i]);
                    for (int i = 0; i < p.miscTutors.Length; i++) miscTutorsListBox.SetItemChecked(i, p.miscTutors[i]);
                    driftveilTutorsListBox.Enabled = false;
                    lentimasTutorsListBox.Enabled = false;
                    humilauTutorsListBox.Enabled = false;
                    nacreneTutorsListBox.Enabled = false;

                    evolutionsListBox.Items.Clear();
                    for (int i = 0; i < 7; i++) evolutionsListBox.Items.Add(p.evolutions.methods[i]);
                    evolutionsListBox.SelectedIndex = 0;
                }

                pokedexNameTextBox.Text = textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text[p.nameID];
                if (p.nameID < textNARC.textFiles[VersionConstants.PokemonName2TextFileID].text.Count && textNARC.textFiles[VersionConstants.PokemonName2TextFileID].text[p.nameID].Length > 10)
                    dexAnCheckBox.Checked = textNARC.textFiles[VersionConstants.PokemonName2TextFileID].text[p.nameID][14] == 'n';
                if (p.nameID < textNARC.textFiles[VersionConstants.PokedexClassificationTextFileID].text.Count)
                {
                    pokedexClassTextBox.Text = textNARC.textFiles[VersionConstants.PokedexClassificationTextFileID].text[p.nameID];
                    pokedexDescriptionTextBox.Text = textNARC.textFiles[VersionConstants.PokedexEntryTextFileID].text[p.nameID];
                }
                if (p.nameID < textNARC.textFiles[VersionConstants.PokedexImpericalHeightTextFileID[0]].text.Count)
                {
                    string imp = textNARC.textFiles[VersionConstants.PokedexImpericalHeightTextFileID[0]].text[p.nameID];
                    pokedexHeightFt.Text = imp.Substring(0, imp.IndexOf('\''));
                    pokedexHeightIn.Text = imp.Substring(imp.IndexOf('\'') + 1, 2);
                }
                if (p.nameID < textNARC.textFiles[VersionConstants.PokedexMetricCommaHeightTextFileID[0]].text.Count)
                {
                    string met = textNARC.textFiles[VersionConstants.PokedexMetricCommaHeightTextFileID[0]].text[p.nameID];
                    pokedexHeightM.Text = met.Substring(0, met.IndexOf(','));
                    pokedexHeightSubM.Text = met.Substring(met.IndexOf(',') + 1, met.IndexOf(' ') - (met.IndexOf(',') + 1));
                }
                if (p.nameID < textNARC.textFiles[VersionConstants.PokedexImpericalWeightTextFileID[0]].text.Count)
                {
                    string imp = textNARC.textFiles[VersionConstants.PokedexImpericalWeightTextFileID[0]].text[p.nameID];
                    pokedexWeightLbs.Text = imp.Substring(0, imp.IndexOf('.'));
                    pokedexWeightSubLbs.Text = imp.Substring(imp.IndexOf('.') + 1, imp.IndexOf(' ') - (imp.IndexOf('.') + 1));
                }
                if (p.nameID < textNARC.textFiles[VersionConstants.PokedexMetricCommaWeightTextFileID[0]].text.Count)
                {
                    string met = textNARC.textFiles[VersionConstants.PokedexMetricCommaWeightTextFileID[0]].text[p.nameID];
                    pokedexWeightKg.Text = met.Substring(0, met.IndexOf(','));
                    pokedexWeightSubKg.Text = met.Substring(met.IndexOf(',') + 1, met.IndexOf(' ') - (met.IndexOf(',') + 1));
                }
            }
            else
            {
                baseStatsGroup.Enabled = false;
                miscStatsGroup.Enabled = false;
                applyPokemonButton.Enabled = false;
                levelUpMovesGroup.Enabled = false;
                tmTutorsGroupBox.Enabled = false;
                evolutionsGroupBox.Enabled = false;
                hexDataGroupBox.Enabled = false;
                pokedexDataGroupBox.Enabled = false;
                eggMovesGroupBox.Enabled = false;
            }
        }

        private void ApplyPokemon(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                p.baseHP = (byte)pokemonBaseHpNumberBox.Value;
                p.baseAttack = (byte)pokemonBaseAttackNumberBox.Value;
                p.baseDefense = (byte)pokemonBaseDefenseNumberBox.Value;
                p.baseSpAtt = (byte)pokemonBaseSpAttNumberBox.Value;
                p.baseSpDef = (byte)pokemonBaseSpDefNumberBox.Value;
                p.baseSpeed = (byte)pokemonBaseSpeedNumberBox.Value;

                if (pokemonTypeDropdown1.SelectedIndex > 0) p.type1 = (byte)pokemonTypeDropdown1.SelectedIndex;
                if (pokemonTypeDropdown2.SelectedIndex > 0) p.type2 = (byte)pokemonTypeDropdown2.SelectedIndex;

                if (pokeAbilityDropdown1.SelectedIndex > 0) p.ability1 = (byte)pokeAbilityDropdown1.SelectedIndex;
                if (pokeAbilityDropdown2.SelectedIndex > 0) p.ability2 = (byte)pokeAbilityDropdown2.SelectedIndex;
                if (pokeAbilityDropdown3.SelectedIndex > 0) p.ability3 = (byte)pokeAbilityDropdown3.SelectedIndex;

                if (pkLevelRateDropdown.SelectedIndex > 0) p.levelRate = (byte)((TextValue)pkLevelRateDropdown.SelectedItem).hexID;
                if (pkGenderRatioDropdown.SelectedIndex > 0) p.genderRatio = (byte)((TextValue)pkGenderRatioDropdown.SelectedItem).hexID;
                p.xpYield = (short)xpYieldNumberBox.Value;
                p.height = (short)heightNumberBox.Value;
                p.weight = (short)weightNumberBox.Value;
                p.baseHappiness = (byte)friendshipNumberBox.Value;
                p.catchRate = (byte)catchRateNumberBox.Value;

                if (heldItem1Dropdown.SelectedIndex > 0) p.heldItem1 = (short)heldItem1Dropdown.SelectedIndex;
                if (heldItem2Dropdown.SelectedIndex > 0) p.heldItem2 = (short)heldItem2Dropdown.SelectedIndex;
                if (heldItem3Dropdown.SelectedIndex > 0) p.heldItem3 = (short)heldItem3Dropdown.SelectedIndex;
                if (eggGroup1Dropdown.SelectedIndex > 0) p.eggGroup1 = (byte)eggGroup1Dropdown.SelectedIndex;
                if (eggGroup2Dropdown.SelectedIndex > 0) p.eggGroup2 = (byte)eggGroup2Dropdown.SelectedIndex;

                p.levelUpMoves.moves = new List<LevelUpMoveSlot>();
                foreach (object o in learnsetListBox.Items) if (o is LevelUpMoveSlot move)
                    {
                        p.levelUpMoves.moves.Add(move);
                    }
                p.levelUpMoves.moves.Sort(CompareLearnsetMoves);
                if (MainEditor.RomType == RomType.BW2)
                {
                    for (int i = 0; i < p.TMs.Length; i++) p.TMs[i] = tmMovesListBox.GetItemChecked(i);
                    for (int i = 0; i < p.miscTutors.Length; i++) p.miscTutors[i] = miscTutorsListBox.GetItemChecked(i);
                    for (int i = 0; i < p.driftveilTutors.Length; i++) p.driftveilTutors[i] = driftveilTutorsListBox.GetItemChecked(i);
                    for (int i = 0; i < p.lentimasTutors.Length; i++) p.lentimasTutors[i] = lentimasTutorsListBox.GetItemChecked(i);
                    for (int i = 0; i < p.humilauTutors.Length; i++) p.humilauTutors[i] = humilauTutorsListBox.GetItemChecked(i);
                    for (int i = 0; i < p.nacreneTutors.Length; i++) p.nacreneTutors[i] = nacreneTutorsListBox.GetItemChecked(i);

                    for (int i = 0; i < 7; i++) p.evolutions.methods[i] = (EvolutionMethod)evolutionsListBox.Items[i];
                }
                if (MainEditor.RomType == RomType.BW1)
                {
                    for (int i = 0; i < p.TMs.Length; i++) p.TMs[i] = tmMovesListBox.GetItemChecked(i);
                    for (int i = 0; i < p.miscTutors.Length; i++) p.miscTutors[i] = miscTutorsListBox.GetItemChecked(i);

                    for (int i = 0; i < 7; i++) p.evolutions.methods[i] = (EvolutionMethod)evolutionsListBox.Items[i];
                }
                babyPokemonNARC.ids[p.nameID] = (short)babyPokemonDropdown.SelectedIndex;
                p.ApplyData();

                if (p.nameID < eggMoveNarc.entries.Count)
                {
                    eggMoveNarc.entries[p.nameID].moves.Clear();
                    foreach (string move in eggMoveListBox.Items)
                    {
                        eggMoveNarc.entries[p.nameID].moves.Add((short)eggMoveDropdown.Items.IndexOf(move));
                    }
                    eggMoveNarc.entries[p.nameID].ApplyData();
                }

                SetupPokemonLearnsetList();
            }
        }

        private void SetupPokemonLearnsetList()
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                learnsetListBox.Items.Clear();
                foreach (LevelUpMoveSlot move in p.levelUpMoves.moves)
                {
                    learnsetListBox.Items.Add(move);
                }
                learnsetListBox.SelectedIndex = 0;
            }
        }

        private void UpdateBaseStatTotal(object sender, EventArgs e)
        {
            pokemonBSTText.Text = "Total: " + (pokemonBaseHpNumberBox.Value + pokemonBaseAttackNumberBox.Value + pokemonBaseDefenseNumberBox.Value
                + pokemonBaseSpAttNumberBox.Value + pokemonBaseSpDefNumberBox.Value + pokemonBaseSpeedNumberBox.Value);
        }

        private void CopyLearnset(object sender, EventArgs e)
        {
            learnsetClipboard = new List<LevelUpMoveSlot>();
            foreach (LevelUpMoveSlot move in learnsetListBox.Items)
            {
                learnsetClipboard.Add(move);
            }
            pasteLearnsetButton.Enabled = true;
        }

        private void PasteLearnset(object sender, EventArgs e)
        {
            learnsetListBox.Items.Clear();
            foreach (LevelUpMoveSlot move in learnsetClipboard)
            {
                learnsetListBox.Items.Add(move);
            }
        }

        private void AddToLearnset(object sender, EventArgs e)
        {
            if (learnsetListBox.Items.Count >= 24)
            {
                MessageBox.Show("Cannot add any more moves");
                return;
            }
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                learnsetListBox.Items.Add(new LevelUpMoveSlot(1, 1));
                learnsetListBox.SelectedIndex = learnsetListBox.Items.Count - 1;
            }
        }

        private void RemoveFromLearnset(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                int storeIndex = learnsetListBox.SelectedIndex;
                if (learnsetListBox.Items.Count > 1) learnsetListBox.Items.Remove((LevelUpMoveSlot)learnsetListBox.SelectedItem);

                learnsetListBox.SelectedIndex = Math.Min(storeIndex, learnsetListBox.Items.Count - 1);
            }
        }

        private void ChangeMoveSlot(object sender, EventArgs e)
        {
            if (learnsetListBox.SelectedItem is LevelUpMoveSlot l)
            {
                learnsetMoveDropdown.SelectedIndex = l.moveID;
                learnsetLevelNumberBox.Value = l.level;
            }
        }

        private void ApplyMoveSlot(object sender, EventArgs e)
        {
            if (learnsetListBox.SelectedItem is LevelUpMoveSlot)
            {
                learnsetListBox.Items[learnsetListBox.SelectedIndex] = new LevelUpMoveSlot((short)learnsetMoveDropdown.SelectedIndex, (short)learnsetLevelNumberBox.Value);
            }
        }

        private void LoadEvolutionMethod(object sender, EventArgs ev)
        {
            if (evolutionsListBox.SelectedItem is EvolutionMethod e)
            {
                evolutionIntoDropdown.SelectedIndex = e.newPokemonID;
                evolutionMethodDropdown.SelectedIndex = e.method;

                evolutionConditionDropdown.Items.Clear();
                int conditionType = EvolutionConditionType(e.method);

                if (conditionType == 0) evolutionConditionDropdown.Items.Add("---");
                if (conditionType == 1) for (int i = 0; i <= 100; i++) evolutionConditionDropdown.Items.Add(i);
                if (conditionType == 2) evolutionConditionDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
                if (conditionType == 3) for (int i = 0; i <= 255; i++) evolutionConditionDropdown.Items.Add(i);
                if (conditionType == 4) evolutionConditionDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.ToArray());
                if (conditionType == 5) evolutionConditionDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.ToArray());

                evolutionConditionDropdown.SelectedIndex = e.condition;
            }
        }

        private void ApplyEvolution(object sender, EventArgs ev)
        {
            evolutionsListBox.Items[evolutionsListBox.SelectedIndex] = new EvolutionMethod((short)evolutionMethodDropdown.SelectedIndex,
                (short)evolutionConditionDropdown.SelectedIndex,
                (short)evolutionIntoDropdown.SelectedIndex);
        }

        private void ChangeEvolutionMethod(object sender, EventArgs e)
        {
            int saveIndex = evolutionConditionDropdown.SelectedIndex;
            evolutionConditionDropdown.Items.Clear();
            int conditionType = EvolutionConditionType(evolutionMethodDropdown.SelectedIndex);

            if (conditionType == 0) evolutionConditionDropdown.Items.Add("---");
            if (conditionType == 1) for (int i = 0; i <= 100; i++) evolutionConditionDropdown.Items.Add(i);
            if (conditionType == 2) evolutionConditionDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            if (conditionType == 3) for (int i = 0; i <= 255; i++) evolutionConditionDropdown.Items.Add(i);
            if (conditionType == 4) evolutionConditionDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.ToArray());
            if (conditionType == 5) evolutionConditionDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.ToArray());

            if (saveIndex < evolutionConditionDropdown.Items.Count) evolutionConditionDropdown.SelectedIndex = saveIndex;
            else evolutionConditionDropdown.SelectedIndex = 0;
        }

        private static int CompareLearnsetMoves(LevelUpMoveSlot m1, LevelUpMoveSlot m2) => m1.level == m2.level ? Math.Sign(m1.moveID - m2.moveID) : Math.Sign(m1.level - m2.level);
    
        private int EvolutionConditionType(int method)
        {
            int conditionType = 0;
            switch (evolutionMethodDropdown.SelectedIndex)
            {
                //Level
                case 4: conditionType = 1; break;
                case 9: conditionType = 1; break;
                case 10: conditionType = 1; break;
                case 11: conditionType = 1; break;
                case 12: conditionType = 1; break;
                case 13: conditionType = 1; break;
                case 14: conditionType = 1; break;
                case 15: conditionType = 1; break;
                case 23: conditionType = 1; break;
                case 24: conditionType = 1; break;

                //Item
                case 6: conditionType = 2; break;
                case 8: conditionType = 2; break;
                case 17: conditionType = 2; break;
                case 18: conditionType = 2; break;
                case 19: conditionType = 2; break;
                case 20: conditionType = 2; break;

                //Beauty
                case 16: conditionType = 3; break;

                //Move
                case 21: conditionType = 4; break;

                //With Pokemon
                case 22: conditionType = 5; break;

                //No Condition
                default: conditionType = 0; break;
            }

            return conditionType;
        }

        private void openPaletteEditorButton_Click(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p && p.nameID > 0 && p.bytes.Length == (MainEditor.RomType == RomType.BW2 ? 76 : MainEditor.RomType == RomType.BW1 ? 60 : 44))
            {
                int sid = p.spriteID;
                if (formSpriteIDNumberBox.Value != 0 && formSpriteIDNumberBox.Value < p.numberOfForms)
                {
                    sid = 685 + p.formSpritesStart + (int)formSpriteIDNumberBox.Value - 1;
                }
                PaletteEditor editor = new PaletteEditor(sid);
                editor.Owner = this;
                editor.Show();
            }
        }

        private void importSpriteButton_Click(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                OpenFileDialog prompt = new OpenFileDialog();
                prompt.Filter = "Pokemon Sprite Data File|*.pksprdat";

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, IEnumerable<byte>> data = FileFunctions.ReadAllSections(prompt.FileName, true);
                    if (data.Count != 20)
                    {
                        MessageBox.Show("Invalid sprite data file");
                        return;
                    }
                    int sid = p.spriteID;
                    if (formSpriteIDNumberBox.Value != 0 && formSpriteIDNumberBox.Value < p.numberOfForms)
                    {
                        sid = 685 + p.formSpritesStart + (int)formSpriteIDNumberBox.Value - 1;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        MainEditor.pokemonSpritesNarc.sprites[sid].files[i] = data["file" + i].ToArray();
                        MainEditor.pokemonSpritesNarc.sprites[sid].ReadData();
                        try
                        {
                            p.RetrieveSprite();
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
        }

        private void exportSpriteButton_Click(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                SaveFileDialog prompt = new SaveFileDialog();
                prompt.Filter = "Pokemon Sprite Data File|*.pksprdat";

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();
                    int sid = p.spriteID;
                    if (formSpriteIDNumberBox.Value != 0 && formSpriteIDNumberBox.Value < p.numberOfForms)
                    {
                        sid = 685 + p.formSpritesStart + (int)formSpriteIDNumberBox.Value - 1;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        data.Add("file" + i, MainEditor.pokemonSpritesNarc.sprites[sid].files[i]);
                    }

                    FileFunctions.WriteAllSections(prompt.FileName, data, true);
                    MessageBox.Show("Sprite data saved to " + prompt.FileName);
                }
            }
        }

        private void applyHexDataButton_Click(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p)
            {
                //Test for improper text length
                if (hexDataTextBox.Text.Length % 3 == 2 && hexDataTextBox.Text[hexDataTextBox.Text.Length - 1] != ' ') hexDataTextBox.Text += ' ';
                if (hexDataTextBox.Text.Length < 3 || hexDataTextBox.Text.Length % 3 != 0)
                {
                    MessageBox.Show("Raw Data detected an incorrect format");
                    return;
                }

                //Test for improper text values
                for (int i = 2; i < hexDataTextBox.Text.Length; i += 3) if (hexDataTextBox.Text[i] != ' ' ||
                        (!char.IsDigit(hexDataTextBox.Text[i - 1]) && !(hexDataTextBox.Text[i - 1] >= 'A' && hexDataTextBox.Text[i - 1] <= 'F')) ||
                        (!char.IsDigit(hexDataTextBox.Text[i - 2]) && !(hexDataTextBox.Text[i - 2] >= 'A' && hexDataTextBox.Text[i - 2] <= 'F')))
                    {
                        MessageBox.Show("Raw Data detected an incorrect format");
                        return;
                    }

                //Convert data to file
                p.bytes = new byte[hexDataTextBox.Text.Length / 3];
                for (int i = 0; i < hexDataTextBox.Text.Length; i += 3)
                {
                    p.bytes[i / 3] = byte.Parse(hexDataTextBox.Text.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                }
                p.ReadDataBW2();
            }
        }

        private void ApplyPokedexData(object sender, EventArgs e)
        {
            if (pokemonNameDropdown.SelectedItem is PokemonEntry p && p.nameID > 0 && p.bytes.Length == (MainEditor.RomType == RomType.BW2 ? 76 : MainEditor.RomType == RomType.BW1 ? 60 : 44))
            {
                textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text[p.nameID] = pokedexNameTextBox.Text;
                textNARC.textFiles[VersionConstants.PokemonName2TextFileID].text[p.nameID] = (dexAnCheckBox.Checked ? "\\xf000봁\\x0000an \\xf000＀\\x0001ÿ" : "\\xf000봁\\x0000a \\xf000＀\\x0001ÿ") + pokedexNameTextBox.Text;
                textNARC.textFiles[VersionConstants.PokedexClassificationTextFileID].text[p.nameID] = pokedexClassTextBox.Text;
                textNARC.textFiles[VersionConstants.PokedexEntryTextFileID].text[p.nameID] = pokedexDescriptionTextBox.Text;

                textNARC.textFiles[VersionConstants.PokemonNameTextFileID].CompressData();
                textNARC.textFiles[VersionConstants.PokemonName2TextFileID].CompressData();
                textNARC.textFiles[VersionConstants.PokedexClassificationTextFileID].CompressData();
                textNARC.textFiles[VersionConstants.PokedexEntryTextFileID].CompressData();

                foreach (int i in VersionConstants.PokedexImpericalHeightTextFileID)
                {
                    if (p.nameID < textNARC.textFiles[i].text.Count) textNARC.textFiles[i].text[p.nameID] = pokedexHeightFt.Text + "\'" + pokedexHeightIn.Text + "\"";
                    textNARC.textFiles[i].CompressData();
                }
                foreach (int i in VersionConstants.PokedexMetricCommaHeightTextFileID)
                {
                    if (p.nameID < textNARC.textFiles[i].text.Count) textNARC.textFiles[i].text[p.nameID] = pokedexHeightM.Text + "," + pokedexHeightSubM.Text + " m";
                    textNARC.textFiles[i].CompressData();
                }
                foreach (int i in VersionConstants.PokedexMetricHeightTextFileID)
                {
                    if (p.nameID < textNARC.textFiles[i].text.Count) textNARC.textFiles[i].text[p.nameID] = pokedexHeightM.Text + "." + pokedexHeightSubM.Text + " m";
                    textNARC.textFiles[i].CompressData();
                }

                foreach (int i in VersionConstants.PokedexImpericalWeightTextFileID)
                {
                    if (p.nameID < textNARC.textFiles[i].text.Count) textNARC.textFiles[i].text[p.nameID] = pokedexWeightLbs.Text + "." + pokedexWeightSubLbs.Text + " lbs.";
                    textNARC.textFiles[i].CompressData();
                }
                foreach (int i in VersionConstants.PokedexMetricCommaWeightTextFileID)
                {
                    if (p.nameID < textNARC.textFiles[i].text.Count) textNARC.textFiles[i].text[p.nameID] = pokedexWeightKg.Text + "," + pokedexWeightSubKg.Text + " kg";
                    textNARC.textFiles[i].CompressData();
                }
                foreach (int i in VersionConstants.PokedexMetricWeightTextFileID)
                {
                    if (p.nameID < textNARC.textFiles[i].text.Count) textNARC.textFiles[i].text[p.nameID] = pokedexWeightKg.Text + "." + pokedexWeightSubKg.Text + " kg";
                    textNARC.textFiles[i].CompressData();
                }
            }
        }

        public static void FullCopyPokemon(int from, int spriteID)
        {
            PokemonEntry pk1 = pokemonNARC.pokemon[from];
            pk1.numberOfForms = 2;
            pk1.ApplyData();
            PokemonEntry pk = new PokemonEntry(new List<byte>(pk1.bytes).ToArray());
            pk1.formSpritesStart = (short)spriteID;
            pk1.formsStart = (short)pokemonNARC.pokemon.Count;
            pk1.ApplyData();
            pk.nameID = pk1.nameID;
            pk.spriteID = 685 + spriteID;
            pk.levelUpMoves = new LevelUpMoveset(new List<byte>(pk1.levelUpMoves.bytes).ToArray());
            pk.evolutions = new EvolutionDataEntry(new List<byte>(pk1.evolutions.bytes).ToArray());
            pk.formID = 1;
            pk.ApplyData();
            pokemonNARC.pokemon.Add(pk);
            learnsetNarc.learnsets.Add(pk.levelUpMoves);
            MainEditor.evolutionsNarc.evolutions.Add(pk.evolutions);
        }

        private void applyEggMoveButton_Click(object sender, EventArgs e)
        {
            if (eggMoveDropdown.SelectedIndex != 0 && eggMoveListBox.SelectedIndex != -1)
            {
                eggMoveListBox.Items[eggMoveListBox.SelectedIndex] = eggMoveDropdown.SelectedItem;
            }
        }

        private void addEggMoveButton_Click(object sender, EventArgs e)
        {
            if (eggMoveDropdown.SelectedIndex != 0 && !eggMoveListBox.Items.Contains(eggMoveDropdown.SelectedItem))
            {
                eggMoveListBox.Items.Add(eggMoveDropdown.SelectedItem);
            }
        }

        private void removeEggMoveButton_Click(object sender, EventArgs e)
        {
            if (eggMoveListBox.SelectedIndex != -1)
            {
                int i = eggMoveListBox.SelectedIndex;
                eggMoveListBox.Items.RemoveAt(eggMoveListBox.SelectedIndex);
                if (i < eggMoveListBox.Items.Count) eggMoveListBox.SelectedIndex = i;
            }
        }
    }
}