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
    public partial class OverlayEditor : Form
    {
        public NDSFileSystem fileSystem;

        public OverlayEditor(NDSFileSystem fileSystem)
        {
            InitializeComponent();
            this.fileSystem = fileSystem;

            fileNumComboBox.Items.Add("Arm9");
            for (int i = 0; i < fileSystem.overlays.Count; i++) fileNumComboBox.Items.Add(i);
        }

        private void LoadTextbox(object sender, EventArgs e)
        {
            
        }

        private void ApplyOverlay(object sender, EventArgs e)
        {
            
        }

        private void DecompressOverlay(object sender, EventArgs e)
        {
            if (fileNumComboBox.SelectedIndex == -1) return;

            List<byte> bytes;
            if (fileNumComboBox.SelectedIndex == 0) bytes = fileSystem.arm9;
            else bytes = fileSystem.overlays[fileNumComboBox.SelectedIndex - 1];

            byte[] bytearr = BLZDecoder.BLZ_DecodePub(bytes.ToArray());

            string str = "";
            for (int i = 0; i < bytearr.Length; i++)
            {
                str += bytearr[i].ToString("X2") + " ";
            }
            textBoxDisplay.Text = str;
        }

        bool updateLine = true;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (updateLine && textBoxDisplay.GetFirstCharIndexFromLine((int)selectedLineNumberBox.Value) > 0)
            {
                textBoxDisplay.SelectionStart = (int)selectedLineNumberBox.Value / 3;
                textBoxDisplay.ScrollToCaret();
            }
        }

        private void textBoxDisplay_Click(object sender, EventArgs e)
        {
            updateLine = false;
            selectedLineNumberBox.Value = textBoxDisplay.SelectionStart / 3;
            updateLine = true;
        }
    }
}
