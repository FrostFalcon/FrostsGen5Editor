using NewEditor.Data;
using NewEditor.Data.NARCTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Forms
{
    public partial class Pokepatcher : Form
    {
        PokemonDataNARC pokemonNARC => MainEditor.pokemonDataNarc;
        PokemonSpritesNARC spriteNARC => MainEditor.pokemonSpritesNarc;

        public Pokepatcher()
        {
            InitializeComponent();

            List<PokemonEntry> pk = new List<PokemonEntry>(pokemonNARC.pokemon);
            pk.Sort((p1, p2) => p2.nameID == p1.nameID ? Math.Sign(pokemonNARC.pokemon.IndexOf(p1) - pokemonNARC.pokemon.IndexOf(p2)) : p1.nameID - p2.nameID);

            pokeNameDropdown.Items.AddRange(pk.ToArray());
        }

        private void addPokeButton_Click(object sender, EventArgs e)
        {
            if (pokeNameDropdown.SelectedItem is PokemonEntry pk)
            {
                pokemonListbox.Items.Add(pk);
            }
        }

        private void removePokeButton_Click(object sender, EventArgs e)
        {
            if (pokemonListbox.SelectedItem is PokemonEntry pk)
            {
                pokemonListbox.Items.Remove(pk);
            }
        }

        private void savePatchButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Pokepatch file|*.pokepatch";
            save.Title = "Select where to save the patch file";

            if (save.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();
                data.Add("RandomID", Encoding.ASCII.GetBytes(randomIDTextBox.Text));

                foreach (PokemonEntry pk in pokemonListbox.Items)
                {
                    data.Add("pk" + pokemonNARC.pokemon.IndexOf(pk), pk.bytes);
                    data.Add("ls" + pokemonNARC.pokemon.IndexOf(pk), pk.levelUpMoves.bytes);
                    data.Add("ev" + pokemonNARC.pokemon.IndexOf(pk), pk.evolutions.bytes);
                    data.Add("sp" + pokemonNARC.pokemon.IndexOf(pk), spriteNARC.sprites[pk.spriteID].PaletteBytes);
                    data.Add("sh" + pokemonNARC.pokemon.IndexOf(pk), spriteNARC.sprites[pk.spriteID].ShinyPaletteBytes);
                }

                FileFunctions.WriteAllSections(save.FileName, data, true);
                MessageBox.Show("Pokepatch created");
            }
        }

        private void loadPatchButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog files = new OpenFileDialog();
            files.Filter = "Pokepatch file|*.pokepatch";
            files.Multiselect = true;

            Random rand = new Random();

            if (files.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, List<Dictionary<string, IEnumerable<byte>>>> input = new Dictionary<string, List<Dictionary<string, IEnumerable<byte>>>>();

                foreach (string file in files.FileNames)
                {
                    Dictionary<string, IEnumerable<byte>> f = FileFunctions.ReadAllSections(file, true);
                    string rid = Encoding.ASCII.GetString(f["RandomID"].ToArray());
                    if (input.ContainsKey(rid)) input[rid].Add(f);
                    else
                    {
                        input.Add(rid, new List<Dictionary<string, IEnumerable<byte>>>() { f });
                    }
                }

                foreach (var rid in input.Values)
                {
                    Dictionary<string, IEnumerable<byte>> data = rid[rand.Next(rid.Count)];

                    foreach (var key in data.Keys)
                    {
                        if (key.StartsWith("pk") && int.TryParse(key.Substring(2), out int pid))
                        {
                            pokemonNARC.pokemon[pid].bytes = data[key].ToArray();
                            pokemonNARC.pokemon[pid].ReadDataBW2();
                        }
                        if (key.StartsWith("ls") && int.TryParse(key.Substring(2), out int lid))
                        {
                            pokemonNARC.pokemon[lid].levelUpMoves.bytes = data[key].ToArray();
                            pokemonNARC.pokemon[lid].levelUpMoves.ReadData();
                        }
                        if (key.StartsWith("ev") && int.TryParse(key.Substring(2), out int eid))
                        {
                            pokemonNARC.pokemon[eid].evolutions.bytes = data[key].ToArray();
                            pokemonNARC.pokemon[eid].evolutions.ReadData();
                        }
                        if (key.StartsWith("sp") && int.TryParse(key.Substring(2), out int spid))
                        {
                            data[key].ToArray().CopyTo(spriteNARC.sprites[pokemonNARC.pokemon[spid].spriteID].PaletteBytes, 0);
                            spriteNARC.sprites[pokemonNARC.pokemon[spid].spriteID].ReadData();
                            pokemonNARC.pokemon[spid].RetrieveSprite();
                        }
                        if (key.StartsWith("sh") && int.TryParse(key.Substring(2), out int shid))
                        {
                            data[key].ToArray().CopyTo(spriteNARC.sprites[pokemonNARC.pokemon[shid].spriteID].ShinyPaletteBytes, 0);
                            spriteNARC.sprites[pokemonNARC.pokemon[shid].spriteID].ReadData();
                            pokemonNARC.pokemon[shid].RetrieveSprite();
                        }
                    }
                }

                MessageBox.Show("Pokepatch applied");
            }
        }
    }
}
