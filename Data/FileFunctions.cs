using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public static class FileFunctions
    {
        private static string rootDirectory = Directory.GetCurrentDirectory() + "/";

        public static void WriteFileSection(string fileName, string sectionTitle, IEnumerable<byte> data, bool overrideRoot = false)
        {
            Dictionary<string, IEnumerable<byte>> sections = ReadAllSections(fileName, overrideRoot);

            if (sections.ContainsKey(sectionTitle)) sections[sectionTitle] = data;
            else sections.Add(sectionTitle, data);

            WriteAllSections(fileName, sections, overrideRoot);
        }

        public static List<byte> ReadFileSection(string fileName, string sectionTitle, bool overrideRoot = false)
        {
            Dictionary<string, IEnumerable<byte>> sections = ReadAllSections(fileName, overrideRoot);

            if (sections.ContainsKey(sectionTitle)) return sections[sectionTitle].ToList();
            else return null;
        }

        public static void WriteAllSections(string fileName, Dictionary<string, IEnumerable<byte>> sections, bool overrideRoot = false)
        {
            string fullPath = overrideRoot ? fileName : (rootDirectory + fileName);
            if (!File.Exists(fullPath)) File.Create(fullPath).Close();

            FileStream fs = File.OpenWrite(fullPath);
            fs.SetLength(0);
            foreach (KeyValuePair<string, IEnumerable<byte>> section in sections)
            {
                fs.Write(Encoding.ASCII.GetBytes("{" + section.Key + "|"), 0, section.Key.Length + 2);
                fs.Write(BitConverter.GetBytes(section.Value.Count()), 0, 4);
                fs.Write(Encoding.ASCII.GetBytes(":"), 0, 1);
                fs.Write(section.Value.ToArray(), 0, section.Value.Count());
                fs.WriteByte((byte)'}');
            }
            fs.Close();
        }

        public static Dictionary<string, IEnumerable<byte>> ReadAllSections(string fileName, bool overrideRoot = false)
        {
            string fullPath = overrideRoot ? fileName : (rootDirectory + fileName);
            Dictionary<string, IEnumerable<byte>> sections = new Dictionary<string, IEnumerable<byte>>();
            if (!File.Exists(fullPath)) return sections;

            FileStream fs = File.OpenRead(fullPath);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '{')
                {
                    i++;
                    List<byte> name = new List<byte>();
                    while (i < data.Length && data[i] != '|')
                    {
                        name.Add(data[i]);
                        i++;
                    }
                    i++;

                    int length = BitConverter.ToInt32(data, i);
                    i += 5;
                    length += i;

                    List<byte> bytes = new List<byte>();
                    while (i < data.Length && (data[i] != '}' || i < length))
                    {
                        bytes.Add(data[i]);
                        i++;
                    }

                    sections.Add(Encoding.ASCII.GetString(name.ToArray()), bytes);
                }
                else break;
            }

            return sections;
        }

        public static void WriteText(this FileStream file, string text) => file.Write(ASCIIEncoding.ASCII.GetBytes(text), 0, text.Length);
        public static void WriteLine(this FileStream file, string text) => file.Write(ASCIIEncoding.ASCII.GetBytes(text + "\n"), 0, text.Length + 1);
    }
}
