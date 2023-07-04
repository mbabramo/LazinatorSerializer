using System;
using System.Runtime.CompilerServices;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Lazinator.Buffers
{

    /// <summary>
    /// Write various primitives to a buffer without compression.
    /// </summary>
    public static class WriteUncompressedPrimitives
    {


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteBool(ref BufferWriter writer, bool value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(byte);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteByte(ref BufferWriter writer, byte value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(byte);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteSByte(ref BufferWriter writer, sbyte value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(sbyte);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteShort(ref BufferWriter writer, short value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(short);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteUShort(ref BufferWriter writer, ushort value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(ushort);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteInt(ref BufferWriter writer, int value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(int);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteUInt(ref BufferWriter writer, uint value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(uint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteLong(ref BufferWriter writer, long value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(long);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteULong(ref BufferWriter writer, ulong value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(ulong);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteSingle(ref BufferWriter writer, float value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(float);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteDouble(ref BufferWriter writer, double value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(double);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteDecimal(ref BufferWriter writer, decimal value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(decimal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteDateTime(ref BufferWriter writer, DateTime value)
        {
            writer.Write(value.Ticks);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(long);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteTimeSpan(ref BufferWriter writer, TimeSpan value)
        {
            writer.Write(value.Ticks);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(long);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteGuid(ref BufferWriter writer, Guid value)
        {
            writer.Write(value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableBool(ref BufferWriter writer, bool? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)(sizeof(byte) + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableByte(ref BufferWriter writer, byte? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)(sizeof(byte) + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableSByte(ref BufferWriter writer, byte? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(sbyte) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableShort(ref BufferWriter writer, short? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(short) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableUShort(ref BufferWriter writer, ushort? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(ushort) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableInt(ref BufferWriter writer, int? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(int) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableUInt(ref BufferWriter writer, uint? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(uint) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableLong(ref BufferWriter writer, long? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(long) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableULong(ref BufferWriter writer, ulong? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(ulong) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableSingle(ref BufferWriter writer, float? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(float) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableDouble(ref BufferWriter writer, double? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)sizeof(double) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableDecimal(ref BufferWriter writer, decimal? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write((decimal)value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)17;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableDateTime(ref BufferWriter writer, DateTime? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(value.Value.Ticks);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)17;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableTimeSpan(ref BufferWriter writer, TimeSpan? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write(((TimeSpan)value).Ticks);
            return (byte)17;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteNullableGuid(ref BufferWriter writer, Guid? value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
#if TRACING_DETAILED
                writer.WriteTrace();
#endif
                return 1;
            }
            writer.Write((byte)1);
            writer.Write((Guid)value);
#if TRACING_DETAILED
            writer.WriteTrace();
#endif
            return (byte)17;
        }
    }
}
