using System;
using System.Buffers.Binary;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Reads from a buffer without compression.
    /// </summary>
    public static class ReadUncompressedPrimitives
    {
        #region Reading data types without compression
        
        public static bool? ToNullableBoolean(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToBoolean(ref index);
        }

        public static byte? ToNullableByte(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToByte(ref index);
        }

        public static sbyte? ToNullableSByte(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToSByte(ref index);
        }


        public static char? ToNullableChar(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToChar(ref index);
        }

        public static float? ToNullableSingle(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToSingle(ref index);
        }

        public static double? ToNullableDouble(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToDouble(ref index);
        }

        public static short? ToNullableShort(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToShort(ref index);
        }

        public static int? ToNullableInt(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToInt(ref index);
        }

        public static long? ToNullableLong(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToLong(ref index);
        }
        public static ushort? ToNullableUshort(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToUshort(ref index);
        }

        public static uint? ToNullableUint(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToUint(ref index);
        }

        public static ulong? ToNullableUlong(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToUlong(ref index);
        }
        public static DateTime? ToNullableDateTime(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToDateTime(ref index);
        }
        public static TimeSpan? ToNullableTimeSpan(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToTimeSpan(ref index);
        }
        public static Guid? ToNullableGuid(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToGuid(ref index);
        }
        public static decimal? ToNullableDecimal(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToDecimal(ref index);
        }

        public static bool ToBoolean(this ReadOnlySpan<byte> b, ref int index)
        {
            return b[index++] != 0;
        }

        public static byte ToByte(this ReadOnlySpan<byte> b, ref int index)
        {
            return b[index++];
        }
        
        public static sbyte ToSByte(this ReadOnlySpan<byte> b, ref int index)
        {
            return (sbyte) b[index++];
        }

        public static char ToChar(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToChar(byteSpan);
            index += sizeof(char);
            return result;
        }
        public static float ToSingle(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.Int32BitsToSingle(BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadInt32LittleEndian(byteSpan) : BinaryPrimitives.ReadInt32BigEndian(byteSpan));
            index += sizeof(float);
            return result;
        }
        public static double ToDouble(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.Int64BitsToDouble(BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadInt64LittleEndian(byteSpan) : BinaryPrimitives.ReadInt64BigEndian(byteSpan));
            index += sizeof(double);
            return result;
        }
        public static short ToShort(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadInt16LittleEndian(b) : BinaryPrimitives.ReadInt16BigEndian(b);
            index += sizeof(short);
            return result;
        }
        public static int ToInt(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadInt32LittleEndian(byteSpan) : BinaryPrimitives.ReadInt32BigEndian(byteSpan);
            index += sizeof(int);
            return result;
        }
        public static long ToLong(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadInt64LittleEndian(byteSpan) : BinaryPrimitives.ReadInt64BigEndian(byteSpan);
            index += sizeof(long);
            return result;
        }
        public static ushort ToUshort(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadUInt16LittleEndian(byteSpan) : BinaryPrimitives.ReadUInt16BigEndian(byteSpan);
            index += sizeof(ushort);
            return result;
        }
        public static uint ToUint(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadUInt32LittleEndian(byteSpan) : BinaryPrimitives.ReadUInt32BigEndian(byteSpan);
            index += sizeof(uint);
            return result;
        }
        public static ulong ToUlong(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BinaryBufferWriter.LittleEndianStorage ? BinaryPrimitives.ReadUInt64LittleEndian(byteSpan) : BinaryPrimitives.ReadUInt64BigEndian(byteSpan);
            index += sizeof(ulong);
            return result;
        }

        public static DateTime ToDateTime(this ReadOnlySpan<byte> b, ref int index)
        {
            long asInt64 = b.ToLong(ref index);
            var result = new DateTime(asInt64);
            return result;
        }
        public static TimeSpan ToTimeSpan(this ReadOnlySpan<byte> b, ref int index)
        {
            long asInt64 = b.ToLong(ref index);
            var result = new TimeSpan(asInt64);
            return result;
        }

        public static Guid ToGuid(this ReadOnlySpan<byte> b, ref int index)
        {
            Guid g = new Guid(b.Slice(index, 16));
            index += 16;
            return g;
        }

        public const byte DecimalSignBit = 128;

        public static decimal ToDecimal(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            decimal result;
            if (BinaryBufferWriter.LittleEndianStorage)
                result = new decimal(
                BinaryPrimitives.ReadInt32LittleEndian(byteSpan),
                BinaryPrimitives.ReadInt32LittleEndian(byteSpan.Slice(sizeof(int))),
                BinaryPrimitives.ReadInt32LittleEndian(byteSpan.Slice(2*sizeof(int))),
                byteSpan[15] == DecimalSignBit,
                byteSpan[14]); // see https://stackoverflow.com/questions/16979164/efficiently-convert-byte-array-to-decimal
            else
                result = new decimal(
                    BinaryPrimitives.ReadInt32BigEndian(byteSpan),
                    BinaryPrimitives.ReadInt32BigEndian(byteSpan.Slice(sizeof(int))),
                    BinaryPrimitives.ReadInt32BigEndian(byteSpan.Slice(2 * sizeof(int))),
                    byteSpan[15] == DecimalSignBit, // still little endian within last int
                    byteSpan[14]); // see https://stackoverflow.com/questions/16979164/efficiently-convert-byte-array-to-decimal
            index += sizeof(decimal);
            return result;
        }
        
        

        #endregion
    }
}
