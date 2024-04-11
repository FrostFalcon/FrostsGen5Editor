using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public static class BLZDecoder
    {
        private const int CMD_DECODE = 0;
        private const int CMD_ENCODE = 1;

        private const int BLZ_NORMAL = 0;
        private const int BLZ_BEST = 1;

        private const int BLZ_SHIFT = 1;
        private const int BLZ_MASK = 0x80;

        private const int BLZ_THRESHOLD = 2;
        private const int BLZ_N = 0x1002;
        private const int BLZ_F = 0x12;
        private const int RAW_MAXIM = 0x00FFFFFF;

        private const int BLZ_MAXIM = 0x01400000;

        private static int[] prepareData(byte[] data)
        {
            int fs = data.Length;
            int[] fb = new int[fs + 3];
            for (int i = 0; i < fs; i++)
            {
                fb[i] = data[i] & 0xFF;
            }
            return fb;
        }
        private static int readUnsigned(int[] buffer, int offset)
        {
            return buffer[offset] | (buffer[offset + 1] << 8) | (buffer[offset + 2] << 16)
                    | ((buffer[offset + 3] & 0x7F) << 24);
        }
        private static void BLZ_Invert(int[] buffer, int offset, int length)
        {
            int bottom, ch;

            bottom = offset + length - 1;

            while (offset < bottom)
            {
                ch = buffer[offset];
                buffer[offset++] = buffer[bottom];
                buffer[bottom--] = ch;
            }
        }

        public static byte[] BLZ_DecodePub(byte[] data)
        {
            int[] result = BLZ_Decode(data);
            if (result != null)
            {
                byte[] retbuf = new byte[result.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    retbuf[i] = (byte)result[i];
                }
                result = null;
                return retbuf;
            }
            else
            {
                return null;
            }
        }

        private static int[] BLZ_Decode(byte[] data)
        {
            int[] pak_buffer, raw_buffer;
            int pak, raw, pak_end, raw_end;
            int pak_len, raw_len, len, pos, inc_len, hdr_len, enc_len, dec_len;
            int flags = 0, mask;

            pak_buffer = prepareData(data);
            pak_len = pak_buffer.Length - 3;

            inc_len = readUnsigned(pak_buffer, pak_len - 4);
            if (inc_len < 1)
            {
                System.Diagnostics.Debug.WriteLine(", WARNING: not coded file!");
                enc_len = 0;
                dec_len = pak_len;
                pak_len = 0;
                raw_len = dec_len;
            }
            else
            {
                if (pak_len < 8)
                {
                    System.Diagnostics.Debug.WriteLine("\nFile has a bad header\n");
                    Program.main.Close();
                    return null;
                }
                hdr_len = pak_buffer[pak_len - 5];
                if (hdr_len < 8 || hdr_len > 0xB)
                {
                    System.Diagnostics.Debug.WriteLine("\nBad header length\n");
                    Program.main.Close();
                    return null;
                }
                if (pak_len <= hdr_len)
                {
                    System.Diagnostics.Debug.WriteLine("\nBad length\n");
                    Program.main.Close();
                    return null;
                }
                enc_len = readUnsigned(pak_buffer, pak_len - 8) & 0x00FFFFFF;
                dec_len = pak_len - enc_len;
                pak_len = enc_len - hdr_len;
                raw_len = dec_len + enc_len + inc_len;
                if (raw_len > RAW_MAXIM)
                {
                    System.Diagnostics.Debug.WriteLine("\nBad decoded length\n");
                    Program.main.Close();
                    return null;
                }
            }

            raw_buffer = new int[raw_len];

            pak = 0;
            raw = 0;
            pak_end = dec_len + pak_len;
            raw_end = raw_len;

            for (len = 0; len < dec_len; len++)
            {
                raw_buffer[raw++] = pak_buffer[pak++];
            }

            BLZ_Invert(pak_buffer, dec_len, pak_len);

            mask = 0;

            while (raw < raw_end)
            {
                if ((mask = (mask >> BLZ_SHIFT)) == 0)
                {
                    if (pak == pak_end)
                    {
                        break;
                    }
                    flags = pak_buffer[pak++];
                    mask = BLZ_MASK;
                }

                if ((flags & mask) == 0)
                {
                    if (pak == pak_end)
                    {
                        break;
                    }
                    raw_buffer[raw++] = pak_buffer[pak++];
                }
                else
                {
                    if ((pak + 1) >= pak_end)
                    {
                        break;
                    }
                    pos = pak_buffer[pak++] << 8;
                    pos |= pak_buffer[pak++];
                    len = (pos >> 12) + BLZ_THRESHOLD + 1;
                    if (raw + len > raw_end)
                    {
                        System.Diagnostics.Debug.WriteLine(", WARNING: wrong decoded length!");
                        len = raw_end - raw;
                    }
                    pos = (pos & 0xFFF) + 3;
                    while ((len--) > 0)
                    {
                        int charHere = 0;
                        if (raw - pos > 0) charHere = raw_buffer[raw - pos];
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Error in raw - pos");
                        }
                        raw_buffer[raw++] = charHere;
                    }
                }
            }

            BLZ_Invert(raw_buffer, dec_len, raw_len - dec_len);

            raw_len = raw;

            if (raw != raw_end)
            {
                System.Diagnostics.Debug.WriteLine(", WARNING: unexpected end of encoded file!");
            }

            return raw_buffer;
        }

        private static void WriteUnsigned(int[] buffer, int offset, int value)
        {
            buffer[offset] = value & 0xFF;
            buffer[offset + 1] = (value >> 8) & 0xFF;
            buffer[offset + 2] = (value >> 16) & 0xFF;
            buffer[offset + 3] = (value >> 24) & 0x7F;
        }

        private static int new_len;

        public static byte[] BLZ_EncodePub(byte[] data, bool best)
        {
            int mode = best ? BLZ_BEST : BLZ_NORMAL;
            BLZResult result = BLZ_Encode(data, mode);
            if (result != null)
            {
                byte[] retbuf = new byte[result.length];
                for (int i = 0; i < result.length; i++)
                {
                    retbuf[i] = (byte)result.buffer[i];
                }
                result = null;
                return retbuf;
            }
            else
            {
                return null;
            }
        }

        private static BLZResult BLZ_Encode(byte[] data, int mode)
        {
            int[] raw_buffer, pak_buffer, new_buffer;
            int raw_len, pak_len;

            new_len = 0;

            raw_buffer = prepareData(data);
            raw_len = raw_buffer.Length - 3;

            pak_buffer = null;
            pak_len = BLZ_MAXIM + 1;

            new_buffer = BLZ_Code(raw_buffer, raw_len, mode);

            if (new_len < pak_len)
            {
                pak_buffer = new_buffer;
                pak_len = new_len;
            }
            return new BLZResult(pak_buffer, pak_len);
        }

        private static int[] BLZ_Code(int[] raw_buffer, int raw_len, int best)
        {
            int[] pak_buffer, tmp;
            int pak, raw, raw_end, flg = 0;
            int pak_len, inc_len, hdr_len, enc_len, len;
            int len_best, pos_best = 0, len_next, pos_next = 0, len_post, pos_post = 0;
            int pak_tmp, raw_tmp, raw_new;
            int mask;

            pak_tmp = 0;
            raw_tmp = raw_len;

            pak_len = raw_len + ((raw_len + 7) / 8) + 11;
            pak_buffer = new int[pak_len];

            raw_new = raw_len;

            // We don't do any of the checks here
            // Presume that we actually are using an arm9
            raw_new -= 0x4000;

            BLZ_Invert(raw_buffer, 0, raw_len);

            pak = 0;
            raw = 0;
            raw_end = raw_new;

            mask = 0;
            while (raw < raw_end)
            {
                if ((mask = (mask >> BLZ_SHIFT)) == 0)
                {
                    pak_buffer[(flg = pak++)] = 0;
                    mask = BLZ_MASK;
                }

                SearchPair sl1 = SEARCH(pos_best, raw_buffer, raw, raw_end);
                len_best = sl1.l;
                pos_best = sl1.p;

                // LZ-CUE optimization start
                if (best == BLZ_BEST)
                {
                    if (len_best > BLZ_THRESHOLD)
                    {
                        if (raw + len_best < raw_end)
                        {
                            raw += len_best;
                            SearchPair sl2 = SEARCH(pos_next, raw_buffer, raw, raw_end);
                            len_next = sl2.l;
                            pos_next = sl2.p;
                            raw -= (len_best - 1);
                            SearchPair sl3 = SEARCH(pos_post, raw_buffer, raw, raw_end);
                            len_post = sl3.l;
                            pos_post = sl3.p;
                            raw--;

                            if (len_next <= BLZ_THRESHOLD)
                            {
                                len_next = 1;
                            }
                            if (len_post <= BLZ_THRESHOLD)
                            {
                                len_post = 1;
                            }
                            if ((len_best + len_next) <= (1 + len_post))
                            {
                                len_best = 1;
                            }
                        }
                    }
                }
                // LZ-CUE optimization end
                pak_buffer[flg] = (pak_buffer[flg] << 1);
                if (len_best > BLZ_THRESHOLD)
                {
                    raw += len_best;
                    pak_buffer[flg] |= 1;
                    pak_buffer[pak++] = ((len_best - (BLZ_THRESHOLD + 1)) << 4) | ((pos_best - 3) >> 8);
                    pak_buffer[pak++] = (pos_best - 3) & 0xFF;
                }
                else
                {
                    pak_buffer[pak++] = raw_buffer[raw++];
                }

                if (pak + raw_len - raw < pak_tmp + raw_tmp)
                {
                    pak_tmp = pak;
                    raw_tmp = raw_len - raw;
                }
            }

            while ((mask > 0) && (mask != 1))
            {
                mask = (mask >> BLZ_SHIFT);
                pak_buffer[flg] = pak_buffer[flg] << 1;
            }

            pak_len = pak;

            BLZ_Invert(raw_buffer, 0, raw_len);
            BLZ_Invert(pak_buffer, 0, pak_len);

            if (pak_tmp == 0 || (raw_len + 4 < ((pak_tmp + raw_tmp + 3) & 0xFFFFFFFC) + 8))
            {
                pak = 0;
                raw = 0;
                raw_end = raw_len;

                while (raw < raw_end)
                {
                    pak_buffer[pak] = raw_buffer[raw];
                }

                while ((pak & 3) > 0)
                {
                    pak_buffer[pak++] = 0;
                }

                pak_buffer[pak++] = 0;
                pak_buffer[pak++] = 0;
                pak_buffer[pak++] = 0;
                pak_buffer[pak++] = 0;
            }
            else
            {
                tmp = new int[raw_tmp + pak_tmp + 11];
                for (len = 0; len < raw_tmp; len++)
                {
                    tmp[len] = raw_buffer[len];
                }
                for (len = 0; len < pak_tmp; len++)
                {
                    tmp[raw_tmp + len] = pak_buffer[len + pak_len - pak_tmp];
                }

                pak = 0;
                pak_buffer = tmp;

                pak = raw_tmp + pak_tmp;

                enc_len = pak_tmp;
                hdr_len = 8;
                inc_len = raw_len - pak_tmp - raw_tmp;

                while ((pak & 3) > 0)
                {
                    pak_buffer[pak++] = 0xFF;
                    hdr_len++;
                }

                WriteUnsigned(pak_buffer, pak, enc_len + hdr_len);
                pak += 3;
                pak_buffer[pak++] = hdr_len;
                WriteUnsigned(pak_buffer, pak, inc_len - hdr_len);
                pak += 4;

            }
            new_len = pak;
            return pak_buffer;
        }

        private static SearchPair SEARCH(int p, int[] raw_buffer, int raw, int raw_end)
        {
            int l = BLZ_THRESHOLD;
            int max = (raw >= BLZ_N) ? BLZ_N : raw;
            for (int pos = 3; pos <= max; pos++)
            {
                int len;
                for (len = 0; len < BLZ_F; len++)
                {
                    if (raw + len == raw_end)
                    {
                        break;
                    }
                    if (len >= pos)
                    {
                        break;
                    }
                    if (raw_buffer[raw + len] != raw_buffer[raw + len - pos])
                    {
                        break;
                    }
                }

                if (len > l)
                {
                    p = pos;
                    if ((l = len) == BLZ_F)
                    {
                        break;
                    }
                }
            }
            return new SearchPair(l, p);
        }
    }

    class BLZResult
    {
        public int[] buffer;
        public int length;

        public BLZResult(int[] raw_buffer, int raw_len)
        {
            this.buffer = raw_buffer;
            this.length = raw_len;
        }
    }

    class SearchPair
    {
        public int l;
        public int p;

        public SearchPair(int l, int p)
        {
            this.l = l;
            this.p = p;
        }
    }
}
