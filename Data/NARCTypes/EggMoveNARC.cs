using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class EggMoveNARC : NARC
    {
        public List<EggMoveEntry> entries;

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
            entries = new List<EggMoveEntry>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                EggMoveEntry m = new EggMoveEntry(bytes);
                entries.Add(m);

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
            foreach (EggMoveEntry i in entries)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += i.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (EggMoveEntry i in entries)
            {
                newByteData.AddRange(i.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(entries.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            EggMoveNARC other = narc as EggMoveNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.entries.Count || !entries[i].bytes.SequenceEqual(other.entries[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(entries[i].bytes.Length));
                    bytes.AddRange(entries[i].bytes);
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

                if (id > entries.Count)
                {
                    //Don't accept extra files here
                }
                else entries[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class EggMoveEntry
    {
        public byte[] bytes;

        public List<short> moves;

        public EggMoveEntry(byte[] bytes)
        {
            this.bytes = bytes;
            ReadData();
        }

        public void ReadData()
        {
            moves = new List<short>();
            for (int i = 2; i < bytes.Length; i += 2)
            {
                moves.Add((short)HelperFunctions.ReadShort(bytes, i));
            }
        }

        public void ApplyData()
        {
            bytes = new byte[moves.Count * 2 + 2];
            HelperFunctions.WriteShort(bytes, 0, moves.Count);
            for (int i = 0; i < moves.Count; i++)
            {
                HelperFunctions.WriteShort(bytes, i * 2 + 2, moves[i]);
            }
        }
    }
}
