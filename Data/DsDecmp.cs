using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//C# Translation of the UPR's version of the DSDECMP Source code

//Copyright (c) 2010 Nick Kraayenbrink
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

namespace NewEditor.Data
{
    public static class DsDecmp
    {
        public static byte[] Decompress(byte[] data)
        {
            if (data.Length == 0) return data;
            return Decompress(data, 0);
        }

        public static byte[] Decompress(byte[] data, int offset)
        {
            switch (data[offset] & 0xFF)
            {
                case 0x10:
                    return decompress10LZ(data, offset);
                case 0x11:
                    return decompress11LZ(data, offset);
                default:
                    return null;
            }
        }

        private static byte[] decompress10LZ(byte[] data, int offset)
        {
            offset++;
            int length = (data[offset] & 0xFF) | ((data[offset + 1] & 0xFF) << 8) | ((data[offset + 2] & 0xFF) << 16);
            offset += 3;
            if (length == 0)
            {
                length = HelperFunctions.ReadInt(data, offset);
                offset += 4;
            }

            byte[] outData = new byte[length];
            int curr_size = 0;
            int flags;
            bool flag;
            int disp, n, b, cdest;
            while (curr_size < outData.Length)
            {
                flags = data[offset++] & 0xFF;
                for (int i = 0; i < 8; i++)
                {
                    flag = (flags & (0x80 >> i)) > 0;
                    if (flag)
                    {
                        disp = 0;
                        b = data[offset++] & 0xFF;
                        n = b >> 4;
                        disp = (b & 0x0F) << 8;
                        disp |= data[offset++] & 0xFF;
                        n += 3;
                        cdest = curr_size;
                        if (disp > curr_size)
                            throw new Exception("Cannot go back more than already written");
                        for (int j = 0; j < n; j++)
                            outData[curr_size++] = outData[cdest - disp - 1 + j];

                        if (curr_size > outData.Length)
                            break;
                    }
                    else
                    {
                        b = data[offset++] & 0xFF;
                        try
                        {
                            outData[curr_size++] = (byte)b;
                        }
                        catch (Exception ex)
                        {
                            if (b == 0)
                                break;
                        }

                        if (curr_size > outData.Length)
                            break;
                    }
                }
            }
            return outData;
        }

        private static byte[] decompress11LZ(byte[] data, int offset)
        {
            Stream stream = new MemoryStream(data, offset, data.Length - offset);
            MemoryStream outStream = new MemoryStream();
            new DSDecmp.Formats.Nitro.LZ11().Decompress(stream, stream.Length, outStream);
            byte[] buffer = outStream.ToArray();
            stream.Dispose();
            outStream.Dispose();
            return buffer;

            offset++;
            int length = (data[offset] & 0xFF) | ((data[offset + 1] & 0xFF) << 8) | ((data[offset + 2] & 0xFF) << 16);
            offset += 3;
            if (length == 0)
            {
                length = HelperFunctions.ReadInt(data, offset);
                offset += 4;
            }

            byte[] outData = new byte[length];

            int curr_size = 0;
            int flags;
            bool flag;
            int b1, bt, b2, b3, len, disp, cdest;

            while (curr_size < outData.Length)
            {
                flags = data[offset++] & 0xFF;

                for (int i = 0; i < 8 && curr_size < outData.Length; i++)
                {
                    flag = (flags & (0x80 >> i)) > 0;
                    if (flag)
                    {
                        b1 = data[offset++] & 0xFF;

                        switch (b1 >> 4)
                        {
                            case 0:
                                // ab cd ef
                                // =>
                                // len = abc + 0x11 = bc + 0x11
                                // disp = def

                                len = b1 << 4;
                                bt = data[offset++] & 0xFF;
                                len |= bt >> 4;
                                len += 0x11;

                                disp = (bt & 0x0F) << 8;
                                b2 = data[offset++] & 0xFF;
                                disp |= b2;
                                break;

                            case 1:
                                // ab cd ef gh
                                // =>
                                // len = bcde + 0x111
                                // disp = fgh
                                // 10 04 92 3F => disp = 0x23F, len = 0x149 + 0x11 =
                                // 0x15A
                                bt = data[offset++] & 0xFF;
                                b2 = data[offset++] & 0xFF;
                                b3 = data[offset++] & 0xFF;

                                len = (b1 & 0xF) << 12; // len = b000
                                len |= bt << 4; // len = bcd0
                                len |= (b2 >> 4); // len = bcde
                                len += 0x111; // len = bcde + 0x111
                                disp = (b2 & 0x0F) << 8; // disp = f
                                disp |= b3; // disp = fgh
                                break;

                            default:
                                // ab cd
                                // =>
                                // len = a + threshold = a + 1
                                // disp = bcd

                                len = (b1 >> 4) + 1;

                                disp = (b1 & 0x0F) << 8;
                                b2 = data[offset++] & 0xFF;
                                disp |= b2;
                                break;
                        }

                        if (disp > curr_size)
                            throw new Exception("Cannot go back more than already written");

                        cdest = curr_size;

                        for (int j = 0; j < len && curr_size < outData.Length; j++)
                            outData[curr_size++] = outData[cdest - disp - 1 + j];

                        if (curr_size > outData.Length)
                            break;
                    }
                    else
                    {
                        outData[curr_size++] = data[offset++];

                        if (curr_size > outData.Length)
                            break;
                    }
                }

            }
            return outData;
        }

