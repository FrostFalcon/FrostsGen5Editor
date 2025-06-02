using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NewEditor.Data
{
    public static class PatchingSystem
    {
        public static void MakePatch(NDSFileSystem newRom, NDSFileSystem oldRom)
        {
            Dictionary<string, IEnumerable<byte>> output = new Dictionary<string, IEnumerable<byte>>();

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
                        bytes.AddRange(BitConverter.GetBytes(i));
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

            FileFunctions.WriteAllSections("output.bin", output, true);
        }
    }

    public class OverlayPatch
    {
        public int overlayID = 0;

        List<byte> addBefore = new List<byte>();
        List<byte> addAfter = new List<byte>();
        Dictionary<int, List<byte>> replaceSections = new Dictionary<int, List<byte>>();
        int removeBefore = 0;
        int removeAfter = 0;

        public OverlayPatch(int ovID, NDSFileSystem newRom, NDSFileSystem oldRom)
        {
            overlayID = ovID;
            bool newCompressed = newRom.y9.entries[ovID].compressed;
            bool oldCompressed = oldRom.y9.entries[ovID].compressed;

            List<byte> newOv = newCompressed ? BLZDecoder.BLZ_DecodePub(newRom.overlays[ovID].ToArray()).ToList() : newRom.overlays[ovID];
            List<byte> oldOv = oldCompressed ? BLZDecoder.BLZ_DecodePub(oldRom.overlays[ovID].ToArray()).ToList() : oldRom.overlays[ovID];

            int newStart = newRom.y9.entries[ovID].mountAddress;
            int oldStart = oldRom.y9.entries[ovID].mountAddress;
            int newEnd = newStart + newOv.Count;
            int oldEnd = oldStart + oldOv.Count;

            if (newStart < oldStart)
            {
                addBefore = newOv.GetRange(0, oldStart - newStart);
            }
            if (oldStart < newStart)
            {
                removeBefore = newStart - oldStart;
            }

            if (newEnd < oldEnd)
            {
                addAfter = newOv.GetRange(newEnd, oldEnd - newEnd);
            }
            if (oldEnd < newEnd)
            {
                removeAfter = newEnd - oldEnd;
            }

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
                        replacements.Add(newOvMid[pos]);
                        replacements.Add(newOvMid[pos + 1]);
                        replacements.Add(newOvMid[pos + 2]);
                        replacements.Add(newOvMid[pos + 3]);
                        pos += 4;
                    } 
                    replaceSections.Add(startPos, replacements);
                }
                pos++;
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes(overlayID));
            bytes.AddRange(BitConverter.GetBytes(removeBefore));
            bytes.AddRange(BitConverter.GetBytes(removeAfter));

            bytes.AddRange(BitConverter.GetBytes(addBefore.Count));
            bytes.AddRange(addBefore);

            bytes.AddRange(BitConverter.GetBytes(addAfter.Count));
            bytes.AddRange(addAfter);

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
