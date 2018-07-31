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

        public MemoryInBuffer(Memory<byte> memory) : this(new SimpleMemoryOwner<byte>(memory), memory.Length)
        {
        }

        public MemoryInBuffer(byte[] array) : this(new SimpleMemoryOwner<byte>(new Memory<byte>(array)), array.Length)
        {
        }

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient. If MemoryInBuffer is copied, then this should be called on
        /// only one instance of the MemoryInBuffer.
        /// </summary>
        public void Dispose()
        {
            OwnedMemory.Dispose();
        }

        public static implicit operator MemoryInBuffer(Memory<byte> memory)
        {
            return new MemoryInBuffer(memory);
        }

        public static implicit operator MemoryInBuffer(byte[] array)
        {
            return new MemoryInBuffer(new Memory<byte>(array));
        }

        public MemoryInBuffer WithBytesFilled(int bytesFilled)
        {
            return new MemoryInBuffer(OwnedMemory, bytesFilled);
        }
    }
}
