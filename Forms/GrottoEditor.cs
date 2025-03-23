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

namespace NewEditor.Forms
{
    public partial class GrottoEditor : Form
    {
        HiddenGrottoNARC grottoNarc = MainEditor.hiddenGrottoNarc;
        List<ComboBox> pokeIDDropdowns;
        List<NumericUpDown> pokeFormDropdowns;
        List<NumericUpDown> pokeMinLvDropdowns;
        List<NumericUpDown> pokeMaxLvDropdowns;
        List<NumericUpDown> pokeGenderDropdowns;
        List<ComboBox> itemDropdowns;
        List<ComboBox> hiddenItemDropdowns;

        public GrottoEditor()
        {
            InitializeComponent();

            pokeIDDropdowns = new List<ComboBox>()
            {
                commonPokeID1, commonPokeID2, commonPokeID3, commonPokeID4,
                comboBox4, comboBox3, comboBox2, comboBox1,
                comboBox8, comboBox7, comboBox6, comboBox5,
            };
            pokeFormDropdowns = new List<NumericUpDown>()
            {
                commonPokeForm1, commonPokeForm2, commonPokeForm3, commonPokeForm4,
                numericUpDown16, numericUpDown12, numericUpDown8, numericUpDown4,
                numericUpDown32, numericUpDown28, numericUpDown24, numericUpDown20
            };
            pokeMinLvDropdowns = new List<NumericUpDown>()
            {
                commonPokeMinLv1, commonPokeMinLv2, commonPokeMinLv3, commonPokeMinLv4,
                numericUpDown15, numericUpDown11, numericUpDown7, numericUpDown3,
                numericUpDown31, numericUpDown27, numericUpDown23, numericUpDown19
            };
            pokeMaxLvDropdowns = new List<NumericUpDown>()
            {
                commonPokeMaxLv1, commonPokeMaxLv2, commonPokeMaxLv3, commonPokeMaxLv4,
                numericUpDown14, numericUpDown10, numericUpDown6, numericUpDown2,
                numericUpDown30, numericUpDown26, numericUpDown22, numericUpDown18
            };
            pokeGenderDropdowns = new List<NumericUpDown>()
            {
                commonPokeGender1, commonPokeGender2, commonPokeGender3, commonPokeGender4,
                numericUpDown13, numericUpDown9, numericUpDown5, numericUpDown1,
                numericUpDown29, numericUpDown25, numericUpDown21, numericUpDown17
            };
            
            itemDropdowns = new List<ComboBox>()
            {
                comboBox24, comboBox23, comboBox22, comboBox21,
                comboBox20, comboBox19, comboBox18, comboBox17,
                comboBox16, comboBox15, comboBox14, comboBox13,
                comboBox12, comboBox11, comboBox10, comboBox9,
            };

            hiddenItemDropdowns = new List<ComboBox>()
            {
                comboBox28, comboBox27, comboBox26, comboBox25,
                comboBox32, comboBox31, comboBox30, comboBox29,
                comboBox36, comboBox35, comboBox34, comboBox33,
                comboBox40, comboBox39, comboBox38, comboBox37,
            };

            foreach (ComboBox c in pokeIDDropdowns)
            {
                c.Items.AddRange(MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.ToArray());
                c.SelectedIndex = 0;
            }

            foreach (ComboBox c in itemDropdowns)
            {
                c.Items.AddRange(MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
                c.SelectedIndex = 0;
            }

            foreach (ComboBox c in hiddenItemDropdowns)
            {
                c.Items.AddRange(MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
                c.SelectedIndex = 0;
            }

            grottoIDDropdown.Items.AddRange(VersionConstants.BW2_HiddenGrottoNames.ToArray());
            grottoIDDropdown.SelectedIndex = 0;
        }

        private void grottoIDDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grottoIDDropdown.SelectedIndex < 0) return;

            HiddenGrottoEncounter[] pool = blackEncountersButton.Checked ? grottoNarc.grottos[grottoIDDropdown.SelectedIndex].blackCommonEncounters : grottoNarc.grottos[grottoIDDropdown.SelectedIndex].whiteCommonEncounters;
            for (int i = 0; i < 4; i++)
            {
                pokeIDDropdowns[i].SelectedIndex = pool[i].pokemonID;
                pokeFormDropdowns[i].Value = pool[i].form;
                pokeMinLvDropdowns[i].Value = pool[i].minLevel;
                pokeMaxLvDropdowns[i].Value = pool[i].maxLevel;
                pokeGenderDropdowns[i].Value = pool[i].gender;
            }

            pool = blackEncountersButton.Checked ? grottoNarc.grottos[grottoIDDropdown.SelectedIndex].blackUncommonEncounters : grottoNarc.grottos[grottoIDDropdown.SelectedIndex].whiteUncommonEncounters;
            for (int i = 0; i < 4; i++)
            {
                pokeIDDropdowns[i + 4].SelectedIndex = pool[i].pokemonID;
                pokeFormDropdowns[i + 4].Value = pool[i].form;
                pokeMinLvDropdowns[i + 4].Value = pool[i].minLevel;
                pokeMaxLvDropdowns[i + 4].Value = pool[i].maxLevel;
                pokeGenderDropdowns[i + 4].Value = pool[i].gender;
            }

            pool = blackEncountersButton.Checked ? grottoNarc.grottos[grottoIDDropdown.SelectedIndex].blackRareEncounters : grottoNarc.grottos[grottoIDDropdown.SelectedIndex].whiteRareEncounters;
            for (int i = 0; i < 4; i++)
            {
                pokeIDDropdowns[i + 8].SelectedIndex = pool[i].pokemonID;
                pokeFormDropdowns[i + 8].Value = pool[i].form;
                pokeMinLvDropdowns[i + 8].Value = pool[i].minLevel;
                pokeMaxLvDropdowns[i + 8].Value = pool[i].maxLevel;
                pokeGenderDropdowns[i + 8].Value = pool[i].gender;
            }

            for (int i = 0; i < itemDropdowns.Count; i++)
            {
                itemDropdowns[i].SelectedIndex = grottoNarc.grottos[grottoIDDropdown.SelectedIndex].items[i];
            }

            for (int i = 0; i < hiddenItemDropdowns.Count; i++)
            {
                hiddenItemDropdowns[i].SelectedIndex = grottoNarc.grottos[grottoIDDropdown.SelectedIndex].hiddenItems[i];
            }
        }

