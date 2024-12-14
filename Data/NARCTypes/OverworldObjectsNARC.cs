using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NewEditor.Data.NARCTypes
{
    public class OverworldObjectsNARC : NARC
    {
        public List<OverworldObjectsEntry> objects;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            objects = new List<OverworldObjectsEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                OverworldObjectsEntry o = new OverworldObjectsEntry(bytes) { nameID = i };
                objects.Add(o);

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
            foreach (OverworldObjectsEntry o in objects)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += o.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (OverworldObjectsEntry o in objects)
            {
                newByteData.AddRange(o.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(objects.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            OverworldObjectsNARC other = narc as OverworldObjectsNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.objects.Count || !objects[i].bytes.SequenceEqual(other.objects[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(objects[i].bytes.Length));
                    bytes.AddRange(objects[i].bytes);
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

                if (id > objects.Count)
                {
                    //Don't accept extra files here
                }
                else objects[id] = new OverworldObjectsEntry(new List<byte>(bytes).GetRange(pos, size).ToArray());
                pos += size;
            }
        }
    }

    public class OverworldObjectsEntry
    {
        public byte[] bytes;
        public int nameID;

        public List<OverworldFurniture> furniture;
        public List<OverworldNPC> NPCs;
        public List<OverworldWarp> warps;
        public List<OverworldTrigger> triggers;

        public List<byte> endData;

        public OverworldObjectsEntry(byte[] bytes)
        {
            this.bytes = bytes;

            ReadData();
        }

        internal void ReadData()
        {
            if (bytes.Length <= 8) return;
            int readPos = 8;

            //Furniture
            furniture = new List<OverworldFurniture>();
            for (int i = 0; i < bytes[4]; i++)
            {
                furniture.Add(new OverworldFurniture()
                {
                    scriptUsed = (short)HelperFunctions.ReadShort(bytes, readPos),
                    unknown1 = (short)HelperFunctions.ReadShort(bytes, readPos + 2),
                    unknown2 = (short)HelperFunctions.ReadShort(bytes, readPos + 4),
                    xPosition = HelperFunctions.ReadInt(bytes, readPos + 6),
                    unknown3 = (short)HelperFunctions.ReadShort(bytes, readPos + 10),
                    yPosition = HelperFunctions.ReadInt(bytes, readPos + 12),
                    zPosition = HelperFunctions.ReadInt(bytes, readPos + 16)
                });
                readPos += 20;
            }

            //NPCs
            NPCs = new List<OverworldNPC>();
            for (int i = 0; i < bytes[5]; i++)
            {
                NPCs.Add(new OverworldNPC()
                {
                    id = (short)HelperFunctions.ReadShort(bytes, readPos),
                    sprite = (short)HelperFunctions.ReadShort(bytes, readPos + 2),
                    movementPermissions = (short)HelperFunctions.ReadShort(bytes, readPos + 4),
                    permissions2 = (short)HelperFunctions.ReadShort(bytes, readPos + 6),
                    flag = (short)HelperFunctions.ReadShort(bytes, readPos + 8),
                    scriptUsed = (short)HelperFunctions.ReadShort(bytes, readPos + 10),
                    defaultDirection = (short)HelperFunctions.ReadShort(bytes, readPos + 12),
                    sightRange = (short)HelperFunctions.ReadShort(bytes, readPos + 14),
                    unknown1 = (short)HelperFunctions.ReadShort(bytes, readPos + 16),
                    unknown2 = (short)HelperFunctions.ReadShort(bytes, readPos + 18),
                    horizontalLeash = (short)HelperFunctions.ReadShort(bytes, readPos + 20),
                    verticalLeash = (short)HelperFunctions.ReadShort(bytes, readPos + 22),
                    unknown3 = (short)HelperFunctions.ReadShort(bytes, readPos + 24),
                    unknown4 = (short)HelperFunctions.ReadShort(bytes, readPos + 26),
                    xPosition = (short)HelperFunctions.ReadShort(bytes, readPos + 28),
                    yPosition = (short)HelperFunctions.ReadShort(bytes, readPos + 30),
                    unknown5 = (short)HelperFunctions.ReadShort(bytes, readPos + 32),
                    zPosition = (short)HelperFunctions.ReadShort(bytes, readPos + 34),
                });
                readPos += 36;
            }

            //Warps
            warps = new List<OverworldWarp>();
            for (int i = 0; i < bytes[6]; i++)
            {
                warps.Add(new OverworldWarp()
                {
                    destinationMap = (short)HelperFunctions.ReadShort(bytes, readPos),
                    destinationWarp = (short)HelperFunctions.ReadShort(bytes, readPos + 2),
                    unknown1 = bytes[readPos + 4],
                    unknown2 = bytes[readPos + 5],
                    exitX = HelperFunctions.ReadInt(bytes, readPos + 6),
                    exitY = HelperFunctions.ReadInt(bytes, readPos + 10),
                    width = (short)HelperFunctions.ReadShort(bytes, readPos + 14),
                    height = (short)HelperFunctions.ReadShort(bytes, readPos + 16),
                    unknown3 = (short)HelperFunctions.ReadShort(bytes, readPos + 18),
                });
                readPos += 20;
            }

            //Triggers
            triggers = new List<OverworldTrigger>();
            for (int i = 0; i < bytes[7]; i++)
            {
                triggers.Add(new OverworldTrigger()
                {
                    scriptUsed = (short)HelperFunctions.ReadShort(bytes, readPos),
                    constantValue = (short)HelperFunctions.ReadShort(bytes, readPos + 2),
                    constantReference = (short)HelperFunctions.ReadShort(bytes, readPos + 4),
                    unknown1 = (short)HelperFunctions.ReadShort(bytes, readPos + 6),
                    unknown2 = (short)HelperFunctions.ReadShort(bytes, readPos + 8),
                    xPosition = (short)HelperFunctions.ReadShort(bytes, readPos + 10),
                    yPosition = (short)HelperFunctions.ReadShort(bytes, readPos + 12),
                    width = (short)HelperFunctions.ReadShort(bytes, readPos + 14),
                    height = (short)HelperFunctions.ReadShort(bytes, readPos + 16),
                    zPosition = (short)HelperFunctions.ReadShort(bytes, readPos + 18),
                    unknown5 = (short)HelperFunctions.ReadShort(bytes, readPos + 20),
                });
                readPos += 22;
            }

            endData = new List<byte>();
            for (int i = readPos; i < bytes.Length; i++) endData.Add(bytes[i]);
        }

        internal void ApplyData()
        {
            if (bytes.Length <= 8) return;
            List<byte> newBytes = new List<byte>();

            //Headers
            newBytes.AddRange(new byte[] { 0, 0, 0, 0 });
            newBytes.Add((byte)furniture.Count);
            newBytes.Add((byte)NPCs.Count);
            newBytes.Add((byte)warps.Count);
            newBytes.Add((byte)triggers.Count);

            foreach (OverworldFurniture o in furniture)
            {
                newBytes.AddRange(BitConverter.GetBytes(o.scriptUsed));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown1));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown2));
                newBytes.AddRange(BitConverter.GetBytes(o.xPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown3));
                newBytes.AddRange(BitConverter.GetBytes(o.yPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.zPosition));
            }

            for (int i = 0; i < NPCs.Count; i++)
            {
                OverworldNPC o = NPCs[i];
                o.id = (short)i;
                newBytes.AddRange(BitConverter.GetBytes(o.id));
                newBytes.AddRange(BitConverter.GetBytes(o.sprite));
                newBytes.AddRange(BitConverter.GetBytes(o.movementPermissions));
                newBytes.AddRange(BitConverter.GetBytes(o.permissions2));
                newBytes.AddRange(BitConverter.GetBytes(o.flag));
                newBytes.AddRange(BitConverter.GetBytes(o.scriptUsed));
                newBytes.AddRange(BitConverter.GetBytes(o.defaultDirection));
                newBytes.AddRange(BitConverter.GetBytes(o.sightRange));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown1));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown2));
                newBytes.AddRange(BitConverter.GetBytes(o.horizontalLeash));
                newBytes.AddRange(BitConverter.GetBytes(o.verticalLeash));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown3));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown4));
                newBytes.AddRange(BitConverter.GetBytes(o.xPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.yPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown5));
                newBytes.AddRange(BitConverter.GetBytes(o.zPosition));
            }

            foreach (OverworldWarp o in warps)
            {
                newBytes.AddRange(BitConverter.GetBytes(o.destinationMap));
                newBytes.AddRange(BitConverter.GetBytes(o.destinationWarp));
                newBytes.Add(o.unknown1);
                newBytes.Add(o.unknown2);
                newBytes.AddRange(BitConverter.GetBytes(o.exitX));
                newBytes.AddRange(BitConverter.GetBytes(o.exitY));
                newBytes.AddRange(BitConverter.GetBytes(o.width));
                newBytes.AddRange(BitConverter.GetBytes(o.height));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown3));
            }

            foreach (OverworldTrigger o in triggers)
            {
                newBytes.AddRange(BitConverter.GetBytes(o.scriptUsed));
                newBytes.AddRange(BitConverter.GetBytes(o.constantValue));
                newBytes.AddRange(BitConverter.GetBytes(o.constantReference));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown1));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown2));
                newBytes.AddRange(BitConverter.GetBytes(o.xPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.yPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.width));
                newBytes.AddRange(BitConverter.GetBytes(o.height));
                newBytes.AddRange(BitConverter.GetBytes(o.zPosition));
                newBytes.AddRange(BitConverter.GetBytes(o.unknown5));
            }

            HelperFunctions.WriteInt(newBytes, 0, newBytes.Count - 4);

            while (endData.Count < 4) endData.Add(0);

            newBytes.AddRange(endData);

            //Pad File Size
            while (newBytes.Count % 4 != 0) newBytes.Add(0);

            bytes = newBytes.ToArray();
        }

        public override string ToString()
        {
            return nameID.ToString();
        }
    }

    public class OverworldFurniture
    {
        public short scriptUsed;
        public short unknown1;
        public short unknown2;
        public int xPosition;
        public short unknown3;
        public int yPosition;
        public int zPosition;
    }

    public class OverworldNPC
    {
        public short id;
        public short sprite;
        public short movementPermissions;
        public short permissions2;
        public short flag;
        public short scriptUsed;
        public short defaultDirection;
        public short sightRange;
        public short unknown1;
        public short unknown2;
        public short horizontalLeash;
        public short verticalLeash;
        public short unknown3;
        public short unknown4;
        public short xPosition;
        public short yPosition;
        public short unknown5;
        public short zPosition;
    }

    public class OverworldWarp
    {
        public short destinationMap;
        public short destinationWarp;
        public byte unknown1;
        public byte unknown2;
        public int exitX;
        public int exitY;
        public short width;
        public short height;
        public short unknown3;
    }

    public class OverworldTrigger
    {
        public short scriptUsed;
        public short constantValue;
        public short constantReference;
        public short unknown1;
        public short unknown2;
        public short xPosition;
        public short yPosition;
        public short width;
        public short height;
        public short zPosition;
        public short unknown5;
    }
}
