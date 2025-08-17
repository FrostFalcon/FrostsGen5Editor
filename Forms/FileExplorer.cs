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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Forms
{
    public partial class FileExplorer : Form
    {
        FileNameTable fnt = MainEditor.fileSystem.fnt;
        Dictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();
        Dictionary<TreeNode, string> reverseNodes = new Dictionary<TreeNode, string>();

        public FileExplorer()
        {
            InitializeComponent();

            List<string> files = MainEditor.fileSystem.fnt.fileNames.Values.ToList();
            files.Sort();

            for (int i = 0; i < MainEditor.fileSystem.overlays.Count; i++)
            {
                AddNode("overlays/ov_" + i.ToString("D3"), true);
            }
            foreach (string str in files)
            {
                AddNode(str, true);
            }
        }

        void AddNode(string path, bool leafNode)
        {
            if (path.Contains('/'))
            {
                string sub = path.Substring(0, path.LastIndexOf('/'));
                if (!nodes.ContainsKey(sub)) AddNode(sub, false);

                TreeNode node = new TreeNode(path.Substring(path.LastIndexOf('/') + 1));
                node.ImageIndex = path.Contains("ov_") ? 2 : !leafNode ? 0 : MainEditor.fileSystem.files[fnt.reverseFileNames[path] - fnt.firstFile] is NARC narc && narc.byteData[0] == (byte)'N' ? 1 : 2;
                node.SelectedImageIndex = node.ImageIndex;
                nodes.Add(path, node);
                reverseNodes.Add(node, path);
                nodes[sub].Nodes.Add(node);
            }
            else
            {
                TreeNode node = new TreeNode(path);
                node.ImageIndex = !leafNode ? 0 : MainEditor.fileSystem.files[fnt.reverseFileNames[path] - fnt.firstFile] is NARC narc && narc.byteData[0] == (byte)'N' ? 1 : 2;
                node.SelectedImageIndex = node.ImageIndex;
                nodes.Add(path, node);
                reverseNodes.Add(node, path);
                fileTree.Nodes.Add(node);
            }
        }

        private void fileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (fileTree.SelectedNode != null && fileTree.SelectedNode.Nodes.Count == 0)
            {
                if (fileTree.SelectedNode.Text.Contains("ov_"))
                {
                    int id = int.Parse(fileTree.SelectedNode.Text.Substring(3));
                    var y9 = MainEditor.fileSystem.y9.entries[id];
                    var obj = MainEditor.fileSystem.overlays[id];
                    string text = "Type: Overlay\nID: " + id.ToString("D3");

                    text += "\n\nRAM Address: 0x" + y9.mountAddress.ToString("X");
                    text += "\nRAM Size: 0x" + y9.mountSize.ToString("X");
                    text += "\nCompressed Size: 0x" + y9.compressedSize.ToString("X");
                    text += "\nCompressed: " + y9.compressed;

                    text += "\n\nFile Size: 0x" + obj.Count.ToString("X");

                    fileText.Text = text;
                    compressOverlayButton.Visible = true;
                    unpackNarcButton.Visible = false;
                    packNarcButton.Visible = false;
                    compressOverlayButton.Text = y9.compressed ? "Decompress" : "Compress";
                }
                else
                {
                    var obj = MainEditor.fileSystem.files[fnt.reverseFileNames[reverseNodes[fileTree.SelectedNode]] - fnt.firstFile];

                    if (obj is NARC narc && narc.byteData[0] == (byte)'N')
                    {
                        string text = "Type: NARC\nPath: " + reverseNodes[fileTree.SelectedNode];

                        text += "\n\nSize: 0x" + narc.byteData.Length.ToString("X");
                        text += "\nFile Count: " + narc.numFileEntries;

                        fileText.Text = text;

                        unpackNarcButton.Visible = true;
                        packNarcButton.Visible = true;
                    }
                    else
                    {
                        fileText.Text = "Type: File\nPath: " + reverseNodes[fileTree.SelectedNode];
                        unpackNarcButton.Visible = false;
                        packNarcButton.Visible = false;
                    }
                    compressOverlayButton.Visible = false;
                }
                importButton.Enabled = true;
                extractButton.Enabled = true;
            }
            else if (fileTree.SelectedNode != null)
            {
                fileText.Text = "Type: Folder\nPath: " + reverseNodes[fileTree.SelectedNode];
                importButton.Enabled = false;
                extractButton.Enabled = false;
                compressOverlayButton.Visible = false;
                unpackNarcButton.Visible = false;
                packNarcButton.Visible = false;
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            if (fileTree.SelectedNode != null && fileTree.SelectedNode.Nodes.Count == 0)
            {
                if (fileTree.SelectedNode.Text.Contains("ov_"))
                {
                    int id = int.Parse(fileTree.SelectedNode.Text.Substring(3));

                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = "ov_" + id.ToString("D3") + ".bin";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(save.FileName, MainEditor.fileSystem.overlays[id].ToArray());
                        statusText.Text = "Extracted Overlay " + id + " - " + DateTime.Now.StatusText();
                    }
                }
                else
                {
                    var obj = MainEditor.fileSystem.files[fnt.reverseFileNames[reverseNodes[fileTree.SelectedNode]] - fnt.firstFile];

                    if (obj is NARC narc)
                    {
                        SaveFileDialog save = new SaveFileDialog();
                        save.FileName = fileTree.SelectedNode.Text;
                        if (save.ShowDialog() == DialogResult.OK)
                        {
                            narc.WriteData();
                            File.WriteAllBytes(save.FileName, narc.byteData);

                            statusText.Text = "Extracted narc " + reverseNodes[fileTree.SelectedNode] + " - " + DateTime.Now.StatusText();
                        }
                    }
                    else if (obj is byte[] b)
                    {
                        SaveFileDialog save = new SaveFileDialog();
                        save.FileName = fileTree.SelectedNode.Text;
                        if (save.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(save.FileName, b);
                            statusText.Text = "Extracted file " + fileTree.SelectedNode.Text + " - " + DateTime.Now.StatusText();
                        }
                    }
                }
            }
        }

        private void compressOverlayButton_Click(object sender, EventArgs e)
        {
            if (fileTree.SelectedNode != null && fileTree.SelectedNode.Nodes.Count == 0 && fileTree.SelectedNode.Text.Contains("ov_"))
            {
                int id = int.Parse(fileTree.SelectedNode.Text.Substring(3));
                var y9 = MainEditor.fileSystem.y9.entries[id];
                var obj = MainEditor.fileSystem.overlays[id];

                if (y9.compressed)
                {
                    MainEditor.fileSystem.overlays[id] = new List<byte>(BLZDecoder.BLZ_DecodePub(obj.ToArray()));
                    y9.compressed = false;
                    y9.compressedSize = 0;
                    y9.Apply();
                    statusText.Text = "Decompressed Overlay " + id + " - " + DateTime.Now.StatusText();
                }
                else
                {
                    MainEditor.fileSystem.overlays[id] = new List<byte>(BLZDecoder.BLZ_EncodePub(obj.ToArray(), true));
                    y9.compressed = true;
                    y9.compressedSize = MainEditor.fileSystem.overlays[id].Count;
                    y9.Apply();
                    statusText.Text = "Compressed Overlay " + id + " - " + DateTime.Now.StatusText();
                }

                fileTree_AfterSelect(null, null);
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            if (fileTree.SelectedNode != null && fileTree.SelectedNode.Nodes.Count == 0)
            {
                if (fileTree.SelectedNode.Text.Contains("ov_"))
                {
                    int id = int.Parse(fileTree.SelectedNode.Text.Substring(3));
                    var y9 = MainEditor.fileSystem.y9.entries[id];

                    OpenFileDialog open = new OpenFileDialog();
                    open.FileName = "ov_" + id.ToString("D3") + ".bin";

                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        byte[] b = File.ReadAllBytes(open.FileName);
                        if (y9.compressed)
                            y9.compressedSize = b.Length;
                        else
                            y9.mountSize = b.Length;
                        y9.Apply();

                        MainEditor.fileSystem.overlays[id] = b.ToList();
                        statusText.Text = "Imported Overlay " + id + " - " + DateTime.Now.StatusText();
                    }
                }
                else
                {
                    var obj = MainEditor.fileSystem.files[fnt.reverseFileNames[reverseNodes[fileTree.SelectedNode]] - fnt.firstFile];

                    if (obj is NARC narc)
                    {
                        OpenFileDialog open = new OpenFileDialog();
                        open.FileName = fileTree.SelectedNode.Text;
                        if (open.ShowDialog() == DialogResult.OK)
                        {
                            byte[] b = File.ReadAllBytes(open.FileName);
                            narc.byteData = b;
                            narc.ReadData();

                            statusText.Text = "Imported narc " + reverseNodes[fileTree.SelectedNode] + " - " + DateTime.Now.StatusText();
                        }
                    }
                    else if (obj is byte[] b)
                    {
                        OpenFileDialog open = new OpenFileDialog();
                        open.FileName = fileTree.SelectedNode.Text;
                        if (open.ShowDialog() == DialogResult.OK)
                        {
                            MainEditor.fileSystem.files[fnt.reverseFileNames[reverseNodes[fileTree.SelectedNode]] - fnt.firstFile] = File.ReadAllBytes(open.FileName);
                            statusText.Text = "Imported file " + fileTree.SelectedNode.Text + " - " + DateTime.Now.StatusText();
                        }
                    }
                }
            }
        }

        private void unpackNarcButton_Click(object sender, EventArgs e)
        {
            if (fileTree.SelectedNode != null && fileTree.SelectedNode.Nodes.Count == 0 &&
                MainEditor.fileSystem.files[fnt.reverseFileNames[reverseNodes[fileTree.SelectedNode]] - fnt.firstFile] is NARC narc && narc.byteData[0] == (byte)'N')
            {
                FolderBrowserDialog prompt = new FolderBrowserDialog();
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text))
                        Directory.CreateDirectory(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text);

                    if (narc is ScriptNARC sn)
                    {
                        if (MessageBox.Show("Would you like to extract the decompiled script files instead of binary files?", "Extract Scripts", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            for (int i = 0; i < sn.scriptFiles.Count; i++)
                            {
                                if (!sn.scriptFiles[i].valid) continue;
                                List<string> headers = new List<string>()
                                {
                                    MainEditor.RomType == RomType.BW1 ? "ScriptHeaders/ScriptCommandsBW1.h" : "ScriptHeaders/FrostScriptCommandsBW2.h",
                                    "ScriptHeaders/MovementCommands.h"
                                };
                                int ow = MainEditor.zoneDataNarc.zones.FindIndex(z => z.scriptFile == i);
                                CommandReference.commandList = new Dictionary<int, Data.NARCTypes.CommandType>(MainEditor.RomType == RomType.BW1 ? CommandReference.bw1CommandList : CommandReference.bw2CommandList);
                                if (OverworldEditor.overlayZones.ContainsKey(ow))
                                {
                                    headers.Add("ScriptHeaders/CommandOverlay" + OverworldEditor.overlayZones[ow] + ".h");
                                    foreach (var cmd in CommandReference.bw2OverlayCommands[OverworldEditor.overlayZones[ow]])
                                        CommandReference.commandList.Add(cmd.Key, cmd.Value);
                                }

                                FileStream fs = File.OpenWrite(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text + "/" + i + ".c");
                                fs.SetLength(0);
                                try
                                {
                                    sn.scriptFiles[i].Export(fs, headers.ToArray());
                                }
                                catch
                                {

                                }
                                fs.Close();
                            }

                            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\ScriptHeaders"))
                            {
                                if (!Directory.Exists(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text + "\\ScriptHeaders")) Directory.CreateDirectory(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text + "\\ScriptHeaders");
                                foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\ScriptHeaders"))
                                {
                                    File.Copy(file, prompt.SelectedPath + "/" + fileTree.SelectedNode.Text + "\\ScriptHeaders\\" + Path.GetFileName(file), true);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < narc.numFileEntries; i++)
                            {
                                byte[] b = narc.GetFileEntry(i).ToArray();
                                File.WriteAllBytes(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text + "/" + i + ".bin", b);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < narc.numFileEntries; i++)
                        {
                            byte[] b = narc.GetFileEntry(i).ToArray();
                            File.WriteAllBytes(prompt.SelectedPath + "/" + fileTree.SelectedNode.Text + "/" + i + ".bin", b);
                        }
                    }
                    statusText.Text = "Unpacked narc to " + prompt.SelectedPath + "\\" + fileTree.SelectedNode.Text + " - " + DateTime.Now.StatusText();
                    MessageBox.Show("Unpacked narc to" + prompt.SelectedPath + "\\" + fileTree.SelectedNode.Text);
                }
            }
        }

        private void packNarcButton_Click(object sender, EventArgs e)
        {
            if (fileTree.SelectedNode != null && fileTree.SelectedNode.Nodes.Count == 0 &&
                MainEditor.fileSystem.files[fnt.reverseFileNames[reverseNodes[fileTree.SelectedNode]] - fnt.firstFile] is NARC narc && narc.byteData[0] == (byte)'N')
            {
                FolderBrowserDialog prompt = new FolderBrowserDialog();
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(prompt.SelectedPath))
                        Directory.CreateDirectory(prompt.SelectedPath);
                    List<string> fileNames = Directory.GetFiles(prompt.SelectedPath).ToList();
                    foreach (string s in fileNames)
                    {
                        if (!int.TryParse(s.Substring(s.LastIndexOf("\\") + 1, s.LastIndexOf(".") - s.LastIndexOf("\\") - 1), out int i1))
                        {
                            MessageBox.Show("Unable to process files in the provided folder.\nFiles must be named numerically.");
                            return;
                        }
                    }
                    fileNames.Sort((s1, s2) =>
                    {
                        if (int.TryParse(s1.Substring(s1.LastIndexOf("\\") + 1, s1.LastIndexOf(".") - s1.LastIndexOf("\\") - 1), out int i1) &&
                            int.TryParse(s2.Substring(s2.LastIndexOf("\\") + 1, s2.LastIndexOf(".") - s2.LastIndexOf("\\") - 1), out int i2))
                        {
                            return i1 - i2;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    for (int i = 0; i < fileNames.Count; i++)
                    {
                        narc.AddFileEntry(i, File.ReadAllBytes(fileNames[i]).ToList());
                    }
                    narc.ReadData();
                    statusText.Text = "Imported narc " + reverseNodes[fileTree.SelectedNode] + " - " + DateTime.Now.StatusText();
                    MessageBox.Show("Successfully packed Narc file");
                }
            }
        }
    }
}
