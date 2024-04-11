using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class PokemartNARC : NARC
    {
        public static List<string> bw2ShopsNames = new List<string>()
        {
            "1 - Stock No Badges",
            "2 - Stock 1+Badges",
            "3 - Stock 3+Badges",
            "4 - Stock 5+Badges",
            "5 - Stock 7+Badges",
            "6 - Stock 8+Badges",
            "7 - Accumula Town Upper Cashier",
            "8 - Striaton City Upper Cashier",
            "9 - Nacrene City Upper Cashier",
            "10 - Castelia City Upper Cashier",
            "11 - Nimbasa TM Department",
            "12 - Driftveil City Upper Cashier",
            "13 - Mistralton City TM Dept.",
            "14 - Icirrus City Upper Cashier",
            "15 - Opelucid City Upper Cashier",
            "16 - Victory Road Upper",
            "17 - Victory Road Upper?",
            "18 - Lacunosa Town",
            "19 - Undella Town",
            "20 - Black City",
            "21 - SM9 Top Right Cashier",
            "22 - Driftveil City Herb Shop",
            "23 - Driftveil City Inscense Shop",
            "24 - SM9 Bottom Section",
            "25 - SM9 Middle Right Cashier",
            "26 - SM9 Middle Left Cashier",
            "27 - SM9 Top Left Cashier",
            "28 - Aspertia City?",
            "29 - Virbank City Lower Cashier",
            "30 - Seigaiha City Upper Cashier",
            "31 - Floccesy Town?",
            "32 - Yamaji Town Upper Cashier"
        };

        public List<PokemartEntry> shops;

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
            shops = new List<PokemartEntry>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                PokemartEntry m = new PokemartEntry(bytes) { name = i < bw2ShopsNames.Count ? bw2ShopsNames[i] : "Name Not Found" };
                shops.Add(m);

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
            foreach (PokemartEntry m in shops)
            {
                newByteData.AddRange(m.bytes);
                newByteData.AddRange(new byte[] { 255, 255 });
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += 2;
            }

            byteData = newByteData.ToArray();

            FixHeaders(shops.Count);

            base.WriteData();
        }
    }

    public class PokemartEntry
    {
        public byte[] bytes;
        public string name;
        public List<int> items;

        public PokemartEntry(byte[] bytes)
        {
            this.bytes = bytes;
            items = new List<int>();
            for (int i = 0; i < bytes.Length; i += 2) items.Add(HelperFunctions.ReadShort(bytes, i));
        }

        public void Apply()
        {
            bytes = new byte[items.Count * 2];
            for (int i = 0; i < items.Count; i++) HelperFunctions.WriteShort(bytes, i * 2, items[i]);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
