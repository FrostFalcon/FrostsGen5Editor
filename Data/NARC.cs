using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public class NARC
    {
        public NDSFileSystem fileSystem;
        protected int pointerStartAddress = 28;
        public byte[] byteData;
        public short ID;
        public bool overrideWrite = false;

        public int numFileEntries => (byteData != null && byteData.Length > 32) ? HelperFunctions.ReadInt(byteData, 24) : 0;

        public virtual void ReadData()
        {
            
        }

        public virtual void WriteData()
        {
            
        }

        protected void FixHeaders(int fileCount)
        {
            int pos = 0;
            while (pos < byteData.Length)
            {
                pos++;
                if (pos >= byteData.Length) return;
                if (byteData[pos] == 'B' && byteData[pos + 1] == 'T' && byteData[pos + 2] == 'N' && byteData[pos + 3] == 'F') break;
            }

            HelperFunctions.WriteInt(byteData, 8, byteData.Length);
            HelperFunctions.WriteInt(byteData, 24, fileCount);
            HelperFunctions.WriteInt(byteData, 20, pos - 16);
            HelperFunctions.WriteInt(byteData, pos + 20, byteData.Length - (pos + 16));
        }

        public virtual byte[] GetPatchBytes(NARC other) => byteData;
        public virtual void ReadPatchBytes(byte[] bytes)
        {
            byteData = bytes;
            ReadData();
        }

        public virtual void DumpNarc(string path)
        {
            File.WriteAllBytes(path, byteData.ToArray());
        }

        public virtual void ReadNarcDump(string path)
        {
            byteData = File.ReadAllBytes(path);
        }
    }
}
