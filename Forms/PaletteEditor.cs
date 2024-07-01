using NewEditor.Data.NARCTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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

        int paletteArrayOffset = 0x8C54C;

        public PaletteEditor(int spriteID)
        {
            this.spriteID = spriteID;
            InitializeComponent();

            List<byte> arm = MainEditor.fileSystem.arm9;
            if (MainEditor.RomType != RomType.BW2 || arm[paletteArrayOffset] != 0 || !(arm[paletteArrayOffset + 1] == 0 || arm[paletteArrayOffset + 1] == 17 || arm[paletteArrayOffset + 1] == 34) ||
                !(arm[paletteArrayOffset + 2] == 0 || arm[paletteArrayOffset + 2] == 17 || arm[paletteArrayOffset + 2] == 34) ||
                !(arm[paletteArrayOffset + 3] == 0 || arm[paletteArrayOffset + 3] == 17 || arm[paletteArrayOffset + 3] == 34)) paletteArrayOffset = 0;
            else
            {
                int value = arm[paletteArrayOffset + spriteID];
                paletteIDNumberBox.Value = value & 0b_1111;
            }

            sprite = MainEditor.pokemonSpritesNarc.sprites[spriteID];
            if (MainEditor.pokemonSpritesNarc.sprites[spriteID].FrontFemaleSpriteBytes.Length == 0) imageTypeDropdown.Items.AddRange(new string[]
            {
                "Front Sprite",
                "Front Rig",
                "Back Sprite",
                "Back Rig",
            });
            else imageTypeDropdown.Items.AddRange(new string[]
            {
                "Front Male Sprite",
                "Front Female Sprite",
                "Front Male Rig",
                "Front Female Rig",
                "Back Male Sprite",
                "Back Female Sprite",
                "Back Male Rig",
                "Back Female Rig",
            });
            imageTypeDropdown.SelectedIndex = 0;
            iconTypeDropdown.SelectedIndex = 0;
            fileIDDropdown.SelectedIndex = 0;

            Setup(spriteID);
        }

        void Setup(int spriteID)
        {
            sprite = MainEditor.pokemonSpritesNarc.sprites[spriteID];

            string type = (string)imageTypeDropdown.SelectedItem;
            if (type == "Front Sprite" || type == "Front Male Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked);
            else if (type == "Front Female Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, false, true);
            else if (type == "Front Rig" || type == "Front Male Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked);
            else if (type == "Front Female Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, false, true);
            else if (type == "Back Sprite" || type == "Back Male Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, true);
            else if (type == "Back Female Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, true, true);
            else if (type == "Back Rig" || type == "Back Male Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, true);
            else if (type == "Back Female Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, true, true);

            Color[] pal = shinyCheckBox.Checked ? sprite.shinyPalette : sprite.palette;

            if (colorBoxes == null)
            {
                colorBoxes = new PictureBox[16];
                colorValueNumberBoxes = new NumericUpDown[16, 3];
                for (int i = 0; i < 16; i++)
                {
                    int iPos = i == 0 ? 0 : i + 2;
                    colorBoxes[i] = new PictureBox()
                    {
                        Location = new Point(20 + 320 * (iPos % 3), 420 + 50 * (iPos / 3)),
                        Size = new Size(32, 32),
                        BackColor = pal[i]
                    };
                    Controls.Add(colorBoxes[i]);

                    for (int j = 0; j < 3; j++)
                    {
                        int num = i;

                        colorValueNumberBoxes[i, j] = new NumericUpDown()
                        {
                            Location = new Point(68 + 320 * (iPos % 3) + 80 * j, 424 + 50 * (iPos / 3)),
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
            string type = (string)imageTypeDropdown.SelectedItem;
            if (type == "Front Sprite" || type == "Front Male Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked);
            else if (type == "Front Female Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, false, true);
            else if (type == "Front Rig" || type == "Front Male Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked);
            else if (type == "Front Female Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, false, true);
            else if (type == "Back Sprite" || type == "Back Male Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, true);
            else if (type == "Back Female Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, true, true);
            else if (type == "Back Rig" || type == "Back Male Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, true);
            else if (type == "Back Female Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, true, true);
        }

        private void shinyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Setup(spriteID);
        }

        private void imageTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setup(spriteID);
        }

        private void saveImageButton_Click(object sender, EventArgs e)
        {
            if (imageTypeDropdown.SelectedIndex == -1) return;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = imageTypeDropdown.Text.Replace(" ", "") + (shinyCheckBox.Checked ? "_Shiny" : "");
            save.Filter = "png file|*.png";
            if (save.ShowDialog() == DialogResult.OK)
            {
                pokemonSpriteBox.Image.Save(save.FileName);
            }
        }

        private void importImageButton_Click(object sender, EventArgs e)
        {
            if (imageTypeDropdown.SelectedIndex == -1) return;
            OpenFileDialog open = new OpenFileDialog();
            open.FileName = imageTypeDropdown.Text.Replace(" ", "") + (shinyCheckBox.Checked ? "_Shiny" : "");
            open.Filter = "png file|*.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenRead(open.FileName);
                Bitmap img = (Bitmap)Image.FromStream(fs);
                fs.Close();
                string type = (string)imageTypeDropdown.SelectedItem;
                if (type == "Front Sprite" || type == "Front Male Sprite") sprite.SetSprite(img, shinyCheckBox.Checked);
                else if (type == "Front Female Sprite") sprite.SetSprite(img, shinyCheckBox.Checked, false, true);
                else if (type == "Front Rig" || type == "Front Male Rig") sprite.SetRig(img, shinyCheckBox.Checked);
                else if (type == "Front Female Rig") sprite.SetRig(img, shinyCheckBox.Checked, false, true);
                else if (type == "Back Sprite" || type == "Back Male Sprite") sprite.SetSprite(img, shinyCheckBox.Checked, true);
                else if (type == "Back Female Sprite") sprite.SetSprite(img, shinyCheckBox.Checked, true, true);
                else if (type == "Back Rig" || type == "Back Male Rig") sprite.SetRig(img, shinyCheckBox.Checked, true);
                else if (type == "Back Female Rig") sprite.SetRig(img, shinyCheckBox.Checked, true, true);

                if (type == "Front Sprite" || type == "Front Male Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked);
                else if (type == "Front Female Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, false, true);
                else if (type == "Front Rig" || type == "Front Male Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked);
                else if (type == "Front Female Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, false, true);
                else if (type == "Back Sprite" || type == "Back Male Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, true);
                else if (type == "Back Female Sprite") pokemonSpriteBox.Image = sprite.GetSprite(shinyCheckBox.Checked, true, true);
                else if (type == "Back Rig" || type == "Back Male Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, true);
                else if (type == "Back Female Rig") pokemonSpriteBox.Image = sprite.GetRig(shinyCheckBox.Checked, true, true);
            }
        }

        private void extractFileButton_Click(object sender, EventArgs e)
        {
            if (fileIDDropdown.SelectedIndex == -1) return;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = fileIDDropdown.SelectedItem.ToString().Replace(" ", "");
            save.Filter = "bin file|*.bin";
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(save.FileName, sprite.files[fileIDDropdown.SelectedIndex]);
            }
        }

        private void replaceFileButton_Click(object sender, EventArgs e)
        {
            if (fileIDDropdown.SelectedIndex == -1) return;
            OpenFileDialog open = new OpenFileDialog();
            open.FileName = fileIDDropdown.SelectedItem.ToString().Replace(" ", "");
            open.Filter = "bin file|*.bin";
            if (open.ShowDialog() == DialogResult.OK)
            {
                sprite.files[fileIDDropdown.SelectedIndex] = File.ReadAllBytes(open.FileName);
                sprite.ReadData();
                Setup(spriteID);
            }
        }

        private void savePaletteButton_Click(object sender, EventArgs e)
        {
            Color[] pal = shinyCheckBox.Checked ? sprite.shinyPalette : sprite.palette;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = shinyCheckBox.Checked ? "ShinyPalette" : "Palette";
            save.Filter = "png file|*.png";
            if (save.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(16, 1);
                for (int i = 0; i < 16; i++) bmp.SetPixel(i, 0, pal[i]);
                bmp.Save(save.FileName);
            }
        }

        private void importPaletteButton_Click(object sender, EventArgs e)
        {
            Color[] pal = shinyCheckBox.Checked ? sprite.shinyPalette : sprite.palette;
            OpenFileDialog open = new OpenFileDialog();
            open.FileName = shinyCheckBox.Checked ? "ShinyPalette" : "Palette";
            open.Filter = "png file|*.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenRead(open.FileName);
                Bitmap img = (Bitmap)Image.FromStream(fs);
                fs.Close();
                for (int i = 0; i < Math.Min(img.Width, 16); i++) pal[i] = img.GetPixel(i, 0);
                Setup(spriteID);
            }
        }

        private void paletteIDNumberBox_ValueChanged(object sender, EventArgs e)
        {
            pokemonIconBox.Image = MainEditor.pokemonIconNarc.GetIcon(spriteID, iconTypeDropdown.SelectedIndex == 1, (int)paletteIDNumberBox.Value);
        }

        private void iconTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            pokemonIconBox.Image = MainEditor.pokemonIconNarc.GetIcon(spriteID, iconTypeDropdown.SelectedIndex == 1, (int)paletteIDNumberBox.Value);
            if (paletteArrayOffset != 0)
            {
                List<byte> arm = MainEditor.fileSystem.arm9;
                int value = arm[paletteArrayOffset + spriteID + (spriteID > 680 ? 2 : 0)];
                if (iconTypeDropdown.SelectedIndex == 1) paletteIDNumberBox.Value = (value & 0b_11110000) >> 4;
                else paletteIDNumberBox.Value = value & 0b_1111;
            }
        }

        private void setIconPaletteButton_Click(object sender, EventArgs e)
        {
            if (paletteArrayOffset == 0)
            {
                MessageBox.Show("Palette array not found");
                return;
            }
            List<byte> arm = MainEditor.fileSystem.arm9;
            int value = arm[paletteArrayOffset + spriteID + (spriteID > 680 ? 2 : 0)];
            if (iconTypeDropdown.SelectedIndex == 1) value = (value & 0b1111) | ((int)paletteIDNumberBox.Value << 4);
            else value = (value & 0b11110000) | (int)paletteIDNumberBox.Value;
            arm[paletteArrayOffset + spriteID + (spriteID > 680 ? 2 : 0)] = (byte)value;
        }

        private void saveIconButton_Click(object sender, EventArgs e)
        {
            if (iconTypeDropdown.SelectedIndex == -1) return;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = iconTypeDropdown.SelectedIndex == 1 ? "Icon_Female" : "Icon";
            save.Filter = "png file|*.png";
            if (save.ShowDialog() == DialogResult.OK)
            {
                pokemonIconBox.Image.Save(save.FileName);
            }
        }

        private void importIconButton_Click(object sender, EventArgs e)
        {
            if (iconTypeDropdown.SelectedIndex == -1) return;
            OpenFileDialog open = new OpenFileDialog();
            open.FileName = iconTypeDropdown.SelectedIndex == 1 ? "Icon_Female" : "Icon";
            open.Filter = "png file|*.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.OpenRead(open.FileName);
                Bitmap img = (Bitmap)Image.FromStream(fs);
                fs.Close();
                MainEditor.pokemonIconNarc.SetIcon(spriteID, img, iconTypeDropdown.SelectedIndex == 1, (int)paletteIDNumberBox.Value);
            }
            pokemonIconBox.Image = MainEditor.pokemonIconNarc.GetIcon(spriteID, iconTypeDropdown.SelectedIndex == 1, (int)paletteIDNumberBox.Value);
        }

        private void saveIconPaletteButton_Click(object sender, EventArgs e)
        {
            Color[] pal = MainEditor.pokemonIconNarc.palettes[(int)paletteIDNumberBox.Value];
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "IconPalette" + (int)paletteIDNumberBox.Value;
            save.Filter = "png file|*.png";
            if (save.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(16, 1);
                for (int i = 0; i < 16; i++) bmp.SetPixel(i, 0, pal[i]);
                bmp.Save(save.FileName);
            }
        }
    }
}
