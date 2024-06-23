using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class XPCurveNARC : NARC
    {
        public List<XPCurveEntry> curves;

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
            curves = new List<XPCurveEntry>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                XPCurveEntry m = new XPCurveEntry(bytes);
                curves.Add(m);

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
            foreach (XPCurveEntry i in curves)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += i.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (XPCurveEntry i in curves)
            {
                newByteData.AddRange(i.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(curves.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            ItemDataNARC other = narc as ItemDataNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.items.Count || !curves[i].bytes.SequenceEqual(other.items[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(curves[i].bytes.Length));
                    bytes.AddRange(curves[i].bytes);
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

                if (id > curves.Count)
                {
                    //Don't accept extra files here
                }
                else curves[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class XPCurveEntry
    {
        public byte[] bytes;

        public XPCurveEntry(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public int GetXPAtLevel(int level)
        {
            return HelperFunctions.ReadInt(bytes, level * 4);
        }

        public void SetXPAtLevel(int level, int totalXP)
        {
            HelperFunctions.WriteInt(bytes, level * 4, totalXP);
        }
    }
}
