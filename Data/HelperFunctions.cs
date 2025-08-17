using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public static class HelperFunctions
    {
        public static int Clamp(int value, int min, int max)
        {
            return value > min ? (value < max ? value : max) : min;
        }

        public static int ReadInt(this FileStream fs)
        {
            byte[] toInt = new byte[4];
            fs.Read(toInt, 0, 4);
            return BitConverter.ToInt32(toInt, 0);
        }
        public static int ReadShort(byte[] data, int offset) => offset < data.Length - 1 ? ((data[offset] & 0xFF) + ((data[offset + 1] & 0xFF) << 8)) : 0;
        public static int ReadShort(List<byte> data, int offset) => offset < data.Count - 1 ? ((data[offset] & 0xFF) + ((data[offset + 1] & 0xFF) << 8)) : 0;
        public static int ReadInt(byte[] data, int offset) => offset < data.Length - 3 ? (data[offset] & 0xFF) + ((data[offset + 1] & 0xFF) << 8) + ((data[offset + 2] & 0xFF) << 16)
                + ((data[offset + 3] & 0xFF) << 24) : 0;
        public static int ReadInt(List<byte> data, int offset) => offset < data.Count - 3 ? (data[offset] & 0xFF) + ((data[offset + 1] & 0xFF) << 8) + ((data[offset + 2] & 0xFF) << 16)
                + ((data[offset + 3] & 0xFF) << 24) : 0;
        public static int ReadFlipInt(byte[] data, int offset) => offset < data.Length - 3 ? (data[offset + 3] & 0xFF) + ((data[offset + 2] & 0xFF) << 8) + ((data[offset + 1] & 0xFF) << 16)
                + ((data[offset] & 0xFF) << 24) : 0;

        public static int ReadShort(RefByte[] data, int offset) => offset < data.Length - 1 ? ((data[offset] & 0xFF) + ((data[offset + 1] & 0xFF) << 8)) : 0;
        public static int ReadInt(RefByte[] data, int offset) => offset < data.Length - 3 ? (data[offset] & 0xFF) + ((data[offset + 1] & 0xFF) << 8) + ((data[offset + 2] & 0xFF) << 16)
                + ((data[offset + 3] & 0xFF) << 24) : 0;

        public static void WriteShort(byte[] data, int offset, int value)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
        }

        public static void WriteInt(byte[] data, int offset, int value)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
            data[offset + 2] = (byte)((value >> 16) & 0xFF);
            data[offset + 3] = (byte)((value >> 24) & 0xFF);
        }

        public static void WriteInt(RefByte[] data, int offset, int value)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
            data[offset + 2] = (byte)((value >> 16) & 0xFF);
            data[offset + 3] = (byte)((value >> 24) & 0xFF);
        }

        public static void WriteInt(List<byte> data, int offset, int value)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
            data[offset + 2] = (byte)((value >> 16) & 0xFF);
            data[offset + 3] = (byte)((value >> 24) & 0xFF);
        }

        public static Color Lerp(this Color color1, Color color2, float amount)
        {
            int r = (int)Math.Round((1 - amount) * color1.R + amount * color2.R);
            int g = (int)Math.Round((1 - amount) * color1.G + amount * color2.G);
            int b = (int)Math.Round((1 - amount) * color1.B + amount * color2.B);
            return Color.FromArgb(r, g, b);
        }

        public static (float r, float g, float b) ratio(this Color color)
        {
            float total = color.R + color.G + color.B;
            return (color.R / total, color.G / total, color.B / total);
        }

        public static float intensity(this Color color)
        {
            return (color.R + color.G + color.B) / 765f;
        }

        public static bool AreColorsSimilar(Color c1, Color c2, float tolerance)
        {
            (float r, float g, float b) ratio1 = c1.ratio();
            (float r, float g, float b) ratio2 = c2.ratio();
            return Math.Abs(ratio1.r - ratio2.r) <= tolerance && Math.Abs(ratio1.g - ratio2.g) <= tolerance && Math.Abs(ratio1.b - ratio2.b) <= tolerance;
        }

        public static int IndexOf(this RefByte[] arr, RefByte value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == value) return i;
            }
            return -1;
        }

        public static void Shuffle<T>(this IList<T> ts, Random rand)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; i++)
            {
                var r = rand.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        public static string StatusText(this DateTime dt)
        {
            string text = dt.ToString();
            if (text.Contains(" ")) text = text.Substring(text.IndexOf(" ") + 1);
            return text;
        }
    }

    public class RefByte : Object
    {
        private byte value;

        public static implicit operator byte(RefByte b) => b.value;
        public static implicit operator RefByte(byte b) => new RefByte() { value = b };

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class TextValue
    {
        public int hexID;
        public string name;

        public TextValue(int hexID, string name)
        {
            this.hexID = hexID;
            this.name = name;
        }

        public override string ToString() => name;

        public static implicit operator int(TextValue v) => v.hexID;
    }
}
