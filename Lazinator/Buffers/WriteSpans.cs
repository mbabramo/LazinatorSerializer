﻿using System;
using System.IO;

namespace Lazinator.Buffers
{
    public static class WriteSpans
    {

        public static void Write(this ReadOnlySpan<byte> b, BinaryBufferWriter writer)
        {
            for (int i = 0; i < b.Length; i++)
                writer.Write(b[i]);
        }

        public static void Write_WithVarLongLengthPrefix(this ReadOnlySpan<byte> b, BinaryBufferWriter writer)
        {
            CompressedIntegralTypes.WriteCompressedLong(writer, (long)b.Length);
            b.Write(writer);
        }

        public static void Write_WithUintLengthPrefix(this ReadOnlySpan<byte> b, BinaryBufferWriter writer)
        {
            writer.Write((uint)b.Length);
            b.Write(writer);
        }

        public static void Write(this ReadOnlyMemory<byte> m, BinaryBufferWriter writer)
        {
            ReadOnlySpan<byte> toConvert = m.Span;
            for (int i = 0; i < toConvert.Length; i++)
                writer.Write(toConvert[i]);
        }
    }
}
