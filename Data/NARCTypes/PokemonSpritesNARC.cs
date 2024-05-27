using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NewEditor.Data.NARCTypes
{
    public class PokemonSpritesNARC : NARC
    {
        public List<PokemonSpriteEntry> sprites;
        List<byte[]> remainingfiles;

        public override void ReadData()
        {
            base.ReadData();

            //Find first file instance
            int pos = numFileEntries * 8;
            while (pos < byteData.Length)
            {
                pos++;
                if (pos >= byteData.Length) return;
                if (byteData[pos] == 'B' && byteData[pos + 1] == 'T' && byteData[pos + 2] == 'N' && byteData[pos + 3] == 'F') break;
            }
            int initialPosition = pos + 24;

            //Register data files
            sprites = new List<PokemonSpriteEntry>();

            pos = pointerStartAddress;

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

            //Find start of file instances
            int pos = 0;
            while (pos < byteData.Length)
            {
                pos++;
                if (pos >= byteData.Length) return;
                if (byteData[pos] == 'B' && byteData[pos + 1] == 'T' && byteData[pos + 2] == 'N' && byteData[pos + 3] == 'F') break;
            }

            newByteData.AddRange(oldByteData.GetRange(pos, 24));

            //Write Files
            int totalSize = 0;
            int pPos = pointerStartAddress;
            foreach (PokemonSpriteEntry p in sprites)
            {
                for (int j = 0; j < 20; j++)
                {
                    newByteData.AddRange(p.files[j]);
                    newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                    pPos += 4;
                    totalSize += p.files[j].Length;
                    newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                    pPos += 4;
                }
            }

            foreach (byte[] b in remainingfiles)
            {
                newByteData.AddRange(b);
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += b.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
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
        public byte[] FrontRigBytes => files[2];
        public byte[] BackSpriteBytes => files[9];
        public byte[] BackRigBytes => files[11];
        public byte[] PaletteBytes => files[18];
        public byte[] ShinyPaletteBytes => files[19];

        public int nameID;

        public byte[] decompressedFrontSprite;
        public byte[] decompressedBackSprite;

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
            decompressedBackSprite = DsDecmp.Decompress(BackSpriteBytes);
        }

        public void ApplyData()
        {
            for (int i = 0; i < 16; i++) HelperFunctions.WriteShort(PaletteBytes, 40 + i * 2, DsDecmp.Write16BitColor(palette[i]));
            for (int i = 0; i < 16; i++) HelperFunctions.WriteShort(ShinyPaletteBytes, 40 + i * 2, DsDecmp.Write16BitColor(shinyPalette[i]));
        }

        public Bitmap GetFrontSprite(bool shiny = false)
        {
            if (decompressedFrontSprite == null) return null;
            Bitmap img = DsDecmp.DrawTiledImage(decompressedFrontSprite, shiny ? shinyPalette : palette, 48, 64, 144, 8, 8, 4);
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

        public Bitmap GetBackSprite(bool shiny = false)
        {
            if (decompressedBackSprite == null) return null;
            Bitmap img = DsDecmp.DrawTiledImage(decompressedBackSprite, shiny ? shinyPalette : palette, 48, 64, 144, 8, 8, 4);
            //return img;

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

        public override string ToString()
        {
            return nameID < MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[nameID] + " - " + nameID : "Name not found";
        }
    }
}
