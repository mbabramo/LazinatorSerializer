using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Lazinator.Core;
using Lazinator.Exceptions;

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
        public static void WriteStringUtf8WithVarIntPrefix(this BinaryBufferWriter writer, string s)
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

        public static string ToString_VarIntLengthUtf8(this ReadOnlySpan<byte> b, ref int index)
        {
            int? length = b.ToDecompressedNullableInt(ref index);
            if (length == null)
                return null;
            string s = b.ToString(ref index, (int)length);
            return s;
        }

        public static void WriteBrotliCompressed(this BinaryBufferWriter writer, string s)
        {
            bool success = false;
            int bytesWritten = 0;
            const int maxTries = 1000;
            int tryNum = 0;
            while (!success)
            {
                success = System.IO.Compression.BrotliEncoder.TryCompress(MemoryMarshal.Cast<char, byte>(s.AsSpan()), writer.Free, out bytesWritten);
                if (success)
                    writer.Position += bytesWritten;
                else
                {
                    tryNum++;
                    if (tryNum > maxTries)
                        throw new LazinatorSerializationException("BadBrotliString");
                    writer.Resize();
                }
            }
        }

        public static void WriteBrotliCompressedWithIntPrefix(this BinaryBufferWriter writer, string s)
        {
            if (s == null)
                writer.Write((int) -1); // signify null
            else
                LazinatorUtilities.WriteToBinaryWithIntLengthPrefix(writer, (w) => { WriteBrotliCompressed(w, s); });
        }


        public static string ToString_BrotliCompressedWithLength(this ReadOnlySpan<byte> b, ref int index)
        {
            int length = b.ToInt32(ref index);
            if (length == -1)
                return null;
            ReadOnlySpan<byte> source = b.Slice(index, length);
            BinaryBufferWriter decompressionBuffer = new BinaryBufferWriter(4096); // rent a buffer
            bool success = false;
            const int maxTries = 1000;
            int tryNum = 0;
            while (!success)
            {
                success = System.IO.Compression.BrotliDecoder.TryDecompress(source, decompressionBuffer.Free,
                    out int bytesRead);
                if (success)
                {
                    decompressionBuffer.Position += bytesRead;
                    index += length;
                }
                else
                {
                    tryNum++;
                    if (tryNum > maxTries)
                        throw new LazinatorSerializationException("BadBrotliString");
                    decompressionBuffer.Resize();
                }
            }

            return new string(MemoryMarshal.Cast<byte, char>(decompressionBuffer.Written));
        }
    }
}