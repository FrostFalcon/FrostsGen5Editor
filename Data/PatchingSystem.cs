using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using NewEditor.Data.NARCTypes;
using NewEditor.Forms;
using System.Drawing;

namespace NewEditor.Data
{
    public static class PatchingSystem
    {
        public static Dictionary<string, IEnumerable<byte>> MakePatch(NDSFileSystem newRom, NDSFileSystem oldRom)
        {
            Dictionary<string, IEnumerable<byte>> output = new Dictionary<string, IEnumerable<byte>>();
            output.Add("version", Encoding.ASCII.GetBytes(newRom.romType));

            MakeArm9(newRom, oldRom, output);

            //Y9
            if (!newRom.y9.bytes.SequenceEqual(oldRom.y9.bytes))
            {
                List<byte> bytes = new List<byte>();
                bytes.AddRange(BitConverter.GetBytes(oldRom.y9.bytes.Length > newRom.y9.bytes.Length ? oldRom.y9.bytes.Length - newRom.y9.bytes.Length : 0));
                bytes.AddRange(BitConverter.GetBytes(newRom.y9.bytes.Length > oldRom.y9.bytes.Length ? newRom.y9.bytes.Length - oldRom.y9.bytes.Length : 0));
                bytes.AddRange(new List<byte>(newRom.y9.bytes).GetRange(oldRom.y9.bytes.Length, newRom.y9.bytes.Length - oldRom.y9.bytes.Length));

                for (int i = 0; i < Math.Min(newRom.y9.entries.Count, oldRom.y9.entries.Count); i++)
                {
                    List<byte> b1 = new List<byte>(newRom.y9.bytes).GetRange(i * 32, 32);
                    List<byte> b2 = new List<byte>(oldRom.y9.bytes).GetRange(i * 32, 32);
                    if (!b1.SequenceEqual(b2))
                    {
                        bytes.AddRange(b1);
                    }
                }
                output.Add("y9", bytes);
            }

            List<OverlayPatch> ovList = new List<OverlayPatch>();
            for (int i = 0; i < Math.Min(newRom.overlays.Count, oldRom.overlays.Count); i++)
            {
                if (!newRom.overlays[i].SequenceEqual(oldRom.overlays[i]))
                    ovList.Add(new OverlayPatch(i, newRom, oldRom));
            }

            foreach (var ov in ovList)
            {
                output.Add("ov_" + ov.overlayID, ov.GetBytes());
            }

            //Add new Overlays
            for (int i = oldRom.overlays.Count; i < newRom.overlays.Count; i++)
            {
                output.Add("addov_" + i, newRom.overlays[i]);
            }
            //Mark removed overlays
            for (int i = newRom.overlays.Count; i < oldRom.overlays.Count; i++)
            {
                output.Add("remov_" + i, new byte[0]);
            }

            //Narcs
            for (int i = 0; i < Math.Min(newRom.narcs.Count, oldRom.narcs.Count); i++)
            {
                NARC newNarc = newRom.narcs[i];
                newNarc.WriteData();
                NARC oldNarc = oldRom.narcs[i];

                if (newNarc is HabitatListNARC || newNarc.byteData.SequenceEqual(oldNarc.byteData)) continue;

                //Handle fake narc files
                if (newNarc.byteData[0] != (byte)'N')
                {
                    if (!newNarc.byteData.SequenceEqual(oldNarc.byteData))
                    {
                        output.Add("fullnarc_" + i, newNarc.byteData);
                    }
                }
                else
                {
                    //Changed Files
                    for (int j = 0; j < Math.Min(newNarc.numFileEntries, oldNarc.numFileEntries); j++)
                    {
                        List<byte> f1 = newNarc.GetFileEntry(j);
                        List<byte> f2 = oldNarc.GetFileEntry(j);
                        if (!f1.SequenceEqual(f2))
                        {
                            output.Add("narc_" + i + "_" + j, f1);
                        }
                    }
                    //New Files
                    for (int j = oldNarc.numFileEntries; j < newNarc.numFileEntries; j++)
                    {
                        output.Add("narcadd_" + i + "_" + j, newNarc.GetFileEntry(j));
                    }
                    //Removed Files
                    for (int j = newNarc.numFileEntries; j < oldNarc.numFileEntries; j++)
                    {
                        output.Add("narcrem_" + i + "_" + j, new byte[0]);
                    }
                }
            }

            return output;
        }

