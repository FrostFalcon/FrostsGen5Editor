using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class ItemDataNARC : NARC
    {
        public List<ItemDataEntry> items;

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
            items = new List<ItemDataEntry>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                ItemDataEntry m = new ItemDataEntry(bytes) { nameID = i };
                items.Add(m);

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
            foreach (ItemDataEntry i in items)
            {
                newByteData.AddRange(i.bytes);
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += i.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }

            byteData = newByteData.ToArray();

            FixHeaders(items.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            ItemDataNARC other = narc as ItemDataNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.items.Count || !items[i].bytes.SequenceEqual(other.items[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(items[i].bytes.Length));
                    bytes.AddRange(items[i].bytes);
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

                if (id > items.Count)
                {
                    //Don't accept extra files here
                }
                else items[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class ItemDataEntry
    {
        public byte[] bytes;
        public int BuyPrice
        {
            get
            {
                return HelperFunctions.ReadShort(bytes, 0) * 10;
            }
            set
            {
                HelperFunctions.WriteShort(bytes, 0, value / 10);
            }
        }
        public int nameID;

        public ItemDataEntry(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public override string ToString()
        {
            return nameID.ToString();
        }
    }
}
