using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class ZoneDataNARC : NARC
    {
        public List<ZoneDataEntry> zones;

        public override void ReadData()
        {
            base.ReadData();

            int initialPosition = 60;

            //Register data files
            zones = new List<ZoneDataEntry>();

            for (int i = initialPosition; i < byteData.Length - 1; i += 48)
            {
                ZoneDataEntry z = new ZoneDataEntry(byteData.ToList().GetRange(i, Math.Min(48, byteData.Length - i)).ToArray(), zones.Count);
                zones.Add(z);
            }
        }

        public override void WriteData()
        {
            List<byte> newByteData = new List<byte>(byteData);

            int initialPos = 60;

            foreach (ZoneDataEntry o in zones)
            {
                newByteData.RemoveRange(initialPos, o.bytes.Length);
                newByteData.InsertRange(initialPos, o.bytes);
                initialPos += o.bytes.Length;
            }

            byteData = newByteData.ToArray();

            FixHeaders(1);

            base.WriteData();
        }
    }

    public class ZoneDataEntry
    {
        public byte[] bytes;

        public short index;

        public byte mapType;
        public byte unknown1;
        public short texture;
        public short matrix;
        public short scriptFile;
        public short levelScripts;
        public short storyTextFile;
        public short music_Spring;
        public short music_Summer;
        public short music_Autumn;
        public short music_Winter;
        public byte encounterFile;
        public byte unknown2;
        public short mapId;
        public short parentMapId;
        public byte nameId;
        public byte nameStyle;
        public byte weather;
        public byte camera;
        public byte unknown3;
        public byte flags;
        public short unknown4;
        public byte nameIcon;
        public byte unknown5;
        public int flyX;
        public int flyY;
        public int flyZ;

        public ZoneDataEntry(byte[] bytes, int index)
        {
            this.bytes = bytes;
            this.index = (short)index;

            if (bytes.Length == 48)
                ReadData();
        }

        public void ReadData()
        {
            mapType = bytes[0];
            unknown1 = bytes[1];
            texture = (short)HelperFunctions.ReadShort(bytes, 2);
            matrix = (short)HelperFunctions.ReadShort(bytes, 4);
            scriptFile = (short)HelperFunctions.ReadShort(bytes, 6);
            levelScripts = (short)HelperFunctions.ReadShort(bytes, 8);
            storyTextFile = (short)HelperFunctions.ReadShort(bytes, 10);
            music_Spring = (short)HelperFunctions.ReadShort(bytes, 12);
            music_Summer = (short)HelperFunctions.ReadShort(bytes, 14);
            music_Autumn = (short)HelperFunctions.ReadShort(bytes, 16);
            music_Winter = (short)HelperFunctions.ReadShort(bytes, 18);
            encounterFile = bytes[20];
            unknown2 = bytes[21];
            mapId = (short)HelperFunctions.ReadShort(bytes, 22);
            parentMapId = (short)HelperFunctions.ReadShort(bytes, 24);
            nameId = bytes[26];
            nameStyle = bytes[27];
            weather = bytes[28];
            camera = bytes[29];
            unknown3 = bytes[30];
            flags = bytes[31];
            unknown4 = (short)HelperFunctions.ReadShort(bytes, 32);
            nameIcon = bytes[34];
            unknown5 = bytes[35];
            flyX = HelperFunctions.ReadInt(bytes, 36);
            flyY = HelperFunctions.ReadInt(bytes, 40);
            flyZ = HelperFunctions.ReadInt(bytes, 44);
        }

        public void ApplyData()
        {
            if (bytes.Length != 48) return;
            bytes[0] = mapType;
            bytes[1] = unknown1;
            HelperFunctions.WriteShort(bytes, 2, texture);
            HelperFunctions.WriteShort(bytes, 4, matrix);
            HelperFunctions.WriteShort(bytes, 6, scriptFile);
            HelperFunctions.WriteShort(bytes, 8, levelScripts);
            HelperFunctions.WriteShort(bytes, 10, storyTextFile);
            HelperFunctions.WriteShort(bytes, 12, music_Spring);
            HelperFunctions.WriteShort(bytes, 14, music_Summer);
            HelperFunctions.WriteShort(bytes, 16, music_Autumn);
            HelperFunctions.WriteShort(bytes, 18, music_Winter);
            bytes[20] = encounterFile;
            bytes[21] = unknown2;
            HelperFunctions.WriteShort(bytes, 22, mapId);
            HelperFunctions.WriteShort(bytes, 24, parentMapId);
            bytes[26] = nameId;
            bytes[27] = nameStyle;
            bytes[28] = weather;
            bytes[29] = camera;
            bytes[30] = unknown3;
            bytes[31] = flags;
            HelperFunctions.WriteShort(bytes, 32, unknown4);
            bytes[34] = nameIcon;
            bytes[35] = unknown5;
            HelperFunctions.WriteInt(bytes, 36, flyX);
            HelperFunctions.WriteInt(bytes, 40, flyY);
            HelperFunctions.WriteInt(bytes, 44, flyZ);
        }

        public override string ToString()
        {
            return nameId < MainEditor.textNarc.textFiles[VersionConstants.ZoneNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.ZoneNameTextFileID].text[nameId] + " - " + index : "Name not found";
        }
    }
}
