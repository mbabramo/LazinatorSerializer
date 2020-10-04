using System;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Read and write to a buffer integral types (including short and long, signed and unsigned, as well as dates and time spans), using compression.
    /// </summary>
    public static class CompressedIntegralTypes
    {
        #region Compression of int/long

        public static byte WriteCompressedInt(BinaryBufferWriter writer, int value)
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

        public static int ToDecompressedInt32(this ReadOnlySpan<byte> bytes, ref int index)
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

        public static byte WriteCompressedLong(BinaryBufferWriter writer, long value)
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

        public static long ToDecompressedInt64(this ReadOnlySpan<byte> bytes, ref int index)
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

        public static byte WriteCompressedShort(BinaryBufferWriter writer, short value) => WriteCompressedInt(writer, value);

        public static short ToDecompressedInt16(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int value = ToDecompressedInt32(bytes, ref index);
            if (value > short.MaxValue || value < short.MinValue)
                throw new FormatException("Format_BadCompressedUShort");
            return (short)value;
        }

        public static byte WriteCompressedUShort(BinaryBufferWriter writer, ushort value) => WriteCompressedUInt(writer, value);

        public static ushort ToDecompressedUInt16(this ReadOnlySpan<byte> bytes, ref int index)
        {
            uint value = ToDecompressedUInt32(bytes, ref index);
            if (value > ushort.MaxValue)
                throw new FormatException("Format_BadCompressedUShort");
            return (ushort)value;
        }

        #endregion

        #region Unsigned (using int/long methods)

        public static byte WriteCompressedUInt(BinaryBufferWriter writer, uint value) => WriteCompressedInt(writer, (int) value);

        public static uint ToDecompressedUInt32(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int value = ToDecompressedInt32(bytes, ref index);
            return (uint) value;
        }

        public static byte WriteCompressedULong(BinaryBufferWriter writer, ulong value) => WriteCompressedLong(writer, (long) value);

        public static ulong ToDecompressedUInt64(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long value = ToDecompressedInt64(bytes, ref index);
            return (ulong) value;
        }


        #endregion

        #region Nullable encoding int/long

        // Supporting null: We use the usual approach to 7-bit encoding, but we take advantage of the fact that in standard 7-bit encoding, there are two byte sequences not corresponding to a number, specifically where the high bit of the first byte is set (indicating that there is more data) and the second byte is 0 (indicating that actually there isn't). (Note that 128 is represented as 127, 1.) We could just assign this to null, but since null is probably more common than 127, we use this special combination to represent null and we encode null as 127 (thus using only a single byte).

        public static byte WriteCompressedNullableInt(BinaryBufferWriter writer, int? value)
        {
            if (value == null)
                value = 127;
            else if (value == 127)
            {
                writer.Write((byte) (127 + 128));
                writer.Write((byte) 0);
                return 2;
            }
            return WriteCompressedInt(writer, (int) value);
        }

        public static int? ToDecompressedNullableInt32(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int initialIndex = index;
            int initialValue = ToDecompressedInt32(bytes, ref index);
            if (initialValue == 127)
            {
                if (index == initialIndex + 1)
                    return null;
                return 127;
            }

            return (int?) initialValue;
        }

        public static byte WriteCompressedNullableLong(BinaryBufferWriter writer, long? value)
        {
            if (value == null)
                value = 127;
            else if (value == 127)
            {
                writer.Write((byte) (127 + 128));
                writer.Write((byte) 0);
                return 2;
            }
            return WriteCompressedLong(writer, (long) value);
        }

        public static long? ToDecompressedNullableInt64(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int initialIndex = index;
            long initialValue = ToDecompressedInt64(bytes, ref index);
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

        public static byte WriteCompressedNullableShort(BinaryBufferWriter writer, short? value) => WriteCompressedNullableInt(writer, value);

        public static short? ToDecompressedNullableInt16(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt32(bytes, ref index);
            if (value == null)
                return null;
            if (value > short.MaxValue || value < short.MinValue)
                throw new FormatException("Format_BadCompressedNullableShort");
            return (short) (int) value;
        }

        public static byte WriteCompressedNullableUShort(BinaryBufferWriter writer, ushort? value) => WriteCompressedNullableInt(writer, (int?)value);

        public static ushort? ToDecompressedNullableUInt16(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt32(bytes, ref index);
            if (value > ushort.MaxValue || value < ushort.MinValue)
                throw new FormatException("Format_BadCompressedNullableUShort");
            return (ushort?)value;
        }

        public static byte WriteCompressedNullableUInt(BinaryBufferWriter writer, uint? value) => WriteCompressedNullableInt(writer, (int?) value);

        public static uint? ToDecompressedNullableUInt32(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt32(bytes, ref index);
            return (uint?) value;
        }

        public static byte WriteCompressedNullableULong(BinaryBufferWriter writer, ulong? value) => WriteCompressedNullableLong(writer, (long?) value);

        public static ulong? ToDecompressedNullableUInt64(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long? value = ToDecompressedNullableInt64(bytes, ref index);
            return (ulong?) value;
        }

        #endregion

        #region Nullable bytes and bools (using int methods for bytes)

        public static byte WriteCompressedNullableSByte(BinaryBufferWriter writer, sbyte? value) => WriteCompressedNullableByte(writer, (byte?)value);

        public static sbyte? ToDecompressedNullableSByte(this ReadOnlySpan<byte> bytes, ref int index) =>
            (sbyte?) ToDecompressedNullableByte(bytes, ref index);

        public static byte WriteCompressedNullableByte(BinaryBufferWriter writer, byte? value) => WriteCompressedNullableInt(writer, value);

        public static byte? ToDecompressedNullableByte(this ReadOnlySpan<byte> bytes, ref int index)
        {
            int? value = ToDecompressedNullableInt32(bytes, ref index);
            if (value == null)
                return null;
            if (value > byte.MaxValue)
                throw new FormatException("Format_BadCompressedNullableByte");
            return (byte) (int) value;
        }

        public static byte WriteCompressedNullableBool(BinaryBufferWriter writer, bool? value)
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

        public static byte WriteCompressedTimeSpan(BinaryBufferWriter writer, TimeSpan value) => WriteCompressedLong(writer, value.Ticks);

        public static TimeSpan ToDecompressedTimeSpan(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long value = ToDecompressedInt64(bytes, ref index);
            return new TimeSpan(value);
        }

        public static byte WriteCompressedNullableTimeSpan(BinaryBufferWriter writer, TimeSpan? value) => WriteCompressedNullableLong(writer, value?.Ticks);

        public static TimeSpan? ToDecompressedNullableTimeSpan(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long? value = ToDecompressedNullableInt64(bytes, ref index);
            return value == null ? (TimeSpan?) null : new TimeSpan((long) value);
        }

        public static byte WriteCompressedDateTime(BinaryBufferWriter writer, DateTime value) => WriteCompressedLong(writer, value.Ticks);

        public static DateTime ToDecompressedDateTime(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long value = ToDecompressedInt64(bytes, ref index);
            return new DateTime(value);
        }

        public static byte WriteCompressedNullableDateTime(BinaryBufferWriter writer, DateTime? value) => WriteCompressedNullableLong(writer, value?.Ticks);

        public static DateTime? ToDecompressedNullableDateTime(this ReadOnlySpan<byte> bytes, ref int index)
        {
            long? value = ToDecompressedNullableInt64(bytes, ref index);
            return value == null ? (DateTime?)null : new DateTime((long)value);
        }

        #endregion
    }
}
