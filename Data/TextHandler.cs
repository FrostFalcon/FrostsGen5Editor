using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;
using NewEditor.Data.NARCTypes;
using System.Runtime.InteropServices;

//Modified version of the UPR's version of PPTxtHandler

namespace NewEditor.Data
{
    public static class PPTxtHandler
    {
        static List<int> keys;
        static List<int> unknowns;

        //Decompression
        public static List<string> GetStrings(byte[] ds)
        {
            List<string> strings = new List<string>();
            keys = new List<int>();
            unknowns = new List<int>();
            {
                int pos = 0;
                int i = 0;

                int numSections, numEntries, tmpCharCount, tmpUnknown, tmpChar;
                int tmpOffset;
                int[] sizeSections = new int[16];
                int[] sectionOffset = new int[16];
                Dictionary<int, List<int>> tableOffsets = new Dictionary<int, List<int>>();
                Dictionary<int, List<int>> characterCount = new Dictionary<int, List<int>>();
                Dictionary<int, List<int>> unknown = new Dictionary<int, List<int>>();
                Dictionary<int, List<List<int>>> encText = new Dictionary<int, List<List<int>>>();
                Dictionary<int, List<List<string>>> decText = new Dictionary<int, List<List<string>>>();
                string str;
                int key;

                numSections = HelperFunctions.ReadShort(ds, 0);
                numEntries = HelperFunctions.ReadShort(ds, 2);
                sizeSections[0] = HelperFunctions.ReadInt(ds, 4);

                pos += 12;
                if (numSections > i)
                {
                    for (int z = 0; z < Math.Min(numSections, sectionOffset.Length); z++)
                    {
                        sectionOffset[z] = HelperFunctions.ReadInt(ds, pos);
                        pos += 4;
                    }
                    pos = sectionOffset[i];
                    sizeSections[i] = HelperFunctions.ReadInt(ds, pos);
                    pos += 4;

                    tableOffsets.Add(i, new List<int>());
                    characterCount.Add(i, new List<int>());
                    unknown.Add(i, new List<int>());
                    encText.Add(i, new List<List<int>>());
                    decText.Add(i, new List<List<string>>());

                    for (int j = 0; j < numEntries; j++)
                    {
                        tmpOffset = HelperFunctions.ReadInt(ds, pos);
                        pos += 4;
                        tmpCharCount = HelperFunctions.ReadShort(ds, pos);
                        pos += 2;
                        tmpUnknown = HelperFunctions.ReadShort(ds, pos);
                        pos += 2;
                        tableOffsets[i].Add(tmpOffset);
                        characterCount[i].Add(tmpCharCount);
                        unknown[i].Add(tmpUnknown);
                        unknowns.Add(tmpUnknown);
                    }
                    for (int j = 0; j < numEntries; j++)
                    {
                        List<int> tmpEncChars = new List<int>();
                        pos = sectionOffset[i] + tableOffsets[i][j];
                        for (int k = 0; k < characterCount[i][j]; k++)
                        {
                            tmpChar = HelperFunctions.ReadShort(ds, pos);
                            pos += 2;
                            tmpEncChars.Add(tmpChar);
                        }
                        encText[i].Add(tmpEncChars);
                        key = encText[i][j][characterCount[i][j] - 1] ^ 0xFFFF;
                        for (int k = characterCount[i][j] - 1; k >= 0; k--)
                        {
                            encText[i][j][k] = encText[i][j][k] ^ key;
                            if (k == 0)
                            {
                                keys.Add(key);
                            }
                            key = ((key >> 3) | (key << 13)) & 0xffff;
                        }
                        if (encText[i][j][0] == 0xF100)
                        {
                            encText[i][j] = Decompress(encText[i][j]);
                            characterCount[i][j] = encText[i][j].Count;
                        }

                        List<string> chars = new List<string>();
                        str = "";
                        for (int k = 0; k < characterCount[i][j]; k++)
                        {
                            if (encText[i][j][k] == 0xFFFF)
                            {
                                chars.Add("\\xFFFF");
                            }
                            else
                            {
                                if (encText[i][j][k] > 20 && encText[i][j][k] <= 0xFFF0 && encText[i][j][k] != 0xF000)
                                {
                                    chars.Add("" + (char)encText[i][j][k]);
                                }
                                else
                                {
                                    string num = encText[i][j][k].ToString("x4");
                                    chars.Add("\\x" + num);
                                }
                                str += chars[k];
                            }
                        }
                        strings.Add(str);
                        decText[i].Add(chars);
                    }
                }
            }
            return strings;
        }

