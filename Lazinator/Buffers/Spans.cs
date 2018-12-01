using Lazinator.Exceptions;
using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Lazinator.Buffers
{
    public static class Spans
    {
        public static ReadOnlySpan<Int16> CastToInt16(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, Int16>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            Int16[] reversed = new Int16[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<Int16>(reversed).Span;
        }
        public static ReadOnlySpan<Int32> CastToInt32(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, Int32>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            Int32[] reversed = new Int32[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<Int32>(reversed).Span;
        }
        public static ReadOnlySpan<Int64> CastToInt64(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, Int64>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            Int64[] reversed = new Int64[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<Int64>(reversed).Span;
        }
        public static ReadOnlySpan<DateTime> CastToDateTime(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, DateTime>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            DateTime[] reversed = new DateTime[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new DateTime(BinaryPrimitives.ReverseEndianness(cast[i].Ticks));
            return new Memory<DateTime>(reversed).Span;
        }
        public static ReadOnlySpan<TimeSpan> CastToTimeSpan(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, TimeSpan>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            TimeSpan[] reversed = new TimeSpan[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new TimeSpan(BinaryPrimitives.ReverseEndianness(cast[i].Ticks));
            return new Memory<TimeSpan>(reversed).Span;
        }
        public static ReadOnlySpan<UInt16> CastToUInt16(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, UInt16>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            UInt16[] reversed = new UInt16[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<UInt16>(reversed).Span;
        }
        public static ReadOnlySpan<UInt32> CastToUInt32(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, UInt32>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            UInt32[] reversed = new UInt32[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<UInt32>(reversed).Span;
        }
        public static ReadOnlySpan<UInt64> CastToUInt64(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, UInt64>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            UInt64[] reversed = new UInt64[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<UInt64>(reversed).Span;
        }

        public static ReadOnlySpan<byte> CastFromInt16(ReadOnlySpan<Int16> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<Int16, byte>(s);
            Int16[] reversed = new Int16[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<Int16, byte>(new Memory<Int16>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromInt32(ReadOnlySpan<Int32> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<Int32, byte>(s);
            Int32[] reversed = new Int32[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<Int32, byte>(new Memory<Int32>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromInt64(ReadOnlySpan<Int64> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<Int64, byte>(s);
            Int64[] reversed = new Int64[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<Int64, byte>(new Memory<Int64>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromDateTime(ReadOnlySpan<DateTime> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<DateTime, byte>(s);
            DateTime[] reversed = new DateTime[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new DateTime(BinaryPrimitives.ReverseEndianness(s[i].Ticks));
            return MemoryMarshal.Cast<DateTime, byte>(new Memory<DateTime>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromTimeSpan(ReadOnlySpan<TimeSpan> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<TimeSpan, byte>(s);
            TimeSpan[] reversed = new TimeSpan[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new TimeSpan(BinaryPrimitives.ReverseEndianness(s[i].Ticks));
            return MemoryMarshal.Cast<TimeSpan, byte>(new Memory<TimeSpan>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromUInt16(ReadOnlySpan<UInt16> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<UInt16, byte>(s);
            UInt16[] reversed = new UInt16[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<UInt16, byte>(new Memory<UInt16>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromUInt32(ReadOnlySpan<UInt32> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<UInt32, byte>(s);
            UInt32[] reversed = new UInt32[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<UInt32, byte>(new Memory<UInt32>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastFromUInt64(ReadOnlySpan<UInt64> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<UInt64, byte>(s);
            UInt64[] reversed = new UInt64[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<UInt64, byte>(new Memory<UInt64>(reversed).Span);
        }

        public static void Write_WithVarLongLengthPrefix(this ReadOnlySpan<byte> b, ref BinaryBufferWriter writer)
        {
            CompressedIntegralTypes.WriteCompressedLong(ref writer, (long)b.Length);
            writer.Write(b);
        }

        public static void Write_WithIntLengthPrefix(this ReadOnlySpan<byte> b, ref BinaryBufferWriter writer)
        {
            writer.Write((uint)b.Length);
            writer.Write(b);
        }

        public static void Write_WithByteLengthPrefix(this ReadOnlySpan<byte> b, ref BinaryBufferWriter writer)
        {
            if (b.Length > 250)
                throw new LazinatorSerializationException("Span exceeded length of 250 bytes even though it was guaranteed to be no more than that.");
            writer.Write((byte)b.Length);
            writer.Write(b);
        }

        public static void Write(this ReadOnlyMemory<byte> m, ref BinaryBufferWriter writer)
        {
            ReadOnlySpan<byte> toConvert = m.Span;
            writer.Write(toConvert);
        }
    }
}
