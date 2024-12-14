using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class KeyboardNARC : NARC
    {
        public List<KeyboardLayoutEntry> layouts;

        public static List<string> keyboardLayoutNames = new List<string>()
        {
            "Upper",
            "Lower",
            "Other",
            "Qwerty",
            "Qwerty Shift",
            "------",
            "------",
            "------",
            "------",
            "------",
            "------",
        };

        public override void ReadData()
        {
            base.ReadData();

            //Find first file instance

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            layouts = new List<KeyboardLayoutEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                KeyboardLayoutEntry m = new KeyboardLayoutEntry(bytes) { name = i < keyboardLayoutNames.Count ? keyboardLayoutNames[i] : "Name Not Found" };
                layouts.Add(m);

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
            foreach (KeyboardLayoutEntry m in layouts)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += 2;
            }
            foreach (KeyboardLayoutEntry m in layouts)
            {
                newByteData.AddRange(m.bytes);
                newByteData.AddRange(new byte[] { 255, 255 });
            }

            byteData = newByteData.ToArray();

            FixHeaders(layouts.Count);

            base.WriteData();
        }
    }

    public class KeyboardLayoutEntry
    {
        public byte[] bytes;
        public string name;
        public int width;
        public int height;
        public List<short> characters;

        public KeyboardLayoutEntry(byte[] bytes)
        {
            this.bytes = bytes;
            characters = new List<short>();
            width = HelperFunctions.ReadShort(bytes, 0);
            height = HelperFunctions.ReadShort(bytes, 2);
            for (int i = 4; i < bytes.Length; i += 2) characters.Add((short)HelperFunctions.ReadShort(bytes, i));
        }

        public void Apply()
        {
            bytes = new byte[characters.Count * 2 + 4];
            HelperFunctions.WriteShort(bytes, 0, width);
            HelperFunctions.WriteShort(bytes, 2, height);
            for (int i = 0; i < characters.Count; i++)
            {
                HelperFunctions.WriteShort(bytes, i * 2 + 4, characters[i]);
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