        private static List<int> Decompress(List<int> chars)
        {
            List<int> uncomp = new List<int>();
            int j = 1;
            int shift1 = 0;
            int trans = 0;
            while (true)
            {
                int tmp = chars[j];
                tmp = tmp >> shift1;
                int tmp1 = tmp;
                if (shift1 >= 0x10)
                {
                    shift1 -= 0x10;
                    if (shift1 > 0)
                    {
                        tmp1 = (trans | ((chars[j] << (9 - shift1)) & 0x1FF));
                        if ((tmp1 & 0xFF) == 0xFF)
                        {
                            break;
                        }
                        if (tmp1 != 0x0 && tmp1 != 0x1)
                        {
                            uncomp.Add(tmp1);
                        }
                    }
                }
                else
                {
                    tmp1 = ((chars[j] >> shift1) & 0x1FF);
                    if ((tmp1 & 0xFF) == 0xFF)
                    {
                        break;
                    }
                    if (tmp1 != 0x0 && tmp1 != 0x1)
                    {
                        uncomp.Add(tmp1);
                    }
                    shift1 += 9;
                    if (shift1 < 0x10)
                    {
                        trans = (chars[j] >> shift1) & 0x1FF;
                        shift1 += 9;
                    }
                    j += 1;
                }
            }
            return uncomp;
        }

        //Compression
        public static byte[] SaveEntry(byte[] originalData, List<string> text)
        {

            // Parse strings against the reverse table
            //for (int sn = 0; sn < text.size(); sn++)
            //{
            //    text.set(sn, bulkReplace(text.get(sn), textToPokePattern, textToPoke));
            //}

            // Make sure we have the original unknowns etc
            GetStrings(originalData);

            // Start getting stuff
            int numSections, numEntries;
            int[] sizeSections = new int[] { 0, 0, 0 };
            int[] sectionOffset = new int[] { 0, 0, 0 };
            int[] newsizeSections = new int[] { 0, 0, 0 };
            int[] newsectionOffset = new int[] { 0, 0, 0 };

            // Data-Stream
            byte[] ds = originalData;
            int pos = 0;

            numSections = HelperFunctions.ReadShort(ds, 0);
            numEntries = text.Count;
            HelperFunctions.WriteShort(ds, 2, numEntries);
            sizeSections[0] = HelperFunctions.ReadInt(ds, 4);

            pos += 12;
            while (text.Count < numEntries)
            {
                text.Add("");
            }
            while (keys.Count() < text.Count) keys.Add(0);
            while (unknowns.Count() < text.Count) unknowns.Add(0);

            byte[] newEntry = MakeSection(text, numEntries);
            for (int z = 0; z < numSections; z++)
            {
                sectionOffset[z] = HelperFunctions.ReadInt(ds, pos);
                pos += 4;
            }
            for (int z = 0; z < numSections; z++)
            {
                pos = sectionOffset[z];
                sizeSections[z] = HelperFunctions.ReadInt(ds, pos);
                pos += 4;
            }
            newsizeSections[0] = newEntry.Length;

            byte[] newData = new byte[ds.Length - sizeSections[0] + newsizeSections[0]];
            Array.Copy(ds, 0, newData, 0, Math.Min(ds.Length, newData.Length));
            HelperFunctions.WriteInt(newData, 4, newsizeSections[0]);
            if (numSections == 2)
            {
                newsectionOffset[1] = newsizeSections[0] + sectionOffset[0];
                HelperFunctions.WriteInt(newData, 0x10, newsectionOffset[1]);
            }
            Array.Copy(newEntry, 0, newData, sectionOffset[0], newEntry.Length);
            if (numSections == 2)
            {
                Array.Copy(ds, sectionOffset[1], newData, newsectionOffset[1], sizeSections[1]);
            }
            return newData;
        }

