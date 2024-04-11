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
    public partial class TypeChartEditor : Form
    {
        private int tableLocation = 0x3DC40;

        ComboBox[,] table;

        List<string> names = new List<string>()
        {
            "Norm",
            "Gras",
            "Fire",
            "Watr",
            "Elec",
            "Ice",
            "Pois",
            "Bug",
            "Rock",
            "Grnd",
            "Fly",
            "Fght",
            "Psy",
            "Dark",
            "Ghst",
            "Drgn",
            "Steel",
        };

        List<int> reorder = new List<int>()
        {
            0, 11, 9, 10, 12, 14, 3, 6, 5, 4, 2, 1, 13, 16, 7, 15, 8
        };

        public TypeChartEditor()
        {
            InitializeComponent();

            table = new ComboBox[17, 17];
            SuspendLayout();

            for (int x = 0; x < 17; x++)
            {
                Controls.Add(new Label()
                {
                    Text = names[x],
                    Location = new Point(160 + 44 * x, 90),
                    Size = new Size(40, 20)
                });
                Controls.Add(new Label()
                {
                    Text = names[x],
                    Location = new Point(120, 124 + 28 * x),
                    Size = new Size(40, 20)
                });
            }

            for (int x = 0; x < 17; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    table[y, x] = new ComboBox()
                    {
                        Location = new Point(160 + 44 * x, 120 + 28 * y),
                        Size = new Size(40, 24)
                    };
                    table[y, x].Items.Add(0);
                    table[y, x].Items.Add(0.5);
                    table[y, x].Items.Add(1);
                    table[y, x].Items.Add(2);
                    Controls.Add(table[y, x]);

                    byte b = MainEditor.fileSystem.overlays[167][tableLocation + reorder[y] * 17 + reorder[x]];
                    int index = b == 0 ? 0 : (int)Math.Log(b, 2);
                    table[y, x].SelectedIndex = Math.Min(3, index);
                }
            }
            ResumeLayout();
            tableAddressNumberBox.Value = tableLocation;
        }

        private void recalculateAddressButton_Click(object sender, EventArgs e)
        {
            tableLocation = (int)tableAddressNumberBox.Value;
            for (int x = 0; x < 17; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    byte b = MainEditor.fileSystem.overlays[167][tableLocation + reorder[y] * 17 + reorder[x]];
                    int index = b == 0 ? 0 : (int)Math.Log(b, 2);
                    table[y, x].SelectedIndex = index;
                }
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 17; x++)
            {
                for (int y = 0; y < 17; y++)
                {
                    MainEditor.fileSystem.overlays[167][tableLocation + reorder[y] * 17 + reorder[x]] = (byte)(table[y, x].SelectedIndex == 0 ? 0 : Math.Pow(2, table[y, x].SelectedIndex));
                }
            }
        }
    }
}
