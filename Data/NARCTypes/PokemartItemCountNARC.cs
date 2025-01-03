using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class PokemartItemCountNARC : NARC
    {
        public List<byte> itemCounts;

        public override void ReadData()
        {
            base.ReadData();

            //Register data files
            itemCounts = new List<byte>();

            for (int i = FileEntryStart; i < byteData.Length; i++)
            {
                itemCounts.Add(byteData[i]);
            }
        }

        public override void WriteData()
        {
            List<byte> newByteData = new List<byte>(byteData);

            int initialPos = FileEntryStart;

            foreach (byte b in itemCounts)
            {
                newByteData[initialPos] = b;
                initialPos++;
            }

            byteData = newByteData.ToArray();

            FixHeaders(1);

            base.WriteData();
        }
    }
}
