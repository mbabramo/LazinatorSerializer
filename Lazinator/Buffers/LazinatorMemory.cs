﻿using System;
using System.Buffers;
using Lazinator.Exceptions;
using Lazinator.Support;

namespace Lazinator.Buffers
{
    /// <summary>
    /// An immutable memory owner used by Lazinator to store unserialized Lazinator properties. 
    /// </summary>
    public readonly struct LazinatorMemory : IMemoryOwner<byte>
    {
        public readonly IMemoryOwner<byte> OwnedMemory;
        public readonly int StartPosition;
        public readonly int Length;
        public bool IsEmpty => OwnedMemory == null || Length == 0;
        public long? AllocationID => (OwnedMemory as ExpandableBytes)?.AllocationID;
        public Memory<byte> Memory => IsEmpty ? EmptyMemory : OwnedMemory.Memory.Slice(StartPosition, Length);
        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public Span<byte> Span => Memory.Span;
        public ReadOnlySpan<byte> ReadOnlySpan => Memory.Span;
        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(new Memory<byte>());

        public override string ToString()
        {
            return $@"{(AllocationID != null ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", Span.Slice(0, Math.Min(Span.Length, 100)).ToArray())}";
        }

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int bytesFilled)
        {
            OwnedMemory = ownedMemory;
            if (startPosition < 0)
                throw new ArgumentException();
            StartPosition = startPosition;
            if (bytesFilled < 0)
                Length = 0;
            else
                Length = bytesFilled;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int bytesFilled) : this(ownedMemory, 0, bytesFilled)
        {
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory) : this(ownedMemory, 0, ownedMemory.Memory.Length)
        {
        }

        public LazinatorMemory(Memory<byte> memory) : this(new SimpleMemoryOwner<byte>(memory), memory.Length)
        {
        }

        public LazinatorMemory(byte[] array) : this(new SimpleMemoryOwner<byte>(new Memory<byte>(array)), array.Length)
        {
        }

        public bool Disposed => OwnedMemory != null && (OwnedMemory is ExpandableBytes e && e.Disposed) || (OwnedMemory is SimpleMemoryOwner<byte> s && s.Disposed);

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

        public LazinatorMemory Slice(int position) => Length - position is int revisedLength ? Slice(position, revisedLength) : LazinatorMemory.EmptyLazinatorMemory;
        public LazinatorMemory Slice(int position, int length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(OwnedMemory, StartPosition + position, length);

        public override bool Equals(object obj) => obj == null ? throw new LazinatorSerializationException("Invalid comparison of LazinatorMemory to null") : 
            obj is LazinatorMemory lm && lm.OwnedMemory.Equals(OwnedMemory) && lm.StartPosition == StartPosition && lm.Length == Length;

        public override int GetHashCode()
        {
            return (int) FarmhashByteSpans.Hash32(Span);
        }

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
