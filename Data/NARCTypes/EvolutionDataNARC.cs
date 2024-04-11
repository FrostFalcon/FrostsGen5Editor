using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class EvolutionDataNARC : NARC
    {
        PokemonDataNARC pokemonNarc => fileSystem.pokemonDataNarc;

        public List<EvolutionDataEntry> evolutions;

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
            evolutions = new List<EvolutionDataEntry>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                EvolutionDataEntry m = new EvolutionDataEntry(bytes) { nameID = i };
                evolutions.Add(m);

                pos += 8;
            }

            //Assign evolution sets to pokemon
            for (int i = 0; i < pokemonNarc.pokemon.Count; i++)
            {
                if (i < evolutions.Count) pokemonNarc.pokemon[i].evolutions = evolutions[i];
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
            foreach (EvolutionDataEntry m in evolutions)
            {
                newByteData.AddRange(m.bytes);
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }

            byteData = newByteData.ToArray();

            FixHeaders(evolutions.Count);

            base.WriteData();
        }
    }

    public class EvolutionDataEntry
    {
        public byte[] bytes;
        public EvolutionMethod[] methods;

        public int nameID;

        public EvolutionDataEntry(byte[] bytes)
        {
            this.bytes = bytes;

            ReadData();
        }

        internal void ReadData()
        {
            methods = new EvolutionMethod[7];
            for (int i = 0; i < 7; i++)
            {
                int pos = i * 6;
                methods[i] = new EvolutionMethod((short)HelperFunctions.ReadShort(bytes, pos),
                    (short)HelperFunctions.ReadShort(bytes, pos + 2),
                    (short)HelperFunctions.ReadShort(bytes, pos + 4));
            }
        }

        internal void ApplyData()
        {
            for (int i = 0; i < 7; i++)
            {
                int pos = i * 6;
                HelperFunctions.WriteShort(bytes, pos, methods[i].method);
                HelperFunctions.WriteShort(bytes, pos + 2, methods[i].condition);
                HelperFunctions.WriteShort(bytes, pos + 4, methods[i].newPokemonID);
            }
        }

        public override string ToString()
        {
            try
            {
                return nameID < MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[nameID] : "Name not found";
            }
            catch
            {
                return "Error";
            }
        }
    }

    public struct EvolutionMethod
    {
        public short newPokemonID;
        public short method;
        public short condition;

        public EvolutionMethod(short method, short condition, short newPokemonID)
        {
            this.newPokemonID = newPokemonID;
            this.method = method;
            this.condition = condition;
        }

        public override string ToString()
        {
            try
            {
                return MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[newPokemonID] + " by " + VersionConstants.BW2_EvolutionMethodNames[method];
            }
            catch
            {
                return "Error";
            }
        }
    }
}
