using System;
using System.Text;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Buffers.Text;
using Lazinator.Exceptions;

namespace Lazinator.Buffers
{
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

        public static short? ToNullableInt16(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToInt16(ref index);
        }

        public static int? ToNullableInt32(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToInt32(ref index);
        }

        public static long? ToNullableInt64(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToInt64(ref index);
        }
        public static ushort? ToNullableUint16(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToUInt16(ref index);
        }

        public static uint? ToNullableUint32(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToUInt32(ref index);
        }

        public static ulong? ToNullableUint64(this ReadOnlySpan<byte> b, ref int index)
        {
            bool isNull = !b.ToBoolean(ref index);
            if (isNull)
                return null;
            return b.ToUInt64(ref index);
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
            var result = BitConverter.ToSingle(byteSpan);
            index += sizeof(float);
            return result;
        }
        public static double ToDouble(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToDouble(byteSpan);
            index += sizeof(double);
            return result;
        }
        public static short ToInt16(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToInt16(byteSpan);
            index += sizeof(short);
            return result;
        }
        public static int ToInt32(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToInt32(byteSpan);
            index += sizeof(int);
            return result;
        }
        public static long ToInt64(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToInt64(byteSpan);
            index += sizeof(long);
            return result;
        }
        public static ushort ToUInt16(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToUInt16(byteSpan);
            index += sizeof(ushort);
            return result;
        }
        public static uint ToUInt32(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToUInt32(byteSpan);
            index += sizeof(uint);
            return result;
        }
        public static ulong ToUInt64(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = BitConverter.ToUInt64(byteSpan);
            index += sizeof(ulong);
            return result;
        }

        public static DateTime ToDateTime(this ReadOnlySpan<byte> b, ref int index)
        {
            long asInt64 = b.ToInt64(ref index);
            var result = new DateTime(asInt64);
            return result;
        }
        public static TimeSpan ToTimeSpan(this ReadOnlySpan<byte> b, ref int index)
        {
            long asInt64 = b.ToInt64(ref index);
            var result = new TimeSpan(asInt64);
            return result;
        }

        public static Guid ToGuid(this ReadOnlySpan<byte> b, ref int index)
        {
            int itema = b.ToInt32(ref index);
            short itemb = b.ToInt16(ref index);
            short itemc = b.ToInt16(ref index);
            byte itemd = b.ToByte(ref index);
            byte iteme = b.ToByte(ref index);
            byte itemf = b.ToByte(ref index);
            byte itemg = b.ToByte(ref index);
            byte itemh = b.ToByte(ref index);
            byte itemi = b.ToByte(ref index);
            byte itemj = b.ToByte(ref index);
            byte itemk = b.ToByte(ref index);
            var result = new Guid(itema, itemb, itemc, itemd, iteme, itemf, itemg, itemh, itemi, itemj, itemk);
            return result;
        }

        public const byte DecimalSignBit = 128;

        public static decimal ToDecimal(this ReadOnlySpan<byte> b, ref int index)
        {
            ReadOnlySpan<byte> byteSpan = b.Slice(index);
            var result = new decimal(
                BitConverter.ToInt32(byteSpan),
                BitConverter.ToInt32(byteSpan.Slice(sizeof(int))),
                BitConverter.ToInt32(byteSpan.Slice(2*sizeof(int))),
                byteSpan[15] == DecimalSignBit,
                byteSpan[14]); // see https://stackoverflow.com/questions/16979164/efficiently-convert-byte-array-to-decimal
            index += sizeof(decimal);
            return result;
        }
        
        

        #endregion
    }
}
