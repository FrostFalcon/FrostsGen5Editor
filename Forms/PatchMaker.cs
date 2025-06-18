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
    public partial class PatchMaker : Form
    {
        Dictionary<string, IEnumerable<byte>> input;
        Dictionary<string, List<string>> entries = new Dictionary<string, List<string>>();
        Dictionary<string, int> sizes = new Dictionary<string, int>();
        Dictionary<string, List<string>> details = new Dictionary<string, List<string>>();

        public PatchMaker(Dictionary<string, IEnumerable<byte>> input)
        {
            InitializeComponent();

            this.input = input;
            foreach (var v in input)
            {
                if (v.Key.Contains("arm9"))
                {
                    entries.Add("arm9", new List<string>() { v.Key });
                    sizes.Add("arm9", v.Value.Count());
                    details.Add("arm9", new List<string>());

                    int pos = 1;
                    byte[] b = v.Value.ToArray();
                    while (pos < v.Value.Count())
                    {
                        details["arm9"].Add("Edited " + HelperFunctions.ReadInt(b, pos + 4) + " bytes at 0x" + HelperFunctions.ReadInt(b, pos).ToString("X"));
                        pos += 8 + HelperFunctions.ReadInt(b, pos + 4);
                    }
                }
                if (v.Key.Contains("y9"))
                {
                    entries.Add("y9", new List<string>() { v.Key });
                    sizes.Add("y9", v.Value.Count());
                    details.Add("y9", new List<string>());

                    byte[] b = v.Value.ToArray();
                    if (HelperFunctions.ReadInt(b, 0) > 0) details["y9"].Add("Removed " + (HelperFunctions.ReadInt(b, 0) / 32) + " entries");
                    if (HelperFunctions.ReadInt(b, 4) > 0) details["y9"].Add("Added " + (HelperFunctions.ReadInt(b, 4) / 32) + " entries");
                    int pos = 8 + HelperFunctions.ReadInt(b, 4);
                    while (pos < b.Length)
                    {
                        details["y9"].Add("Edited overlay entry " + HelperFunctions.ReadInt(b, pos));
                        pos += 32;
                    }
                }
                if (v.Key.Contains("ov_"))
                {
                    string key = "Overlay " + v.Key.Split('_')[1];
                    entries.Add(key, new List<string>() { v.Key });
                    sizes.Add(key, v.Value.Count());
                    details.Add(key, new List<string>());

                    if (v.Key.Contains("add"))
                    {
                        details[key].Add("Added overlay");
                    }
                    else if (v.Key.Contains("rem"))
                    {
                        details[key].Add("Removed overlay");
                    }
                    else
                    {
                        byte[] b = v.Value.ToArray();
                        int start = HelperFunctions.ReadInt(b, 0);
                        int size = HelperFunctions.ReadInt(b, 4);
                        int end = start + size;
                        int oldStart = HelperFunctions.ReadInt(b, 8);
                        int oldSize = HelperFunctions.ReadInt(b, 12);
                        int oldEnd = oldStart + oldSize;
                        if (oldStart < start)
                            details[key].Add("Removed " + (start - oldStart) + " bytes at the start");
                        if (oldEnd > end)
                            details[key].Add("Removed " + (oldEnd - end) + " bytes at the end");
                        int pos = 12;
                        if (start < oldStart)
                            details[key].Add("Added " + (oldStart - start) + " bytes to the start");
                        if (end > oldEnd)
                            details[key].Add("Added " + (end - oldEnd) + " bytes to the end");
                        pos = 16;
                        while (pos < b.Count())
                        {
                            details[key].Add("Edited " + HelperFunctions.ReadInt(b, pos + 4) + " bytes at 0x" + HelperFunctions.ReadInt(b, pos).ToString("X"));
                            pos += 8 + HelperFunctions.ReadInt(b, pos + 4);
                        }
                    }
                }
                if (v.Key.Contains("narc"))
                {
                    string key = "Narc " + v.Key.Split('_')[1];
                    if (entries.ContainsKey(key))
                    {
                        entries[key].Add(v.Key);
                        sizes[key] += v.Value.Count();

                        if (v.Key.Contains("full"))
                        {
                            details[key].Add("Full narc edit");
                        }
                        else if (v.Key.Contains("add"))
                        {
                            details[key].Add("Added file " + v.Key.Split('_')[2]);
                        }
                        else if (v.Key.Contains("rem"))
                        {
                            details[key].Add("Removed file " + v.Key.Split('_')[2]);
                        }
                        else
                        {
                            details[key].Add("Edited file " + v.Key.Split('_')[2]);
                        }
                    }
                    else
                    {
                        entries.Add(key, new List<string>() { v.Key });
                        sizes.Add(key, v.Value.Count());
                        details.Add(key, new List<string>());

                        if (v.Key.Contains("full"))
                        {
                            details[key].Add("Full narc edit");
                        }
                        else if (v.Key.Contains("add"))
                        {
                            details[key].Add("Added file " + v.Key.Split('_')[2]);
                        }
                        else if (v.Key.Contains("rem"))
                        {
                            details[key].Add("Removed file " + v.Key.Split('_')[2]);
                        }
                        else
                        {
                            details[key].Add("Edited file " + v.Key.Split('_')[2]);
                        }
                    }
                }
            }

            foreach (var entry in entries)
            {
                changesListBox.Items.Add(entry.Key);
                changesListBox.SetItemChecked(changesListBox.Items.Count - 1, true);
            }
        }

        private void changesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = changesListBox.SelectedItem.ToString();
            int size = sizes[selected];
            string text = selected + " patch:\n" + size + " bytes";
            size /= 1024;
            if (size < 1) text += " (<1 KB)\n";
            else text += " (" + size + " KB)\n";
            sizeLabel.Text = text;

            text = "";
            int i = 0;
            foreach (string str in details[selected])
            {
                text += "\n" + str;
                i++;
            }
            if (text.Length > 0)
                text = text.Substring(1, text.Length - 1);
            detailsTextBox.Text = text;
        }

        private void savePatchButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Gen 5 Patch File|*.Vpatch";
            save.Title = "Select where to save the patch file";

            if (save.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < changesListBox.Items.Count; i++)
                {
                    if (!changesListBox.GetItemChecked(i))
                    {
                        string item = changesListBox.Items[i].ToString();
                        foreach (string str in entries[item]) input.Remove(str);
                    }
                }

                FileFunctions.WriteAllSections(save.FileName, input, true);

                MessageBox.Show("Patch Created");
                Close();
            }
        }
    }
}
