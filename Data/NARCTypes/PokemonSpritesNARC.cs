using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class PokemonSpritesNARC : NARC
    {
        public List<PokemonSpriteEntry> sprites;
        List<byte[]> remainingfiles;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            sprites = new List<PokemonSpriteEntry>();

            //Populate data types
            int nameID = 0;
            for (int i = 0; i < numFileEntries - 25; i += 20)
            {
                PokemonSpriteEntry p = new PokemonSpriteEntry() { nameID = nameID };
                for (int j = 0; j < 20; j++)
                {
                    int start = HelperFunctions.ReadInt(byteData, pos);
                    int end = HelperFunctions.ReadInt(byteData, pos + 4);
                    byte[] bytes = new byte[end - start];

                    for (int k = 0; k < end - start; k++) bytes[k] = byteData[initialPosition + start + k];
                    p.files.Add(bytes);
                    pos += 8;
                }

                sprites.Add(p);

                p.ReadData();

                nameID++;
            }

            remainingfiles = new List<byte[]>();

            for (int i = numFileEntries - 25; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                remainingfiles.Add(bytes);

                pos += 8;
            }
        }

        public override void WriteData()
        {
            List<byte> newByteData = new List<byte>();
            List<byte> oldByteData = new List<byte>(byteData);

            newByteData.AddRange(oldByteData.GetRange(0, pointerStartAddress));
            newByteData.AddRange(oldByteData.GetRange(BTNFPosition, FileEntryStart - BTNFPosition));

            //Write Files
            int totalSize = 0;
            int pPos = pointerStartAddress;
            foreach (PokemonSpriteEntry p in sprites)
            {
                for (int j = 0; j < 20; j++)
                {
                    newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                    pPos += 4;
                    totalSize += p.files[j].Length;
                    newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                    pPos += 4;
                }
            }

            foreach (byte[] b in remainingfiles)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += b.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }

            foreach (PokemonSpriteEntry p in sprites)
            {
                for (int j = 0; j < 20; j++)
                {
                    newByteData.AddRange(p.files[j]);
                }
            }
            foreach (byte[] b in remainingfiles)
            {
                newByteData.AddRange(b);
            }

            byteData = newByteData.ToArray();

            FixHeaders(sprites.Count * 20 + remainingfiles.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            PokemonSpritesNARC other = narc as PokemonSpritesNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries - 25; i++)
            {
                if (i / 20 > other.sprites.Count || !sprites[i / 20].files[i % 20].SequenceEqual(other.sprites[i / 20].files[i % 20]))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(sprites[i / 20].files[i % 20].Length));
                    bytes.AddRange(sprites[i / 20].files[i % 20]);
                }
            }

            return bytes.ToArray();
        }

        public override void ReadPatchBytes(byte[] bytes)
        {
            int pos = 0;
            while (pos < bytes.Length)
            {
                int id = HelperFunctions.ReadInt(bytes, pos);
                int size = HelperFunctions.ReadInt(bytes, pos + 4);
                pos += 8;

                if (id / 20 > sprites.Count)
                {
                    //Don't accept extra files here
                }
                else sprites[id / 20].files[id % 20] = new List<byte>(bytes).GetRange(pos, size).ToArray();
                sprites[id / 20].ReadData();
                pos += size;
            }
        }
    }

    public class PokemonSpriteEntry
    {
        public List<byte[]> files;

        public byte[] FrontSpriteBytes => files[0];
        public byte[] FrontFemaleSpriteBytes => files[1];
        public byte[] FrontRigBytes => files[2];
        public byte[] FrontFemaleRigBytes => files[3];
        public byte[] FrontCellsBytes => files[4];
        public byte[] FrontAnimationBytes => files[5];
        public byte[] FrontTileBytes => files[8];
        public byte[] BackSpriteBytes => files[9];
        public byte[] BackFemaleSpriteBytes => files[10];
        public byte[] BackRigBytes => files[11];
        public byte[] BackFemaleRigBytes => files[12];
        public byte[] BackCellsBytes => files[13];
        public byte[] BackAnimationBytes => files[14];
        public byte[] BackTileBytes => files[17];
        public byte[] PaletteBytes => files[18];
        public byte[] ShinyPaletteBytes => files[19];

        public int nameID;

        public byte[] decompressedFrontSprite;
        public byte[] decompressedFrontRig;
        public byte[] decompressedFrontFemaleSprite;
        public byte[] decompressedFrontFemaleRig;
        public byte[] decompressedBackSprite;
        public byte[] decompressedBackRig;
        public byte[] decompressedBackFemaleSprite;
        public byte[] decompressedBackFemaleRig;

        public Color[] palette;
        public Color[] shinyPalette;

        public PokemonSpriteEntry() { files = new List<byte[]>(); }

        public void ReadData()
        {
            palette = new Color[16];
            for (int i = 0; i < 16; i++) palette[i] = DsDecmp.Read16BitColor(HelperFunctions.ReadShort(PaletteBytes, 40 + i * 2));
            shinyPalette = new Color[16];
            for (int i = 0; i < 16; i++) shinyPalette[i] = DsDecmp.Read16BitColor(HelperFunctions.ReadShort(ShinyPaletteBytes, 40 + i * 2));
            decompressedFrontSprite = DsDecmp.Decompress(FrontSpriteBytes);
            decompressedFrontFemaleSprite = DsDecmp.Decompress(FrontFemaleSpriteBytes);
            decompressedFrontRig = DsDecmp.Decompress(FrontRigBytes);
            decompressedFrontFemaleRig = DsDecmp.Decompress(FrontFemaleRigBytes);
            decompressedBackSprite = DsDecmp.Decompress(BackSpriteBytes);
            decompressedBackFemaleSprite = DsDecmp.Decompress(BackFemaleSpriteBytes);
            decompressedBackRig = DsDecmp.Decompress(BackRigBytes);
            decompressedBackFemaleRig = DsDecmp.Decompress(BackFemaleRigBytes);
        }

        public void ApplyData()
        {
            for (int i = 0; i < 16; i++) HelperFunctions.WriteShort(PaletteBytes, 40 + i * 2, DsDecmp.Write16BitColor(palette[i]));
            for (int i = 0; i < 16; i++) HelperFunctions.WriteShort(ShinyPaletteBytes, 40 + i * 2, DsDecmp.Write16BitColor(shinyPalette[i]));
        }

        public Bitmap GetSprite(bool shiny = false, bool back = false, bool female = false)
        {
            byte[] b = decompressedFrontSprite;
            if (back && female) b = decompressedBackFemaleSprite;
            else if (back) b = decompressedBackSprite;
            else if (female) b = decompressedFrontFemaleSprite;
            if (b == null || b.Length == 0) return null;
            Bitmap img = new Bitmap(64, 144);
            Color[] pal = shiny ? shinyPalette : palette;
            for (int tile = 0; tile < 144; tile++)
            {
                int tileX = tile % 8;
                int tileY = tile / 8;
                for (int yT = 0; yT < 8; yT++)
                {
                    for (int xT = 0; xT < 8; xT++)
                    {
                        int value = b[tile * 32 + yT * 4 + xT / 2 + 48];
                        value = (value >> (xT % 2) * 4) & ((1 << 4) - 1);
                        img.SetPixel(tileX * 8 + xT, tileY * 8 + yT, pal[value]);
                    }
                }
            }
            if (img == null) return img;

            //Unscrambling
            Bitmap finalImage = new Bitmap(96, 96, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(finalImage);
            g.DrawImage(img, new Rectangle(0, 0, 64, 64), new Rectangle(0, 0, 64, 64), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 0, 32, 8), new Rectangle(0, 64, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 8, 32, 8), new Rectangle(32, 64, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 16, 32, 8), new Rectangle(0, 72, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 24, 32, 8), new Rectangle(32, 72, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 32, 32, 8), new Rectangle(0, 80, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 40, 32, 8), new Rectangle(32, 80, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 48, 32, 8), new Rectangle(0, 88, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 56, 32, 8), new Rectangle(32, 88, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(0, 64, 64, 32), new Rectangle(0, 96, 64, 32), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 64, 32, 8), new Rectangle(0, 128, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 72, 32, 8), new Rectangle(32, 128, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 80, 32, 8), new Rectangle(0, 136, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(64, 88, 32, 8), new Rectangle(32, 136, 32, 8), GraphicsUnit.Pixel);
            return finalImage;
        }

        public Bitmap GetRig(bool shiny = false, bool back = false, bool female = false)
        {
            byte[] b = decompressedFrontRig;
            if (back && female) b = decompressedBackFemaleRig;
            else if (back) b = decompressedBackRig;
            else if (female) b = decompressedFrontFemaleRig;
            if (b == null || b.Length == 0) return null;
            Color[] pal = shiny ? shinyPalette : palette;
            Bitmap bmp = new Bitmap(256, 128);
            for (int i = 0; i < 256 * 128; i++)
            {
                int c = b[(i / 2) + 48];
                c = i % 2 == 0 ? c & 0b_1111 : (c & 0b_1111_0000) >> 4;
                bmp.SetPixel(i % 256, i / 256, pal[c]);
            }
            return bmp;
        }

        public void SetSprite(Bitmap image, bool shiny = false, bool back = false, bool female = false)
        {
            if (image.Width != 96 || image.Height != 96)
            {
                MessageBox.Show("Sprite size should be 96 x 96");
                return;
            }
            Bitmap scrambled = new Bitmap(64, 144, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(scrambled);
            g.DrawImage(image, new Rectangle(0, 0, 64, 64), new Rectangle(0, 0, 64, 64), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 64, 32, 8), new Rectangle(64, 0, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(32, 64, 32, 8), new Rectangle(64, 8, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 72, 32, 8), new Rectangle(64, 16, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(32, 72, 32, 8), new Rectangle(64, 24, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 80, 32, 8), new Rectangle(64, 32, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(32, 80, 32, 8), new Rectangle(64, 40, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 88, 32, 8), new Rectangle(64, 48, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(32, 88, 32, 8), new Rectangle(64, 56, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 96, 64, 32), new Rectangle(0, 64, 64, 32), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 128, 32, 8), new Rectangle(64, 64, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(32, 128, 32, 8), new Rectangle(64, 72, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(0, 136, 32, 8), new Rectangle(64, 80, 32, 8), GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(32, 136, 32, 8), new Rectangle(64, 88, 32, 8), GraphicsUnit.Pixel);

            byte[] b = decompressedFrontSprite;
            if (back && female) b = decompressedBackFemaleSprite;
            else if (back) b = decompressedBackSprite;
            else if (female) b = decompressedFrontFemaleSprite;
            Color[] pal = shiny ? shinyPalette : palette;

            for (int tile = 0; tile < 144; tile++)
            {
                int tileX = tile % 8;
                int tileY = tile / 8;
                for (int yT = 0; yT < 8; yT++)
                {
                    for (int xT = 0; xT < 8; xT++)
                    {
                        int value = b[tile * 32 + yT * 4 + xT / 2 + 48];
                        value = (value >> (xT % 2) * 4) & ((1 << 4) - 1);
                        Color c = scrambled.GetPixel(tileX * 8 + xT, tileY * 8 + yT);
                        int cid = 0;
                        for (int j = 1; j < 16; j++) if (c.R == pal[j].R && c.G == pal[j].G && c.B == pal[j].B) cid = j;

                        if (xT % 2 == 0) b[tile * 32 + yT * 4 + xT / 2 + 48] = (byte)cid;
                        else b[tile * 32 + yT * 4 + xT / 2 + 48] = (byte)((cid << 4) | b[tile * 32 + yT * 4 + xT / 2 + 48]);
                    }
                }
            }

            int n = 0;
            if (female) n++;
            if (back) n += 9;
            files[n] = DsDecmp.Compress(b);
        }

        public void SetRig(Bitmap image, bool shiny = false, bool back = false, bool female = false)
        {
            if (image.Width != 256 || image.Height != 128)
            {
                MessageBox.Show("Sprite size should be 256 x 128");
                return;
            }
            byte[] b = decompressedFrontRig;
            if (back && female) b = decompressedBackFemaleRig;
            else if (back) b = decompressedBackRig;
            else if (female) b = decompressedFrontFemaleRig;
            Color[] pal = shiny ? shinyPalette : palette;
            for (int i = 0; i < 256 * 128; i++)
            {
                Color c = image.GetPixel(i % 256, i / 256);
                int cid = 0;
                for (int j = 1; j < 16; j++) if (c.R == pal[j].R && c.G == pal[j].G && c.B == pal[j].B) cid = j;
                if (i % 2 == 0) b[(i / 2) + 48] = (byte)cid;
                else b[(i / 2) + 48] = (byte)((cid << 4) | b[(i / 2) + 48]);
            }

            int n = 2;
            if (female) n++;
            if (back) n += 9;
            files[n] = DsDecmp.Compress(b);
        }

        public override string ToString()
        {
            return nameID < MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[nameID] + " - " + nameID : "Name not found";
        }
    }
}
