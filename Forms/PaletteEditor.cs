using DarkModeForms;
using NewEditor.Data;
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
using System.Threading;
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
            sprite = MainEditor.pokemonSpritesNarc.sprites[spriteID];
            InitializeComponent();

            if (MainEditor.fileSystem.romHeader[8] == 0x57)
            {
                paletteArrayOffset = 0x8C578;
            }

            List<byte> arm = MainEditor.fileSystem.arm9;

            bool valid = true;
            if (arm.Count < paletteArrayOffset)
            {
                valid = false;
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (arm[paletteArrayOffset + i] != 0 && arm[paletteArrayOffset + i] != 17 && arm[paletteArrayOffset + i] != 34)
                    {
                        valid = false;
                        break;
                    }
                }
            }

            if (!valid || spriteID >= 754) paletteArrayOffset = 0;
            else
            {
                int value = arm[paletteArrayOffset + spriteID];
                paletteIDNumberBox.Value = value & 0b_1111;
            }

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

            sprite.ReadRigCellsFile();
            RenderRigCells();

            selectedCell = sprite.frontRigCells.cells[0];
            LoadRigCell(sprite.frontRigCells.cells[0], "0");

            string text = "";
            foreach (byte b in sprite.frontRigCells.flags)
            {
                text += b.ToString("X2") + " ";
            }
            rigFlagsTextBox.Text = text.Substring(0, text.Length - 1);
        }

        public void Setup(int spriteID)
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
                        Location = new Point(20 + 312 * (iPos % 3), 380 + 50 * (iPos / 3)),
                        Size = new Size(32, 32),
                        BackColor = pal[i]
                    };
                    mainTabs.TabPages[0].Controls.Add(colorBoxes[i]);

                    for (int j = 0; j < 3; j++)
                    {
                        int num = i;

                        colorValueNumberBoxes[i, j] = new NumericUpDown()
                        {
                            Location = new Point(68 + 312 * (iPos % 3) + 80 * j, 384 + 50 * (iPos / 3)),
                            Size = new Size(60, 24),
                            Maximum = 255,
                            Value = j == 0 ? pal[i].R : j == 1 ? pal[i].G : pal[i].B,
                        };
                        colorValueNumberBoxes[i, j].ValueChanged += (o, e) =>
                        {
                            colorBoxes[num].BackColor = Color.FromArgb((byte)colorValueNumberBoxes[num, 0].Value, (byte)colorValueNumberBoxes[num, 1].Value, (byte)colorValueNumberBoxes[num, 2].Value);
                        };
                        mainTabs.TabPages[0].Controls.Add(colorValueNumberBoxes[i, j]);
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

            for (int i = 0; i < 4; i++)
            {
                rigImages[i] = (Bitmap)sprite.GetRig(false, i > 1, i % 2 == 1 && sprite.FrontFemaleSpriteBytes.Length > 0).Clone();
                for (int x = 0; x < 256; x++)
                {
                    for (int y = 0; y < 128; y++)
                    {
                        if (rigImages[i].GetPixel(x, y).Equals(sprite.palette[0])) rigImages[i].SetPixel(x, y, Color.Transparent);
                    }
                }
            }
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

                for (int i = 0; i < 4; i++)
                {
                    rigImages[i] = (Bitmap)sprite.GetRig(false, i > 1, i % 2 == 1 && sprite.FrontFemaleSpriteBytes.Length > 0).Clone();
                    for (int x = 0; x < 256; x++)
                    {
                        for (int y = 0; y < 128; y++)
                        {
                            if (rigImages[i].GetPixel(x, y).Equals(sprite.palette[0])) rigImages[i].SetPixel(x, y, Color.Transparent);
                        }
                    }
                }
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
                applyPaletteButton_Click(sender, e);
            }
        }

        private void paletteIDNumberBox_ValueChanged(object sender, EventArgs e)
        {
            pokemonIconBox.Image = MainEditor.pokemonIconNarc.GetIcon(spriteID, iconTypeDropdown.SelectedIndex == 1, (int)paletteIDNumberBox.Value);
        }

        private void iconTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            pokemonIconBox.Image = MainEditor.pokemonIconNarc.GetIcon(spriteID, iconTypeDropdown.SelectedIndex == 1, (int)paletteIDNumberBox.Value);
            if (spriteID >= 754) return;
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

        private void dumpFilesButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog prompt = new FolderBrowserDialog();
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                int start = (int)dumpFilesPIDNumberBox.Value * 20;
                for (int i = 0; i < 20; i++)
                {
                    string location = prompt.SelectedPath + "\\004_" + start.ToString("D8") + ".bin";
                    File.WriteAllBytes(location, sprite.files[i]);
                    start++;
                }
            }
        }

        private void RenderRigCells()
        {
            Bitmap image = new Bitmap(768, 384);
            Bitmap image2 = new Bitmap(512, 256);
            Graphics g = Graphics.FromImage(image);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            Graphics g2 = Graphics.FromImage(image2);
            g2.InterpolationMode = InterpolationMode.NearestNeighbor;
            rigCellsRender.Image = image;
            cellPreview.Image = image2;

            for (int i = 0; i < 4; i++)
            {
                rigImages[i] = (Bitmap)sprite.GetRig(false, i > 1, i % 2 == 1 && sprite.FrontFemaleSpriteBytes.Length > 0).Clone();
                for (int x = 0; x < 256; x++)
                {
                    for (int y = 0; y < 128; y++)
                    {
                        if (rigImages[i].GetPixel(x, y).Equals(sprite.palette[0])) rigImages[i].SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            renderer = Task.Run(() =>
            {
                Thread.Sleep(500);
                while (!IsDisposed)
                {
                    //Main
                    Pen pen = new Pen(Color.Black, 1);
                    g.Clear(Color.FromArgb(180, 200, 210));
                    Bitmap img = rigImages[(frontRigCheckBox.Checked ? 0 : 2) + (maleRigCheckBox.Checked ? 0 : 1)];
                    g.DrawImage(img, new Rectangle(0, 0, 768, 384));
                    for (int i = 0; i < 64; i++)
                    {
                        g.DrawLines(pen, new Point[]
                        {
                        new Point(i * 24, 0),
                        new Point(i * 24, 384),
                        });
                    }
                    for (int i = 0; i < 32; i++)
                    {
                        g.DrawLines(pen, new Point[]
                        {
                        new Point(0, i * 24),
                        new Point(768, i * 24),
                        });
                    }
                    pen = new Pen(Color.White, 2);
                    Pen pen2 = new Pen(Color.Red, 2);
                    List<RigCell> cells = new List<RigCell>(frontRigCheckBox.Checked ? sprite.frontRigCells.cells : sprite.backRigCells.cells);
                    foreach (RigCell cell in cells)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(120, cell == selectedCell ? Color.White : Color.LawnGreen)), cell.cellX * 3, cell.cellY * 3, cell.width * 3, cell.height * 3);
                        g.DrawRectangle(pen, cell.cellX * 3, cell.cellY * 3, cell.width * 3, cell.height * 3);
                        g.DrawString(cells.IndexOf(cell).ToString(), new Font(Font.FontFamily, 16), new SolidBrush(Color.Black), cell.cellX * 3, cell.cellY * 3);
                        if (cell.subCell.width > 0)
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(120, cell.subCell == selectedCell ? Color.White : Color.Gold)), cell.subCell.cellX * 3, cell.subCell.cellY * 3, cell.subCell.width * 3, cell.subCell.height * 3);
                            g.DrawRectangle(pen, cell.subCell.cellX * 3, cell.subCell.cellY * 3, cell.subCell.width * 3, cell.subCell.height * 3);
                            g.DrawString(cells.IndexOf(cell).ToString(), new Font(Font.FontFamily, 16), new SolidBrush(Color.Black), cell.subCell.cellX * 3, cell.subCell.cellY * 3);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(120, cell.subCell == selectedCell ? Color.White : Color.Gold)), cell.subCell.cellX * 3, cell.subCell.cellY * 3 + cell.subCell.height * 3, cell.subCell.width * 3, cell.subCell.height * 3);
                            g.DrawRectangle(pen, cell.subCell.cellX * 3, cell.subCell.cellY * 3 + cell.subCell.height * 3, cell.subCell.width * 3, cell.subCell.height * 3);
                        }
                    }

                    //Preview
                    pen = new Pen(Color.FromArgb(80, 80, 80), 1);
                    g2.Clear(Color.FromArgb(120, 160, 160));
                    RigCell parentCell = cells.FirstOrDefault(c => c.subCell == selectedCell);
                    if (parentCell != null)
                    {
                        g2.DrawImage(img,
                            new Rectangle(256 + parentCell.spriteX * 2, 128 - parentCell.spriteY * 2, parentCell.width * 2, parentCell.height * 2),
                            new Rectangle(parentCell.cellX, parentCell.cellY, parentCell.width, parentCell.height), GraphicsUnit.Pixel);
                    }
                    if (selectedCell != null)
                    {
                        int y = selectedCell.cellY;
                        if (parentCell != null && DateTime.Now.Second % 2 == 1) y += selectedCell.height;
                        g2.DrawImage(img,
                            new Rectangle(256 + selectedCell.spriteX * 2, 128 - selectedCell.spriteY * 2, selectedCell.width * 2, selectedCell.height * 2),
                            new Rectangle(selectedCell.cellX, y, selectedCell.width, selectedCell.height), GraphicsUnit.Pixel);
                    }
                    for (int i = 0; i < 64; i++)
                    {
                        g2.DrawLines(pen, new Point[]
                        {
                        new Point(i * 16, 0),
                        new Point(i * 16, 256),
                        });
                    }
                    for (int i = 0; i < 32; i++)
                    {
                        g2.DrawLines(pen, new Point[]
                        {
                        new Point(0, i * 16),
                        new Point(512, i * 16),
                        });
                    }
                    pen = new Pen(Color.LawnGreen, 2);
                    g2.DrawLine(pen, new Point(256, 0), new Point(256, 256));
                    pen = new Pen(Color.Red, 2);
                    g2.DrawLine(pen, new Point(0, 128), new Point(512, 128));

                    if (endRender) break;
                    try
                    {
                        rigCellsRender.Invoke((MethodInvoker)delegate { if (!endRender) rigCellsRender.Refresh(); });
                        cellPreview.Invoke((MethodInvoker)delegate { if (!endRender) cellPreview.Refresh(); });
                    }
                    catch { return; }
                    Thread.Sleep(5);
                }
            });
        }


        RigCell draggingCell = null;
        RigCell selectedCell = null;
        Bitmap[] rigImages = new Bitmap[4];
        int dragType;
        Point grabAnchor;
        Point mouse = new Point(0, 0);
        Task renderer;
        bool endRender = false;

        private void rigCellsRender_MouseDown(object sender, MouseEventArgs e)
        {
            List<RigCell> cells = new List<RigCell>(frontRigCheckBox.Checked ? sprite.frontRigCells.cells : sprite.backRigCells.cells);
            grabAnchor = rigCellsRender.PointToClient(MousePosition);
            grabAnchor.X /= 3;
            grabAnchor.Y /= 3;
            dragType = 0;
            foreach (RigCell cell in cells)
            {
                if (new Rectangle(cell.cellX, cell.cellY, cell.width, cell.height).Contains(grabAnchor))
                {
                    if (cell != draggingCell) LoadRigCell(cell, cells.IndexOf(cell).ToString());
                    draggingCell = cell;
                    break;
                }
                if (new Rectangle(cell.subCell.cellX, cell.subCell.cellY, cell.subCell.width, cell.subCell.height * 2).Contains(grabAnchor))
                {
                    if (cell != draggingCell) LoadRigCell(cell.subCell, cells.IndexOf(cell).ToString() + "b");
                    draggingCell = cell.subCell;
                    break;
                }
            }
        }

        private void rigCellsRender_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingCell == null) return;
            mouse = rigCellsRender.PointToClient(MousePosition);
            mouse.X /= 3;
            mouse.Y /= 3;

            while (mouse.X > grabAnchor.X + 4 && draggingCell.cellX + draggingCell.width < 256)
            {
                grabAnchor.X += 8;
                draggingCell.cellX += 8;
                LoadRigCell(draggingCell);
            }
            while (mouse.X < grabAnchor.X - 4 && draggingCell.cellX > 0)
            {
                grabAnchor.X -= 8;
                draggingCell.cellX -= 8;
                LoadRigCell(draggingCell);
            }

            while (mouse.Y > grabAnchor.Y + 4 && draggingCell.cellY + draggingCell.height < 128)
            {
                grabAnchor.Y += 8;
                draggingCell.cellY += 8;
                LoadRigCell(draggingCell);
            }
            while (mouse.Y < grabAnchor.Y - 4 && draggingCell.cellY > 0)
            {
                grabAnchor.Y -= 8;
                draggingCell.cellY -= 8;
                LoadRigCell(draggingCell);
            }
        }

        private void rigCellsRender_MouseUp(object sender, MouseEventArgs e)
        {
            draggingCell = null;
        }

        private void PaletteEditor_FormClosing(object sender, FormClosedEventArgs e)
        {
            endRender = true;
        }

        private void LoadRigCell(RigCell cell, string cellID = "")
        {
            selectedCell = cell;
            cellXNumberBox.Value = cell.cellX;
            cellYNumberBox.Value = cell.cellY;
            cellWidthNumberBox.Value = cell.width;
            cellHeightNumberBox.Value = cell.height;
            cellSpriteXNumberBox.Value = cell.spriteX;
            cellSpriteYNumberBox.Value = cell.spriteY;

            if (cellID.Length > 0)
                selectedCellText.Text = "Cell " + cellID + ":";

            if (cell.subCell != null && cell.subCell.width == 0) addSubCellButton.Text = "Add Sub Cell";
            else addSubCellButton.Text = "Remove Sub Cell";
        }

        private void cellXNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (selectedCell == null) return;
            if (cellXNumberBox.Value % 8 != 0)
            {
                cellXNumberBox.Value -= cellXNumberBox.Value % 8;
            }
            else if (cellXNumberBox.Value + selectedCell.width > 256)
            {
                cellXNumberBox.Value = 256 - selectedCell.width;
            }
            selectedCell.cellX = (int)cellXNumberBox.Value;
        }

        private void cellYNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (selectedCell == null) return;
            if (cellYNumberBox.Value % 8 != 0)
            {
                cellYNumberBox.Value -= cellYNumberBox.Value % 8;
            }
            else if (cellYNumberBox.Value + selectedCell.height > 128)
            {
                cellYNumberBox.Value = 128 - selectedCell.height;
            }
            selectedCell.cellY = (int)cellYNumberBox.Value;
        }

        private void cellWidthNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (selectedCell == null) return;
            if (cellWidthNumberBox.Value % 8 != 0)
            {
                cellWidthNumberBox.Value -= cellWidthNumberBox.Value % 8;
            }
            else if (cellWidthNumberBox.Value + selectedCell.cellX > 256)
            {
                cellWidthNumberBox.Value = 256 - selectedCell.cellX;
            }
            selectedCell.width = (int)cellWidthNumberBox.Value;
        }

        private void cellHeightNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (selectedCell == null) return;
            if (cellHeightNumberBox.Value % 8 != 0)
            {
                cellHeightNumberBox.Value -= cellHeightNumberBox.Value % 8;
            }
            else if (cellHeightNumberBox.Value + selectedCell.cellY > 128)
            {
                cellHeightNumberBox.Value = 128 - selectedCell.cellY;
            }
            selectedCell.height = (int)cellHeightNumberBox.Value;
        }

        private void cellSpriteXNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (selectedCell == null) return;
            selectedCell.spriteX = (int)cellSpriteXNumberBox.Value;
        }

        private void cellSpriteYNumberBox_ValueChanged(object sender, EventArgs e)
        {
            if (selectedCell == null) return;
            selectedCell.spriteY = (int)cellSpriteYNumberBox.Value;
        }

        private void frontrigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadRigCell(frontRigCheckBox.Checked ? sprite.frontRigCells.cells[0] : sprite.backRigCells.cells[0], "0");

            string text = "";
            foreach (byte b in frontRigCheckBox.Checked ? sprite.frontRigCells.flags : sprite.backRigCells.flags)
            {
                text += b.ToString("X2") + " ";
            }
            rigFlagsTextBox.Text = text.Substring(0, text.Length - 1);
        }

        private void applyRigCellButton_Click(object sender, EventArgs e)
        {
            RigCellsFile rc = frontRigCheckBox.Checked ? sprite.frontRigCells : sprite.backRigCells;
            List<byte> b = new List<byte>();
            for (int i = 0; i < rigFlagsTextBox.Text.Length; i += 3)
            {
                if (rigFlagsTextBox.Text.Length % 3 == 1)
                {
                    MessageBox.Show("Failed to parse bytes for rig flags.");
                    return;
                }
                else if (i + 3 < rigFlagsTextBox.Text.Length && rigFlagsTextBox.Text[i + 2] != ' ')
                {
                    MessageBox.Show("Failed to parse bytes for rig flags.");
                    return;
                }
                else if (byte.TryParse(rigFlagsTextBox.Text.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, null, out byte b2))
                {
                    b.Add(b2);
                }
                else
                {
                    MessageBox.Show("Failed to parse bytes for rig flags.");
                    return;
                }
            }
            if (b.Count != rc.flags.Length)
            {
                MessageBox.Show($"Failed to apply flags. Expected {rc.flags.Length} bytes but found {b.Count}.");
                return;
            }
            rc.flags = b.ToArray();

            sprite.WriteRigCellsFile();
        }

        private void subCellHelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sub cells are animated cells that follow the motion of their parent cell. They are primarily used to make eyes open and close when the pokemon blinks or is asleep. You can only have one sub cell per cell.");
        }

        private void addSubCellButton_Click(object sender, EventArgs e)
        {
            if (selectedCell.subCell != null && selectedCell.subCell.width == 0)
            {
                //Add
                selectedCell.subCell.width = 8;
                selectedCell.subCell.height = 8;
                selectedCell.subCell.cellX = 0;
                selectedCell.subCell.cellY = 0;
                selectedCell.subCell.spriteX = 0;
                selectedCell.subCell.spriteY = 0;
                List<RigCell> cells = new List<RigCell>(frontRigCheckBox.Checked ? sprite.frontRigCells.cells : sprite.backRigCells.cells);
                RigCell cell = selectedCell.subCell == null ? cells.FirstOrDefault(cl => cl.subCell == selectedCell) : selectedCell;
                LoadRigCell(selectedCell.subCell, cells.IndexOf(cell).ToString() + "b");
            }
            else
            {
                //Remove
                RigCell c = selectedCell.subCell == null ? selectedCell : selectedCell.subCell;
                c.width = 0;
                c.height = 0;
                c.cellX = 0;
                c.cellY = 0;
                c.spriteX = 0;
                c.spriteY = 0;
                if (selectedCell.subCell == null)
                {
                    List<RigCell> cells = new List<RigCell>(frontRigCheckBox.Checked ? sprite.frontRigCells.cells : sprite.backRigCells.cells);
                    RigCell cell = cells.FirstOrDefault(cl => cl.subCell == selectedCell);
                    LoadRigCell(cell, cells.IndexOf(cell).ToString());
                }
            }
        }
    }
}
