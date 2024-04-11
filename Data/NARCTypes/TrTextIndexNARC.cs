using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class TrTextIndexNARC : NARC
    {
        public byte[] tableBytes;

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
            pos = pointerStartAddress;

            int start = HelperFunctions.ReadInt(byteData, pos);
            int end = HelperFunctions.ReadInt(byteData, pos + 4);
            tableBytes = new byte[end - start];
            for (int i = 0; i < end - start; i++) tableBytes[i] = byteData[initialPosition + i + start];
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
            newByteData.AddRange(tableBytes);
            newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
            pPos += 4;
            totalSize += tableBytes.Length;
            newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));

            byteData = newByteData.ToArray();

            FixHeaders(1);

            base.WriteData();
        }
    }
}
