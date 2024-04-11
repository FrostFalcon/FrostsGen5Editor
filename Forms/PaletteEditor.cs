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
    public partial class PaletteEditor : Form
    {
        PokemonSpriteEntry sprite;

        PictureBox[] colorBoxes;
        NumericUpDown[,] colorValueNumberBoxes;
        int spriteID;

        public PaletteEditor(int spriteID)
        {
            this.spriteID = spriteID;
            InitializeComponent();
            Setup(spriteID);
        }

        void Setup(int spriteID)
        {
            sprite = MainEditor.pokemonSpritesNarc.sprites[spriteID];
            pokemonSpriteBox.Image = sprite.GetFrontSprite(shinyCheckBox.Checked);
            backSpriteBox.Image = sprite.GetBackSprite(shinyCheckBox.Checked);
            Color[] pal = shinyCheckBox.Checked ? sprite.shinyPalette : sprite.palette;
            Debug.WriteLine(pal[0]);

            if (colorBoxes == null)
            {
                colorBoxes = new PictureBox[16];
                colorValueNumberBoxes = new NumericUpDown[16, 3];
                for (int i = 0; i < 16; i++)
                {
                    colorBoxes[i] = new PictureBox()
                    {
                        Location = new Point(400, 20 + 40 * i),
                        Size = new Size(32, 32),
                        BackColor = pal[i]
                    };
                    Controls.Add(colorBoxes[i]);

                    for (int j = 0; j < 3; j++)
                    {
                        int num = i;

                        colorValueNumberBoxes[i, j] = new NumericUpDown()
                        {
                            Location = new Point(440 + 80 * j, 24 + 40 * i),
                            Size = new Size(60, 24),
                            Maximum = 255,
                            Value = j == 0 ? pal[i].R : j == 1 ? pal[i].G : pal[i].B,
                        };
                        colorValueNumberBoxes[i, j].ValueChanged += (o, e) =>
                        {
                            colorBoxes[num].BackColor = Color.FromArgb((byte)colorValueNumberBoxes[num, 0].Value, (byte)colorValueNumberBoxes[num, 1].Value, (byte)colorValueNumberBoxes[num, 2].Value);
                        };
                        Controls.Add(colorValueNumberBoxes[i, j]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    colorBoxes[i].BackColor = pal[i];

                    for (int j = 0; j < 3; j++)
                    {
                        int num = i;

                        colorValueNumberBoxes[i, j].Value = j == 0 ? pal[i].R : j == 1 ? pal[i].G : pal[i].B;
                    }
                }
            }
        }

        private void applyPaletteButton_Click(object sender, EventArgs e)
        {
            Color[] pal = shinyCheckBox.Checked ? sprite.shinyPalette : sprite.palette;
            for (int i = 0; i < 16; i++)
            {
                pal[i] = colorBoxes[i].BackColor;
            }
            sprite.ApplyData();
            pokemonSpriteBox.Image = sprite.GetFrontSprite(shinyCheckBox.Checked);
            backSpriteBox.Image = sprite.GetBackSprite(shinyCheckBox.Checked);
        }

        private void shinyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Setup(spriteID);
        }
    }
}
