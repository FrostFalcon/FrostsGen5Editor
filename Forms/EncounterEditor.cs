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
    public partial class EncounterEditor : Form
    {
        EncounterNARC encounterNarc => MainEditor.encounterNarc;
        TextNARC textNARC => MainEditor.textNarc;

        List<EncounterSlot> encountersClipboard;

        public EncounterEditor()
        {
            InitializeComponent();

            encounterRouteNameDropdown.Items.AddRange(encounterNarc.encounterPools.ToArray());
            encounterPokemonNameDropDown.Items.AddRange(textNARC.textFiles[VersionConstants.PokemonNameTextFileID].text.ToArray());
        }

        private void encounterRouteNameDropdown_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry e)
            {
                routeEncounterTypesPanel.Enabled = true;
                encounterGroupListBox.Enabled = true;
                addEncounterGroupButton.Enabled = true;
                removeEncounterGroupButton.Enabled = true;
                encounterPokemonNameDropDown.Enabled = true;
                encounterGroupFormNumber.Enabled = true;
                encounterGroupMinLvNumber.Enabled = true;
                encounterGroupMaxLvNumber.Enabled = true;
                encounterGroupRateNumber.Enabled = true;
                encounterSlotApplyButton.Enabled = true;
                copyGrassToDarkGrassButton.Enabled = true;
                copyEncounterGroupButton.Enabled = true;
                encounterGroupApplyRouteButton.Enabled = true;
                toggleSeasonsButton.Enabled = true;

                if (e.season == -1) toggleSeasonsButton.Text = "Add Seasons";
                else toggleSeasonsButton.Text = "Remove Seasons";

                e.EncounterSlotsToGroups();
                SetupEncounterListBox(true);
            }
            else
            {
                routeEncounterTypesPanel.Enabled = false;
                encounterGroupListBox.Enabled = false;
                addEncounterGroupButton.Enabled = false;
                removeEncounterGroupButton.Enabled = false;
                encounterPokemonNameDropDown.Enabled = false;
                encounterGroupFormNumber.Enabled = false;
                encounterGroupMinLvNumber.Enabled = false;
                encounterGroupMaxLvNumber.Enabled = false;
                encounterGroupRateNumber.Enabled = false;
                encounterSlotApplyButton.Enabled = false;
                copyGrassToDarkGrassButton.Enabled = false;
                copyEncounterGroupButton.Enabled = false;
                encounterGroupApplyRouteButton.Enabled = false;
                toggleSeasonsButton.Enabled = false;
            }
        }

        private void SetupEncounterListBox(bool redoListBox)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry e)
            {
                List<RadioButton> radios = new List<RadioButton>()
                {
                    radioButton1,
                    radioButton2,
                    radioButton3,
                    radioButton4,
                    radioButton5,
                    radioButton6,
                    radioButton7
                };

                int selectedType = 0;
                for (int i = 0; i < 7; i++) if (radios[i].Checked) selectedType = i;

                if (redoListBox)
                {
                    encounterGroupListBox.Items.Clear();
                    if (selectedType <= 2) foreach (EncounterSlot slot in e.groupedLandSlots[selectedType])
                        {
                            encounterGroupListBox.Items.Add(slot);
                        }
                    else foreach (EncounterSlot slot in e.groupedWaterSlots[selectedType - 3])
                        {
                            encounterGroupListBox.Items.Add(slot);
                        }
                    encounterGroupListBox.SelectedIndex = 0;
                }

                if (encounterGroupListBox.SelectedItem is EncounterSlot encSlot)
                {
                    encounterPokemonNameDropDown.SelectedIndex = encSlot.pokemonID;
                    encounterGroupFormNumber.Value = encSlot.pokemonForm;
                    encounterGroupMinLvNumber.Value = Math.Max(encSlot.minLevel, (byte)1);
                    encounterGroupMaxLvNumber.Value = Math.Max(encSlot.maxLevel, (byte)1);
                    encounterGroupRateNumber.Value = encSlot.rate;
                }
            }
        }

        private void encounterGroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupEncounterListBox(false);
        }

        private void ChangeRouteEncounterType(object sender, EventArgs e)
        {
            SetupEncounterListBox(true);
        }

        private void encounterGroupApplyRouteButton_Click(object sender, EventArgs e)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry route)
            {
                List<RadioButton> radios = new List<RadioButton>()
                {
                    radioButton1,
                    radioButton2,
                    radioButton3,
                    radioButton4,
                    radioButton5,
                    radioButton6,
                    radioButton7
                };

                int selectedType = 0;
                for (int i = 0; i < 7; i++) if (radios[i].Checked) selectedType = i;

                if (selectedType <= 2)
                {
                    route.groupedLandSlots[selectedType].Clear();
                    foreach (EncounterSlot slot in encounterGroupListBox.Items) route.groupedLandSlots[selectedType].Add(slot);
                }
                else
                {
                    route.groupedWaterSlots[selectedType - 3].Clear();
                    foreach (EncounterSlot slot in encounterGroupListBox.Items) route.groupedWaterSlots[selectedType - 3].Add(slot);
                }
                route.ApplyData();
            }
        }

        private void encounterSlotApplyButton_Click(object sender, EventArgs e)
        {
            if (encounterGroupListBox.SelectedItem is EncounterSlot slot)
            {
                slot.pokemonID = (short)encounterPokemonNameDropDown.SelectedIndex;
                slot.pokemonForm = (int)encounterGroupFormNumber.Value;
                slot.minLevel = (byte)encounterGroupMinLvNumber.Value;
                slot.maxLevel = (byte)encounterGroupMaxLvNumber.Value;
                slot.rate = (int)encounterGroupRateNumber.Value;
            }

            int storeIndex = encounterGroupListBox.SelectedIndex;
            SetupEncounterListBox(true);
            encounterGroupListBox.SelectedIndex = Math.Min(storeIndex, encounterGroupListBox.Items.Count - 1);
        }

        private void copyEncounterGroupButton_Click(object sender, EventArgs e)
        {
            encountersClipboard = new List<EncounterSlot>();
            foreach (EncounterSlot slot in encounterGroupListBox.Items) encountersClipboard.Add(slot.Clone());
            pasteEncounterGroupButton.Enabled = true;
        }

        private void pasteEncounterGroupButton_Click(object sender, EventArgs e)
        {
            encounterGroupListBox.Items.Clear();
            foreach (EncounterSlot slot in encountersClipboard) encounterGroupListBox.Items.Add(slot.Clone());
        }

        private void addEncounterGroupButton_Click(object sender, EventArgs e)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry route)
            {
                int selectedType = 0;
                for (int i = 6; i >= 0; i--) if (((RadioButton)routeEncounterTypesPanel.Controls[i]).Checked) selectedType = 6 - i;

                List<EncounterSlot> slotList = new List<EncounterSlot>();
                if (selectedType <= 2) slotList = route.groupedLandSlots[selectedType];
                else slotList = route.groupedWaterSlots[selectedType - 3];

                if (slotList.Count < 12) slotList.Add(new EncounterSlot(1, 0, 1, 1, 0));
                SetupEncounterListBox(true);
                encounterGroupListBox.SelectedIndex = slotList.Count - 1;
            }
        }

        private void removeEncounterGroupButton_Click(object sender, EventArgs e)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry route)
            {
                int selectedType = 0;
                for (int i = 6; i >= 0; i--) if (((RadioButton)routeEncounterTypesPanel.Controls[i]).Checked) selectedType = 6 - i;

                List<EncounterSlot> slotList = new List<EncounterSlot>();
                if (selectedType <= 2) slotList = route.groupedLandSlots[selectedType];
                else slotList = route.groupedWaterSlots[selectedType - 3];

                if (slotList.Count > 1) slotList.RemoveAt(encounterGroupListBox.SelectedIndex);
                int storeIndex = encounterGroupListBox.SelectedIndex;
                SetupEncounterListBox(true);
                encounterGroupListBox.SelectedIndex = Math.Min(storeIndex, slotList.Count - 1);
            }
        }

        private void ViewPokemonButton(object sender, EventArgs e)
        {
            Program.main.OpenPokemonEditor(sender, e);
            if (MainEditor.pokemonEditor != null)
            {
                if (MainEditor.pokemonDataNarc.pokemon.Exists(p => p.nameID == encounterPokemonNameDropDown.SelectedIndex)) MainEditor.pokemonEditor.pokemonNameDropdown.SelectedItem = MainEditor.pokemonDataNarc.pokemon.First(p => p.nameID == encounterPokemonNameDropDown.SelectedIndex);
                else MessageBox.Show("Could not find the pokemon");
            }
        }

        private void copyGrassToDarkGrassButton_Click(object sender, EventArgs e)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry route)
            {
                route.groupedLandSlots[1].Clear();
                foreach (EncounterSlot slot in route.groupedLandSlots[0])
                {
                    EncounterSlot s = slot.Clone();
                    s.minLevel = (byte)Math.Min((int)Math.Ceiling(s.minLevel * 1.1f), 100);
                    s.maxLevel = (byte)Math.Min((int)Math.Ceiling(s.maxLevel * 1.1f), 100);
                    route.groupedLandSlots[1].Add(s);
                }
            }
        }

        private void KeepMinLevelUnderMaxLevel(object sender, EventArgs e)
        {
            if (encounterGroupMaxLvNumber.Value < encounterGroupMinLvNumber.Value) encounterGroupMinLvNumber.Value = encounterGroupMaxLvNumber.Value;
        }

        private void KeepMaxLevelOverMinLevel(object sender, EventArgs e)
        {
            if (encounterGroupMinLvNumber.Value > encounterGroupMaxLvNumber.Value) encounterGroupMaxLvNumber.Value = encounterGroupMinLvNumber.Value;
        }

        private void toggleSeasonsButton_Click(object sender, EventArgs e)
        {
            if (encounterRouteNameDropdown.SelectedItem is EncounterEntry route)
            {
                if (encounterNarc.subEncounterPools.Contains(route))
                {
                    MessageBox.Show("Please select the Spring entry for this route");
                    return;
                }

                if (route.season == 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete the seasonal enounters for this route?", "Delete Seasons", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    encounterNarc.subEncounterPools.RemoveAll(r => r.parentPool == route);
                    route.season = -1;
                    toggleSeasonsButton.Text = "Add Seasons";
                }
                else if (route.season == -1)
                {
                    for (int i = 1; i <= 3; i++) encounterNarc.subEncounterPools.Add(new EncounterEntry(route.bytes.ToArray()) { nameID = route.nameID, season = i, parentPool = route });
                    route.season = 0;
                    toggleSeasonsButton.Text = "Remove Seasons";
                }

                encounterRouteNameDropdown.Items.Clear();
                encounterRouteNameDropdown.Items.AddRange(encounterNarc.encounterPools.ToArray());
                encounterRouteNameDropdown.SelectedItem = route;
            }
        }
    }
}
