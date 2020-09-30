using System;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Write various primitives to a buffer without compression.
    /// </summary>
    public static class WriteUncompressedPrimitives
    {
        public static byte WriteBool(ref BinaryBufferWriter writer, bool value)
        {
            writer.Write(value);
            return (byte) sizeof(byte);
        }

        public static byte WriteByte(ref BinaryBufferWriter writer, byte value)
        {
            writer.Write(value);
            return (byte)sizeof(byte);
        }

        public static byte WriteSByte(ref BinaryBufferWriter writer, sbyte value)
        {
            writer.Write(value);
            return (byte)sizeof(sbyte);
        }

        public static byte WriteShort(ref BinaryBufferWriter writer, short value)
        {
            writer.Write(value);
            return (byte)sizeof(short);
        }
        public static byte WriteUshort(ref BinaryBufferWriter writer, ushort value)
        {
            writer.Write(value);
            return (byte)sizeof(ushort);
        }
        public static byte WriteInt(ref BinaryBufferWriter writer, int value)
        {
            writer.Write(value);
            return (byte)sizeof(int);
        }
        public static byte WriteUint(ref BinaryBufferWriter writer, uint value)
        {
            writer.Write(value);
            return (byte)sizeof(uint);
        }
        public static byte WriteLong(ref BinaryBufferWriter writer, long value)
        {
            writer.Write(value);
            return (byte)sizeof(long);
        }
        public static byte WriteUlong(ref BinaryBufferWriter writer, ulong value)
        {
            writer.Write(value);
            return (byte)sizeof(ulong);
        }
        public static byte WriteSingle(ref BinaryBufferWriter writer, float value)
        {
            writer.Write(value);
            return (byte)sizeof(float);
        }
        public static byte WriteDouble(ref BinaryBufferWriter writer, double value)
        {
            writer.Write(value);
            return (byte)sizeof(double);
        }
        public static byte WriteDecimal(ref BinaryBufferWriter writer, decimal value)
        {
            writer.Write(value);
            return (byte)sizeof(decimal);
        }
        public static byte WriteDateTime(ref BinaryBufferWriter writer, DateTime value)
        {
            writer.Write(value.Ticks);
            return (byte)sizeof(long);
        }
        public static byte WriteTimeSpan(ref BinaryBufferWriter writer, TimeSpan value)
        {
            writer.Write(value.Ticks);
            return (byte)sizeof(long);
        }
        public static byte WriteGuid(ref BinaryBufferWriter writer, Guid value)
        {
            writer.Write(value); // TODO: when not using BinaryBufferWriter, we should be able to write directly to a destination span, using available extension methods
            return (byte)16;
        }
        public static byte WriteNullableBool(ref BinaryBufferWriter writer, bool? value)
        {
            if (value == null)
            {
                writer.Write((byte) 0);
                return 1;
            }
            writer.Write((byte) 1);
            writer.Write(value.Value);
            return (byte)(sizeof(byte) + 1);
        }

        public static byte WriteNullableByte(ref BinaryBufferWriter writer, byte? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)(sizeof(byte) + 1);
        }

        public static byte WriteNullableSByte(ref BinaryBufferWriter writer, byte? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(sbyte) + 1;
        }

        public static byte WriteNullableShort(ref BinaryBufferWriter writer, short? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(short) + 1;
        }
        public static byte WriteNullableUshort(ref BinaryBufferWriter writer, ushort? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(ushort) + 1;
        }
        public static byte WriteNullableInt(ref BinaryBufferWriter writer, int? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(int) + 1;
        }
        public static byte WriteNullableUint(ref BinaryBufferWriter writer, uint? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(uint) + 1;
        }
        public static byte WriteNullableLong(ref BinaryBufferWriter writer, long? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(long) + 1;
        }
        public static byte WriteNullableUlong(ref BinaryBufferWriter writer, ulong? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(ulong) + 1;
        }
        public static byte WriteNullableSingle(ref BinaryBufferWriter writer, float? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(float) + 1;
        }
        public static byte WriteNullableDouble(ref BinaryBufferWriter writer, double? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
            return (byte)sizeof(double) + 1;
        }
        public static byte WriteNullableDecimal(ref BinaryBufferWriter writer, decimal? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write((decimal)value);
            return (byte)17;
        }
        public static byte WriteNullableDateTime(ref BinaryBufferWriter writer, DateTime? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value.Ticks);
            return (byte)17;
        }
        public static byte WriteNullableTimeSpan(ref BinaryBufferWriter writer, TimeSpan? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(((TimeSpan)value).Ticks);
            return (byte)17;
        }
        public static byte WriteNullableGuid(ref BinaryBufferWriter writer, Guid? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
                return 1;
            }
            writer.Write((byte)1);
            writer.Write((Guid) value);
            return (byte)17;
        }
    }
}
