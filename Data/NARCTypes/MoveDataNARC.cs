using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class MoveDataNARC : NARC
    {
        public List<MoveDataEntry> moves;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            moves = new List<MoveDataEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                MoveDataEntry m = new MoveDataEntry(bytes) { nameID = i };
                moves.Add(m);

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
            foreach (MoveDataEntry m in moves)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (MoveDataEntry m in moves)
            {
                newByteData.AddRange(m.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(moves.Count);

            base.WriteData();
        }
    }

    public class MoveDataEntry
    {
        public byte[] bytes;

        public int nameID;
        public byte element;
        public byte category;

        public byte damageType;
        public byte basePower;
        public byte accuracy;
        public byte powerPoints;

        public sbyte priority;
        public byte minMultiHit;
        public byte maxMultiHit;

        public short statusEffect;
        public byte statusChance;

        public byte minTrapTurns;
        public byte maxTrapTurns;

        public byte critRatio;
        public byte flinchChance;
        public short effectCode;
        public sbyte leechRecoil;
        public byte healPercent;

        public byte target;

        public byte statChange1Stat;
        public sbyte statChange1Stages;
        public byte statChange1Chance;
        public byte statChange2Stat;
        public sbyte statChange2Stages;
        public byte statChange2Chance;
        public byte statChange3Stat;
        public sbyte statChange3Stages;
        public byte statChange3Chance;

        public short flags;

        public MoveDataEntry(byte[] bytes)
        {
            this.bytes = bytes;

            ReadData();
        }

        internal void ReadData()
        {
            element = bytes[0];
            category = bytes[1];

            damageType = bytes[2];
            basePower = bytes[3];
            accuracy = bytes[4];
            powerPoints = bytes[5];

            priority = (sbyte)bytes[6];
            minMultiHit = (byte)(bytes[7] & 0b1111);
            maxMultiHit = (byte)((bytes[7] & 0b1111_0000) >> 4);

            statusEffect = (short)HelperFunctions.ReadShort(bytes, 8);
            statusChance = bytes[10];

            minTrapTurns = bytes[12];
            maxTrapTurns = bytes[13];

            critRatio = bytes[14];
            flinchChance = bytes[15];
            effectCode = (short)HelperFunctions.ReadShort(bytes, 16);
            leechRecoil = (sbyte)bytes[18];
            healPercent = bytes[19];

            target = bytes[20];

            statChange1Stat = bytes[21];
            statChange2Stat = bytes[22];
            statChange3Stat = bytes[23];
            statChange1Stages = (sbyte)bytes[24];
            statChange2Stages = (sbyte)bytes[25];
            statChange3Stages = (sbyte)bytes[26];
            statChange1Chance = bytes[27];
            statChange2Chance = bytes[28];
            statChange3Chance = bytes[29];

            flags = (short)HelperFunctions.ReadShort(bytes, 32);
        }

        internal void ApplyData()
        {
            bytes[0] = element;
            bytes[1] = category;

            bytes[2] = damageType;
            bytes[3] = basePower;
            bytes[4] = accuracy;
            bytes[5] = powerPoints;

            bytes[6] = (byte)priority;
            bytes[7] = (byte)((maxMultiHit << 4) + minMultiHit);

            HelperFunctions.WriteShort(bytes, 8, statusEffect);
            bytes[10] = statusChance;

            bytes[12] = minTrapTurns;
            bytes[13] = maxTrapTurns;

            bytes[14] = critRatio;
            bytes[15] = flinchChance;
            HelperFunctions.WriteShort(bytes, 16, effectCode);
            bytes[18] = (byte)leechRecoil;
            bytes[19] = healPercent;

            bytes[20] = target;

            bytes[21] = statChange1Stat;
            bytes[22] = statChange2Stat;
            bytes[23] = statChange3Stat;
            bytes[24] = (byte)statChange1Stages;
            bytes[25] = (byte)statChange2Stages;
            bytes[26] = (byte)statChange3Stages;
            bytes[27] = statChange1Chance;
            bytes[28] = statChange2Chance;
            bytes[29] = statChange3Chance;

            HelperFunctions.WriteShort(bytes, 32, flags);
        }

        public override string ToString()
        {
            return nameID < MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text[nameID] + " - " + nameID : "Name not found";
        }
    }
}
