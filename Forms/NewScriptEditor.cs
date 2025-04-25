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
    public partial class NewScriptEditor : Form
    {
        public List<object> sequenceClipboard;

        public NewScriptEditor()
        {
            InitializeComponent();

            for (int i = 0; i < MainEditor.scriptNarc.scriptFiles.Count; i++) scriptFileDropdown.Items.Add(i);

            if (MainEditor.RomType == RomType.BW1)
            {
                commandNameSelection2.Checked = true;
                commandNameSelection1.Enabled = false;
            }

            List<byte> names = FileFunctions.ReadFileSection("Preferences.txt", "CommandNames");
            if (names != null)
            {
                if (Encoding.ASCII.GetString(names.ToArray()) == "Pokescript")
                {
                    commandNameSelection2.Checked = true;
                }
            }

            loading = false;
        }

        private void LoadScriptFile(object sender, EventArgs e)
        {
            if (scriptFileDropdown.SelectedIndex >= 0 && scriptFileDropdown.SelectedIndex < scriptFileDropdown.Items.Count)
            {
                ScriptFile sf = MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex];

                if (sf.valid && sf.sequences != null && sf.sequences.Count > 0)
                {
                    rawDataTextBox.Enabled = true;
                    applyRawDataButton.Enabled = true;
                    readScriptFileButton.Enabled = true;
                    exportScriptFileButton.Enabled = true;
                    quickBuildButton.Enabled = quickBuildRom != "";
                }
                else
                {
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
            }
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
            CommandReference.commandList = new Dictionary<int, Data.NARCTypes.CommandType>(MainEditor.RomType == RomType.BW1 ? CommandReference.bw1CommandList :
                commandNameSelection2.Checked ? CommandReference.bw2PokeScriptCommandList : CommandReference.bw2CommandList);
            if (loadedOverlayDropdown.SelectedIndex > 0 && int.TryParse((string)loadedOverlayDropdown.SelectedItem, out int ov))
            {
                foreach (var cmd in CommandReference.bw2OverlayCommands[ov])
                    CommandReference.commandList.Add(cmd.Key, cmd.Value);
            }

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
            CommandReference.commandList = new Dictionary<int, Data.NARCTypes.CommandType>(MainEditor.RomType == RomType.BW1 ? CommandReference.bw1CommandList :
                commandNameSelection2.Checked ? CommandReference.bw2PokeScriptCommandList : CommandReference.bw2CommandList);
            if (loadedOverlayDropdown.SelectedIndex > 0 && int.TryParse((string)loadedOverlayDropdown.SelectedItem, out int ov))
            {
                foreach (var cmd in CommandReference.bw2OverlayCommands[ov])
                    CommandReference.commandList.Add(cmd.Key, cmd.Value);
            }

            MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex].ReadData();

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
                    List<string> headers = new List<string>()
                    {
                        MainEditor.RomType == RomType.BW1 ? "ScriptHeaders/ScriptCommandsBW1.h" :
                        commandNameSelection2.Checked ? "ScriptHeaders/PokeScriptCommandsBW2.h" : "ScriptHeaders/FrostScriptCommandsBW2.h",
                        "ScriptHeaders/MovementCommands.h"
                    };
                    if (loadedOverlayDropdown.SelectedIndex > 0 && int.TryParse((string)loadedOverlayDropdown.SelectedItem, out int ov2))
                    {
                        headers.Add("ScriptHeaders/CommandOverlay" + ov2 + ".h");
                    }

                    try
                    {
                        MainEditor.scriptNarc.scriptFiles[scriptFileDropdown.SelectedIndex].Export(writer, headers.ToArray());
                    }
                    catch
                    {
                        MessageBox.Show("An error has occured while exporting the file.\nThis may be caused by the required overlay commands not being loaded.");
                        writer.Close();
                        return;
                    }
                    writer.Close();
                }

                string root = Path.GetDirectoryName(prompt.FileName);
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\ScriptHeaders"))
                {
                    if (!Directory.Exists(root + "\\ScriptHeaders")) Directory.CreateDirectory(root + "\\ScriptHeaders");
                    foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\ScriptHeaders"))
                    {
                        File.Copy(file, root + "\\ScriptHeaders\\" + Path.GetFileName(file), true);
                    }
                }
                MessageBox.Show("Script file saved to " + prompt.FileName);
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

            quickBuildLabel.Text = "Import " + quickBuildScript.Substring(quickBuildScript.LastIndexOf("\\") + 1) + " as file " + quickBuildID + ",\nSave rom to " + quickBuildRom.Substring(quickBuildRom.LastIndexOf("\\") + 1);
            quickBuildButton.Enabled = exportScriptFileButton.Enabled;
        }

        private void quickBuildButton_Click(object sender, EventArgs e)
        {
            CommandReference.commandList = new Dictionary<int, Data.NARCTypes.CommandType>(MainEditor.RomType == RomType.BW1 ? CommandReference.bw1CommandList :
                commandNameSelection2.Checked ? CommandReference.bw2PokeScriptCommandList : CommandReference.bw2CommandList);
            if (loadedOverlayDropdown.SelectedIndex > 0 && int.TryParse((string)loadedOverlayDropdown.SelectedItem, out int ov))
            {
                foreach (var cmd in CommandReference.bw2OverlayCommands[ov])
                    CommandReference.commandList.Add(cmd.Key, cmd.Value);
            }

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

        bool loading = true;

        private void commandNameSelection1_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
                FileFunctions.WriteFileSection("Preferences.txt", "CommandNames", ASCIIEncoding.ASCII.GetBytes("Frosts"));
        }

        private void commandNameSelection2_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
                FileFunctions.WriteFileSection("Preferences.txt", "CommandNames", ASCIIEncoding.ASCII.GetBytes("Pokescript"));
        }
    }
}
