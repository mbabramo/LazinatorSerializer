using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    public readonly struct LazinatorMemory : IMemoryOwner<byte>
    {
        public readonly IMemoryOwner<byte> OwnedMemory;
        public readonly int StartPosition;
        public readonly int Length;
        public bool IsEmpty => OwnedMemory == null || Length == 0;
        public long? AllocationID => (OwnedMemory as ExpandableBytes)?.AllocationID;
        public Memory<byte> Memory => OwnedMemory.Memory.Slice(StartPosition, Length);
        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public Span<byte> Span => Memory.Span;
        public ReadOnlySpan<byte> ReadOnlySpan => Memory.Span;

        public override string ToString()
        {
            return $@"{(AllocationID != null ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", Span.Slice(0, Math.Min(Span.Length, 100)).ToArray())}";
        }

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            StartPosition = startPosition;
            Length = bytesFilled;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            StartPosition = 0;
            Length = bytesFilled;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory)
        {
            OwnedMemory = ownedMemory;
            StartPosition = 0;
            Length = ownedMemory.Memory.Length;
        }

        public LazinatorMemory(Memory<byte> memory) : this(new SimpleMemoryOwner<byte>(memory), memory.Length)
        {
        }

        public LazinatorMemory(byte[] array) : this(new SimpleMemoryOwner<byte>(new Memory<byte>(array)), array.Length)
        {
        }

        public bool Disposed => OwnedMemory != null && OwnedMemory is ExpandableBytes e && e.Disposed;

        public void Dispose()
        {
            OwnedMemory?.Dispose();
        }

        public void LazinatorShouldNotReturnToPool()
        {
            if (OwnedMemory is ExpandableBytes e)
            {
                e.LazinatorShouldNotReturnToPool = true;
                if (ExpandableBytes.TrackMemoryAllocations)
                {
                    ExpandableBytes.NotReturnedByLazinatorHashSet.Add(e.AllocationID);
                }
            }
        }

        #endregion

        #region Conversions and slicing

        public static implicit operator LazinatorMemory(Memory<byte> memory)
        {
            return new LazinatorMemory(memory);
        }

        public static implicit operator LazinatorMemory(byte[] array)
        {
            return new LazinatorMemory(array);
        }

        public LazinatorMemory Slice(int position) => Slice(position, Length - position);
        public LazinatorMemory Slice(int position, int length) => new LazinatorMemory(OwnedMemory, StartPosition + position, length);

        public override bool Equals(object obj) => obj is LazinatorMemory lm && lm.OwnedMemory.Equals(OwnedMemory) &&
                                                   lm.StartPosition == StartPosition && lm.Length == Length;

        public static bool operator ==(LazinatorMemory x, LazinatorMemory y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(LazinatorMemory x, LazinatorMemory y)
        {
            return !(x == y);
        }

        #endregion

    }
}
