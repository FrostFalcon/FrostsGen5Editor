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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NewEditor.Forms
{
    public partial class TrainerEditor : Form
    {
        TextNARC textNARC => MainEditor.textNarc;
        TrainerDataNARC trainerNARC => MainEditor.trainerNarc;
        TrainerPokeNARC trainerPokeNARC => MainEditor.trainerPokeNarc;
        PokemonDataNARC pokemonNARC => MainEditor.pokemonDataNarc;

        public TrainerEditor()
        {
            InitializeComponent();

            trainerNameDropdown.Items.AddRange(trainerNARC.trainers.ToArray());

            List<string> classes = new List<string>();
            for (int i = 0; i < textNARC.textFiles[VersionConstants.TrainerClassTextFileID].text.Count; i++)
            {
                classes.Add(textNARC.textFiles[VersionConstants.TrainerClassTextFileID].text[i] + " - " + i);
            }
            trainerClassDropdown.Items.AddRange(classes.ToArray());
            item1Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            item2Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            item3Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            item4Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            prizeItemDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());

            pokemonIDDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.ToArray());
            pokemonHeldItemDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
            pokemonMove1Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.ToArray());
            pokemonMove2Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.ToArray());
            pokemonMove3Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.ToArray());
            pokemonMove4Dropdown.Items.AddRange(textNARC.textFiles[VersionConstants.MoveNameTextFileID].text.ToArray());
        }

        private void LoadTrainerIntoEditor(object sender, EventArgs e)
        {
            if (trainerNameDropdown.SelectedItem is TrainerEntry tr && tr.numPokemon > 0)
            {
                trainerClassDropdown.SelectedIndex = tr.trainerClass;
                numPokemonBox.Value = tr.numPokemon;
                battleTypeDropdown.SelectedIndex = tr.battleType;
                heldItemsCheckBox.Checked = tr.heldItems;
                uniqueMovesCheckBox.Checked = tr.uniqueMoves;
                healerCheckBox.Checked = tr.isHealer;
                aiNumberBox.Value = tr.AI;
                item1Dropdown.SelectedIndex = tr.items[0];
                item2Dropdown.SelectedIndex = tr.items[1];
                item3Dropdown.SelectedIndex = tr.items[2];
                item4Dropdown.SelectedIndex = tr.items[3];
                prizeItemDropdown.SelectedIndex = tr.prizeItem;
                prizeMoneyNumberBox.Value = tr.prizeMoney;

                pokemonListBox.Items.Clear();
                foreach (TrainerPokemon p in tr.pokemon.pokemon) pokemonListBox.Items.Add(p);
                pokemonListBox.SelectedIndex = 0;

                if (dialogueTypeDropdown.SelectedIndex == -1)
                    dialogueTypeDropdown.SelectedIndex = 0;
                trDialogueTextBox.Text = TextNARC.UnFormatText(tr.dialogue[dialogueTypeDropdown.SelectedIndex]);
                trainerNameTextBox.Text = textNARC.textFiles[VersionConstants.TrainerNameTextFileID].text[tr.nameID];

                trainerNameTextBox.Enabled = true;
                trainerDataGroup.Enabled = true;
                pokemonGroupBox.Enabled = true;
                dialogueGroup.Enabled = true;
            }
            else
            {
                trainerNameTextBox.Enabled = false;
                trainerDataGroup.Enabled = false;
                pokemonGroupBox.Enabled = false;
                dialogueGroup.Enabled = false;
            }
        }

        private void LoadPokemonIntoEditor(object sender, EventArgs e)
        {
            if (pokemonListBox.SelectedItem is TrainerPokemon p)
            {
                pokemonIDDropdown.SelectedIndex = p.pokemonID;
                pokemonFormNumberBox.Value = p.form;
                pokemonLevelNumberBox.Value = p.level;

                pokemonAbilityDropdown.Items.Clear();
                pokemonAbilityDropdown.Items.Add("1 or 2");
                pokemonAbilityDropdown.Items.Add(textNARC.textFiles[VersionConstants.AbilityNameTextFileID].text[pokemonNARC.pokemon[p.pokemonID].ability1]);
                pokemonAbilityDropdown.Items.Add(textNARC.textFiles[VersionConstants.AbilityNameTextFileID].text[pokemonNARC.pokemon[p.pokemonID].ability2]);
                pokemonAbilityDropdown.Items.Add(textNARC.textFiles[VersionConstants.AbilityNameTextFileID].text[pokemonNARC.pokemon[p.pokemonID].ability3]);
                pokemonAbilityDropdown.SelectedIndex = p.ability;

                pokemonIVsNumberBox.Value = p.IVs;
                pokemonGenderDropdown.SelectedIndex = p.gender;

                if (trainerNameDropdown.SelectedItem is TrainerEntry tr)
                {
                    if (tr.heldItems)
                    {
                        pokemonHeldItemDropdown.Enabled = true;
                        pokemonHeldItemDropdown.SelectedIndex = p.heldItem;
                    }
                    else pokemonHeldItemDropdown.Enabled = false;

                    if (tr.uniqueMoves)
                    {
                        pokemonMove1Dropdown.Enabled = true;
                        pokemonMove1Dropdown.SelectedIndex = p.moves[0];
                        pokemonMove2Dropdown.Enabled = true;
                        pokemonMove2Dropdown.SelectedIndex = p.moves[1];
                        pokemonMove3Dropdown.Enabled = true;
                        pokemonMove3Dropdown.SelectedIndex = p.moves[2];
                        pokemonMove4Dropdown.Enabled = true;
                        pokemonMove4Dropdown.SelectedIndex = p.moves[3];
                    }
                    else
                    {
                        pokemonMove1Dropdown.Enabled = false;
                        pokemonMove2Dropdown.Enabled = false;
                        pokemonMove3Dropdown.Enabled = false;
                        pokemonMove4Dropdown.Enabled = false;
                    }
                }
            }
        }

        private void ApplyTrainer(object sender, EventArgs e)
        {
            if (trainerNameDropdown.SelectedItem is TrainerEntry tr && tr.numPokemon > 0)
            {
                bool oldHeldItem = tr.heldItems;
                bool oldUniqueMoves = tr.uniqueMoves;

                tr.trainerClass = (byte)trainerClassDropdown.SelectedIndex;
                tr.numPokemon = (byte)numPokemonBox.Value;

                tr.battleType = (byte)battleTypeDropdown.SelectedIndex;
                tr.heldItems = heldItemsCheckBox.Checked;
                tr.uniqueMoves = uniqueMovesCheckBox.Checked;
                tr.isHealer = healerCheckBox.Checked;
                tr.AI = (int)aiNumberBox.Value;
                tr.items[0] = (short)item1Dropdown.SelectedIndex;
                tr.items[1] = (short)item2Dropdown.SelectedIndex;
                tr.items[2] = (short)item3Dropdown.SelectedIndex;
                tr.items[3] = (short)item4Dropdown.SelectedIndex;
                tr.prizeItem = (short)prizeItemDropdown.SelectedIndex;
                tr.prizeMoney = (byte)prizeMoneyNumberBox.Value;

                if (pokemonListBox.SelectedItem is TrainerPokemon p)
                {
                    p.pokemonID = (short)pokemonIDDropdown.SelectedIndex;
                    p.form = (short)pokemonFormNumberBox.Value;
                    p.level = (byte)pokemonLevelNumberBox.Value;
                    p.ability = (byte)pokemonAbilityDropdown.SelectedIndex;
                    p.IVs = (byte)pokemonIVsNumberBox.Value;
                    p.gender = (byte)pokemonGenderDropdown.SelectedIndex;

                    if (tr.heldItems && oldHeldItem) p.heldItem = (short)pokemonHeldItemDropdown.SelectedIndex;
                    else p.heldItem = 0;

                    if (tr.uniqueMoves && oldUniqueMoves)
                    {
                        p.moves[0] = (short)pokemonMove1Dropdown.SelectedIndex;
                        p.moves[1] = (short)pokemonMove2Dropdown.SelectedIndex;
                        p.moves[2] = (short)pokemonMove3Dropdown.SelectedIndex;
                        p.moves[3] = (short)pokemonMove4Dropdown.SelectedIndex;
                    }
                    else for (int i = 0; i < 4; i++) p.moves[i] = 0;
                }

                tr.ApplyData();

                int num = pokemonListBox.SelectedIndex;
                pokemonListBox.Items.Clear();
                foreach (TrainerPokemon poke in tr.pokemon.pokemon) pokemonListBox.Items.Add(poke);
                while (num > tr.pokemon.pokemon.Count) num--;
                if (num < pokemonListBox.Items.Count) pokemonListBox.SelectedIndex = num;

                tr.dialogue[dialogueTypeDropdown.SelectedIndex] = TextNARC.FormatText(trDialogueTextBox.Text);

                //Rebuild Dialogue Indices
                List<byte> entryTable = new List<byte>();
                List<byte> indexTable = new List<byte>();
                List<string> textFile = trainerNARC.fileSystem.textNarc.textFiles[VersionConstants.TrainerDialogueTextFileID].text;
                textFile.Clear();
                for (int i = 0; i < trainerNARC.trainers.Count; i++)
                {
                    indexTable.AddRange(BitConverter.GetBytes((short)entryTable.Count));
                    for (int j = 0; j <= 24; j++)
                    {
                        string str = trainerNARC.trainers[i].dialogue[j];
                        if (str != "")
                        {
                            textFile.Add(str);
                            entryTable.AddRange(BitConverter.GetBytes((short)i));
                            entryTable.AddRange(BitConverter.GetBytes((short)j));
                        }
                    }
                }
                trainerNARC.fileSystem.trTextEntriesNarc.tableBytes = entryTable.ToArray();
                trainerNARC.fileSystem.trTextIndicesNarc.tableBytes = indexTable.ToArray();
                trainerNARC.fileSystem.textNarc.textFiles[VersionConstants.TrainerDialogueTextFileID].CompressData();

                List<string> names = trainerNARC.fileSystem.textNarc.textFiles[VersionConstants.TrainerNameTextFileID].text;
                if (tr.nameID < names.Count) names[tr.nameID] = trainerNameTextBox.Text;
                trainerNARC.fileSystem.textNarc.textFiles[VersionConstants.TrainerNameTextFileID].CompressData();

                int n = trainerNARC.trainers.IndexOf(tr);
                trainerNameDropdown.Items.RemoveAt(n);
                trainerNameDropdown.Items.Insert(n, tr);
                trainerNameDropdown.SelectedIndex = n;
            }
        }

        private void movePokemonUpButton_Click(object sender, EventArgs e)
        {
            if (trainerNameDropdown.SelectedItem is TrainerEntry tr && tr.numPokemon > 0 && pokemonListBox.SelectedIndex > 0)
            {
                int index = pokemonListBox.SelectedIndex;
                TrainerPokemon poke = tr.pokemon.pokemon[index];

                tr.pokemon.pokemon.Remove(poke);
                tr.pokemon.pokemon.Insert(index - 1, poke);
                pokemonListBox.SelectedIndex--;

                ApplyTrainer(sender, e);
            }
        }

        private void movePokemonDownButton_Click(object sender, EventArgs e)
        {
            if (trainerNameDropdown.SelectedItem is TrainerEntry tr && tr.numPokemon > 0 && pokemonListBox.SelectedIndex < tr.pokemon.pokemon.Count - 1)
            {
                int index = pokemonListBox.SelectedIndex;
                TrainerPokemon poke = tr.pokemon.pokemon[index];

                tr.pokemon.pokemon.Remove(poke);
                tr.pokemon.pokemon.Insert(index + 1, poke);
                pokemonListBox.SelectedIndex++;

                ApplyTrainer(sender, e);
            }
        }

        private void addTrainerButton_Click(object sender, EventArgs e)
        {
            int id = trainerNameDropdown.SelectedIndex > 0 ? trainerNameDropdown.SelectedIndex : 1;

            List<string> names = trainerNARC.fileSystem.textNarc.textFiles[VersionConstants.TrainerNameTextFileID].text;
            if (trainerNARC.trainers.Count + 1 > names.Count) names.Add("Trainer");
            trainerNARC.fileSystem.textNarc.textFiles[VersionConstants.TrainerNameTextFileID].CompressData();

            TrainerEntry t = new TrainerEntry(new List<byte>(trainerNARC.trainers[id].bytes).ToArray()) { nameID = trainerNARC.trainers.Count };
            TrainerPokemonEntry p = new TrainerPokemonEntry(new List<byte>(trainerNARC.trainers[id].pokemon.bytes).ToArray(), t);
            t.pokemon = p;
            trainerNARC.trainers.Add(t);
            trainerNameDropdown.Items.Add(t);
            trainerPokeNARC.pokemonGroups.Add(p);

            if (trainerNameDropdown.SelectedIndex > 0)
            {
                foreach (var v in trainerNARC.trainers[id].dialogue)
                {
                    t.dialogue[v.Key] = v.Value;
                }
            }

            t.ApplyData();
        }

        private void dialogueTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (trainerNameDropdown.SelectedItem is TrainerEntry tr)
            {
                trDialogueTextBox.Text = TextNARC.UnFormatText(tr.dialogue[dialogueTypeDropdown.SelectedIndex]);
            }
        }

        private void editTrainerAIButton_Click(object sender, EventArgs e)
        {
            int val = (int)aiNumberBox.Value;
            Form prompt = new Form();
            prompt.Font = Font;
            prompt.Width = 240;
            prompt.Height = 592;
            prompt.Text = "AI flags";
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Height = 600;
            CheckBox[] boxes = new CheckBox[]
            {
                new CheckBox() { Text = "Avoid \"No Effect\" Moves", Checked = (val & 1) > 0 },
                new CheckBox() { Text = "Use Strongest Attack", Checked = (val & 2) > 0 },
                new CheckBox() { Text = "Smart AI", Checked = (val & 4) > 0 },
                new CheckBox() { Text = "First Turn Setup", Checked = (val & 8) > 0 },
                new CheckBox() { Text = "First Rival Fight", Checked = (val & 16) > 0 },
                new CheckBox() { Text = "N Final Battle", Checked = (val & 32) > 0 },
                new CheckBox() { Text = "Baton Pass", Checked = (val & 64) > 0 },
                new CheckBox() { Text = "Double/Triple Battle", Checked = (val & 128) > 0 },
                new CheckBox() { Text = "Health Evaluation", Checked = (val & 256) > 0 },
                new CheckBox() { Text = "First Turn Weather", Checked = (val & 512) > 0 },
                new CheckBox() { Text = "Disruption Moves", Checked = (val & 1024) > 0 },
                new CheckBox() { Text = "Roaming Pokemon", Checked = (val & 2048) > 0 },
                new CheckBox() { Text = "Safari Pokemon", Checked = (val & 4096) > 0 },
                new CheckBox() { Text = "Tutorial Battle", Checked = (val & 8192) > 0 },
                new CheckBox() { Text = "Extra 1", Checked = (val & 16384) > 0 },
                new CheckBox() { Text = "Extra 2", Checked = (val & 32768) > 0 },
            };
            Button ok = new Button() { Text = "Apply", Size = new Size(80, 28) };
            ok.Click += (s, ev) =>
            {
                val = 0;
                for (int i = 0; i < boxes.Length; i++) if (boxes[i].Checked) val |= (int)Math.Pow(2, i);
                aiNumberBox.Value = val;
                prompt.Close();
            };
            Button no = new Button() { Text = "Cancel", Size = new Size(80, 28) };
            no.Click += (s, ev) => { prompt.Close(); };
            foreach (CheckBox c in boxes)
            {
                c.Width = 180;
                panel.Controls.Add(c);
                panel.SetFlowBreak(c, true);
            }
            LinkLabel ll = new LinkLabel() { Text = "See this document for details", LinkArea = new LinkArea(4, 4), AutoSize = true, Font = Font };
            ll.LinkClicked += (s, ev) =>
            {
                Process.Start("https://docs.google.com/document/d/1AziiMPsY1TcABKIwl92677A4nYGtByvjFOR7p6PAXY0");
            };
            panel.Controls.Add(ll);
            panel.Controls.Add(ok);
            panel.Controls.Add(no);
            prompt.Controls.Add(panel);
            prompt.ShowDialog();
        }
    }
}