        public static byte[] Compress(byte[] bytes)
        {
            if (bytes.Length == 0) return bytes;
            Stream stream = new MemoryStream(bytes);
            MemoryStream outStream = new MemoryStream();
            new DSDecmp.Formats.Nitro.LZ11().Compress(stream, stream.Length, outStream);
            byte[] buffer = outStream.ToArray();
            stream.Dispose();
            outStream.Dispose();
            return buffer;
        }

        public static Color Read16BitColor(int palValue)
        {
            int red = (int)((palValue & 0x1F) * 8.25);
            int green = (int)(((palValue & 0x3E0) >> 5) * 8.25);
            int blue = (int)(((palValue & 0x7C00) >> 10) * 8.25);
            return Color.FromArgb(255, red, green, blue);
        }

        public static int Write16BitColor(Color color)
        {
            int red = (int)Math.Ceiling(color.R / 8.25);
            int green = (int)Math.Ceiling(color.G / 8.25) << 5;
            int blue = (int)Math.Ceiling(color.B / 8.25) << 10;
            return red | green | blue;
        }

        public static Bitmap DrawTiledImage(byte[] data, Color[] palette, int offset, int width, int height,
            int tileWidth, int tileHeight, int bpp)
        {
            if (bpp != 1 && bpp != 2 && bpp != 4 && bpp != 8)
            {
                throw new Exception("Bits per pixel must be a multiple of 2.");
            }
            int pixelsPerByte = 8 / bpp;
            if (width * height / pixelsPerByte + offset > data.Length)
            {
                return null;
                throw new Exception("Invalid input image.");
            }

            int bytesPerTile = tileWidth * tileHeight / pixelsPerByte;
            int numTiles = width * height / (tileWidth * tileHeight);
            int widthInTiles = width / tileWidth;

            Bitmap bim = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int tile = 0; tile < numTiles; tile++)
            {
                int tileX = tile % widthInTiles;
                int tileY = tile / widthInTiles;
                for (int yT = 0; yT < tileHeight; yT++)
                {
                    for (int xT = 0; xT < tileWidth; xT++)
                    {
                        int value = data[tile * bytesPerTile + yT * tileWidth / pixelsPerByte + xT / pixelsPerByte + offset] & 0xFF;
                        if (pixelsPerByte != 1)
                        {
                            value = (value >> (xT % pixelsPerByte) * bpp) & ((1 << bpp) - 1);
                        }
                        bim.SetPixel(tileX * tileWidth + xT, tileY * tileHeight + yT, palette[value]);
                    }
                }
            }

            return bim;
        }

        static int GetOccurrenceLength(byte[] bytes, int newPtr, int newLength, int oldPtr, int oldLength, out int disp, int minDisp = 1)
        {
            disp = 0;
            if (newLength == 0)
                return 0;
            int maxLength = 0;
            // try every possible 'disp' value (disp = oldLength - i)
            for (int i = 0; i < oldLength - minDisp; i++)
            {
                // work from the start of the old data to the end, to mimic the original implementation's behaviour
                // (and going from start to end or from end to start does not influence the compression ratio anyway)
                int currentOldStart = oldPtr + i;
                int currentLength = 0;
                // determine the length we can copy if we go back (oldLength - i) bytes
                // always check the next 'newLength' bytes, and not just the available 'old' bytes,
                // as the copied data can also originate from what we're currently trying to compress.
                for (int j = 0; j < newLength; j++)
                {
                    // stop when the bytes are no longer the same
                    if (bytes[currentOldStart + j] != bytes[newPtr + j])
                        break;
                    currentLength++;
                }

                // update the optimal value
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                    disp = oldLength - i;

                    // if we cannot do better anyway, stop trying.
                    if (maxLength == newLength)
                        break;
                }
            }
            return maxLength;
        }
    }
}
