using NewEditor.Data.NARCTypes;
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
            int pos = pointerStartAddress + fileCount * 8;

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

        public List<byte> GetFileEntry(int fileID)
        {
            if (byteData[0] != (byte)'N' || fileID < 0 || fileID >= numFileEntries) return null;

            int start = HelperFunctions.ReadInt(byteData, pointerStartAddress + fileID * 8) + FileEntryStart;
            int length = HelperFunctions.ReadInt(byteData, pointerStartAddress + fileID * 8 + 4) + FileEntryStart - start;
            List<byte> list = new List<byte>();
            for (int j = 0; j < length; j++) list.Add(byteData[start + j]);
            return list;
        }

        public void ReplaceFileEntry(int fileID, List<byte> data)
        {
            if (byteData[0] != (byte)'N' || fileID < 0 || fileID >= numFileEntries) return;

            List<byte> oldFile = GetFileEntry(fileID);
            int dif = data.Count - oldFile.Count;

            int pos = pointerStartAddress + fileID * 8;
            int fileStart = HelperFunctions.ReadInt(byteData, pointerStartAddress + fileID * 8) + FileEntryStart;
            int fileLength = HelperFunctions.ReadInt(byteData, pointerStartAddress + fileID * 8 + 4) + FileEntryStart - fileStart;

            //Update File Pointers
            for (int i = fileID; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);

                if (i != fileID)
                {
                    start += dif;
                    HelperFunctions.WriteInt(byteData, pos, start);
                }
                end += dif;
                HelperFunctions.WriteInt(byteData, pos + 4, end);
                pos += 8;
            }

            List<byte> b = new List<byte>(byteData);
            b.RemoveRange(fileStart, fileLength);
            b.InsertRange(fileStart, data);
            byteData = b.ToArray();

            FixHeaders(numFileEntries);
        }

        public void AddFileEntry(int fileID, List<byte> data)
        {
            if (byteData[0] != (byte)'N' || fileID < 0) return;

            if (fileID > numFileEntries)
            {
                AddFileEntry(fileID - 1, new List<byte>());
            }
            if (fileID < numFileEntries)
            {
                ReplaceFileEntry(fileID, data);
                return;
            }

            List<byte> b = new List<byte>(byteData);
            int pos = pointerStartAddress + numFileEntries * 8;
            b.InsertRange(pos, new byte[8]);
            b.AddRange(data);
            byteData = b.ToArray();

            FixHeaders(numFileEntries + 1);
            HelperFunctions.WriteInt(byteData, pos, byteData.Length - FileEntryStart - data.Count);
            HelperFunctions.WriteInt(byteData, pos + 4, byteData.Length - FileEntryStart);
        }
    }
}
