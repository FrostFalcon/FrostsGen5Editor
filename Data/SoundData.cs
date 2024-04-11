using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Data
{
    public class SoundData
    {
        const int WavPointerStart = 0x3D9C0;

        public List<byte> bytes;

        public SoundData(FileStream fs, int startPos, int endPos)
        {
            byte[] sizeBytes = new byte[4];
            fs.Position = startPos + 8;
            fs.Read(sizeBytes, 0, 4);
            int size = HelperFunctions.ReadInt(sizeBytes, 0);

            bytes = new List<byte>();
            byte[] data = new byte[size];

            fs.Position = startPos;
            fs.Read(data, 0, size);

            bytes = new List<byte>(data);
        }

        public SoundData(byte[] bytes)
        {
            this.bytes = new List<byte>(bytes);
        }

        public void WriteToSwav(int fileID)
        {
            OpenFileDialog prompt = new OpenFileDialog();
            prompt.Filter = "nds .wav file|*.swav";

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                int start = HelperFunctions.ReadInt(bytes, WavPointerStart + 16 * fileID);
                int size = HelperFunctions.ReadInt(bytes, WavPointerStart + 16 * fileID + 4);

                FileStream fs = File.OpenRead(prompt.FileName);
                if (fs.Length > size)
                {
                    fs.Close();
                    MessageBox.Show("Input file is too big. The max file size allowed is " + size + " bytes.");
                    return;
                }

                for (int i = 0; i < fs.Length; i++)
                {
                    bytes[start + i] = (byte)fs.ReadByte();
                }

                MessageBox.Show("File Replace Complete");
            }
        }

        public void WriteToSwavFromPatch(int fileID, List<byte> soundBytes)
        {
            int start = HelperFunctions.ReadInt(bytes, WavPointerStart + 16 * fileID);
            int size = HelperFunctions.ReadInt(bytes, WavPointerStart + 16 * fileID + 4);

            for (int i = 0; i < soundBytes.Count; i++)
            {
                bytes[start + i] = soundBytes[i];
            }
        }

        public byte[] GetSoundBytes(int fileID)
        {
            int start = HelperFunctions.ReadInt(bytes, WavPointerStart + 16 * fileID);
            int size = HelperFunctions.ReadInt(bytes, WavPointerStart + 16 * fileID + 4);
            return bytes.GetRange(start, size).ToArray();
        }
    }
}
