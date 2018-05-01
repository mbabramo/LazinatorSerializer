using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
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

        public Span<byte> GetSpanToReadOrWrite()
        {
            MakeWritable();
            return ReadOrWrite.Span;
        }

        public void SetReadOnlySpan(ReadOnlySpan<byte> span)
        {
            ReadOnlyMode = true;
            ReadOnly = span;
            ReadOrWrite = new Memory<byte>();
        }

        public void SetMemory(Memory<byte> memory)
        {
            ReadOnlyMode = false;
            ReadOrWrite = memory;
            ReadOnly = new ReadOnlySpan<byte>();
        }

        public bool GetIsReadOnlyMode() => ReadOnlyMode;

        public void PreSerialization()
        {
            if (!ReadOnlyMode)
            {
                // Convert to read only mode. This is a bit inefficient, because we're allocating memory before serialization. A better implementation might override the code behind so that we write as if we were in read only mode without actually allocating memory. In any event, this improves efficiency for reading the bitarray post-deserialization if no writing is needed.
                byte[] underlyingStorage = new byte[ReadOrWrite.Length];
                ReadOrWrite.CopyTo(underlyingStorage);
                ReadOnly = new ReadOnlySpan<byte>(underlyingStorage);
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
