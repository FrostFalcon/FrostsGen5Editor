using NewEditor.Data;
using NewEditor.Data.NARCTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Forms
{
    public partial class PokedexTools : Form
    {
        PokemonDataNARC pokemonNARC => MainEditor.pokemonDataNarc;
        TextNARC textNARC => MainEditor.textNarc;

        byte[] regionalDexFile;
        byte[] pokemonDataRegionalDex;

        public PokedexTools()
        {
            InitializeComponent();

            regionalDexFile = MainEditor.fileSystem.narcs[VersionConstants.DexOrderNarcID].GetFileEntry(4).ToArray();
            pokemonDataRegionalDex = pokemonNARC.pokemon.First(b => b.bytes.Length > 100).bytes.ToArray();

            List<int> mons = new List<int>();
            for (int i = 0; i < regionalDexFile.Length; i += 2)
            {
                int id = HelperFunctions.ReadShort(regionalDexFile, i);
                int id2 = HelperFunctions.ReadShort(pokemonDataRegionalDex, id * 2);
                if (id2 != 999) regionalDexListBox.Items.Add(pokemonNARC.pokemon[id].Name);
            }

            for (int i = 0; i < textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.Count; i++)
            {
                regionalNameDropdown.Items.Add(textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text[i]);
            }

            if (MainEditor.habitatListNarcID <= 0) syncHabitatsButton.Enabled = false;
        }

        private void moveRegionalUpButton_Click(object sender, EventArgs e)
        {
            if (regionalDexListBox.SelectedIndex > 0)
            {
                var move = regionalDexListBox.SelectedItem;
                int i = regionalDexListBox.SelectedIndex;
                regionalDexListBox.Items.Remove(move);
                regionalDexListBox.Items.Insert(i - 1, move);
                regionalDexListBox.SelectedIndex = i - 1;
            }
        }

        private void moveRegionalDownButton_Click(object sender, EventArgs e)
        {
            if (regionalDexListBox.SelectedIndex >= 0 && regionalDexListBox.SelectedIndex < regionalDexListBox.Items.Count - 1)
            {
                var move = regionalDexListBox.SelectedItem;
                int i = regionalDexListBox.SelectedIndex;
                regionalDexListBox.Items.Remove(move);
                regionalDexListBox.Items.Insert(i + 1, move);
                regionalDexListBox.SelectedIndex = i + 1;
            }
        }

        private void clearRegionalButton_Click(object sender, EventArgs e)
        {
            regionalDexListBox.Items.Clear();
        }

        private void addRegionalButton_Click(object sender, EventArgs e)
        {
            if (regionalNameDropdown.SelectedIndex < 0) return;
            string name = (string)regionalNameDropdown.SelectedItem;
            if (regionalDexListBox.Items.Contains(name)) return;
            int index = regionalDexListBox.SelectedIndex == -1 ? regionalDexListBox.Items.Count : regionalDexListBox.SelectedIndex + 1;
            regionalDexListBox.Items.Insert(index, name);
            if (regionalDexListBox.SelectedIndex >= 0) regionalDexListBox.SelectedIndex++;
        }

        private void removeRegionalButton_Click(object sender, EventArgs e)
        {
            if (regionalDexListBox.SelectedIndex >= 0)
            {
                int index = regionalDexListBox.SelectedIndex;
                regionalDexListBox.Items.RemoveAt(regionalDexListBox.SelectedIndex);
                regionalDexListBox.SelectedIndex = Math.Min(index, regionalDexListBox.Items.Count - 1);
            }
        }

        private void saveRegionalButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < pokemonDataRegionalDex.Length; i += 2)
            {
                HelperFunctions.WriteShort(pokemonDataRegionalDex, i, 999);
            }

            List<int> mons = new List<int>();
            for (int i = 0; i < regionalDexListBox.Items.Count; i++)
            {
                string name = (string)regionalDexListBox.Items[i];
                int id = textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.IndexOf(name);
                if (id != -1)
                {
                    mons.Add(id);
                }
            }

            int pos = 0;
            for (int i = 1; i <= regionalDexFile.Length / 2; i++)
            {
                if (!mons.Contains(i))
                {
                    HelperFunctions.WriteShort(regionalDexFile, pos, i);
                    pos += 2;
                }
            }
            for (int i = 0; i < mons.Count; i++)
            {
                HelperFunctions.WriteShort(regionalDexFile, pos, mons[i]);
                HelperFunctions.WriteShort(pokemonDataRegionalDex, mons[i] * 2, i);
                pos += 2;
            }

            pokemonNARC.pokemon.First(b => b.bytes.Length > 100).bytes = pokemonDataRegionalDex.ToArray();
            MainEditor.fileSystem.narcs[VersionConstants.DexOrderNarcID].ReplaceFileEntry(4, regionalDexFile.ToList());

            statusLabel.Text = "Saved regional dex order - " + DateTime.Now.StatusText();
        }

        private void syncHabitatsButton_Click(object sender, EventArgs e)
        {
            MainEditor.habitatListNarc?.SyncWithEncounterData();
            statusLabel.Text = "Saved habitat list data - " + DateTime.Now.StatusText();
        }
    }
}
