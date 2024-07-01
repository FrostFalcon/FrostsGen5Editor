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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NewEditor.Data.NARCTypes
{
    public class PokemonIconNARC : NARC
    {
        public List<byte[]> files;
        public List<Color[]> palettes;

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
            files = new List<byte[]>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                files.Add(bytes);

                pos += 8;
            }

            palettes = new List<Color[]>();
            for (int i = 0; i < 3; i++)
            {
                palettes.Add(new Color[16]);
                for (int j = 0; j < 16; j++) palettes[i][j] = DsDecmp.Read16BitColor(HelperFunctions.ReadShort(files[0], 40 + 32 * i + j * 2));
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
            foreach (byte[] m in files)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (byte[] m in files)
            {
                newByteData.AddRange(m);
            }

            byteData = newByteData.ToArray();

            FixHeaders(files.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            PokemonIconNARC other = narc as PokemonIconNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.files.Count || !files[i].SequenceEqual(other.files[i]))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(files[i].Length));
                    bytes.AddRange(files[i]);
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

                if (id > files.Count)
                {
                    //Don't accept extra files here
                }
                else files[id] = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }

        public bool GenderedIcons(int pokemonID) => files[9 + pokemonID * 2].Length != 0;
        public Bitmap GetIcon(int pokemonID, bool female = false, int paletteID = 0)
        {
            byte[] b = files[(female ? 9 : 8) + pokemonID * 2 + (MainEditor.RomType == RomType.BW1 ? -1 : 0)];
            if (b == null || b.Length == 0) return null;

            Color[] pal = palettes[paletteID];
            Bitmap bmp = new Bitmap(32, 64);
            for (int tile = 0; tile < 32; tile++)
            {
                int tileX = tile % 4;
                int tileY = tile / 4;
                for (int yT = 0; yT < 8; yT++)
                {
                    for (int xT = 0; xT < 8; xT++)
                    {
                        int value = b[tile * 32 + yT * 4 + xT / 2 + 48];
                        value = (value >> (xT % 2) * 4) & ((1 << 4) - 1);
                        bmp.SetPixel(tileX * 8 + xT, tileY * 8 + yT, pal[value]);
                    }
                }
            }
            return bmp;
        }

        public void SetIcon(int pokemonID, Bitmap image, bool female = false, int paletteID = 0)
        {
            if (image.Width != 32 || image.Height != 64)
            {
                MessageBox.Show("Sprite size should be 32 x 64");
                return;
            }
            byte[] b = files[(female ? 9 : 8) + pokemonID * 2 + (MainEditor.RomType == RomType.BW1 ? -1 : 0)];

            Color[] pal = palettes[paletteID];
            for (int tile = 0; tile < 32; tile++)
            {
                int tileX = tile % 4;
                int tileY = tile / 4;
                for (int yT = 0; yT < 8; yT++)
                {
                    for (int xT = 0; xT < 8; xT++)
                    {
                        int value = b[tile * 32 + yT * 4 + xT / 2 + 48];
                        value = (value >> (xT % 2) * 4) & ((1 << 4) - 1);
                        Color c = image.GetPixel(tileX * 8 + xT, tileY * 8 + yT);
                        int cid = 0;
                        for (int j = 1; j < 16; j++) if (c.R == pal[j].R && c.G == pal[j].G && c.B == pal[j].B) cid = j;

                        if (xT % 2 == 0) b[tile * 32 + yT * 4 + xT / 2 + 48] = (byte)cid;
                        else b[tile * 32 + yT * 4 + xT / 2 + 48] = (byte)((cid << 4) | b[tile * 32 + yT * 4 + xT / 2 + 48]);
                    }
                }
            }
        }
    }
}
