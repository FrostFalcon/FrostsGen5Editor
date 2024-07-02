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
    public partial class OverworldEditor : Form
    {
        TextNARC textNARC => MainEditor.textNarc;
        ZoneDataNARC zoneNARC => MainEditor.zoneDataNarc;
        MapMatrixNARC mapMatrixNarc => MainEditor.mapMatrixNarc;
        OverworldObjectsNARC overworldObjectNarc => MainEditor.overworldsNarc;

        public OverworldEditor()
        {
            InitializeComponent();

            zoneIdDropdown.Items.AddRange(zoneNARC.zones.ToArray());
            mapNameDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.ZoneNameTextFileID].text.ToArray());
        }

        private void LoadZoneIntoEditor(object sender, EventArgs e)
        {
            if (zoneIdDropdown.SelectedItem is ZoneDataEntry z && z.bytes.Length == 48)
            {
                mapTypeNumberBox.Value = z.mapType;
                mapMatrixNumberBox.Value = z.matrix;
                scriptFileNumberBox.Value = z.scriptFile;
                textFileNumberBox.Value = z.storyTextFile;
                encounterFileNumberBox.Value = z.encounterFile;
                mapIDNumberBox.Value = z.mapId;
                parentMapIDNumberBox.Value = z.parentMapId;
                weatherNumberBox.Value = z.weather;
                textureNumberBox.Value = z.texture;
                unk4NumberBox.Value = z.unknown4;
                unk2NumberBox.Value = z.unknown2;
                mapNameDropdown.SelectedIndex = z.nameId;

                mapTypeNumberBox.Enabled = true;
                mapMatrixNumberBox.Enabled = true;
                scriptFileNumberBox.Enabled = true;
                textFileNumberBox.Enabled = true;
                encounterFileNumberBox.Enabled = true;
                mapIDNumberBox.Enabled = true;
                parentMapIDNumberBox.Enabled = true;
                weatherNumberBox.Enabled = true;
                textureNumberBox.Enabled = true;
                unk4NumberBox.Enabled = true;
                unk2NumberBox.Enabled = true;
                mapNameDropdown.Enabled = true;
                applyZoneButton.Enabled = true;

                if (overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs.Count > 0)
                {
                    overworlObjectTabs.TabPages[0].Enabled = true;

                    npcIDNumberBox.Value = 0;
                    npcIDNumberBox.Maximum = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs.Count - 1;
                    npcCountLabel.Text = "/ " + npcIDNumberBox.Maximum.ToString();

                    npcIDNumberBox_ValueChanged(sender, e);
                }
                else
                {
                    overworlObjectTabs.TabPages[0].Enabled = false;
                }

                if (overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture.Count > 0)
                {
                    overworlObjectTabs.TabPages[1].Enabled = true;

                    furnitureIDNumberBox.Value = 0;
                    furnitureIDNumberBox.Maximum = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture.Count - 1;
                    furnitureCountLabel.Text = "/ " + furnitureIDNumberBox.Maximum.ToString();

                    furnitureIDNumberBox_ValueChanged(sender, e);
                }
                else
                {
                    overworlObjectTabs.TabPages[1].Enabled = false;
                }

                if (overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps.Count > 0)
                {
                    overworlObjectTabs.TabPages[2].Enabled = true;

                    warpIDNumberBox.Value = 0;
                    warpIDNumberBox.Maximum = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps.Count - 1;
                    warpCountLabel.Text = "/ " + warpIDNumberBox.Maximum.ToString();

                    warpIDNumberBox_ValueChanged(sender, e);
                }
                else
                {
                    overworlObjectTabs.TabPages[2].Enabled = false;
                }

                if (overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers.Count > 0)
                {
                    overworlObjectTabs.TabPages[3].Enabled = true;

                    triggerIDNumberBox.Value = 0;
                    triggerIDNumberBox.Maximum = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers.Count - 1;
                    triggerCountLabel.Text = "/ " + triggerIDNumberBox.Maximum.ToString();

                    triggerIDNumberBox_ValueChanged(sender, e);
                }
                else
                {
                    overworlObjectTabs.TabPages[3].Enabled = false;
                }

                string text = "";
                for (int n = 0; n < overworldObjectNarc.objects[(int)mapIDNumberBox.Value].endData.Count; n++)
                {
                    text += overworldObjectNarc.objects[(int)mapIDNumberBox.Value].endData[n].ToString("X2");
                    if (n % 6 == 5) text += "\n";
                    else text += " ";
                }
                extraDataTextBox.Text = text;
            }
            else
            {
                mapTypeNumberBox.Enabled = false;
                mapMatrixNumberBox.Enabled = false;
                scriptFileNumberBox.Enabled = false;
                textFileNumberBox.Enabled = false;
                encounterFileNumberBox.Enabled = false;
                mapIDNumberBox.Enabled = false;
                parentMapIDNumberBox.Enabled = false;
                weatherNumberBox.Enabled = false;
                textureNumberBox.Enabled = false;
                unk4NumberBox.Enabled = false;
                unk2NumberBox.Enabled = false;
                mapNameDropdown.Enabled = false;
                applyZoneButton.Enabled = false;

                overworlObjectTabs.Enabled = false;
            }
        }

        private void ApplyZoneData(object sender, EventArgs e)
        {
            if (zoneIdDropdown.SelectedItem is ZoneDataEntry z && z.bytes.Length == 48)
            {
                z.mapType = (byte)mapTypeNumberBox.Value;
                z.matrix = (short)mapMatrixNumberBox.Value;
                z.scriptFile = (short)scriptFileNumberBox.Value;
                z.storyTextFile = (short)textFileNumberBox.Value;
                z.encounterFile = (byte)encounterFileNumberBox.Value;
                z.mapId = (short)mapIDNumberBox.Value;
                z.parentMapId = (short)parentMapIDNumberBox.Value;
                z.unknown4 = (short)unk4NumberBox.Value;
                z.unknown2 = (byte)unk2NumberBox.Value;
                z.weather = (byte)weatherNumberBox.Value;
                z.texture = (short)textureNumberBox.Value;
                z.nameId = (byte)mapNameDropdown.SelectedIndex;

                z.ApplyData();
            }
        }

        private void openTextFileButton_Click(object sender, EventArgs e)
        {
            Program.main.OpenTextViewer(sender, e);
            if (MainEditor.textViewer != null)
            {
                MainEditor.textViewer.storyTextRadioButton.Checked = true;
                if (textFileNumberBox.Value > 0 && textFileNumberBox.Value < MainEditor.textViewer.fileNumComboBox.Items.Count) MainEditor.textViewer.fileNumComboBox.SelectedIndex = (int)textFileNumberBox.Value;
                else MessageBox.Show("Could not find the text file by index");
            }
        }

        private void openScriptFileButton_Click(object sender, EventArgs e)
        {
            Program.main.OpenScriptEditor(sender, e);
            if (MainEditor.scriptEditor != null)
            {
                if (scriptFileNumberBox.Value > 0 && scriptFileNumberBox.Value < MainEditor.scriptEditor.scriptFileDropdown.Items.Count) MainEditor.scriptEditor.scriptFileDropdown.SelectedIndex = (int)scriptFileNumberBox.Value;
                else MessageBox.Show("Could not find the script file by index");
            }
        }

        private void openEncounterFileButton_Click(object sender, EventArgs e)
        {
            Program.main.OpenEncounterEditor(sender, e);
            if (MainEditor.encounterEditor != null)
            {
                if (encounterFileNumberBox.Value > 0 && encounterFileNumberBox.Value < MainEditor.encounterNarc.mainEncounterPools.Count) MainEditor.encounterEditor.encounterRouteNameDropdown.SelectedItem = MainEditor.encounterNarc.mainEncounterPools[(int)encounterFileNumberBox.Value];
                else MessageBox.Show("Could not find the encounter file by index");
            }
        }

        private void npcIDNumberBox_ValueChanged(object sender, EventArgs e)
        {
            OverworldNPC npc = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs[(int)npcIDNumberBox.Value];

            npcSpriteIDNumberBox.Value = npc.sprite;
            npcFlagNumberBox.Value = npc.flag;
            npcScriptNumberBox.Value = npc.scriptUsed;
            npcXLeashNumberBox.Value = npc.horizontalLeash;
            npcYLeashNumberBox.Value = npc.verticalLeash;
            npcSightRangeNumberBox.Value = npc.sightRange;
            npcMovementPermissionsNumberBox.Value = npc.movementPermissions;
            npcXPositionNumberBox.Value = npc.xPosition;
            npcYPositionNumberBox.Value = npc.yPosition;
            npcZPositionNumberBox.Value = npc.zPosition;
            npcDirectionNumberBox.Value = npc.defaultDirection;
        }

        private void furnitureIDNumberBox_ValueChanged(object sender, EventArgs e)
        {
            OverworldFurniture fur = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture[(int)furnitureIDNumberBox.Value];
            furnitureScriptNumberBox.Value = fur.scriptUsed;
            //furnitureXPosNumberBox.Value = fur.xPosition;
            //furnitureYPosNumberBox.Value = fur.yPosition;
            //furnitureZPosNumberBox.Value = fur.zPosition;
        }

        private void warpIDNumberBox_ValueChanged(object sender, EventArgs e)
        {
            OverworldWarp warp = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps[(int)warpIDNumberBox.Value];

            warpDestMapNumberBox.Value = warp.destinationMap;
            warpDestWarpNumberBox.Value = warp.destinationWarp;
            warpExitXNumberBox.Value = warp.exitX;
            warpExitYNumberBox.Value = warp.exitY;
            warpWidthNumberBox.Value = warp.width;
            warpHeightNumberBox.Value = warp.height;
        }

        private void triggerIDNumberBox_ValueChanged(object sender, EventArgs e)
        {
            OverworldTrigger trigger = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers[(int)triggerIDNumberBox.Value];

            triggerConstValNumberBox.Value = trigger.constantValue;
            triggerConstRefNumberBox.Value = trigger.constantReference;
            triggerScriptNumberBox.Value = trigger.scriptUsed;
            triggerXNumberBox.Value = trigger.xPosition;
            triggerYNumberBox.Value = trigger.yPosition;
            triggerWidthNumberBox.Value = trigger.width;
            triggerHeightNumberBox.Value = trigger.height;
            triggerYNumberBox.Value = trigger.yPosition;
            triggerZNumberBox.Value = trigger.zPosition;
        }

        private void addObjectButton_Click(object sender, EventArgs e)
        {
            if (overworlObjectTabs.SelectedIndex == 0) overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs.Add(new OverworldNPC());
            else if (overworlObjectTabs.SelectedIndex == 1) overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture.Add(new OverworldFurniture());
            else if (overworlObjectTabs.SelectedIndex == 2) overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps.Add(new OverworldWarp());
            else if (overworlObjectTabs.SelectedIndex == 3) overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers.Add(new OverworldTrigger());
            overworldObjectNarc.objects[(int)mapIDNumberBox.Value].ApplyData();
            LoadZoneIntoEditor(sender, e);
        }

        private void removeObjectButton_Click(object sender, EventArgs e)
        {
            if (overworlObjectTabs.SelectedIndex == 0 && overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs.Count > 0)
            {
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs.RemoveAt((int)npcIDNumberBox.Value);
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].ApplyData();
                LoadZoneIntoEditor(sender, e);
            }
            else if (overworlObjectTabs.SelectedIndex == 1 && overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture.Count > 0)
            {
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture.RemoveAt((int)furnitureIDNumberBox.Value);
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].ApplyData();
                LoadZoneIntoEditor(sender, e);
            }
            else if (overworlObjectTabs.SelectedIndex == 2 && overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps.Count > 0)
            {
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps.RemoveAt((int)warpIDNumberBox.Value);
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].ApplyData();
                LoadZoneIntoEditor(sender, e);
            }
            else if (overworlObjectTabs.SelectedIndex == 3 && overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers.Count > 0)
            {
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers.RemoveAt((int)triggerIDNumberBox.Value);
                overworldObjectNarc.objects[(int)mapIDNumberBox.Value].ApplyData();
                LoadZoneIntoEditor(sender, e);
            }
        }

        private void applyObjectButton_Click(object sender, EventArgs e)
        {
            if (overworlObjectTabs.SelectedIndex == 0)
            {
                OverworldNPC npc = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].NPCs[(int)npcIDNumberBox.Value];

                npc.sprite = (short)npcSpriteIDNumberBox.Value;
                npc.flag = (short)npcFlagNumberBox.Value;
                npc.scriptUsed = (short)npcScriptNumberBox.Value;
                npc.horizontalLeash = (short)npcXLeashNumberBox.Value;
                npc.verticalLeash = (short)npcYLeashNumberBox.Value;
                npc.sightRange = (short)npcSightRangeNumberBox.Value;
                npc.movementPermissions = (short)npcMovementPermissionsNumberBox.Value;
                npc.xPosition = (short)npcXPositionNumberBox.Value;
                npc.yPosition = (short)npcYPositionNumberBox.Value;
                npc.zPosition = (short)npcZPositionNumberBox.Value;
                npc.defaultDirection = (short)npcDirectionNumberBox.Value;
            }
            if (overworlObjectTabs.SelectedIndex == 1)
            {
                OverworldFurniture fur = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].furniture[(int)furnitureIDNumberBox.Value];

                fur.scriptUsed = (short)furnitureScriptNumberBox.Value;
                //fur.xPosition = (short)furnitureXPosNumberBox.Value;
                //fur.yPosition = (short)furnitureYPosNumberBox.Value;
                //fur.zPosition = (short)furnitureZPosNumberBox.Value;
            }
            else if (overworlObjectTabs.SelectedIndex == 2)
            {
                OverworldWarp warp = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].warps[(int)warpIDNumberBox.Value];

                warp.destinationMap = (short)warpDestMapNumberBox.Value;
                warp.destinationWarp = (short)warpDestWarpNumberBox.Value;
                warp.exitX = (int)warpExitXNumberBox.Value;
                warp.exitY = (int)warpExitYNumberBox.Value;
                warp.width = (short)warpWidthNumberBox.Value;
                warp.height = (short)warpHeightNumberBox.Value;
            }
            else if (overworlObjectTabs.SelectedIndex == 3)
            {
                OverworldTrigger trigger = overworldObjectNarc.objects[(int)mapIDNumberBox.Value].triggers[(int)triggerIDNumberBox.Value];

                trigger.constantValue = (short)triggerConstValNumberBox.Value;
                trigger.constantReference = (short)triggerConstRefNumberBox.Value;
                trigger.scriptUsed = (short)triggerScriptNumberBox.Value;
                trigger.xPosition = (short)triggerXNumberBox.Value;
                trigger.yPosition = (short)triggerYNumberBox.Value;
                trigger.zPosition = (short)triggerZNumberBox.Value;
                trigger.width = (short)triggerWidthNumberBox.Value;
                trigger.height = (short)triggerHeightNumberBox.Value;
            }
            else if (overworlObjectTabs.SelectedIndex == 4) ApplyEndData(overworldObjectNarc.objects[(int)mapIDNumberBox.Value].endData);

            overworldObjectNarc.objects[(int)mapIDNumberBox.Value].ApplyData();
        }

        private void ApplyEndData(List<byte> destination)
        {
            string str = extraDataTextBox.Text.Replace('\n', ' ');

            //Test for improper text length
            if (str.Length % 3 == 2 && str[extraDataTextBox.Text.Length - 1] != ' ') str += ' ';
            if (str.Length < 3 || str.Length % 3 != 0)
            {
                MessageBox.Show("Byte Data detected an incorrect format");
                return;
            }

            //Test for improper text values
            for (int i = 2; i < str.Length; i += 3) if (str[i] != ' ' ||
                    (!char.IsDigit(str[i - 1]) && !(str[i - 1] >= 'A' && str[i - 1] <= 'F')) ||
                    (!char.IsDigit(str[i - 2]) && !(str[i - 2] >= 'A' && str[i - 2] <= 'F')))
                {
                    MessageBox.Show("Byte Data detected an incorrect format");
                    return;
                }

            //Convert data to file
            destination.Clear();
            for (int i = 0; i < str.Length; i += 3)
            {
                destination.Add(byte.Parse(str.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
            }
        }

        private void npcScriptNumberBox_ValueChanged(object sender, EventArgs e)
        {
            int i = (int)npcScriptNumberBox.Value;
            if (i >= 7000 && i < 7400)
            {
                if (MainEditor.scriptNarc != null)
                {
                    //try
                    {
                        int item = MainEditor.scriptNarc.scriptFiles[MainEditor.RomType == RomType.BW2 ? 1240 : 864].sequences[i - 7000].commands[1].parameters[1];
                        string name = MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text[item];
                        giveItemLabel.Text = "Give Item: " + name;
                    }
                    //catch
                    {
                        //giveItemLabel.Text = "";
                    }
                }
            }
            else giveItemLabel.Text = "";
        }
    }
}
