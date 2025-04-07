using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected int BTNFPosition => (byteData == null || byteData.Length < 32) ? 0 : HelperFunctions.ReadInt(byteData, 20) + 16;
        protected int FileEntryStart => BTNFPosition == 0 ? 0 : HelperFunctions.ReadInt(byteData, BTNFPosition + 4) + BTNFPosition + 8;
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
            HelperFunctions.WriteInt(byteData, FileEntryStart - 4, byteData.Length - (pos + 16));
        }

        public virtual byte[] GetPatchBytes(NARC other) => byteData;
        public virtual void ReadPatchBytes(byte[] bytes)
        {
            byteData = bytes;
            ReadData();
        }

        public virtual void DumpNarc(string path)
        {
            //if (Encoding.ASCII.GetString(byteData, 0, 4) != "NARC")
                File.WriteAllBytes(path, byteData.ToArray());
            //else
            //{
            //    Directory.CreateDirectory(path);
            //    string format = "D" + Math.Ceiling(Math.Log10(numFileEntries));
            //    List<Task> tasks = new List<Task>();
            //
            //    for (int i = 0; i < numFileEntries; i++)
            //    {
            //        int id = i;
            //        tasks.Add(Task.Run(() =>
            //        {
            //            int start = HelperFunctions.ReadInt(byteData, pointerStartAddress + 8 * id);
            //            int len = HelperFunctions.ReadInt(byteData, pointerStartAddress + 4 + 8 * id) - start;
            //            byte[] entry = new byte[len];
            //            for (int j = 0; j < len; j++) entry[j] = byteData[start + j];
            //            File.WriteAllBytes(path + "/" + id.ToString(format), entry);
            //        }));
            //    }
            //}
        }

        public virtual void ReadNarcDump(string path)
        {
            byteData = File.ReadAllBytes(path);
        }
    }
}
