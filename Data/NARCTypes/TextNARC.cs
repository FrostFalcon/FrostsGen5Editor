using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using NewEditor.Forms;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace NewEditor.Data.NARCTypes
{
    public class TextNARC : NARC
    {
        public List<TextFile> textFiles;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register text files
            textFiles = new List<TextFile>();

            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                textFiles.Add(new TextFile(bytes));
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
            foreach (TextFile t in textFiles)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += t.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (TextFile t in textFiles)
            {
                newByteData.AddRange(t.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(textFiles.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            TextNARC other = narc as TextNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                bool lineChanges = false;
                if (textFiles[i].text.Count != other.textFiles[i].text.Count) lineChanges = true;
                else
                {
                    for (int j = 0; j < textFiles[i].text.Count; j++)
                        if (textFiles[i].text[j] != other.textFiles[i].text[j])
                        {
                            lineChanges = true;
                            break;
                        }
                }
                if (i > other.textFiles.Count || lineChanges)
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(textFiles[i].bytes.Length));
                    bytes.AddRange(textFiles[i].bytes);
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

                if (id > textFiles.Count)
                {
                    //Don't accept extra files here
                }
                else textFiles[id] = new TextFile(new List<byte>(bytes).GetRange(pos, size).ToArray());
                pos += size;
            }
        }

        public void ApplyTextList(RichTextBox textbox, int fileIndex)
        {
            TextFile t = textFiles[fileIndex];

            t.text = new List<string>();
            for (int i = 0; i < textbox.Lines.Length; i++)
            {
                t.text.Add(textbox.Lines[i].Replace("[C]", "\\xf000븁\\x0000\\xfffe")
                    .Replace("[L]", "\\xf000븀\\x0000\\xfffe")
                    .Replace("[N]", "\\xfffe")
                    .Replace("[E]", "\\xf000븁\\x0000")
                    .Replace("[V0]", "\\xf000Ā\\x0001\\x0000")
                    .Replace("[V1]", "\\xf000Ā\\x0001\\x0001")
                    .Replace("[V2]", "\\xf000Ā\\x0001\\x0002")
                    .Replace("[V3]", "\\xf000Ā\\x0001\\x0003"));
            }
            t.CompressData();
        }

        public string GetLine(int file, int line) => textFiles[file].text[line];
    }

    public class TextFile
    {
        public byte[] bytes;
        public List<string> text;

        public TextFile(byte[] bytes)
        {
            this.bytes = bytes;

            if (MainEditor.RomType == RomType.BW2 || MainEditor.RomType == RomType.BW1)
            {
                this.text = PPTxtHandler.GetStrings(bytes);
            }
            else
            {
                PokeTextData ptd = new PokeTextData(bytes);
                ptd.decrypt();
                this.text = ptd.strlist;
            }
        }

        public void CompressData()
        {
            if (MainEditor.RomType == RomType.BW2 || MainEditor.RomType == RomType.BW1)
            {
                bytes = PPTxtHandler.SaveEntry(bytes, text);
            }
            else
            {
                byte[] b = TextToPoke.MakeFile(text, false);
                PokeTextData encrypt = new PokeTextData(b);
                encrypt.SetKey(0xD00E);
                encrypt.encrypt();
                bytes = encrypt.get();
            }
        }
    }
}
