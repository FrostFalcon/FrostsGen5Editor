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
    public partial class TextViewer : Form
    {
        TextNARC activeNarc = MainEditor.textNarc;

        public TextViewer()
        {
            InitializeComponent();

            ChangeNarc(null, null);
        }

        private void ChangeNarc(object sender, EventArgs e)
        {
            activeNarc = miscTextRadioButton.Checked ? MainEditor.textNarc : MainEditor.storyTextNarc;

            searchTextBox.Text = "";

            fileNumComboBox.Items.Clear();

            for (int i = 0; i < activeNarc.textFiles.Count; i++) fileNumComboBox.Items.Add(i);
            fileNumComboBox.SelectedIndex = 0;

            LoadTextbox(sender, e);
        }

        private void LoadTextbox(object sender, EventArgs e)
        {
            int fileID;
            if (int.TryParse(fileNumComboBox.Text, out fileID) && fileID >= 0 && activeNarc != null && fileID < activeNarc.textFiles.Count)
            {
                StringBuilder text = new StringBuilder();
                foreach (string str in activeNarc.textFiles[fileID].text) text.Append(str + '\n');
                if (text.Length > 0) text.Remove(text.Length - 1, 1);
                textBoxDisplay.Text = text.ToString().Replace("\\xf000븁\\x0000\\xfffe", "[C]")
                    .Replace("\\xf000븀\\x0000\\xfffe", "[L]")
                    .Replace("\\xfffe", "[N]")
                    .Replace("\\xf000븁\\x0000", "[E]")
                    .Replace("\\xf000Ā\\x0001\\x0000", "[V0]")
                    .Replace("\\xf000Ā\\x0001\\x0001", "[V1]")
                    .Replace("\\xf000Ā\\x0001\\x0002", "[V2]")
                    .Replace("\\xf000Ā\\x0001\\x0003", "[V3]");
                selectedLineNumberBox.Value = 0;
                selectedLineNumberBox.Maximum = textBoxDisplay.Lines.Length;
                lineCountLabel.Text = "/ " + (activeNarc.textFiles[fileID].text.Count - 1);
            }
        }

        private void ApplyTextFile(object sender, EventArgs e)
        {
            int fileID;
            if (int.TryParse(fileNumComboBox.Text, out fileID) && fileID >= 0 && activeNarc != null && fileID < activeNarc.textFiles.Count)
            {
                activeNarc.ApplyTextList(textBoxDisplay, fileID);
            }
        }

        private void FilterFiles(object sender, EventArgs e)
        {
            if (activeNarc != null && fileNumComboBox.SelectedItem != null)
            {
                int currentFile = (int)fileNumComboBox.SelectedItem;
                fileNumComboBox.Items.Clear();

                for (int i = 0; i < activeNarc.textFiles.Count; i++)
                {
                    bool search = false;

                    foreach (string str in activeNarc.textFiles[i].text) if (str.Contains(searchTextBox.Text))
                        {
                            search = true;
                            break;
                        }

                    if (search) fileNumComboBox.Items.Add(i);
                }
                if (fileNumComboBox.Items.Contains(currentFile))
                {
                    fileNumComboBox.SelectedItem = currentFile;
                }

                //Highlight searched text
                int fileID;
                if (int.TryParse(fileNumComboBox.Text, out fileID) && fileID >= 0 && activeNarc != null && fileID < activeNarc.textFiles.Count)
                {
                    for (int i = 0; i < activeNarc.textFiles[fileID].text.Count; i++)
                    {
                        string line = activeNarc.textFiles[fileID].text[i];
                        if (line.Contains(searchTextBox.Text))
                        {
                            selectedLineNumberBox.Value = i;
                        }
                    }
                }
            }

            if (fileNumComboBox.Items.Count <= 0)
            {
                textBoxDisplay.Text = "";
                fileNumComboBox.Items.Add("");
                fileNumComboBox.SelectedIndex = 0;
            }
        }

        private void addLinesButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(fileNumComboBox.Text, out int fileID) && fileID >= 0 && activeNarc != null && fileID < activeNarc.textFiles.Count)
            {
                //Get the text file with the most lines to use as a template
                TextFile template = activeNarc.textFiles[0];
                for (int i = 0; i < activeNarc.textFiles.Count; i++) if (activeNarc.textFiles[i].text.Count > template.text.Count) template = activeNarc.textFiles[i];

                while (activeNarc.textFiles[fileID].text.Count < template.text.Count) activeNarc.textFiles[fileID].text.Add("");

                activeNarc.textFiles[fileID].bytes = PPTxtHandler.SaveEntry(template.bytes, activeNarc.textFiles[fileID].text);

                LoadTextbox(sender, e);
            }
        }

        bool updateLine = true;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (updateLine && textBoxDisplay.GetFirstCharIndexFromLine((int)selectedLineNumberBox.Value) > 0)
            {
                textBoxDisplay.SelectionStart = textBoxDisplay.GetFirstCharIndexFromLine((int)selectedLineNumberBox.Value);
                textBoxDisplay.ScrollToCaret();
            }
        }

        private void textBoxDisplay_Click(object sender, EventArgs e)
        {
            updateLine = false;
            selectedLineNumberBox.Value = textBoxDisplay.GetLineFromCharIndex(textBoxDisplay.SelectionStart);
            updateLine = true;
        }
    }
}
