using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    [Serializable]
    public readonly struct MemoryInBuffer
    {
        public readonly IMemoryOwner<byte> OwnedMemory;
        public readonly int BytesFilled;
        public Memory<byte> FilledMemory => OwnedMemory.Memory.Slice(0, BytesFilled);

        public MemoryInBuffer(IMemoryOwner<byte> ownedMemory, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            BytesFilled = bytesFilled;
        }

        public MemoryInBuffer WithBytesFilled(int bytesFilled)
        {
            return new MemoryInBuffer(OwnedMemory, bytesFilled);
        }
    }
}
