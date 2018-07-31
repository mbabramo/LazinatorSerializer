using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    [Serializable]
    public class LazinatorMemory : IMemoryOwner<byte>
    {
        public readonly IMemoryOwner<byte> OwnedMemory;
        public int BytesFilled { get; set; }
        public Memory<byte> Memory => OwnedMemory.Memory.Slice(0, BytesFilled);
        public Span<byte> Span => Memory.Span;
        public ReadOnlySpan<byte> ReadOnlySpan => (ReadOnlySpan<byte>)Memory.Span;
        public int Length => Memory.Length;

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            BytesFilled = bytesFilled;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory)
        {
            OwnedMemory = ownedMemory;
            BytesFilled = ownedMemory.Memory.Length;
        }

        public LazinatorMemory(Memory<byte> memory) : this(new SimpleMemoryOwner<byte>(memory), memory.Length)
        {
        }

        public LazinatorMemory(byte[] array) : this(new SimpleMemoryOwner<byte>(new Memory<byte>(array)), array.Length)
        {
        }

        public LazinatorMemory Slice(int position) => new LazinatorMemory(Memory.Slice(position));
        public LazinatorMemory Slice(int position, int length) => new LazinatorMemory(Memory.Slice(position, length));

        private void FreeMemory()
        {
            OwnedMemory.Dispose();
        }

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public void Dispose()
        {
            FreeMemory();
        }

        public static implicit operator LazinatorMemory(Memory<byte> memory)
        {
            return new LazinatorMemory(memory);
        }

        public static implicit operator LazinatorMemory(byte[] array)
        {
            return new LazinatorMemory(new Memory<byte>(array));
        }
    }
}
