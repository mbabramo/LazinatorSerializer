using Lazinator.Exceptions;
using System;

namespace Lazinator.Buffers
{
    public static class WriteSpans
    {

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
