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
    public partial class ExpCurveEditor : Form
    {
        static List<TextValue> levelRates = new List<TextValue>()
        {
            new TextValue(1, "Erratic"),
            new TextValue(4, "Fast"),
            new TextValue(0, "Medium Fast"),
            new TextValue(3, "Medium Slow"),
            new TextValue(5, "Slow"),
            new TextValue(2, "Fluctuating"),
        };

        public ExpCurveEditor()
        {
            InitializeComponent();

            for (int i = 1; i <= 100; i++)
            {
                xpRateDataGrid.Columns.Add("Level" + i, i.ToString());
                xpRateDataGrid.Columns[i - 1].Width = 64;
                xpRateDataGrid.Columns[i - 1].Resizable = DataGridViewTriState.False;
            }
            xpRateDataGrid.RowHeadersWidth = 96;
            xpRateDataGrid.Rows.Add(1);
            xpRateDataGrid.Rows[0].ReadOnly = false;
            xpRateDataGrid.Rows[0].HeaderCell.Value = "To Next";
            xpRateDataGrid.Rows[0].Resizable = DataGridViewTriState.False;
            xpRateDataGrid.Rows[1].ReadOnly = true;
            xpRateDataGrid.Rows[1].HeaderCell.Value = "Total";
            xpRateDataGrid.Rows[1].Resizable = DataGridViewTriState.False;
            xpRateDataGrid.Rows[1].DefaultCellStyle.BackColor = Color.LightGray;

            curveIDDropdown.Items.AddRange(levelRates.ToArray());
            curveIDDropdown.SelectedIndex = 3;
        }

        private void shopIDDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            xpRateChart.Series[0].Points.Clear();
            for (int i = 1; i <= 100; i++)
            {
                int xp = MainEditor.xpCurveNarc.curves[((TextValue)curveIDDropdown.SelectedItem).hexID].GetXPAtLevel(i);
                xpRateChart.Series[0].Points.AddXY(i, xp);
                xpRateChart.Series[0].BorderWidth = 2;

                xpRateDataGrid[i - 1, 0].Value = i == 100 ? 0 : MainEditor.xpCurveNarc.curves[((TextValue)curveIDDropdown.SelectedItem).hexID].GetXPAtLevel(i + 1) - xp;
                xpRateDataGrid[i - 1, 1].Value = xp;
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (!int.TryParse(xpRateDataGrid[i - 1, 0].Value.ToString(), out int n) || n < 0)
                {
                    MessageBox.Show("Input at level " + i + " is not a valid integer");
                    return;
                }
            }

            int total = 0;
            for (int i = 1; i <= 100; i++)
            {
                MainEditor.xpCurveNarc.curves[((TextValue)curveIDDropdown.SelectedItem).hexID].SetXPAtLevel(i, total);
                total += int.Parse(xpRateDataGrid[i - 1, 0].Value.ToString());
            }

            xpRateChart.Series[0].Points.Clear();
            for (int i = 1; i <= 100; i++)
            {
                int xp = MainEditor.xpCurveNarc.curves[((TextValue)curveIDDropdown.SelectedItem).hexID].GetXPAtLevel(i);
                xpRateChart.Series[0].Points.AddXY(i, xp);
                xpRateChart.Series[0].BorderWidth = 2;

                xpRateDataGrid[i - 1, 0].Value = i == 100 ? 0 : MainEditor.xpCurveNarc.curves[((TextValue)curveIDDropdown.SelectedItem).hexID].GetXPAtLevel(i + 1) - xp;
                xpRateDataGrid[i - 1, 1].Value = xp;
            }
        }
    }
}
