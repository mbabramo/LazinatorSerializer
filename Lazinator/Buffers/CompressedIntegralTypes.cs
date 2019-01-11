using System;

namespace Lazinator.Buffers
{

    public static class CompressedIntegralTypes
    {
        #region Compression of int/long

        public static byte WriteCompressedInt(ref BinaryBufferWriter writer, int value)
        {
            uint num = (uint) value;
            byte numBytes = 0;

            while (num >= 128U)
            {
                writer.Write((byte) (num | 128U));
                num >>= 7;
                numBytes++;
            }

            writer.Write((byte) num);
            numBytes++;

            return numBytes;
        }

        public static int ToDecompressedInt(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int returnValue = 0;
            int bitIndex = 0;
            byte numBytes = 0;

            while (bitIndex != 35)
            {
                byte currentByte = bytes[index + numBytes++];
                returnValue |= ((int) currentByte & (int) sbyte.MaxValue) << bitIndex;
                bitIndex += 7;

                if (((int) currentByte & 128) == 0)
                {
                    index += numBytes;
                    return returnValue;
                }
            }

            throw new FormatException("Format_Bad7BitInt32");
        }

        public static byte WriteCompressedLong(ref BinaryBufferWriter writer, long value)
        {
            ulong num = (ulong) value;
            byte numBytes = 0;

            while (num >= 128U)
            {
                writer.Write((byte) (num | 128U));
                num >>= 7;
                numBytes++;
            }
            
            writer.Write((byte) num);
            numBytes++;

            return numBytes;
        }

        public static long ToDecompressedLong(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long returnValue = 0;
            int bitIndex = 0;
            byte numBytes = 0;

            while (bitIndex != 70)
            {
                byte currentByte = bytes[index + numBytes++];
                returnValue |= ((long) currentByte & (long) sbyte.MaxValue) << bitIndex;
                bitIndex += 7;

                if (((long) currentByte & 128) == 0)
                {
                    index += numBytes;
                    return returnValue;
                }
            }

            throw new FormatException("Format_Bad7BitInt64");
        }

        #endregion

        #region Shorts and ushorts (using int methods)

        public static byte WriteCompressedShort(ref BinaryBufferWriter writer, short value) => WriteCompressedInt(ref writer, value);

        public static short ToDecompressedShort(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int value = ToDecompressedInt(bytes, ref index);
            if (value > short.MaxValue || value < short.MinValue)
                throw new FormatException("Format_BadCompressedUshort");
            return (short)value;
        }

        public static byte WriteCompressedUshort(ref BinaryBufferWriter writer, ushort value) => WriteCompressedUint(ref writer, value);

        public static ushort ToDecompressedUshort(this ReadOnlySpan<byte> bytes, ref int index)
        {
            uint value = ToDecompressedUint(bytes, ref index);
            if (value > ushort.MaxValue)
                throw new FormatException("Format_BadCompressedUshort");
            return (ushort)value;
        }

        #endregion

        #region Unsigned (using int/long methods)

        public static byte WriteCompressedUint(ref BinaryBufferWriter writer, uint value) => WriteCompressedInt(ref writer, (int) value);

        public static uint ToDecompressedUint(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int value = ToDecompressedInt(bytes, ref index);
            return (uint) value;
        }

        public static byte WriteCompressedUlong(ref BinaryBufferWriter writer, ulong value) => WriteCompressedLong(ref writer, (long) value);

        public static ulong ToDecompressedUlong(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long value = ToDecompressedLong(bytes, ref index);
            return (ulong) value;
        }


        #endregion

        #region Nullable encoding int/long

        // Supporting null: We use the usual approach to 7-bit encoding, but we take advantage of the fact that in standard 7-bit encoding, there are two byte sequences not corresponding to a number, specifically where the high bit of the first byte is set (indicating that there is more data) and the second byte is 0 (indicating that actually there isn't). We could just assign this to null, but since null is probably more common than 127, we use this special combination to represent null and we encode null as 127 (thus using only a single byte).

        public static byte WriteCompressedNullableInt(ref BinaryBufferWriter writer, int? value)
        {
            if (value == null)
                value = 127;
            else if (value == 127)
            {
                writer.Write((byte) (127 + 128));
                writer.Write((byte) 0);
                return 2;
            }
            return WriteCompressedInt(ref writer, (int) value);
        }

        public static int? ToDecompressedNullableInt(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int initialIndex = index;
            int initialValue = ToDecompressedInt(bytes, ref index);
            if (initialValue == 127)
            {
                if (index == initialIndex + 1)
                    return null;
                return 127;
            }

            return (int?) initialValue;
        }

        public static byte WriteCompressedNullableLong(ref BinaryBufferWriter writer, long? value)
        {
            if (value == null)
                value = 127;
            else if (value == 127)
            {
                writer.Write((byte) (127 + 128));
                writer.Write((byte) 0);
                return 2;
            }
            return WriteCompressedLong(ref writer, (long) value);
        }

