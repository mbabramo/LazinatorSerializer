using Lazinator.Exceptions;
using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Read and write to a buffer spans, including support for casting spans to be spans of various integral types.
    /// </summary>
    public static class Spans
    {
        public static ReadOnlySpan<Int16> CastSpanToInt16(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, Int16>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            Int16[] reversed = new Int16[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<Int16>(reversed).Span;
        }
        public static ReadOnlySpan<Int32> CastSpanToInt32(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, Int32>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            Int32[] reversed = new Int32[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<Int32>(reversed).Span;
        }
        public static ReadOnlySpan<Int64> CastSpanToInt64(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, Int64>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            Int64[] reversed = new Int64[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<Int64>(reversed).Span;
        }
        public static ReadOnlySpan<DateTime> CastSpanToDateTime(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, DateTime>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            DateTime[] reversed = new DateTime[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new DateTime(BinaryPrimitives.ReverseEndianness(cast[i].Ticks));
            return new Memory<DateTime>(reversed).Span;
        }
        public static ReadOnlySpan<TimeSpan> CastSpanToTimeSpan(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, TimeSpan>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            TimeSpan[] reversed = new TimeSpan[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new TimeSpan(BinaryPrimitives.ReverseEndianness(cast[i].Ticks));
            return new Memory<TimeSpan>(reversed).Span;
        }
        public static ReadOnlySpan<UInt16> CastSpanToUInt16(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, UInt16>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            UInt16[] reversed = new UInt16[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<UInt16>(reversed).Span;
        }
        public static ReadOnlySpan<UInt32> CastSpanToUInt32(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, UInt32>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            UInt32[] reversed = new UInt32[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<UInt32>(reversed).Span;
        }
        public static ReadOnlySpan<UInt64> CastSpanToUInt64(ReadOnlySpan<byte> s)
        {
            var cast = MemoryMarshal.Cast<byte, UInt64>(s);
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return cast;
            UInt64[] reversed = new UInt64[cast.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(cast[i]);
            return new Memory<UInt64>(reversed).Span;
        }

        public static ReadOnlySpan<byte> CastSpanFromInt16(ReadOnlySpan<Int16> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<Int16, byte>(s);
            Int16[] reversed = new Int16[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<Int16, byte>(new Memory<Int16>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromInt32(ReadOnlySpan<Int32> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<Int32, byte>(s);
            Int32[] reversed = new Int32[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<Int32, byte>(new Memory<Int32>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromInt64(ReadOnlySpan<Int64> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<Int64, byte>(s);
            Int64[] reversed = new Int64[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<Int64, byte>(new Memory<Int64>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromDateTime(ReadOnlySpan<DateTime> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<DateTime, byte>(s);
            DateTime[] reversed = new DateTime[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new DateTime(BinaryPrimitives.ReverseEndianness(s[i].Ticks));
            return MemoryMarshal.Cast<DateTime, byte>(new Memory<DateTime>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromTimeSpan(ReadOnlySpan<TimeSpan> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<TimeSpan, byte>(s);
            TimeSpan[] reversed = new TimeSpan[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = new TimeSpan(BinaryPrimitives.ReverseEndianness(s[i].Ticks));
            return MemoryMarshal.Cast<TimeSpan, byte>(new Memory<TimeSpan>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromUInt16(ReadOnlySpan<UInt16> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<UInt16, byte>(s);
            UInt16[] reversed = new UInt16[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<UInt16, byte>(new Memory<UInt16>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromUInt32(ReadOnlySpan<UInt32> s)
        {
            if (BinaryBufferWriter.LittleEndianStorage == BitConverter.IsLittleEndian)
                return MemoryMarshal.Cast<UInt32, byte>(s);
            UInt32[] reversed = new UInt32[s.Length];
            for (int i = 0; i < reversed.Length; i++)
                reversed[i] = BinaryPrimitives.ReverseEndianness(s[i]);
            return MemoryMarshal.Cast<UInt32, byte>(new Memory<UInt32>(reversed).Span);
        }
        public static ReadOnlySpan<byte> CastSpanFromUInt64(ReadOnlySpan<UInt64> s)
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
            writer.Write(b.Length);
            writer.Write(b);
        }

        public static void Write_WithByteLengthPrefix(this ReadOnlySpan<byte> b, ref BinaryBufferWriter writer)
        {
            if (b.Length > byte.MaxValue)
                ThrowHelper.ThrowTooLargeException(byte.MaxValue);
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
