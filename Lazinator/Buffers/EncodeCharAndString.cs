using System;
using System.IO;
using System.Text;

namespace Lazinator.Buffers
{
    public static class EncodeCharAndString
    {
        public static void WriteCharInTwoBytes(this BinaryBufferWriter writer, char c)
        {
            writer.Write((byte)((int)c)); // write low byte
            writer.Write((byte)(((int)c) >> 8)); // write high byte
        }

        public static void WriteNullableChar(this BinaryBufferWriter writer, char? c)
        {
            if (c == null)
            {
                writer.Write((byte) 1);
                return;
            }
            writer.Write((byte) 0); 
            // save method call by repeating code from above
            writer.Write((byte)((int)c)); // write low byte
            writer.Write((byte)(((int)c) >> 8)); // write high byte
        }

        /// <summary>
        /// Encode a string to a binary writer by first encoding with a long the length of the string.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="s"></param>
        public static void WriteStringWithVarIntPrefix(this BinaryBufferWriter writer, string s)
        {
            if (s == null)
                CompressedIntegralTypes.WriteCompressedNullableInt(writer, null);
            else
            {
                CompressedIntegralTypes.WriteCompressedNullableInt(writer, s.Length);
                writer.Write(System.Text.Encoding.UTF8.GetBytes(s));
            }
        }

        // The string methods require either a specific range of bytes or a length followed by the number of bytes specified. That length can be stored in different sizes corresponding to different methods. For signed length types, we use -1 to represent null; otherwise, we use the maximum value.
        public static string ToString(this ReadOnlySpan<byte> b, ref int index, int length)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index, length);
            string result = System.Text.Encoding.UTF8.GetString(byteSpan);
            index += length;
            return result;
        }

        public static string ToString_VarIntLength(this ReadOnlySpan<byte> b, ref int index)
        {
            int? length = b.ToDecompressedNullableInt(ref index);
            if (length == null)
                return null;
            string s = b.ToString(ref index, (int)length);
            return s;
        }
    }
}