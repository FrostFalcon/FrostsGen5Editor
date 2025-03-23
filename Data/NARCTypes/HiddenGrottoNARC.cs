using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class HiddenGrottoNARC : NARC
    {
        public List<HiddenGrottoEntry> grottos;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            grottos = new List<HiddenGrottoEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                HiddenGrottoEntry g = new HiddenGrottoEntry(bytes);
                grottos.Add(g);

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
            foreach (HiddenGrottoEntry i in grottos)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += i.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (HiddenGrottoEntry i in grottos)
            {
                newByteData.AddRange(i.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(grottos.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            HiddenGrottoNARC other = narc as HiddenGrottoNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.grottos.Count || !grottos[i].bytes.SequenceEqual(other.grottos[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(grottos[i].bytes.Length));
                    bytes.AddRange(grottos[i].bytes);
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

                if (id > grottos.Count)
                {
                    //Don't accept extra files here
                }
                else grottos[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class HiddenGrottoEntry
    {
        public byte[] bytes;

        private List<HiddenGrottoEncounter[]> encounterListList;
        public HiddenGrottoEncounter[] whiteCommonEncounters;
        public HiddenGrottoEncounter[] whiteUncommonEncounters;
        public HiddenGrottoEncounter[] whiteRareEncounters;
        public HiddenGrottoEncounter[] blackCommonEncounters;
        public HiddenGrottoEncounter[] blackUncommonEncounters;
        public HiddenGrottoEncounter[] blackRareEncounters;
        public short[] items;
        public short[] hiddenItems;

        public HiddenGrottoEntry(byte[] bytes)
        {
            this.bytes = bytes;
            ReadData();
        }

        public void ReadData()
        {
            whiteRareEncounters = new HiddenGrottoEncounter[4];
            whiteUncommonEncounters = new HiddenGrottoEncounter[4];
            whiteCommonEncounters = new HiddenGrottoEncounter[4];
            blackRareEncounters = new HiddenGrottoEncounter[4];
            blackUncommonEncounters = new HiddenGrottoEncounter[4];
            blackCommonEncounters = new HiddenGrottoEncounter[4];

            encounterListList = new List<HiddenGrottoEncounter[]>
            {
                whiteRareEncounters, whiteUncommonEncounters, whiteCommonEncounters,
                blackRareEncounters, blackUncommonEncounters, blackCommonEncounters
            };
            for (int n = 0; n < 6; n++)
            {
                int pos = n * 26;
                for (int i = 0; i < 4; i++)
                {
                    encounterListList[n][i] = new HiddenGrottoEncounter()
                    {
                        pokemonID = (short)HelperFunctions.ReadShort(bytes, pos + 2 * i),
                        maxLevel = bytes[pos + 8 + i],
                        minLevel = bytes[pos + 12 + i],
                        gender = bytes[pos + 16 + i],
                        form = bytes[pos + 20 + i],
                    };
                }
            }

            items = new short[16];
            for (int i = 0x9C; i < 0xBC; i += 2)
            {
                items[(i - 0x9C) / 2] = (short)HelperFunctions.ReadShort(bytes, i);
            }

            hiddenItems = new short[16];
            for (int i = 0xBC; i < 0xDC; i += 2)
            {
                hiddenItems[(i - 0xBC) / 2] = (short)HelperFunctions.ReadShort(bytes, i);
            }
        }

        public void ApplyData()
        {
            List<byte> newBytes = new List<byte>();

            for (int n = 0; n < 6; n++)
            {
                for (int i = 0; i < 4; i++) newBytes.AddRange(BitConverter.GetBytes(encounterListList[n][i].pokemonID));
                for (int i = 0; i < 4; i++) newBytes.Add(encounterListList[n][i].maxLevel);
                for (int i = 0; i < 4; i++) newBytes.Add(encounterListList[n][i].minLevel);
                for (int i = 0; i < 4; i++) newBytes.Add(encounterListList[n][i].gender);
                for (int i = 0; i < 4; i++) newBytes.Add(encounterListList[n][i].form);
                newBytes.Add(0);
                newBytes.Add(0);
            }

            for (int i = 0; i < 16; i++)
            {
                newBytes.AddRange(BitConverter.GetBytes(items[i]));
            }
            for (int i = 0; i < 16; i++)
            {
                newBytes.AddRange(BitConverter.GetBytes(hiddenItems[i]));
            }

            bytes = newBytes.ToArray();
        }
    }

    public class HiddenGrottoEncounter
    {
        public short pokemonID;
        public byte minLevel;
        public byte maxLevel;
        public byte gender;
        public byte form;
    }
}
