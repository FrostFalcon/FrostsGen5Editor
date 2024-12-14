using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;
using static System.Net.WebRequestMethods;

namespace NewEditor.Data.NARCTypes
{
    public class MoveAnimationNARC : NARC
    {
        public List<MoveAnimationEntry> animations;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            animations = new List<MoveAnimationEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                MoveAnimationEntry m = new MoveAnimationEntry(bytes) { nameID = i };
                animations.Add(m);

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
            foreach (MoveAnimationEntry m in animations)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (MoveAnimationEntry m in animations)
            {
                newByteData.AddRange(m.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(animations.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            MoveAnimationNARC other = narc as MoveAnimationNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.animations.Count || !animations[i].bytes.SequenceEqual(other.animations[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(animations[i].bytes.Length));
                    bytes.AddRange(animations[i].bytes);
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

                if (id > animations.Count)
                {
                    //Don't accept extra files here
                }
                else animations[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class MoveAnimationEntry
    {
        public byte[] bytes;
        public int nameID;

        public MoveAnimationEntry(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public override string ToString()
        {
            return nameID < MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text[nameID] + " - " + nameID : "Name not found";
        }
    }
}


/*
-----Move animation Commands-----

generally start with for 1 turn attacks (not fly, dig, etc.)
01 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00 3C 00 00 00

end with
38 00 00 00 00 00 4D 00

Zoom in on user
00 00 01 00 00 00 09 00 00 00 10 00 00 00 00 00 00 00 08 00 00 00 3A 00 00 00 00 00

Zoom in on target
00 00 01 00 00 00 0B 00 00 00 10 00 00 00 00 00 00 00 08 00 00 00 38 00 00 00 00 00


*/