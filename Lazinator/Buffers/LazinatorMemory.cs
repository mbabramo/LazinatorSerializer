using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    public class LazinatorMemory : IMemoryOwner<byte>
    {
        public IMemoryOwner<byte> OwnedMemory;
        public int StartPosition { get; set; }
        public int Length { get; private set; }
        public Memory<byte> Memory => OwnedMemory.Memory.Slice(StartPosition, Length);
        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public Span<byte> Span => Memory.Span;
        public ReadOnlySpan<byte> ReadOnlySpan => Memory.Span;
        private LazinatorMemory _OriginalSource;
        public LazinatorMemory OriginalSource
        {
            get => _OriginalSource ?? this;
            set { _OriginalSource = value; }
        }

        private HashSet<LazinatorMemory> DisposeTogether = null;

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int bytesFilled, LazinatorMemory originalSource)
        {
            OwnedMemory = ownedMemory;
            StartPosition = startPosition;
            Length = bytesFilled;
            OriginalSource = originalSource;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            Length = bytesFilled;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory)
        {
            OwnedMemory = ownedMemory;
            Length = ownedMemory.Memory.Length;
        }

        public LazinatorMemory(Memory<byte> memory) : this(new SimpleMemoryOwner<byte>(memory), memory.Length)
        {
        }

        public LazinatorMemory(byte[] array) : this(new SimpleMemoryOwner<byte>(new Memory<byte>(array)), array.Length)
        {
        }

        public void CopyFrom(LazinatorMemory existingMemoryToCopy)
        {
            OwnedMemory = existingMemoryToCopy.OwnedMemory;
            StartPosition = existingMemoryToCopy.StartPosition;
            Length = existingMemoryToCopy.Length;
            OriginalSource = existingMemoryToCopy.OriginalSource;
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

        public LazinatorMemory Slice(int position) => Slice(position, Length - position);
        public LazinatorMemory Slice(int position, int length) => new LazinatorMemory(OwnedMemory, StartPosition + position, length, OriginalSource);

        #endregion

        #region Memory management

        /// <summary>
        /// Remembers an additional buffer that should be disposed when this is disposed. 
        /// </summary>
        /// <param name="additionalBuffer">The buffer to dispose with this buffer.</param>
        public void DisposeWhenOriginalSourceDisposed(LazinatorMemory additionalBuffer)
        {
            if (OriginalSource != this)
                OriginalSource.DisposeWhenOriginalSourceDisposed(additionalBuffer);
            else
            {
                if (DisposeTogether == null)
                    DisposeTogether = new HashSet<LazinatorMemory>();
                DisposeTogether.Add(additionalBuffer);
            }
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
            if (OriginalSource != this)
                OriginalSource.FreeMemory();
            else
                FreeMemory();
        }

        #endregion
    }
}
