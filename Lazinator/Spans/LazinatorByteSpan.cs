using Lazinator.Attributes;
using System;
using System.Runtime.InteropServices;

namespace Lazinator.Spans
{
    [Implements(new string[] { "PreSerialization", "PostDeserialization" })]
    public partial class LazinatorByteSpan : ILazinatorByteSpan
    {
        public LazinatorByteSpan()
        {
        }

        public LazinatorByteSpan(byte[] bytes)
        {
            ReadOrWrite = new Memory<byte>(bytes);
            ReadOnlyMode = false;
        }

        public LazinatorByteSpan(Memory<byte> bytes)
        {
            ReadOrWrite = bytes;
            ReadOnlyMode = false;
        }

        public ReadOnlySpan<byte> GetSpanToReadOnly()
        {
            if (ReadOnlyMode)
                return ReadOnly;
            return ReadOrWrite.Span;
        }

        public ReadOnlySpan<T> GetSpanToReadOnly<T>() where T : struct
        {
            return MemoryMarshal.Cast<byte, T>(GetSpanToReadOnly());
        }

        public Span<byte> GetSpanToReadOrWrite()
        {
            MakeWritable();
            return ReadOrWrite.Span;
        }

        public Span<T> GetSpanToReadOrWrite<T>() where T : struct
        {
            return MemoryMarshal.Cast<byte, T>(GetSpanToReadOrWrite());
        }

        public void SetReadOnlySpan(ReadOnlySpan<byte> span)
        {
            ReadOnlyMode = true;
            ReadOnly = span;
            ReadOrWrite = new Memory<byte>();
        }

        public void SetReadOnlySpan<T>(ReadOnlySpan<T> span) where T : struct
        {
            SetReadOnlySpan(MemoryMarshal.Cast<T, byte>(span));
        }

        public void SetMemory(Memory<byte> memory)
        {
            ReadOnlyMode = false;
            ReadOrWrite = memory;
            ReadOnly = new ReadOnlySpan<byte>();
        }

        // No SetMemory<T> is available, because the underlying data structure cannot be something like T[]. See https://github.com/dotnet/corefx/issues/24293.

        public bool GetIsReadOnlyMode() => ReadOnlyMode;

        public void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            if (!ReadOnlyMode)
            {
                // Convert to read only mode. This is a bit inefficient, because we're allocating memory before serialization. A better implementation might override the code behind so that we write as if we were in read only mode without actually allocating memory. In any event, this improves efficiency for reading the bitarray post-deserialization if no writing is needed.
                ReadOnly = MemoryMarshal.CreateReadOnlySpan<byte>(ref ReadOrWrite.Span[0], ReadOrWrite.Length); // the copying occurs within the ReadOnly setter
                ReadOnlyMode = false;
            }
        }

        public void PostDeserialization()
        {
            ReadOnlyMode = true; 
        }

        private bool ReadOnlyMode;

        private void MakeWritable()
        {
            if (!ReadOnlyMode)
                return;
            byte[] underlyingStorage = new byte[ReadOnly.Length];
            ReadOnly.CopyTo(underlyingStorage);
            ReadOnly = new ReadOnlySpan<byte>(); // clear memory
            ReadOrWrite = new Memory<byte>(underlyingStorage);
            ReadOnlyMode = false;
        }
    }
}
