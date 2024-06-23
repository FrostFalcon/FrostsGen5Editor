using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class MapMatrixNARC : NARC
    {
        public List<MapMatrixEntry> matricies;

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
            matricies = new List<MapMatrixEntry>();

            pos = pointerStartAddress;

            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                MapMatrixEntry moveset = new MapMatrixEntry(bytes);
                matricies.Add(moveset);

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
            foreach (MapMatrixEntry m in matricies)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (MapMatrixEntry m in matricies)
            {
                newByteData.AddRange(m.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(matricies.Count);

            base.WriteData();
        }
    }

    public class MapMatrixEntry
    {
        public byte[] bytes;

        public byte type;
        public short width;
        public short height;

        public int[,] mapFiles;
        public int[,] mapHeaders;

        public MapMatrixEntry(byte[] bytes)
        {
            this.bytes = bytes;

            type = bytes[0];

            ReadData();
        }

        public void ReadData()
        {
            width = (short)HelperFunctions.ReadShort(bytes, 4);
            height = (short)HelperFunctions.ReadShort(bytes, 6);

            mapFiles = new int[width, height];

            int pos = 8;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapFiles[x, y] = HelperFunctions.ReadInt(bytes, pos);
                    pos += 4;
                }
            }

            if (type == 1)
            {
                mapHeaders = new int[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        mapHeaders[x, y] = HelperFunctions.ReadInt(bytes, pos);
                        pos += 4;
                    }
                }
            }
        }

        public void ApplyData()
        {
            List<byte> newBytes = new List<byte>(new byte[] { type, 0, 0, 0 });
            newBytes.AddRange(BitConverter.GetBytes(width));
            newBytes.AddRange(BitConverter.GetBytes(height));

            for (int y = 0; y < height; y++) for (int x = 0; x < width; x++) newBytes.AddRange(BitConverter.GetBytes(x < mapFiles.GetLength(0) && y < mapFiles.GetLength(1) ? mapFiles[x, y] : -1));

            for (int y = 0; y < height; y++) for (int x = 0; x < width; x++) newBytes.AddRange(BitConverter.GetBytes(x < mapHeaders.GetLength(0) && y < mapHeaders.GetLength(1) ? mapHeaders[x, y] : -1));

            bytes = newBytes.ToArray();
        }
    }
}