        private static byte[] MakeSection(List<string> strings, int numEntries)
        {
            List<List<int>> data = new List<List<int>>();
            int size = 0;
            int offset = 4 + 8 * numEntries;
            int charCount = 0;
            for (int i = 0; i < numEntries; i++)
            {
                data.Add(ParseString(strings[i], i));
                size += data[i].Count * 2;
            }
            if (size % 4 == 2)
            {
                size += 2;
                int tmpKey = keys[numEntries - 1];
                for (int i = 0; i < data[numEntries - 1].Count; i++)
                {
                    tmpKey = ((tmpKey << 3) | (tmpKey >> 13)) & 0xFFFF;
                }
                data[numEntries - 1].Add(0xFFFF ^ tmpKey);
            }
            size += offset;
            byte[] section = new byte[size];
            int pos = 0;
            HelperFunctions.WriteInt(section, pos, size);
            pos += 4;
            for (int i = 0; i < numEntries; i++)
            {
                charCount = data[i].Count;
                HelperFunctions.WriteInt(section, pos, offset);
                pos += 4;
                HelperFunctions.WriteShort(section, pos, charCount);
                pos += 2;
                HelperFunctions.WriteShort(section, pos, unknowns[i]);
                pos += 2;
                offset += (charCount * 2);
            }
            for (int i = 0; i < numEntries; i++)
            {
                foreach (int word in data[i])
                {
                    HelperFunctions.WriteShort(section, pos, word);
                    pos += 2;
                }
            }
            return section;
        }

