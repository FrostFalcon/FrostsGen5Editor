using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public class FileNameTable
    {
        public byte[] data;
        public Dictionary<int, string> fileNames;
        public Dictionary<string, int> reverseFileNames;
        public int firstFile;

        public FileNameTable(byte[] data)
        {
            this.data = data;
            firstFile = HelperFunctions.ReadShort(data, 4);

            fileNames = new Dictionary<int, string>();
            reverseFileNames = new Dictionary<string, int>();

            int pos = 0;
            List<(int, string)> stack = new List<(int, string)> { (0, "") };

            while (stack.Count > 0)
            {
                pos = stack[0].Item1;
                string dir = stack[0].Item2;
                stack.RemoveAt(0);

                int fildID = HelperFunctions.ReadShort(data, pos + 4);
                pos = HelperFunctions.ReadInt(data, pos);

                while (data[pos] != 0)
                {
                    int len = data[pos] & 127;
                    bool subDir = data[pos] >= 128;
                    string name = Encoding.ASCII.GetString(data, pos + 1, len);

                    pos += len + 1;

                    if (subDir)
                    {
                        int id = HelperFunctions.ReadShort(data, pos) & 0xFFF;
                        stack.Add((id * 8, dir + name + "/"));
                        pos += 2;
                    }
                    else
                    {
                        fileNames.Add(fildID, dir + name);
                        reverseFileNames.Add(dir + name, fildID);
                        fildID++;
                    }
                }
            }
        }

        public int NarcToFileID(int narcID)
        {
            string output = "a/";
            int onesPlace = narcID % 10;
            int tensPlace = narcID % 100 - onesPlace;
            int hundredsPlace = narcID - tensPlace - onesPlace;
            output += hundredsPlace.ToString()[0] + "/" + tensPlace.ToString()[0] + "/" + onesPlace.ToString()[0];
            return reverseFileNames[output];
        }
    }
}