        private void blackEncountersButton_CheckedChanged(object sender, EventArgs e)
        {
            grottoIDDropdown_SelectedIndexChanged(sender, e);
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (grottoIDDropdown.SelectedIndex < 0) return;

            HiddenGrottoEncounter[] pool = blackEncountersButton.Checked ? grottoNarc.grottos[grottoIDDropdown.SelectedIndex].blackCommonEncounters : grottoNarc.grottos[grottoIDDropdown.SelectedIndex].whiteCommonEncounters;
            for (int i = 0; i < 4; i++)
            {
                if (pokeIDDropdowns[i].SelectedIndex >= 0) pool[i].pokemonID = (short)pokeIDDropdowns[i].SelectedIndex;
                pool[i].form = (byte)pokeFormDropdowns[i].Value;
                pool[i].minLevel = (byte)pokeMinLvDropdowns[i].Value;
                pool[i].maxLevel = (byte)pokeMaxLvDropdowns[i].Value;
                pool[i].gender = (byte)pokeGenderDropdowns[i].Value;
            }

            pool = blackEncountersButton.Checked ? grottoNarc.grottos[grottoIDDropdown.SelectedIndex].blackUncommonEncounters : grottoNarc.grottos[grottoIDDropdown.SelectedIndex].whiteUncommonEncounters;
            for (int i = 0; i < 4; i++)
            {
                if (pokeIDDropdowns[i + 4].SelectedIndex >= 0) pool[i].pokemonID = (short)pokeIDDropdowns[i + 4].SelectedIndex;
                pool[i].form = (byte)pokeFormDropdowns[i + 4].Value;
                pool[i].minLevel = (byte)pokeMinLvDropdowns[i + 4].Value;
                pool[i].maxLevel = (byte)pokeMaxLvDropdowns[i + 4].Value;
                pool[i].gender = (byte)pokeGenderDropdowns[i + 4].Value;
            }

            pool = blackEncountersButton.Checked ? grottoNarc.grottos[grottoIDDropdown.SelectedIndex].blackRareEncounters : grottoNarc.grottos[grottoIDDropdown.SelectedIndex].whiteRareEncounters;
            for (int i = 0; i < 4; i++)
            {
                if (pokeIDDropdowns[i + 8].SelectedIndex >= 0) pool[i].pokemonID = (short)pokeIDDropdowns[i + 8].SelectedIndex;
                pool[i].form = (byte)pokeFormDropdowns[i + 8].Value;
                pool[i].minLevel = (byte)pokeMinLvDropdowns[i + 8].Value;
                pool[i].maxLevel = (byte)pokeMaxLvDropdowns[i + 8].Value;
                pool[i].gender = (byte)pokeGenderDropdowns[i + 8].Value;
            }

            for (int i = 0; i < itemDropdowns.Count; i++)
            {
                if (itemDropdowns[i].SelectedIndex >= 0) grottoNarc.grottos[grottoIDDropdown.SelectedIndex].items[i] = (short)itemDropdowns[i].SelectedIndex;
            }

            for (int i = 0; i < hiddenItemDropdowns.Count; i++)
            {
                if (hiddenItemDropdowns[i].SelectedIndex >= 0) grottoNarc.grottos[grottoIDDropdown.SelectedIndex].hiddenItems[i] = (short)hiddenItemDropdowns[i].SelectedIndex;
            }

            grottoNarc.grottos[grottoIDDropdown.SelectedIndex].ApplyData();
        }
    }
}
