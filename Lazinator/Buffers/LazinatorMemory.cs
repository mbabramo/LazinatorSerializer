using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    public class LazinatorMemory : IMemoryOwner<byte>
    {
        public readonly IMemoryOwner<byte> OwnedMemory;
        public int StartPosition { get; set; }
        public int BytesFilled { get; private set; }
        public int Length => BytesFilled;
        public Memory<byte> Memory => OwnedMemory.Memory.Slice(StartPosition, BytesFilled);
        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public Span<byte> Span => Memory.Span;
        public ReadOnlySpan<byte> ReadOnlySpan => Memory.Span;

        private HashSet<LazinatorMemory> DisposeTogether = null;

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            StartPosition = startPosition;
            BytesFilled = bytesFilled;
        }

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

        #endregion

        #region Conversions and slicing

        public static implicit operator LazinatorMemory(Memory<byte> memory)
        {
            return new LazinatorMemory(memory);
        }

        public static implicit operator LazinatorMemory(byte[] array)
        {
            return new LazinatorMemory(new Memory<byte>(array));
        }

        public LazinatorMemory Slice(int position) => new LazinatorMemory(OwnedMemory, position, BytesFilled - position);
        public LazinatorMemory Slice(int position, int length) => new LazinatorMemory(OwnedMemory, position, length);

        #endregion

        #region Memory management

        /// <summary>
        /// Remembers an additional buffer that should be disposed when this is disposed. 
        /// </summary>
        /// <param name="additionalBuffer">The buffer to dispose with this buffer.</param>
        public void PlanJointDisposal(LazinatorMemory additionalBuffer)
        {
            if (DisposeTogether == null)
                DisposeTogether = new HashSet<LazinatorMemory>();
            DisposeTogether.Add(additionalBuffer);
        }

        private void FreeMemory()
        {
            OwnedMemory.Dispose();
            if (DisposeTogether != null)
                foreach (LazinatorMemory m in DisposeTogether)
                    m.Dispose();
        }

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public void Dispose()
        {
            FreeMemory();
        }

        #endregion
    }
}
