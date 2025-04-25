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
    public partial class MoveEditor : Form
    {
        TextNARC textNARC => MainEditor.textNarc;
        MoveDataNARC moveDataNARC => MainEditor.moveDataNarc;
        MoveAnimationNARC moveAnimNARC => MainEditor.moveAnimationNarc;
        MoveAnimationNARC moveAnim2NARC => MainEditor.moveAnimationExtraNarc;

        static List<TextValue> damageTypes = new List<TextValue>()
        {
            new TextValue(1, "Physical"),
            new TextValue(2, "Special"),
            new TextValue(0, "Status")
        };

        static List<TextValue> categories = new List<TextValue>()
        {
            new TextValue(0, "Damage"),
            new TextValue(4, "Dmg + Status Effect"),
            new TextValue(6, "Dmg + Target Stat Change"),
            new TextValue(7, "Dmg + User Stat Change"),
            new TextValue(8, "Dmg + Life Steal"),
            new TextValue(1, "Status Effect"),
            new TextValue(2, "Stat Change"),
            new TextValue(3, "Healing"),
            new TextValue(5, "Status + Stat Change"),
            new TextValue(10, "Effect On Field"),
            new TextValue(11, "Effect On One Side"),
            new TextValue(12, "Force Switch Out"),
            new TextValue(9, "One Hit KO"),
            new TextValue(13, "Unique Effect")
        };

        static List<TextValue> targetTypes = new List<TextValue>()
        {
            new TextValue(0, "Single Target"),
            new TextValue(7, "Self"),
            new TextValue(4, "All Adjacent Pokemon"),
            new TextValue(3, "One Adjacent Enemy"),
            new TextValue(9, "Single Adjacent Enemy"),
            new TextValue(5, "All Adjacent Enemies"),
            new TextValue(1, "Single Ally"),
            new TextValue(2, "Single Adjacent Ally"),
            new TextValue(6, "All Allies"),
            new TextValue(8, "All Pokemon On Field"),
            new TextValue(10, "Entire Field"),
            new TextValue(11, "Opponent's Field"),
            new TextValue(12, "User's Field"),
            new TextValue(13, "Self (Protective)")
        };

        static List<TextValue> statusEffects = new List<TextValue>()
        {
            new TextValue(0, "None"),
            new TextValue(1, "Paralyze"),
            new TextValue(2, "Sleep"),
            new TextValue(3, "Freeze"),
            new TextValue(4, "Burn"),
            new TextValue(5, "Poison"),
            new TextValue(6, "Confusion"),
            new TextValue(7, "Attract"),
            new TextValue(8, "Trap"),
            new TextValue(9, "Nightmare"),
            new TextValue(10, "Curse"),
            new TextValue(11, "Taunt"),
            new TextValue(12, "Torment"),
            new TextValue(13, "Disable"),
            new TextValue(14, "Yawn"),
            new TextValue(15, "Heal Block"),
            new TextValue(16, "?"),
            new TextValue(17, "Detect"),
            new TextValue(18, "Leech Seed"),
            new TextValue(19, "Embargo"),
            new TextValue(20, "Perish Song"),
            new TextValue(21, "Ingrain"),
            new TextValue(-1, "Special"),
        };

        static List<TextValue> statChanges = new List<TextValue>()
        {
            new TextValue(0, "None"),
            new TextValue(1, "Attack"),
            new TextValue(2, "Defense"),
            new TextValue(3, "Sp. Att"),
            new TextValue(4, "Sp. Def"),
            new TextValue(5, "Speed"),
            new TextValue(6, "Accuracy"),
            new TextValue(7, "Evasion"),
            new TextValue(8, "All"),
        };

        public MoveEditor()
        {
            InitializeComponent();

            moveNameDropdown.Items.AddRange(moveDataNARC.moves.ToArray());
            copyAnimationDropdown.Items.AddRange(moveDataNARC.moves.ToArray());

            moveTypeDropdown.Items.AddRange(textNARC.textFiles[VersionConstants.TypeNameTextFileID].text.ToArray());
            moveCategoryDropdown.Items.AddRange(categories.ToArray());
            moveDamageTypeDropdown.Items.AddRange(damageTypes.ToArray());
            moveTargetDropdown.Items.AddRange(targetTypes.ToArray());

            statChangeStatDropdown1.Items.AddRange(statChanges.ToArray());
            statChangeStatDropdown2.Items.AddRange(statChanges.ToArray());
            statChangeStatDropdown3.Items.AddRange(statChanges.ToArray());

            statusEffectDropdown.Items.AddRange(statusEffects.ToArray());
            addMovesButton.Enabled = moveDataNARC.moves.Count < 1000;
        }

        private void LoadMoveIntoEditor(object sender, EventArgs e)
        {
            if (moveNameDropdown.SelectedItem is MoveDataEntry m)
            {
                moveTypeDropdown.SelectedIndex = m.element;
                moveCategoryDropdown.SelectedItem = categories.First(l => l.hexID == m.category);
                moveDamageTypeDropdown.SelectedItem = damageTypes.First(l => l.hexID == m.damageType);
                moveTargetDropdown.SelectedItem = targetTypes.First(l => l.hexID == m.target);
                moveBasePowerNumberBox.Value = m.basePower;
                moveAccuracyNumberBox.Value = m.accuracy;
                movePPNumberBox.Value = m.powerPoints;

                moveEffectCodeNumberBox.Value = m.effectCode;
                movePriorityNumberBox.Value = m.priority;
                moveCritRatioNumberBox.Value = m.critRatio;
                moveFlinchChanceNumberBox.Value = m.flinchChance;
                moveHealPercentNumberBox.Value = m.healPercent;
                moveLeechRecoilNumberBox.Value = m.leechRecoil;
                moveMinMultihitsNumberBox.Value = m.minMultiHit;
                moveMaxMultihitsNumberBox.Value = m.maxMultiHit;
                moveMinTrapTurnsNumberBox.Value = m.minTrapTurns;
                moveMaxTrapTurnsNumberBox.Value = m.maxTrapTurns;

                statChangeStatDropdown1.SelectedItem = statChanges.First(l => l.hexID == m.statChange1Stat);
                statChangeStatDropdown2.SelectedItem = statChanges.First(l => l.hexID == m.statChange2Stat);
                statChangeStatDropdown3.SelectedItem = statChanges.First(l => l.hexID == m.statChange3Stat);
                statChangeStagesNumberBox1.Value = m.statChange1Stages;
                statChangeStagesNumberBox2.Value = m.statChange2Stages;
                statChangeStagesNumberBox3.Value = m.statChange3Stages;
                statChangeChanceNumberBox1.Value = m.statChange1Chance;
                statChangeChanceNumberBox2.Value = m.statChange2Chance;
                statChangeChanceNumberBox3.Value = m.statChange3Chance;

                statusEffectDropdown.SelectedItem = statusEffects.First(l => l.hexID == m.statusEffect);
                moveStatusChanceNumberBox.Value = m.statusChance;

                baseDataGroup.Enabled = true;
                additionalStatsGroupBox.Enabled = true;
                inflictionsGroupBox.Enabled = true;
                applyMoveButton.Enabled = true;
                renameMoveButton.Enabled = true;
                setDescriptionButton.Enabled = true;
                copyAnimationButton.Enabled = true;
                flagsListBox.Enabled = true;

                if (moveNameDropdown.SelectedIndex >= 0)
                {
                    MoveAnimationEntry mAnim =  moveAnimNARC.animations[moveNameDropdown.SelectedIndex];
                    string text = "";
                    foreach (byte b in mAnim.bytes) text += b.ToString("X2") + " ";
                    animDataTextBox.Text = text;

                    animDataTextBox.Enabled = true;
                    applyAnimDataButton.Enabled = true;
                }
                else
                {
                    animDataTextBox.Enabled = false;
                    applyAnimDataButton.Enabled = false;
                }

                int id = moveDataNARC.moves.IndexOf(m);
                if (id >= 0)
                {
                    newMoveNameTextBox.Text = textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[id];
                    setDescriptionTextBox.Text = textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].text[id];
                }

                for (int i = 0; i < 16; i++)
                {
                    flagsListBox.SetItemChecked(i, (m.flags & (int)Math.Pow(2, i)) != 0);
                }
            }
            else
            {
                baseDataGroup.Enabled = false;
                additionalStatsGroupBox.Enabled = false;
                inflictionsGroupBox.Enabled = false;
                applyMoveButton.Enabled = false;
                renameMoveButton.Enabled = false;
                setDescriptionButton.Enabled = false;
                copyAnimationButton.Enabled = false;
                flagsListBox.Enabled = false;

                animDataTextBox.Enabled = false;
                applyAnimDataButton.Enabled = false;
            }
        }

        private void ApplyMove(object sender, EventArgs e)
        {
            if (moveNameDropdown.SelectedItem is MoveDataEntry m)
            {
                m.element = (byte)moveTypeDropdown.SelectedIndex;
                m.category = (byte)((TextValue)moveCategoryDropdown.SelectedItem).hexID;
                m.damageType = (byte)((TextValue)moveDamageTypeDropdown.SelectedItem).hexID;
                m.target = (byte)((TextValue)moveTargetDropdown.SelectedItem).hexID;
                m.basePower = (byte)moveBasePowerNumberBox.Value;
                m.accuracy = (byte)moveAccuracyNumberBox.Value;
                m.powerPoints = (byte)movePPNumberBox.Value;

                m.effectCode = (short)moveEffectCodeNumberBox.Value;
                m.priority = (sbyte)movePriorityNumberBox.Value;
                m.critRatio = (byte)moveCritRatioNumberBox.Value;
                m.flinchChance = (byte)moveFlinchChanceNumberBox.Value;
                m.healPercent = (byte)moveHealPercentNumberBox.Value;
                m.leechRecoil = (sbyte)moveLeechRecoilNumberBox.Value;
                m.minMultiHit = (byte)moveMinMultihitsNumberBox.Value;
                m.maxMultiHit = (byte)moveMaxMultihitsNumberBox.Value;
                m.minTrapTurns = (byte)moveMinTrapTurnsNumberBox.Value;
                m.maxTrapTurns = (byte)moveMaxTrapTurnsNumberBox.Value;

                m.statChange1Stat = (byte)((TextValue)statChangeStatDropdown1.SelectedItem).hexID;
                m.statChange2Stat = (byte)((TextValue)statChangeStatDropdown2.SelectedItem).hexID;
                m.statChange3Stat = (byte)((TextValue)statChangeStatDropdown3.SelectedItem).hexID;
                m.statChange1Stages = (sbyte)statChangeStagesNumberBox1.Value;
                m.statChange2Stages = (sbyte)statChangeStagesNumberBox2.Value;
                m.statChange3Stages = (sbyte)statChangeStagesNumberBox3.Value;
                m.statChange1Chance = (byte)statChangeChanceNumberBox1.Value;
                m.statChange2Chance = (byte)statChangeChanceNumberBox2.Value;
                m.statChange3Chance = (byte)statChangeChanceNumberBox3.Value;

                m.statusEffect = (short)((TextValue)statusEffectDropdown.SelectedItem).hexID;
                m.statusChance = (byte)moveStatusChanceNumberBox.Value;

                short f = 0;
                for (int i = 0; i < 16; i++)
                {
                    if (flagsListBox.GetItemChecked(i)) f += (short)Math.Pow(2, i);
                }
                m.flags = f;

                m.ApplyData();
            }
        }

        private void renameMoveButton_Click(object sender, EventArgs e)
        {
            if (moveNameDropdown.SelectedItem is MoveDataEntry m)
            {
                int id = moveDataNARC.moves.IndexOf(m);
                if (id <= 0) return;

                string oldText = textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[id];

                textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[id] = newMoveNameTextBox.Text;
                textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[id * 3] = textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[3].Replace("Pound", newMoveNameTextBox.Text);
                textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[id * 3 + 1] = textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[4].Replace("Pound", newMoveNameTextBox.Text);
                textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[id * 3 + 2] = textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[5].Replace("Pound", newMoveNameTextBox.Text);

                textNARC.textFiles[VersionConstants.MoveNameTextFileID].CompressData();
                textNARC.textFiles[VersionConstants.MoveUsageTextFileID].CompressData();
            }
        }

        private void setDescriptionButton_Click(object sender, EventArgs e)
        {
            if (moveNameDropdown.SelectedItem is MoveDataEntry m)
            {
                int id = moveDataNARC.moves.IndexOf(m);
                if (id <= 0) return;

                textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].text[id] = setDescriptionTextBox.Text;
                textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].CompressData();
            }
        }

        private void copyAnimationButton_Click(object sender, EventArgs e)
        {
            if (moveNameDropdown.SelectedItem is MoveDataEntry m1 && copyAnimationDropdown.SelectedItem is MoveDataEntry m2)
            {

                string text = "";
                MoveAnimationEntry mAnim = moveAnimNARC.animations[copyAnimationDropdown.SelectedIndex];
                foreach (byte b in mAnim.bytes) text += b.ToString("X2") + " ";
                animDataTextBox.Text = text;
            }
        }

        private void applyAnimDataButton_Click(object sender, EventArgs e)
        {
            animDataTextBox.Text = animDataTextBox.Text.Replace("\n", " ");

            if (moveNameDropdown.SelectedIndex >= 0)
            {
                MoveAnimationEntry anim = moveAnimNARC.animations[moveNameDropdown.SelectedIndex];

                //Test for improper text length
                if (animDataTextBox.Text.Length % 3 == 2 && animDataTextBox.Text[animDataTextBox.Text.Length - 1] != ' ') animDataTextBox.Text += ' ';
                if (animDataTextBox.Text.Length < 3 || animDataTextBox.Text.Length % 3 != 0)
                {
                    MessageBox.Show("Raw Data detected an incorrect format");
                    return;
                }

                //Test for improper text values
                for (int i = 2; i < animDataTextBox.Text.Length; i += 3) if (animDataTextBox.Text[i] != ' ' ||
                        (!char.IsDigit(animDataTextBox.Text[i - 1]) && !(animDataTextBox.Text[i - 1] >= 'A' && animDataTextBox.Text[i - 1] <= 'F')) ||
                        (!char.IsDigit(animDataTextBox.Text[i - 2]) && !(animDataTextBox.Text[i - 2] >= 'A' && animDataTextBox.Text[i - 2] <= 'F')))
                    {
                        MessageBox.Show("Raw Data detected an incorrect format");
                        return;
                    }

                //Convert data to file
                anim.bytes = new byte[animDataTextBox.Text.Length / 3];
                for (int i = 0; i < animDataTextBox.Text.Length; i += 3)
                {
                    anim.bytes[i / 3] = byte.Parse(animDataTextBox.Text.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
        }

        private void addMovesButton_Click(object sender, EventArgs e)
        {
            while (moveDataNARC.moves.Count < 800)
            {
                moveDataNARC.moves.Add(new MoveDataEntry(moveDataNARC.moves[1].bytes.ToArray()) { nameID = moveDataNARC.moves.Count });
                if (textNARC.textFiles[403].text.Count < moveDataNARC.moves.Count) textNARC.textFiles[403].text.Add(textNARC.textFiles[403].text[1]);
                if (textNARC.textFiles[402].text.Count < moveDataNARC.moves.Count) textNARC.textFiles[402].text.Add(textNARC.textFiles[402].text[1]);
                if (textNARC.textFiles[16].text.Count < moveDataNARC.moves.Count * 3)
                {
                    textNARC.textFiles[16].text.Add(textNARC.textFiles[16].text[3]);
                    textNARC.textFiles[16].text.Add(textNARC.textFiles[16].text[4]);
                    textNARC.textFiles[16].text.Add(textNARC.textFiles[16].text[5]);
                }
                if (moveDataNARC.moves.Count <= 680) textNARC.textFiles[403].text[moveDataNARC.moves.Count - 1] = "DontUse";
            }
            while (moveAnimNARC.animations.Count < 800)
            {
                moveAnimNARC.animations.Add(new MoveAnimationEntry(moveAnimNARC.animations[1].bytes.ToArray()));
            }

            TextFile template = textNARC.textFiles[0];
            for (int i = 0; i < textNARC.textFiles.Count; i++) if (textNARC.textFiles[i].text.Count > template.text.Count && textNARC.textFiles[i].text.Count < 2900) template = textNARC.textFiles[i];
            while (textNARC.textFiles[16].text.Count < template.text.Count) textNARC.textFiles[16].text.Add("");
            textNARC.textFiles[16].bytes = PPTxtHandler.SaveEntry(template.bytes, textNARC.textFiles[16].text);

            while (textNARC.textFiles[403].text.Count < template.text.Count) textNARC.textFiles[403].text.Add("");
            textNARC.textFiles[403].bytes = PPTxtHandler.SaveEntry(template.bytes, textNARC.textFiles[403].text);

            while (textNARC.textFiles[402].text.Count < template.text.Count) textNARC.textFiles[402].text.Add("");
            textNARC.textFiles[402].bytes = PPTxtHandler.SaveEntry(template.bytes, textNARC.textFiles[402].text);

            moveNameDropdown.Items.Clear();
            moveNameDropdown.Items.AddRange(moveDataNARC.moves.ToArray());
            copyAnimationDropdown.Items.Clear();
            copyAnimationDropdown.Items.AddRange(moveDataNARC.moves.ToArray());
            addMovesButton.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "Nds Roms|*.nds";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = File.OpenRead(prompt.FileName);
                NDSFileSystem other = NDSFileSystem.FromRom(fileStream);
                fileStream.Close();

                TextNARC otherText = other.textNarc;
                MoveDataNARC otherMoves = other.moveDataNarc;
                MoveAnimationNARC otherAnims = other.moveAnimationNarc;

                int slot = 690;
                for (int i = 0; i < otherMoves.moves.Count; i++)
                {
                    if (otherText.textFiles[403].text[i] != textNARC.textFiles[403].text[i])
                    {
                        moveDataNARC.moves[slot] = new MoveDataEntry(moveDataNARC.moves[i].bytes.ToArray()) { nameID = slot };
                        moveAnim2NARC.animations[slot - 561] = new MoveAnimationEntry(moveAnimNARC.animations[i].bytes.ToArray());

                        textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[slot] = textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[i];
                        textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[slot * 3] = textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[3].Replace("Pound", textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[i]);
                        textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[slot * 3 + 1] = textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[4].Replace("Pound", textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[i]);
                        textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[slot * 3 + 2] = textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[5].Replace("Pound", textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[i]);
                        textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].text[slot] = textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].text[i];

                        moveDataNARC.moves[i] = new MoveDataEntry(otherMoves.moves[i].bytes.ToArray()) { nameID = i };
                        moveAnimNARC.animations[i] = new MoveAnimationEntry(otherAnims.animations[i].bytes.ToArray());

                        textNARC.textFiles[VersionConstants.MoveNameTextFileID].text[i] = otherText.textFiles[VersionConstants.MoveNameTextFileID].text[i];
                        textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[i * 3] = otherText.textFiles[VersionConstants.MoveUsageTextFileID].text[3].Replace("Pound", otherText.textFiles[VersionConstants.MoveNameTextFileID].text[i]);
                        textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[i * 3 + 1] = otherText.textFiles[VersionConstants.MoveUsageTextFileID].text[4].Replace("Pound", otherText.textFiles[VersionConstants.MoveNameTextFileID].text[i]);
                        textNARC.textFiles[VersionConstants.MoveUsageTextFileID].text[i * 3 + 2] = otherText.textFiles[VersionConstants.MoveUsageTextFileID].text[5].Replace("Pound", otherText.textFiles[VersionConstants.MoveNameTextFileID].text[i]);
                        textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].text[i] = otherText.textFiles[VersionConstants.MoveDescriptionTextFileID].text[i];

                        foreach (PokemonEntry poke in moveDataNARC.fileSystem.pokemonDataNarc.pokemon)
                        {
                            if (poke.levelUpMoves != null)
                            {
                                for (int j = 0; j < poke.levelUpMoves.moves.Count; j++)
                                {
                                    if (poke.levelUpMoves.moves[j].moveID == i) poke.levelUpMoves.moves[j] = new LevelUpMoveSlot((short)slot, poke.levelUpMoves.moves[j].level);
                                }
                            }
                        }
                        slot++;
                    }
                }
                textNARC.textFiles[VersionConstants.MoveNameTextFileID].CompressData();
                textNARC.textFiles[VersionConstants.MoveUsageTextFileID].CompressData();
                textNARC.textFiles[VersionConstants.MoveDescriptionTextFileID].CompressData();

                MessageBox.Show("NARC replace complete");
            }
        }
    }
}