        public static long? ToDecompressedNullableLong(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int initialIndex = index;
            long initialValue = ToDecompressedLong(bytes, ref index);
            if (initialValue == 127)
            {
                if (index == initialIndex + 1)
                    return null;
                return 127;
            }

            return (long?) initialValue;
        }

        #endregion

        #region Nullable shorts, ushorts, uint, ulong

        public static byte WriteCompressedNullableShort(ref BinaryBufferWriter writer, short? value) => WriteCompressedNullableInt(ref writer, value);

        public static short? ToDecompressedNullableShort(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt(bytes, ref index);
            if (value == null)
                return null;
            if (value > short.MaxValue || value < short.MinValue)
                throw new FormatException("Format_BadCompressedNullableShort");
            return (short) (int) value;
        }

        public static byte WriteCompressedNullableUshort(ref BinaryBufferWriter writer, ushort? value) => WriteCompressedNullableInt(ref writer, (int?)value);

        public static ushort? ToDecompressedNullableUshort(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt(bytes, ref index);
            if (value > ushort.MaxValue || value < ushort.MinValue)
                throw new FormatException("Format_BadCompressedNullableUshort");
            return (ushort?)value;
        }

        public static byte WriteCompressedNullableUint(ref BinaryBufferWriter writer, uint? value) => WriteCompressedNullableInt(ref writer, (int?) value);

        public static uint? ToDecompressedNullableUint(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt(bytes, ref index);
            return (uint?) value;
        }

        public static byte WriteCompressedNullableUlong(ref BinaryBufferWriter writer, ulong? value) => WriteCompressedNullableLong(ref writer, (long?) value);

        public static ulong? ToDecompressedNullableUlong(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long? value = ToDecompressedNullableLong(bytes, ref index);
            return (ulong?) value;
        }

        #endregion

        #region Nullable bytes and bools (using int methods for bytes)

        public static byte WriteCompressedNullableSByte(ref BinaryBufferWriter writer, sbyte? value) => WriteCompressedNullableByte(ref writer, (byte?)value);

        public static sbyte? ToDecompressedNullableSByte(this ReadOnlySpan<byte> bytes, ref int index) =>
            (sbyte?) ToDecompressedNullableByte(bytes, ref index);

        public static byte WriteCompressedNullableByte(ref BinaryBufferWriter writer, byte? value) => WriteCompressedNullableInt(ref writer, value);

        public static byte? ToDecompressedNullableByte(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt(bytes, ref index);
            if (value == null)
                return null;
            if (value > byte.MaxValue)
                throw new FormatException("Format_BadCompressedNullableByte");
            return (byte) (int) value;
        }

        public static byte WriteCompressedNullableBool(ref BinaryBufferWriter writer, bool? value)
        {
            if (value == true)
                writer.Write((byte) 1);
            else if (value == false)
                writer.Write((byte) 0);
            else
                writer.Write((byte) 2);
            return 1;
        }

        public static bool? ToDecompressedNullableBool(this ReadOnlySpan<byte> bytes, ref int index)
        {
            byte value = bytes.ToByte(ref index);
            if (value > 2)
                throw new FormatException("Format_BadCompressedNullableBool");
            if (value == 0)
                return false;
            else if (value == 1)
                return true;
            else
                return null;
        }

        #endregion

        #region Dates and time spans (including nullables)

        public static byte WriteCompressedTimeSpan(ref BinaryBufferWriter writer, TimeSpan value) => WriteCompressedLong(ref writer, value.Ticks);

        public static TimeSpan ToDecompressedTimeSpan(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long value = ToDecompressedLong(bytes, ref index);
            return new TimeSpan(value);
        }

        public static byte WriteCompressedNullableTimeSpan(ref BinaryBufferWriter writer, TimeSpan? value) => WriteCompressedNullableLong(ref writer, value?.Ticks);

        public static TimeSpan? ToDecompressedNullableTimeSpan(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long? value = ToDecompressedNullableLong(bytes, ref index);
            return value == null ? (TimeSpan?) null : new TimeSpan((long) value);
        }

        public static byte WriteCompressedDateTime(ref BinaryBufferWriter writer, DateTime value) => WriteCompressedLong(ref writer, value.Ticks);

        public static DateTime ToDecompressedDateTime(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long value = ToDecompressedLong(bytes, ref index);
            return new DateTime(value);
        }

        public static byte WriteCompressedNullableDateTime(ref BinaryBufferWriter writer, DateTime? value) => WriteCompressedNullableLong(ref writer, value?.Ticks);

        public static DateTime? ToDecompressedNullableDateTime(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long? value = ToDecompressedNullableLong(bytes, ref index);
            return value == null ? (DateTime?)null : new DateTime((long)value);
        }

        #endregion
    }
}
