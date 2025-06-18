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
    public class ChildPokemonNARC : NARC
    {
        public List<short> ids;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            ids = new List<short>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                ids.Add(BitConverter.ToInt16(byteData, initialPosition + start));

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
            foreach (short i in ids)
            {
                newByteData.AddRange(BitConverter.GetBytes(i));
                newByteData.Add(0xFF);
                newByteData.Add(0xFF);
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += 4;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize - 2));
                pPos += 4;
            }

            byteData = newByteData.ToArray();

            FixHeaders(ids.Count);

            base.WriteData();
        }
    }
}
