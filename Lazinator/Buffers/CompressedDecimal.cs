using System;
using System.Runtime.InteropServices;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Read and write to a buffer decimal numbers in a compressed format.
    /// </summary>
    public static class CompressedDecimal
    {
        // The Decimal type contains private fields. To interpret the fields (without using Decimal.GetBits, which allocates an array), we can do the following:
        public struct DecomposedDecimal
        {
            public int flags;
            public int hi;
            public int lo;
            public int mid;
            public uint High => (uint)hi;
            public uint Low => (uint)lo;
            public uint Mid => (uint)mid;
            public bool negative => (flags & 0b10000000_00000000_00000000_00000000) != 0b0;
            public byte scale => (byte) ((flags >> 16) & 0b01111111);

            public DecomposedDecimal(int f, int h, int m, int l)
            {
                flags = f;
                hi = h;
                lo = l;
                mid = m;
            }

            public DecomposedDecimal(bool negative, byte scale, int h, int m, int l)
            {
                flags = GetFlags(negative, scale);
                hi = h;
                lo = l;
                mid = m;
            }

            private static int GetFlags(bool negative, byte scale)
            {
                // flags contains the scale factor and sign of the Decimal: bits 0-15
                // (the lower word) are unused; bits 16-23 contain a value between 0 and
                // 28, indicating the power of 10 to divide the 96-bit integer part by to
                // produce the Decimal value; bits 24-30 are unused; and finally bit 31
                // indicates the sign of the Decimal value, 0 meaning positive and 1
                // meaning negative.
                if (scale > 28)
                    throw new NotSupportedException();
                if (negative)
                    return (int)((scale << 16) | 0b10000000_00000000_00000000_00000000);
                else
                    return scale << 16;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct DecomposableDecimal
        {
            [FieldOffset(0)] public decimal Decimal;
            [FieldOffset(0)] public DecomposedDecimal DecomposedDecimal;

            public DecomposableDecimal(decimal d)
            {
                DecomposedDecimal = new DecomposedDecimal(); // will be overwritten
                Decimal = d;
            }
            public DecomposableDecimal(DecomposedDecimal d)
            {
                Decimal = 0; // will be overwritten
                DecomposedDecimal = d;
            }
        }

        // Decimal: Decimals are 128 bits, but many decimals popularly used can be stored in a relatively small number of bits. 
        // 128 bit -- indicates that the compact format is to be used. Otherwise, we just read the next 128 bits.
        // 64 bit -- indicates null
        // 32 bit -- sign
        // 16 bit -- indicates whether number (ignoring scale) can be stored in a single byte
        // 8 bit -- indicates whether number (ignoring scale) can be stored in a short
        // 4 bit, 2 bit, 1 bit -- combine to produce a number from 0 to 7. The number is divided by 10^this scale.

        public static byte WriteCompressedDecimal(ref BinaryBufferWriter writer, decimal d) => WriteCompressedNullableDecimal(ref writer, d);

        public static byte WriteCompressedNullableDecimal(ref BinaryBufferWriter writer, decimal? d)
        {
            if (d == null)
            {
                writer.Write((byte) (128 + 64));
                return 1;
            }
            decimal d2 = (decimal) d;
            var result = ConvertToInt32egralComponentAndExponent(d2);
            if (result == null)
            {
                writer.Write((byte) 0); // not using compact format
                writer.Write(d2);
                return 17;
            }
            var resultNotNull = result.Value;
            byte b = 128; // use compact format
            if (resultNotNull.negative)
                b += 32;
            if (resultNotNull.fitsInByte)
                b += 16;
            if (resultNotNull.fitsInShort)
                b += 8;
            b += resultNotNull.scale;
            writer.Write(b);
            if (resultNotNull.fitsInByte)
            {
                writer.Write((byte) resultNotNull.integralComponent);
                return 2;
            }
            else if (resultNotNull.fitsInShort)
            {
                writer.Write((ushort) resultNotNull.integralComponent);
                return 3;
            }
            else
            {
                writer.Write((uint) resultNotNull.integralComponent);
                return 5;
            }
        }

        public static decimal ToDecompressedDecimal(this ReadOnlySpan<byte> bytes, ref int index)
        {
            decimal? result = ToDecompressedNullableDecimal(bytes, ref index);
            if (result == null)
                throw new FormatException("CompressedDecimal was null.");
            return (decimal) result;
        }

        public static decimal? ToDecompressedNullableDecimal(this ReadOnlySpan<byte> bytes, ref int index)
        {
            byte b = bytes[index++];
            if (b == (byte) 128 + (byte) 64)
                return null;
            if ((b & (byte) 128) == 0) // if not using compact format
                return bytes.ToDecimal(ref index);
            bool negative = (b & (byte) 32) != 0;
            byte scale = 0;
            if ((b & (byte) 4) != 0)
                scale += 4;
            if ((b & (byte) 2) != 0)
                scale += 2;
            if ((b & (byte) 1) != 0)
                scale += 1;
            uint integralComponent = 0;
            if (((b & (byte) 16) != 0))
            {
                integralComponent = bytes[index++];
            }
            else if (((b & (byte) 8) != 0))
            {
                integralComponent = bytes.ToUInt16(ref index);
            }
            else
            {
                integralComponent = bytes.ToUInt32(ref index);
            }
            DecomposableDecimal d = new DecomposableDecimal(new DecomposedDecimal(negative, scale, 0, 0, (int) integralComponent));
            return d.Decimal;
        }

        private static (int integralComponent, byte scale, bool negative, bool fitsInShort, bool fitsInByte)? ConvertToInt32egralComponentAndExponent(Decimal d)
        {
            DecomposableDecimal de = new DecomposableDecimal(d);
            if (de.DecomposedDecimal.mid != 0 || de.DecomposedDecimal.hi != 0)
                return null;
            if (de.DecomposedDecimal.scale > 0b111)
                return null;

            return (de.DecomposedDecimal.lo, de.DecomposedDecimal.scale, de.DecomposedDecimal.negative, de.DecomposedDecimal.lo <= ushort.MaxValue, de.DecomposedDecimal.lo <= byte.MaxValue);
        }

    }
}