        static void MakeArm9(NDSFileSystem newRom, NDSFileSystem oldRom, Dictionary<string, IEnumerable<byte>> output)
        {
            bool newCompressed = newRom.arm9.Count < 600000;
            bool oldCompressed = oldRom.arm9.Count < 600000;

            List<byte> newArm = newCompressed ? BLZDecoder.BLZ_DecodePub(newRom.arm9.ToArray()).ToList() : newRom.arm9;
            List<byte> oldArm = oldCompressed ? BLZDecoder.BLZ_DecodePub(oldRom.arm9.ToArray()).ToList() : oldRom.arm9;

            if (!newArm.SequenceEqual(oldArm))
            {
                Dictionary<int, List<byte>> replaceSections = new Dictionary<int, List<byte>>();

                int pos = 0;
                while (pos < newArm.Count)
                {
                    if (newArm[pos] != oldArm[pos])
                    {
                        int startPos = pos;
                        List<byte> replacements = new List<byte>();
                        while (pos < newArm.Count - 4 && newArm[pos] != oldArm[pos])
                        {
                            replacements.AddRange(newArm.GetRange(pos, 4));
                            pos += 4;
                        }
                        replaceSections.Add(startPos, replacements);
                    }
                    pos++;
                }

                List<byte> bytes = new List<byte>() { (byte)(newCompressed ? 1 : 0) };
                foreach (var rep in replaceSections)
                {
                    bytes.AddRange(BitConverter.GetBytes(rep.Key));
                    bytes.AddRange(BitConverter.GetBytes(rep.Value.Count));
                    bytes.AddRange(rep.Value);
                }

                output.Add("arm9", bytes);
            }
        }

        public static void ApplyPatch(NDSFileSystem fileSystem, Dictionary<string, IEnumerable<byte>> patch)
        {
            Y9Table oldY9 = fileSystem.y9;
            foreach (KeyValuePair<string, IEnumerable<byte>> entry in patch)
            {
                if (entry.Key.StartsWith("arm9"))
                {
                    List<byte> b = entry.Value.ToList();
                    if (fileSystem.arm9.Count < 600000) fileSystem.arm9 = BLZDecoder.BLZ_DecodePub(fileSystem.arm9.ToArray()).ToList();
                    bool compress = b[0] != 0;

                    int pos = 1;
                    while (pos < b.Count)
                    {
                        int addr = HelperFunctions.ReadInt(b, pos);
                        int size = HelperFunctions.ReadInt(b, pos + 4);
                        pos += 8;
                        for (int i = 0; i < size; i++) fileSystem.arm9[addr + i] = b[pos + i];
                        pos += size;
                    }
                    if (compress) fileSystem.arm9 = BLZDecoder.BLZ_EncodePub(fileSystem.arm9.ToArray(), false).ToList();
                }

                if (entry.Key.StartsWith("y9"))
                {
                    List<byte> b = entry.Value.ToList();
                    List<byte> y9 = fileSystem.y9.bytes.ToList();

                    int remove = HelperFunctions.ReadInt(b, 0);
                    if (remove > 0) y9.RemoveRange(y9.Count - 1 - remove, remove);
                    int add = HelperFunctions.ReadInt(b, 4);
                    if (add > 0) y9.AddRange(b.GetRange(y9.Count, add));

                    int pos = 8 + add;
                    while (pos < b.Count)
                    {
                        List<byte> ov = b.GetRange(pos, 32);
                        int id = HelperFunctions.ReadInt(ov, 0);
                        for (int i = 0; i < 32; i++) y9[id * 32 + i] = b[pos + i];
                        pos += 32;
                    }

                    fileSystem.y9 = new Y9Table(y9.ToArray());
                }

                List<List<byte>> ovs = new List<List<byte>>(fileSystem.overlays);
                if (entry.Key.StartsWith("remov_"))
                {
                    int id = int.Parse(entry.Key.Split('_')[1]);
                    fileSystem.overlays.Remove(ovs[id]);
                }
                if (entry.Key.StartsWith("addov_"))
                {
                    int id = int.Parse(entry.Key.Split('_')[1]);
                    fileSystem.overlays.Insert(Math.Min(id, fileSystem.overlays.Count), entry.Value.ToList());
                }
                if (entry.Key.StartsWith("ov_"))
                {
                    int id = int.Parse(entry.Key.Split('_')[1]);
                    List<byte> b = entry.Value.ToList();
                    int startAddr = HelperFunctions.ReadInt(b, 0);
                    int ovSize = HelperFunctions.ReadInt(b, 4);

                    List<byte> ov = BLZDecoder.BLZ_DecodePub(fileSystem.overlays[id].ToArray()).ToList();

                    while (oldY9.entries[id].mountAddress > startAddr)
                    {
                        ov.Insert(0, 0);
                        oldY9.entries[id].mountAddress--;
                    }
                    while (ov.Count < ovSize)
                    {
                        ov.Add(0);
                    }

                    int pos = 16;
                    while (pos < b.Count)
                    {
                        int addr = HelperFunctions.ReadInt(b, pos);
                        int size = HelperFunctions.ReadInt(b, pos + 4);
                        pos += 8;
                        for (int i = 0; i < size; i++) ov[addr + i] = b[pos + i];
                        pos += size;
                    }

                    fileSystem.overlays[id] = ov;
                    fileSystem.y9.bytes[32 * id + 31] = 2;
                }

                if (entry.Key.StartsWith("fullnarc_"))
                {
                    int id = int.Parse(entry.Key.Split('_')[1]);
                    fileSystem.narcs[id].byteData = entry.Value.ToArray();
                }

                if (entry.Key.StartsWith("narc_"))
                {
                    int id = int.Parse(entry.Key.Split('_')[1]);
                    int file = int.Parse(entry.Key.Split('_')[2]);
                    fileSystem.narcs[id].ReplaceFileEntry(file, entry.Value.ToList());
                }

                if (entry.Key.StartsWith("narcadd_"))
                {
                    int id = int.Parse(entry.Key.Split('_')[1]);
                    int file = int.Parse(entry.Key.Split('_')[2]);
                    fileSystem.narcs[id].AddFileEntry(file, entry.Value.ToList());
                }
            }
            foreach (var v in fileSystem.narcs) v.ReadData();
        }
    }

