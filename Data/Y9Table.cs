using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public class Y9Table
    {
        public byte[] bytes;
        public List<Y9Entry> entries;

        public Y9Table(byte[] bytes)
        {
            this.bytes = bytes;

            entries = new List<Y9Entry>();

            for (int i = 0; i < bytes.Length; i += 32)
            {
                Y9Entry entry = new Y9Entry()
                {
                    id = HelperFunctions.ReadInt(bytes, i),
                    mountAddress = HelperFunctions.ReadInt(bytes, i + 4),
                    mountSize = HelperFunctions.ReadInt(bytes, i + 8),
                    BSSSize = HelperFunctions.ReadInt(bytes, i + 12),
                    staticInitAddress = HelperFunctions.ReadInt(bytes, i + 16),
                    staticInitEndAddress = HelperFunctions.ReadInt(bytes, i + 20),
                    compressedSize = HelperFunctions.ReadInt(bytes, i + 28) & 0xFFFFFF,
                    compressed = bytes[i + 31] == 3
                };
                entries.Add(entry);
            }
        }
    }

    public class Y9Entry
    {
        public int id;
        public int mountAddress;
        public int mountSize;
        public int BSSSize;
        public int staticInitAddress;
        public int staticInitEndAddress;
        public int compressedSize;
        public bool compressed;

        public override string ToString()
        {
            return "----------\nOverlay: " + id + "\nMount Address: 0x" + mountAddress.ToString("X") + "\nMount Size: 0x" + mountSize.ToString("X") + "\nBSS Size: 0x" + BSSSize.ToString("X") + "\nStatic Init Address: 0x" + staticInitAddress.ToString("X") +
                "\nStatic Init End Address: 0x" + staticInitEndAddress.ToString("X") + "\nCompressed Size: 0x" + compressedSize.ToString("X") + "\nCompressed: " + compressed;
        }
    }
}
