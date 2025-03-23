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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Forms
{
    public partial class ScriptEditor : Form
    {
        public List<object> sequenceClipboard;

        public ScriptEditor()
        {
            InitializeComponent();

            for (int i = 0; i < MainEditor.scriptNarc.scriptFiles.Count; i++) scriptFileDropdown.Items.Add(i);

            foreach (KeyValuePair<int, NewEditor.Data.NARCTypes.CommandType> c in CommandReference.commandList) commandTypeDropdown.Items.Add(c.Value.name);
        }

        private void LoadScriptFile(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];

                if (sf.valid && sf.sequences != null && sf.sequences.Count > 0)
                {
                    sequenceIDNumberBox.Value = 0;
                    sequenceIDNumberBox.Maximum = sf.sequences.Count - 1;
                    sequenceCountLabel.Text = "/ " + (sf.sequences.Count - 1).ToString();

                    LoadSequence(sender, e);


                    sequenceIDNumberBox.Enabled = true;
                    commandsListBox.Enabled = true;
                    commandTypeDropdown.Enabled = true;
                    saveButton.Enabled = true;
                    rawDataTextBox.Enabled = true;
                    applyRawDataButton.Enabled = true;
                    readScriptFileButton.Enabled = true;
                    exportScriptFileButton.Enabled = true;
                    quickBuildButton.Enabled = quickBuildRom != "";
                }
                else
                {
                    sequenceIDNumberBox.Enabled = false;
                    commandsListBox.Enabled = false;
                    commandTypeDropdown.Enabled = false;
                    saveButton.Enabled = false;
                    rawDataTextBox.Enabled = true;
                    applyRawDataButton.Enabled = true;
                    readScriptFileButton.Enabled = false;
                    exportScriptFileButton.Enabled = false;
                    quickBuildButton.Enabled = false;
                }
                string text = "";
                foreach (byte b in sf.bytes) text += b.ToString("X2") + " ";
                rawDataTextBox.Text = text;
            }
        }

        private void LoadSequence(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];
                ScriptSequence seq = sf.sequences[(int)sequenceIDNumberBox.Value];

                commandsListBox.Items.Clear();
                foreach (ScriptCommand c in seq.commands) commandsListBox.Items.Add(c);
                foreach (byte b in seq.miscBytes) commandsListBox.Items.Add(b);
            }
        }

        private void ChangeSelectedListboxCommand(object sender, EventArgs e)
        {
            if (commandsListBox.SelectedItem is ScriptCommand c && c.commandID < commandTypeDropdown.Items.Count)
            {
                commandTypeDropdown.SelectedIndex = c.commandID;
            }

            ChangeCommandDropdown(sender, e);
        }

        private void DoubleClickListBox(object sender, EventArgs e)
        {
            if (commandsListBox.SelectedItem is ScriptCommand c)
            {
                //Jump
                int jumpDistance = -1;
                if (c.commandID == 0x1E) jumpDistance = c.parameters[0];
                if (c.commandID == 0x1F) jumpDistance = c.parameters[1];

                if (jumpDistance >= 0)
                {
                    int listPos = commandsListBox.SelectedIndex;
                    int jumpPos = 0;
                    while (listPos < commandsListBox.Items.Count - 1 && jumpPos <= jumpDistance)
                    {
                        listPos++;
                        jumpPos += ((ScriptCommand)commandsListBox.Items[listPos]).ByteLength;
                    }

                    if (listPos < commandsListBox.Items.Count)
                    {
                        commandsListBox.SelectedItems.Clear();
                        commandsListBox.SelectedIndex = listPos;
                    }
                }

                if (jumpDistance < -1)
                {
                    int listPos = commandsListBox.SelectedIndex + 1;
                    int jumpPos = 0;
                    while (listPos > 0 && jumpPos >= jumpDistance)
                    {
                        listPos--;
                        jumpPos -= ((ScriptCommand)commandsListBox.Items[listPos]).ByteLength;
                    }

                    if (listPos >= 0)
                    {
                        commandsListBox.SelectedItems.Clear();
                        commandsListBox.SelectedIndex = listPos;
                    }
                }

                //Find Message
                if (c.commandID == 0x3C || c.commandID == 0x3D || c.commandID == 0x48)
                {
                    if (MainEditor.textViewer != null && MainEditor.textViewer.storyTextRadioButton.Checked && MainEditor.textViewer.textBoxDisplay.Lines.Length > c.parameters[2])
                    {
                        int pos = MainEditor.textViewer.textBoxDisplay.GetFirstCharIndexFromLine(c.parameters[2]);
                        int length = MainEditor.textViewer.textBoxDisplay.Lines[c.parameters[2]].Length;
                        MainEditor.textViewer.textBoxDisplay.Select(pos, length);
                        MainEditor.textViewer.BringToFront();
                    }
                }

                //Find Movement
                if (c.commandID == 0x64)
                {
                    ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];
                    ScriptSequence seq = sf.sequences[(int)sequenceIDNumberBox.Value];

                    int n = HelperFunctions.ReadInt(sf.bytes, (int)sequenceIDNumberBox.Value * 4);
                    if (commandsListBox.SelectedIndex != -1)
                    {
                        for (int i = 0; i <= commandsListBox.SelectedIndex; i++) n += seq.commands[i].ByteLength;
                    }
                    n += c.parameters[1];
                    n = (int)sequenceIDNumberBox.Value * 4 + n + 4;
                    byteNumberBox.Value = n;
                    rawDataTextBox.Select();
                    rawDataTextBox.Select(rawDataTextBox.SelectionStart, 3);
                }
            }
        }

        private void ApplySequence(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];

                ScriptSequence seq = sf.sequences[(int)sequenceIDNumberBox.Value];

                seq.commands = new List<ScriptCommand>();
                seq.miscBytes = new List<RefByte>();
                foreach (object o in commandsListBox.Items)
                {
                    if (o is ScriptCommand com)
                    {
                        seq.commands.Add(com);
                    }
                    else if (o is byte b) seq.miscBytes.Add(b);
                }

                sf.ApplyData();
            }
        }

        private void ApplyRawData(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];

                //Test for improper text length
                if (rawDataTextBox.Text.Length % 3 == 2 && rawDataTextBox.Text[rawDataTextBox.Text.Length - 1] != ' ') rawDataTextBox.Text += ' ';
                if (rawDataTextBox.Text.Length < 3 || rawDataTextBox.Text.Length % 3 != 0)
                {
                    MessageBox.Show("Raw Data detected an incorrect format");
                    return;
                }

                //Test for improper text values
                for (int i = 2; i < rawDataTextBox.Text.Length; i += 3) if (rawDataTextBox.Text[i] != ' ' ||
                        (!char.IsDigit(rawDataTextBox.Text[i - 1]) && !(rawDataTextBox.Text[i - 1] >= 'A' && rawDataTextBox.Text[i - 1] <= 'F')) ||
                        (!char.IsDigit(rawDataTextBox.Text[i - 2]) && !(rawDataTextBox.Text[i - 2] >= 'A' && rawDataTextBox.Text[i - 2] <= 'F')))
                    {
                        MessageBox.Show("Raw Data detected an incorrect format");
                        return;
                    }

                //Convert data to file
                sf.bytes = new RefByte[rawDataTextBox.Text.Length / 3];
                for (int i = 0; i < rawDataTextBox.Text.Length; i += 3)
                {
                    sf.bytes[i / 3] = byte.Parse(rawDataTextBox.Text.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                }
                sf.ReadData();

                LoadSequence(sender, e);
            }
        }

        private void CopyCommandSequence(object sender, EventArgs e)
        {
            sequenceClipboard = new List<object>();
            foreach (object com in commandsListBox.Items)
            {
                sequenceClipboard.Add(com);
            }
            pasteSequenceButton.Enabled = true;
        }

        private void PasteCommandSequence(object sender, EventArgs e)
        {
            commandsListBox.Items.Clear();
            foreach (object com in sequenceClipboard)
            {
                commandsListBox.Items.Add(com);
            }
        }

        private void AddCommandToSequence(object sender, EventArgs e)
        {
            if (commandsListBox.SelectedIndex != -1)
                commandsListBox.Items.Insert(commandsListBox.SelectedIndex + (addAfterBubble.Checked ? 1 : 0), new ScriptCommand(2, new int[0]));
            if (addAfterBubble.Checked) commandsListBox.SelectedIndex++;
            else if (commandsListBox.SelectedIndex != -1) commandsListBox.SelectedIndex--;
        }

        private void RemoveCommandFromSequence(object sender, EventArgs e)
        {
            //if (commandsListBox.SelectedItem is ScriptCommand c)
            {
                int storeIndex = commandsListBox.SelectedIndex;
                while (commandsListBox.SelectedItems.Count > 0 && commandsListBox.Items.Count > 1)
                {
                    commandsListBox.Items.Remove(commandsListBox.SelectedItem);
                }

                commandsListBox.SelectedIndex = Math.Min(storeIndex, commandsListBox.Items.Count - 1);
            }
        }

        List<Label> parameterLabels = new List<Label>();
        List<NumericUpDown> parameterNumberBoxes = new List<NumericUpDown>();

        private void ChangeCommandDropdown(object sender, EventArgs e)
        {
            foreach (NumericUpDown n in parameterNumberBoxes) Controls.Remove(n);
            parameterNumberBoxes.Clear();

            if (CommandReference.commandList.ContainsKey(commandTypeDropdown.SelectedIndex)) for(int i = 0; i < CommandReference.commandList[commandTypeDropdown.SelectedIndex].numParameters; i++)
                {
                    parameterNumberBoxes.Add(new NumericUpDown() { Location = new Point(440 + 80 * i, 224), Size = new Size(60, 22) });

                    int max = (int)Math.Pow(2, CommandReference.commandList[commandTypeDropdown.SelectedIndex].parameterBytes[i] * 8);
                    parameterNumberBoxes[i].Maximum = 0x90000;
                    parameterNumberBoxes[i].Minimum = -0x90000;

                    if (commandsListBox.SelectedItem is ScriptCommand c && c.parameters.Length > i)
                    {
                        parameterNumberBoxes[i].Value = c.parameters[i];
                    }

                    Controls.Add(parameterNumberBoxes[i]);
                }

            foreach (Label n in parameterLabels) Controls.Remove(n);
            parameterLabels.Clear();

            if (CommandReference.parameters.ContainsKey(commandTypeDropdown.SelectedIndex)) for (int i = 0; i < CommandReference.parameters[commandTypeDropdown.SelectedIndex].Count; i++)
                {
                    parameterLabels.Add(new Label() { Location = new Point(436 + 80 * i, 196), Size = new Size(80, 22) });

                    parameterLabels[i].Text = CommandReference.parameters[commandTypeDropdown.SelectedIndex][i];

                    Controls.Add(parameterLabels[i]);
                }
        }

        private void ApplyCommand(object sender, EventArgs e)
        {
            List<int> parameters = new List<int>();
            foreach (NumericUpDown n in parameterNumberBoxes) parameters.Add((int)n.Value);

            commandsListBox.Items[commandsListBox.SelectedIndex] = new ScriptCommand((short)commandTypeDropdown.SelectedIndex, parameters.ToArray());
        }

        private void addScriptButton_Click(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];

                if (sf.valid && sf.sequences != null && sf.sequences.Count > 0)
                {
                    sf.sequences.Add(new ScriptSequence() { commands = new List<ScriptCommand>() { new ScriptCommand(2, new int[0]) } });

                    LoadScriptFile(sender, e);
                }
            }
        }

        private void locateByteButton_Click(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];
                ScriptSequence seq = sf.sequences[(int)sequenceIDNumberBox.Value];

                int n = HelperFunctions.ReadInt(sf.bytes, (int)sequenceIDNumberBox.Value * 4);
                if (commandsListBox.SelectedIndex != -1)
                {
                    for (int i = 0; i < commandsListBox.SelectedIndex; i++) n += seq.commands[i].ByteLength;
                }
                rawDataTextBox.Select();
                rawDataTextBox.Select((int)(((float)sequenceIDNumberBox.Value * 4 + n + 4) * 3f), 2);
            }
        }

        private void cutsceneIDButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("" +
                "0 - Crash\n" +
                "1 - Scenic boat ride\n" +
                "2 - Big cruise from Castelia\n" +
                "3 - Ferris wheel\n" +
                "4 - ?\n" +
                "5 - ?\n" +
                "6 - ?\n" +
                "7 - ?\n" +
                "8 - ?\n" +
                "9 - Summon Zekrom from orb\n" +
                "10 - Boat from Castelia to Liberty Garden\n" +
                "11 - ?\n" +
                "12 - ?\n" +
                "13 - ?\n" +
                "14 - ?\n" +
                "15 - ?\n" +
                "16 - ?\n" +
                "17 - ?\n" +
                "...");
        }

        bool updateByte = true;
        private void byteNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (updateByte)
            {
                rawDataTextBox.SelectionStart = (int)byteNumberBox.Value * 3;
                rawDataTextBox.ScrollToCaret();
            }
        }

        private void rawDataTextBox_Click(object sender, EventArgs e)
        {
            updateByte = false;
            byteNumberBox.Value = rawDataTextBox.SelectionStart / 3;
            updateByte = true;
        }

        private void ReadScriptFile(object sender, EventArgs e)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "c file|*.c";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                try
                {
                    reader = File.OpenText(prompt.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open file");
                    return;
                }

                if (reader != null)
                {
                    ScriptFile newFile = null;
                    try
                    {
                        newFile = ScriptFile.FromFile(reader);
                    }
                    catch (Exception ex)
                    {
                        reader.Close();
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    reader.Close();
                    MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex] = newFile;
                    LoadScriptFile(sender, e);
                }
            }
        }

        private void ExportScriptFile(object sender, EventArgs e)
        {
            SaveFileDialog prompt = new SaveFileDialog();
            prompt.Filter = "c file|*.c";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                FileStream writer = null;
                try
                {
                    writer = File.OpenWrite(prompt.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open file");
                    return;
                }

                if (writer != null)
                {
                    writer.SetLength(0);
                    MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex].Export(writer);
                    writer.Close();
                    MessageBox.Show("Script file saved to " + prompt.FileName);
                }
            }
        }

        string quickBuildScript = "";
        string quickBuildRom = "";
        int quickBuildID = 0;

        private void setupQuickBuildButton_Click(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a script file before setting up a quick build");
                return;
            }

            OpenFileDialog script = new OpenFileDialog();
            script.Title = "Select a script file to import";
            script.Filter = "C file|*.c";
            if (script.ShowDialog() != DialogResult.OK) return;

            SaveFileDialog rom = new SaveFileDialog();
            rom.Title = "Select where to save your rom";
            rom.Filter = "Nds file|*.nds";
            if (rom.ShowDialog() != DialogResult.OK) return;

            quickBuildScript = script.FileName;
            quickBuildRom = rom.FileName;
            quickBuildID = scriptFileDropdown.SelectedIndex;

            quickBuildLabel.Text = "Import " + quickBuildScript.Substring(quickBuildScript.LastIndexOf("\\") + 1) + " as file " + quickBuildID + ", Save rom to " + quickBuildRom.Substring(quickBuildRom.LastIndexOf("\\") + 1);
            quickBuildButton.Enabled = exportScriptFileButton.Enabled;
        }

        private void quickBuildButton_Click(object sender, EventArgs e)
        {
            StreamReader reader = null;
            try
            {
                reader = File.OpenText(quickBuildScript);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open file");
                return;
            }

            if (reader != null)
            {
                ScriptFile newFile = null;
                try
                {
                    newFile = ScriptFile.FromFile(reader);
                }
                catch (Exception ex)
                {
                    reader.Close();
                    MessageBox.Show(ex.Message);
                    return;
                }

                reader.Close();
                MainEditor.scriptNarc.scriptFiles[quickBuildID] = newFile;
                LoadScriptFile(sender, e);
            }

            FileStream fileStream = null;
            try
            {
                fileStream = File.OpenWrite(quickBuildRom);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save rom");
                return;
            }
            fileStream.SetLength(0);
            byte[] data = MainEditor.fileSystem.BuildRom();
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
            MessageBox.Show("Rom saved to " + quickBuildRom);
        }
    }
}