    public class OverlayPatch
    {
        public int overlayID = 0;

        Dictionary<int, List<byte>> replaceSections = new Dictionary<int, List<byte>>();
        int startAddr = 0;
        int ovSize = 0;
        int oldStart = 0;
        int oldSize = 0;

        public OverlayPatch(int ovID, NDSFileSystem newRom, NDSFileSystem oldRom)
        {
            overlayID = ovID;
            bool newCompressed = newRom.y9.entries[ovID].compressed;
            bool oldCompressed = oldRom.y9.entries[ovID].compressed;

            List<byte> newOv = newCompressed ? BLZDecoder.BLZ_DecodePub(newRom.overlays[ovID].ToArray()).ToList() : newRom.overlays[ovID];
            List<byte> oldOv = oldCompressed ? BLZDecoder.BLZ_DecodePub(oldRom.overlays[ovID].ToArray()).ToList() : oldRom.overlays[ovID];

            int newStart = newRom.y9.entries[ovID].mountAddress;
            oldStart = oldRom.y9.entries[ovID].mountAddress;
            int newEnd = newStart + newOv.Count;
            int oldEnd = oldStart + oldOv.Count;
            startAddr = newStart;
            ovSize = newOv.Count;
            oldSize = oldOv.Count;

            List<byte> newOvMid = newOv.GetRange(newStart < oldStart ? oldStart - newStart : 0, Math.Min(oldEnd - oldStart, newEnd - newStart));
            List<byte> oldOvMid = oldOv.GetRange(oldStart < newStart ? newStart - oldStart : 0, Math.Min(oldEnd - oldStart, newEnd - newStart));

            int pos = 0;
            while (pos < newOvMid.Count)
            {
                if (newOvMid[pos] != oldOvMid[pos])
                {
                    int startPos = pos;
                    List<byte> replacements = new List<byte>();
                    while (pos < newOvMid.Count - 4 && newOvMid[pos] != oldOvMid[pos])
                    {
                        replacements.AddRange(newOvMid.GetRange(pos, 4));
                        pos += 4;
                    } 
                    replaceSections.Add(startPos + oldStart - newStart, replacements);
                }
                pos++;
            }

            if (newStart < oldStart)
            {
                replaceSections.Add(0, newOv.GetRange(0, oldStart - newStart));
            }

            if (newEnd > oldEnd)
            {
                replaceSections.Add(ovSize, newOv.GetRange(oldStart - newStart + oldOvMid.Count, newEnd - oldEnd));
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes(startAddr));
            bytes.AddRange(BitConverter.GetBytes(ovSize));
            bytes.AddRange(BitConverter.GetBytes(oldStart));
            bytes.AddRange(BitConverter.GetBytes(oldSize));

            foreach (var rep in replaceSections)
            {
                bytes.AddRange(BitConverter.GetBytes(rep.Key));
                bytes.AddRange(BitConverter.GetBytes(rep.Value.Count));
                bytes.AddRange(rep.Value);
            }

            return bytes;
        }
    }
}
