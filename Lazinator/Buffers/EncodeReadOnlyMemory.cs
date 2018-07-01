using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lazinator.Buffers
{
    public static class EncodeReadOnlyMemory
    {
        public static void WriteReadOnlyMemoryWithVarIntPrefix<T>(ref this BinaryBufferWriter writer, ReadOnlyMemory<T> readOnlyMemory) where T : struct
        {
            CompressedIntegralTypes.WriteCompressedNullableInt(ref writer, readOnlyMemory.Length);
            ReadOnlySpan<byte> s = MemoryMarshal.Cast<T, byte>(readOnlyMemory.Span);
            for (int i = 0; i < s.Length; i++)
                writer.Write(s[i]);
        }

        public static void WriteNullableReadOnlyMemoryWithVarIntPrefix<T>(ref this BinaryBufferWriter writer, ReadOnlyMemory<T>? readOnlyMemory) where T : struct
        {
            if (readOnlyMemory == null)
                writer.Write((byte)0);
            else
            {
                writer.Write((byte)1);
                WriteReadOnlyMemoryWithVarIntPrefix(ref writer, readOnlyMemory.Value);
            }
        }

        public static ReadOnlyMemory<T> ToReadOnlyMemory<T>(this ReadOnlySpan<byte> b, ref int index, int length) where T : struct
        {
            ReadOnlySpan<T> itemSpan = MemoryMarshal.Cast<byte, T>(b.Slice(index, length));
            T[] itemStorage = new T[itemSpan.Length];
            for (int i = 0; i < length; i++)
                itemStorage[i] = itemSpan[i];
            ReadOnlyMemory<T> itemMemory = new ReadOnlyMemory<T>(itemStorage);
            index += length;
            return itemMemory;
        }

        public static ReadOnlyMemory<T> ToReadOnlyMemory_VarIntLength<T>(this ReadOnlySpan<byte> b, ref int index) where T : struct
        {
            int? length = b.ToDecompressedNullableInt(ref index);
            if (length == null)
                return null;
            return ToReadOnlyMemory<T>(b, ref index, (int) length);
        }
        
    }
}