        private static List<int> ParseString(string str, int entry_id)
        {
            List<int> chars = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '\\')
                {
                    chars.Add(str[i]);
                }
                else
                {
                    if (((i + 2) < str.Length) && str[i + 2] == '{')
                    {
                        chars.Add(str[i]);
                    }
                    else
                    {
                        chars.Add(int.Parse(str.Substring(i + 2, 4), System.Globalization.NumberStyles.HexNumber));
                        i += 5;
                    }
                }
            }
            chars.Add(0xFFFF);
            int key = keys[entry_id];
            for (int i = 0; i < chars.Count; i++)
            {
                chars[i] = (chars[i] ^ key) & 0xFFFF;
                key = ((key << 3) | (key >> 13)) & 0xFFFF;
            }
            return chars;
        }
    }

    public class PokeTextData
    {
        private byte[] data;
        public List<PointerEntry> ptrlist;
        public List<string> strlist;
        public bool compressFlag;

        public PokeTextData(byte[] data)
        {
            this.data = data.ToArray();
        }

        public byte[] get()
        {
            return data;
        }

        private int read16(int ofs)
        {
            return (data[ofs] & 0xFF) | ((data[ofs + 1] & 0xFF) << 8);
        }

        private void write16(int d, int ofs)
        {
            data[ofs] = (byte)(d & 0xFF);
            data[ofs + 1] = (byte)((d >> 8) & 0xFF);
        }

        private int read32(int ofs)
        {
            return (data[ofs] & 0xFF) | ((data[ofs + 1] & 0xFF) << 8) | ((data[ofs + 2] & 0xFF) << 16)
                    | ((data[ofs + 3] & 0xFF) << 24);
        }

        private void write32(int d, int ofs)
        {
            data[ofs] = (byte)(d & 0xFF);
            data[ofs + 1] = (byte)((d >> 8) & 0xFF);
            data[ofs + 2] = (byte)((d >> 16) & 0xFF);
            data[ofs + 3] = (byte)((d >> 24) & 0xFF);
        }

        public void decrypt()
        {
            DecyptPtrs(read16(0), read16(2), 4);
            this.ptrlist = CreatePtrList(read16(0), 4);

            this.strlist = new List<string>();

            int num = read16(0);

            for (int i = 0; i < num; i++)
            {
                PointerEntry entry = this.ptrlist[i];
                DecyptTxt(entry.getChars(), i + 1, entry.getPtr());
                this.strlist.Add(MakeString(entry.getChars(), entry.getPtr()));
            }
        }

        public void encrypt()
        {
            this.ptrlist = CreatePtrList(read16(0), 4);
            int num = read16(0);
            for (int i = 0; i < num; i++)
            {
                PointerEntry entry = this.ptrlist[i];
                DecyptTxt(entry.getChars(), i + 1, entry.getPtr());
            }

            DecyptPtrs(read16(0), read16(2), 4);
        }

        private void DecyptPtrs(int count, int key, int sdidx)
        {
            key = (key * 0x2FD) & 0xFFFF;

            for (int i = 0; i < count; i++)
            {
                int key2 = (key * (i + 1) & 0xFFFF);
                int realkey = key2 | (key2 << 16);
                write32(read32(sdidx) ^ realkey, sdidx);
                write32(read32(sdidx + 4) ^ realkey, sdidx + 4);
                sdidx += 8;
            }

        }

        private List<PointerEntry> CreatePtrList(int count, int sdidx)
        {
            List<PointerEntry> ptrlist = new List<PointerEntry>();
            for (int i = 0; i < count; i++)
            {
                ptrlist.Add(new PointerEntry(read32(sdidx), read32(sdidx + 4)));
                sdidx += 8;
            }
            return ptrlist;
        }

        private void DecyptTxt(int count, int id, int idx)
        {
            int key = (0x91BD3 * id) & 0xFFFF;
            for (int i = 0; i < count; i++)
            {
                write16(read16(idx) ^ key, idx);
                key += 0x493D;
                key = key & 0xFFFF;
                idx += 2;
            }

        }

        private string MakeString(int count, int idx)
        {
            StringBuilder str = new StringBuilder();
            List<int> chars = new List<int>();
            List<int> uncomp = new List<int>();
            for (int j = 0; j < count; j++)
            {
                chars.Add(read16(idx));
                idx += 2;
            }

            if (chars[0] == 0xF100)
            {
                compressFlag = true;
                int j = 1;
                int shift1 = 0;
                int trans = 0;
                while (true)
                {
                    int tmp = chars[j];
                    tmp = tmp >> shift1;
                    int tmp1 = tmp;
                    if (shift1 >= 0xF)
                    {
                        shift1 -= 0xF;
                        if (shift1 > 0)
                        {
                            tmp1 = (trans | ((chars[j] << (9 - shift1)) & 0x1FF));
                            if (tmp1 == 0x1FF)
                            {
                                break;
                            }
                            uncomp.Add(tmp1);
                        }
                    }
                    else
                    {
                        tmp1 = ((chars[j] >> shift1) & 0x1FF);
                        if (tmp1 == 0x1FF)
                        {
                            break;
                        }
                        uncomp.Add(tmp1);
                        shift1 += 9;
                        if (shift1 < 0xF)
                        {
                            trans = ((chars[j] >> shift1) & 0x1FF);
                            shift1 += 9;
                        }
                        j += 1;
                    }
                }
                chars = uncomp;
            }
            int i = 0;
            for (int c = 0; c < chars.Count; c++)
            {
                int currChar = chars[i];
                if (UnicodeParser.tb[currChar] != null)
                {
                    str.Append(UnicodeParser.tb[currChar]);
                }
                else
                {
                    if (currChar == 0xFFFE)
                    {
                        i++;
                        str.Append("\\v" + chars[i].ToString("X4"));
                        i++;
                        int total = chars[i];
                        for (int z = 0; z < total; z++)
                        {
                            i++;
                            str.Append("\\z" + chars[i].ToString("X4"));
                        }
                    }
                    else if (currChar == 0xFFFF)
                    {
                        break;
                    }
                    else
                    {
                        str.Append("\\x" + chars[i].ToString("X4"));
                    }
                }
                i++;
            }
            return str.ToString();
        }

        public void SetKey(int key)
        {
            write16(key, 2);
        }

        public int GetKey()
        {
            return read16(2);
        }

        public class PointerEntry
        {
            private int ptr;
            private int chars;

            public PointerEntry(int ptr, int chars)
            {
                this.ptr = ptr;
                this.chars = chars;
            }

            public int getPtr()
            {
                return ptr;
            }

            public int getChars()
            {
                return chars;
            }
        }
    }

    public static class TextToPoke
    {
        public static byte[] MakeFile(List<string> textarr, bool compressed)
        {
            int bas = textarr.Count * 8 + 4;
            List<PointerEntry> ptrtable = new List<PointerEntry>();
            List<List<int>> rawdata = new List<List<int>>();
            for (int i = 0; i < textarr.Count; i++)
            {
                List<int> data = ToCode(textarr[i], compressed);
                int l = data.Count;
                ptrtable.Add(new PointerEntry(bas, l));
                rawdata.Add(data);
                bas += l * 2;
            }

            List<int> hdr = new List<int>() { textarr.Count, 0 };

            return join(new List<byte[]>() { wordListToBarr(hdr), pointerListToBarr(ptrtable), listOfWordListToBarr(rawdata) });
        }

        private static List<int> ToCode(string text, bool compressed)
        {
            List<int> data = new List<int>();
            while (text.Length != 0)
            {
                int i = Math.Max(0, 6 - text.Length);
                if (text[0] == '\\')
                {
                    if (text[1] == 'x')
                    {
                        data.Add(int.Parse(text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber));
                        text = text.Substring(6);
                    }
                    else if (text[1] == 'v')
                    {
                        data.Add(0xFFFE);
                        data.Add(int.Parse(text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber));
                        text = text.Substring(6);
                    }
                    else if (text[1] == 'z')
                    {
                        List<int> var = new List<int>();
                        int w = 0;
                        while (text.Length != 0)
                        {
                            if (text[0] == '\\' && text[1] == 'z')
                            {
                                w++;
                                var.Add(int.Parse(text.Substring(2, 4), System.Globalization.NumberStyles.HexNumber));
                                text = text.Substring(6);
                            }
                            else
                            {
                                break;
                            }
                        }
                        data.Add(w);
                        data.AddRange(var);
                    }
                    else if (text[1] == 'n')
                    {
                        data.Add(0xE000);
                        text = text.Substring(2);
                    }
                    else if (text[1] == 'p')
                    {
                        data.Add(0x25BC);
                        text = text.Substring(2);
                    }
                    else if (text[1] == 'l')
                    {
                        data.Add(0x25BD);
                        text = text.Substring(2);
                    }
                    else if (text.Substring(1, 3).Equals("and"))
                    {
                        data.Add(0x1C2);
                        text = text.Substring(4);
                    }
                    else
                    {
                        text = text.Substring(2);
                    }
                }
                else
                {
                    while (!(UnicodeParser.d.ContainsKey(text.Substring(0, 6 - i)) || (i == 6)))
                    {
                        i++;
                    }
                    if (i == 6)
                    {
                        text = text.Substring(1);
                    }
                    else
                    {
                        data.Add(UnicodeParser.d[text.Substring(0, 6 - i)]);
                        text = text.Substring(6 - i);
                    }
                }
            }
            if (compressed)
            {
                if (data.Count % 5 != 0 || data.Count == 0)
                {
                    data.Add(0x1FF);
                }
                byte[] bits = new byte[data.Count * 9];
                int bc = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        bits[bc++] = (byte)((data[i] >> j) & 1);
                    }
                }
                int tmp_uint16 = 0;
                data.Clear();
                data.Add(0xF100);
                for (int i = 0; i < bits.Length; i++)
                {
                    if (i % 15 == 0 && i != 0)
                    {
                        data.Add(tmp_uint16);
                        tmp_uint16 = 0;
                    }
                    tmp_uint16 |= (bits[i] << (i % 15));
                }
                data.Add(tmp_uint16);
            }
            data.Add(0xFFFF);
            return data;
        }

        private static byte[] join(List<byte[]> args)
        {
            int tlen = 0;
            foreach (byte[] arr in args)
            {
                tlen += arr.Length;
            }
            byte[] barr = new byte[tlen];
            int offs = 0;
            foreach (byte[] arr in args)
            {
                Array.Copy(arr, 0, barr, offs, arr.Length);
                offs += arr.Length;
            }
            return barr;
        }

        private static byte[] wordListToBarr(List<int> list)
        {
            byte[] barr = new byte[list.Count * 2];
            int l = list.Count;
            for (int i = 0; i < l; i++)
            {
                barr[i * 2] = (byte)(list[i] & 0xFF);
                barr[i * 2 + 1] = (byte)((list[i] >> 8) & 0xFF);
            }
            return barr;
        }

        private static byte[] pointerListToBarr(List<PointerEntry> ptrList)
        {
            byte[] data = new byte[ptrList.Count * 8];
            int l = ptrList.Count;
            for (int i = 0; i < l; i++)
            {
                int ofs = i * 8;
                PointerEntry ent = ptrList[i];
                data[ofs] = (byte)(ent.ptr & 0xFF);
                data[ofs + 1] = (byte)((ent.ptr >> 8) & 0xFF);
                data[ofs + 2] = (byte)((ent.ptr >> 16) & 0xFF);
                data[ofs + 3] = (byte)((ent.ptr >> 24) & 0xFF);
                data[ofs + 4] = (byte)(ent.chars & 0xFF);
                data[ofs + 5] = (byte)((ent.chars >> 8) & 0xFF);
                data[ofs + 6] = (byte)((ent.chars >> 16) & 0xFF);
                data[ofs + 7] = (byte)((ent.chars >> 24) & 0xFF);
            }
            return data;
        }

        private static byte[] listOfWordListToBarr(List<List<int>> list)
        {
            int tlen = 0;
            foreach (List<int> subList in list)
            {
                tlen += subList.Count * 2;
            }
            byte[] barr = new byte[tlen];
            int offs = 0;
            int l1 = list.Count;
            for (int j = 0; j < l1; j++)
            {
                List<int> slist = list[j];
                int l2 = slist.Count;
                for (int i = 0; i < l2; i++)
                {
                    barr[offs] = (byte)(slist[i] & 0xFF);
                    barr[offs + 1] = (byte)((slist[i] >> 8) & 0xFF);
                    offs += 2;
                }
            }
            return barr;
        }

        private class PointerEntry
        {
            internal int ptr;
            internal int chars;

            public PointerEntry(int ptr, int chars)
            {
                this.ptr = ptr;
                this.chars = chars;
            }
        }
    }


    public static class UnicodeParser
    {
        public static string[] tb = new string[65536];
        public static Dictionary<string, int> d = new Dictionary<string, int>();

        static UnicodeParser()
        {
            string[] strs = Gen4UnicodeTable.tableData.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string s in strs)
            {
                if (s.Trim() != "")
                {
                    string[] r = s.Split('=');
                    if (!d.ContainsKey(r[1]))
                    {
                        tb[int.Parse(r[0], System.Globalization.NumberStyles.HexNumber)] = r[1];
                        d.Add(r[1], int.Parse(r[0], System.Globalization.NumberStyles.HexNumber));
                    }
                }
            }
        }
    }
}
