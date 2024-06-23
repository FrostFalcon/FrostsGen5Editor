using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class PokemonIconNARC : NARC
    {
        public List<PokemonIconFile> files;

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
            files = new List<PokemonIconFile>();

            pos = pointerStartAddress;

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                PokemonIconFile pi = new PokemonIconFile(bytes);
                files.Add(pi);

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
            foreach (PokemonIconFile m in files)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (PokemonIconFile m in files)
            {
                newByteData.AddRange(m.bytes);
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
                if (i > other.files.Count || !files[i].bytes.SequenceEqual(other.files[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(files[i].bytes.Length));
                    bytes.AddRange(files[i].bytes);
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
                else files[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class PokemonIconFile
    {
        public byte[] bytes;

        public PokemonIconFile(byte[] bytes)
        {
            this.bytes = bytes;
        }
    }
}
