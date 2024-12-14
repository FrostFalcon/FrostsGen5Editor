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
    public class MapModelsNARC : NARC
    {
        public List<MapModelEntry> models;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            models = new List<MapModelEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                MapModelEntry m = new MapModelEntry(bytes) { nameID = i };
                models.Add(m);

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
            foreach (MapModelEntry m in models)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (MapModelEntry m in models)
            {
                newByteData.AddRange(m.bytes);
            }
            byteData = newByteData.ToArray();

            FixHeaders(models.Count);
            base.WriteData();
        }
    }

    public class MapModelEntry
    {
        public byte[] bytes;
        public int nameID;



        public MapModelEntry(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public override string ToString()
        {
            return nameID.ToString();
        }
    }

    public class BuildingEntry
    {
        public short preX;
        public short x;
        public short preY;
        public short y;
        public short preZ;
        public short z;
    }
}
