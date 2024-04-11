using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class TrainerDataNARC : NARC
    {
        public List<TrainerEntry> trainers;

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
            trainers = new List<TrainerEntry>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                TrainerEntry p = new TrainerEntry(bytes) { nameID = i };
                trainers.Add(p);

                pos += 8;
            }

            //Get Dialogue
            byte[] table = fileSystem.trTextEntriesNarc.tableBytes;
            for (int i = 0; i < table.Length; i += 4)
            {
                int id = HelperFunctions.ReadShort(table, i);
                int type = HelperFunctions.ReadShort(table, i + 2);
                int line = i / 4;
                trainers[id].dialogue[type] = fileSystem.textNarc.textFiles[VersionConstants.TrainerDialogueTextFileID].text[line];
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
            foreach (TrainerEntry t in trainers)
            {
                newByteData.AddRange(t.bytes);
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += t.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }

            byteData = newByteData.ToArray();

            FixHeaders(trainers.Count);

            base.WriteData();
        }
    }

    public class TrainerEntry
    {
        public byte[] bytes;

        public TrainerPokemonEntry pokemon;

        public int nameID;

        public bool heldItems;
        public bool uniqueMoves;
        public bool isHealer;

        public byte trainerClass;
        public byte battleType;
        public byte numPokemon;

        public short[] items;

        public byte AI;

        public byte prizeMoney;
        public short prizeItem;

        public Dictionary<int, string> dialogue = new Dictionary<int, string>();

        public int TrPokeFormat
        {
            get
            {
                int val = 0;
                if (uniqueMoves) val += 1;
                if (heldItems) val += 2;
                return val;
            }

            set
            {
                uniqueMoves = value % 2 == 1;
                heldItems = value >= 2;
            }
        }

        public int TrPokeEntryLength => 8 + (uniqueMoves ? 8 : 0) + (heldItems ? 2 : 0);

        public TrainerEntry(byte[] bytes)
        {
            this.bytes = bytes;

            ReadData();

            for (int i = 0; i <= 24; i++)
            {
                dialogue.Add(i, "");
            }
        }

        internal void ReadData()
        {
            TrPokeFormat = bytes[0];

            trainerClass = bytes[1];
            battleType = bytes[2];
            numPokemon = bytes[3];

            items = new short[4];
            for (int i = 0; i < items.Length; i++) items[i] = (short)HelperFunctions.ReadShort(bytes, 4 + (2 * i));

            AI = bytes[12];

            if (bytes.Length > 16)
            {
                isHealer = bytes[16] == 1;
                prizeMoney = bytes[17];
                prizeItem = (short)HelperFunctions.ReadShort(bytes, 18);
            }
        }

        public void ApplyData()
        {
            bytes[0] = (byte)TrPokeFormat;

            bytes[1] = trainerClass;
            bytes[2] = battleType;
            bytes[3] = numPokemon;

            for (int i = 0; i < items.Length; i++) HelperFunctions.WriteShort(bytes, 4 + (i * 2), items[i]);

            bytes[12] = AI;

            if (bytes.Length > 16)
            {
                bytes[16] = (byte)(isHealer ? 1 : 0);
                bytes[17] = prizeMoney;
                HelperFunctions.WriteShort(bytes, 18, prizeItem);
            }

            if (pokemon != null) pokemon.ApplyData();
        }

        public override string ToString()
        {
            return nameID < MainEditor.textNarc.textFiles[VersionConstants.TrainerNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.TrainerNameTextFileID].text[nameID] + (pokemon != null && pokemon.pokemon.Count > 0 ? " (lv " + pokemon.pokemon[0].level + ") - " : " - ") + nameID : "Name not found";
        }
    }
}
