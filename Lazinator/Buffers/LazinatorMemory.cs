using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public readonly List<IMemoryOwner<byte>> MoreOwnedMemory;
        public readonly int StartIndex;
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

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int length)
        {
            OwnedMemory = ownedMemory;
            MoreOwnedMemory = null;
            StartIndex = 0;
            if (startPosition < 0)
                throw new ArgumentException();
            StartPosition = startPosition;
            if (length < 0)
                Length = 0;
            else
                Length = length;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, List<IMemoryOwner<byte>> moreOwnedMemory, int startIndex, int startPosition, int length) : this(ownedMemory, startPosition, length)
        {
            MoreOwnedMemory = moreOwnedMemory;
            StartIndex = startIndex;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int length) : this(ownedMemory, 0, length)
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
            // DEBUG -- should we do this only if Index and StartPosition are 0?
            OwnedMemory?.Dispose();
            if (MoreOwnedMemory != null)
                foreach (var additional in MoreOwnedMemory)
                    additional?.Dispose();
        }

        public void LazinatorShouldNotReturnToPool()
        {
            IMemoryOwner<byte> ownedMemory = OwnedMemory;
            Helper(ownedMemory);

            if (MoreOwnedMemory != null)
                foreach (var additional in MoreOwnedMemory)
                {
                    Helper(additional);
                }

            static void Helper(IMemoryOwner<byte> ownedMemory)
            {
                if (ownedMemory is ExpandableBytes e)
                {
                    e.LazinatorShouldNotReturnToPool = true;
                    if (ExpandableBytes.TrackMemoryAllocations)
                    {
                        ExpandableBytes.NotReturnedByLazinatorHashSet.Add(e.AllocationID);
                    }
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

        private IMemoryOwner<byte> MemoryAtIndex(int i) => i == 0 ? OwnedMemory : MoreOwnedMemory[i - 1];

        private LazinatorMemory SliceInitial(int position) => Length - position is int revisedLength ? Slice(position, revisedLength) : LazinatorMemory.EmptyLazinatorMemory;
        private LazinatorMemory SliceInitial(int position, int length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(OwnedMemory, StartPosition + position, length);

        public LazinatorMemory Slice(int position) => Slice(position, Length);

        public LazinatorMemory Slice(int position, int length)
        {

            if (SingleMemory)
            {
                return SliceInitial(position);
            }

            // position is relative to StartPosition within memory chunk index StartIndex. 
            // We use up "position" by advancing StartPosition up to the end of the Length of the starting index.
            // If we go all the way to the end, then we increment the starting index.
            // Note that we never change the Length (which is the Length of all combined).
            int revisedStartIndex = StartIndex;
            int revisedStartPosition = StartPosition;
            while (position > 0)
            {
                IMemoryOwner<byte> current = MemoryAtIndex(revisedStartIndex);
                int remainingBytesThisMemory = current.Memory.Length - revisedStartPosition; // DEBUG -- we might want to have another IMemoryOwner variant IMemoryOwnerWithLength that has the length without accessing the Memory, for use with memory mapped files.
                if (remainingBytesThisMemory <= position)
                {
                    position -= remainingBytesThisMemory;
                    revisedStartIndex++;
                    revisedStartPosition = 0;
                }
                else
                {
                    revisedStartPosition += position;
                }
            }

            return new LazinatorMemory(OwnedMemory, MoreOwnedMemory, revisedStartIndex, revisedStartPosition, Length);
        }

        public override bool Equals(object obj) => obj == null ? throw new LazinatorSerializationException("Invalid comparison of LazinatorMemory to null") :
            obj is LazinatorMemory lm && lm.OwnedMemory.Equals(OwnedMemory) && lm.StartPosition == StartPosition && lm.Length == Length;

        public override int GetHashCode()
        {
            return (int)FarmhashByteSpans.Hash32(Span);
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

        #region Multiple memories management


        public bool SingleMemory => true;
        public Memory<byte> InitialMemory
        {
            get
            {
                if (IsEmpty)
                    return EmptyMemory;
                if (SingleMemory)
                    return OwnedMemory.Memory.Slice(StartPosition, Length);
                else
                    return MemoryAtIndex(StartIndex).Memory.Slice(StartPosition, Length);
            }
        }
        public Span<byte> InitialSpan => InitialMemory.Span; // DEBUG

        public IEnumerable<byte> EnumerateBytes(bool includeBeforeStart)
        {
            int startIndexOrZero = includeBeforeStart ? 0 : StartIndex;
            int totalItems = NumMemoryChunks();
            for (int i = StartIndex; i < totalItems; i++)
            {
                var m = MemoryAtIndex(i);
                var s = m.Memory.Span;
                int startPositionOrZero;
                if (i == StartIndex && !includeBeforeStart)
                    startPositionOrZero = StartPosition;
                else
                    startPositionOrZero = 0;
                for (int j = startPositionOrZero; j <= m.Memory.Length; j++)
                    yield return s[j];
            }
        }

        public int NumMemoryChunks()
        {
            return 1 + (MoreOwnedMemory == null ? 0 : MoreOwnedMemory.Count);
        }

        public int GetLengthFromStartPosition()
        {
            if (SingleMemory)
                return Length - StartPosition;
            int total = MemoryAtIndex(StartIndex).Memory.Length - StartPosition; // DEBUG -- see above
            int numMemoryChunks = NumMemoryChunks();
            for (int i = StartIndex + 1; i < numMemoryChunks; i++)
                total += MemoryAtIndex(i).Memory.Length; // DEBUG again
            return total;
        }

        public Memory<byte> GetConsolidatedMemory(bool includeBeforeStart)
        {
            if (SingleMemory)
            {
                if (includeBeforeStart)
                    return OwnedMemory.Memory;
                else
                    return OwnedMemory.Memory.Slice(StartPosition);
            }

            int netLength = includeBeforeStart ? Length : GetLengthFromStartPosition();
            BinaryBufferWriter w = new BinaryBufferWriter(netLength);
            foreach (byte b in EnumerateBytes(includeBeforeStart))
                w.Write(b);
            return w.UnderlyingMemory.Memory;
        }

        #endregion

    }
}
