using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lazinator.Exceptions;
using Lazinator.Support;

namespace Lazinator.Buffers
{
    /// <summary>
    /// An immutable memory owner used by Lazinator to store unserialized Lazinator properties. 
    /// </summary>
    public readonly struct LazinatorMemory : IMemoryOwner<byte>
    {
        /// <summary>
        /// The first chunk of owned memory (and in ordinary usage, the only chunk).
        /// </summary>
        public readonly IMemoryOwner<byte> InitialOwnedMemory;
        /// <summary>
        /// Additional chunks of owned memory, where the memory storage is split across chunks.
        /// </summary>
        public readonly List<IMemoryOwner<byte>> MoreOwnedMemory;
        /// <summary>
        /// The starting index from the set consisting of InitialOwnedMemory and MoreOwnedMemory for the referenced range.
        /// </summary>
        public readonly int StartIndex;
        /// <summary>
        /// The starting position within the chunk of memory referred to by StartIndex of the referenced range.
        /// </summary>
        public readonly int StartPosition;
        /// <summary>
        /// The total number of bytes in the referenced range, potentially spanning multiple chunks of memory.
        /// </summary>
        public readonly int Length;
        public bool IsEmpty => InitialOwnedMemory == null || Length == 0;
        public long? AllocationID => (InitialOwnedMemory as ExpandableBytes)?.AllocationID;
        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(new Memory<byte>());

        // DEBUG -- should eliminate access to these

        /// <summary>
        /// The first chunk of the memory. To obtain all of the memory, use GetConsolidatedMemory(). 
        /// </summary>
        public Memory<byte> Memory => InitialMemory; 
        public ReadOnlySpan<byte> ReadOnlySpan => Memory.Span;


        public override string ToString()
        {
            return $@"{(AllocationID != null ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", InitialMemory.Span.Slice(0, Math.Min(InitialMemory.Span.Length, 100)).ToArray())}";
        }

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int length)
        {
            InitialOwnedMemory = ownedMemory;
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

        public bool Disposed => InitialOwnedMemory != null && (InitialOwnedMemory is ExpandableBytes e && e.Disposed) || (InitialOwnedMemory is SimpleMemoryOwner<byte> s && s.Disposed);

        public void Dispose()
        {
            // DEBUG -- should we do this only if Index and StartPosition are 0?
            InitialOwnedMemory?.Dispose();
            if (MoreOwnedMemory != null)
                foreach (var additional in MoreOwnedMemory)
                    additional?.Dispose();
        }

        public void LazinatorShouldNotReturnToPool()
        {
            IMemoryOwner<byte> ownedMemory = InitialOwnedMemory;
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

        private IMemoryOwner<byte> MemoryAtIndex(int i) => i == 0 ? InitialOwnedMemory : MoreOwnedMemory[i - 1];

        private LazinatorMemory SliceInitial(int position) => Length - position is int revisedLength and > 0 ? SliceInitial(position, revisedLength) : LazinatorMemory.EmptyLazinatorMemory;
        private LazinatorMemory SliceInitial(int position, int length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(InitialOwnedMemory, StartPosition + position, length);

        public LazinatorMemory Slice(int position) => Slice(position, Length - position);

        public LazinatorMemory Slice(int position, int length)
        {
            if (Length == 0)
                return EmptyLazinatorMemory;

            if (SingleMemory)
            {
                return SliceInitial(position, length);
            }

            int positionRemaining = position;
            // position is relative to StartPosition within memory chunk index StartIndex. 
            // We use up "positionRemaining" by advancing StartPosition up to the end of the Length of the starting index.
            // If we go all the way to the end, then we increment the starting index.
            // Note that we never change the Length (which is the Length of all combined).
            int revisedStartIndex = StartIndex;
            int revisedStartPosition = StartPosition;
            while (positionRemaining > 0)
            {
                IMemoryOwner<byte> current = MemoryAtIndex(revisedStartIndex);
                int remainingBytesThisMemory = current.Memory.Length - revisedStartPosition; // DEBUG -- we might want to have another IMemoryOwner variant IMemoryOwnerWithLength that has the length without accessing the Memory, for use with memory mapped files.
                if (remainingBytesThisMemory <= positionRemaining)
                {
                    positionRemaining -= remainingBytesThisMemory;
                    revisedStartIndex++;
                    revisedStartPosition = 0;
                }
                else
                {
                    revisedStartPosition += positionRemaining;
                    positionRemaining = 0;
                }
            }

            return new LazinatorMemory(InitialOwnedMemory, MoreOwnedMemory, revisedStartIndex, revisedStartPosition, length);
        }

        public override bool Equals(object obj) => obj == null ? throw new LazinatorSerializationException("Invalid comparison of LazinatorMemory to null") :
            obj is LazinatorMemory lm && lm.InitialOwnedMemory.Equals(InitialOwnedMemory) && lm.StartPosition == StartPosition && lm.Length == Length;

        public override int GetHashCode()
        {
            return (int)FarmhashByteSpans.Hash32(OnlyMemory.Span);
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


        public bool SingleMemory => MoreOwnedMemory == null || MoreOwnedMemory.Count() == 0;
        public Memory<byte> InitialMemory
        {
            get
            {
                if (IsEmpty)
                    return EmptyMemory;
                if (SingleMemory)
                    return InitialOwnedMemory.Memory.Slice(StartPosition, Length);
                else
                {
                    var memory = MemoryAtIndex(StartIndex).Memory;
                    int overallMemoryLength = memory.Length;
                    int lengthOfMemoryChunkAfterStartPosition = overallMemoryLength - StartPosition;
                    return memory.Slice(StartPosition, lengthOfMemoryChunkAfterStartPosition);
                }
            }
        }

        public Span<byte> InitialSpan => InitialMemory.Span;



        public Memory<byte> OnlyMemory
        {
            get 
            {
                var initialMemory = InitialMemory;
                if (initialMemory.Length == Length)
                    return initialMemory;
                throw new LazinatorCompoundMemoryException();
            }
        }

        public IEnumerable<Memory<byte>> EnumerateMemoryChunks(bool includeOutsideOfRange = false)
        {
            if (!includeOutsideOfRange && Length == 0)
                yield break;
            int startIndexOrZero = includeOutsideOfRange ? 0 : StartIndex;
            int totalItems = NumMemoryChunks();
            int lengthRemaining = Length;
            for (int i = StartIndex; i < totalItems; i++)
            {
                var m = MemoryAtIndex(i);
                int startPositionOrZero;
                if (i == StartIndex && !includeOutsideOfRange)
                    startPositionOrZero = StartPosition;
                else
                    startPositionOrZero = 0;
                int numBytes = m.Memory.Length - startPositionOrZero;
                if (numBytes > lengthRemaining)
                    numBytes = lengthRemaining;
                yield return m.Memory.Slice(startPositionOrZero, numBytes);
                lengthRemaining -= numBytes;
                if (lengthRemaining == 0)
                    yield break;
            }
        }

        public void WriteToBinaryBuffer(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            foreach (Memory<byte> memory in EnumerateMemoryChunks(includeOutsideOfRange))
                writer.Write(memory.Span);
        }
        public void WriteToBinaryBuffer_WithBytePrefix(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            if (Length > 250)
                throw new LazinatorSerializationException("Span exceeded length of 250 bytes even though it was guaranteed to be no more than that.");
            writer.Write((byte)Length);
            WriteToBinaryBuffer(ref writer, includeOutsideOfRange);
        }
        public void WriteToBinaryBuffer_WithIntPrefix(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            writer.Write((uint)Length);
            WriteToBinaryBuffer(ref writer, includeOutsideOfRange);
        }

        public IEnumerable<byte> EnumerateBytes(bool includeOutsideOfRange = false)
        {
            if (!includeOutsideOfRange && Length == 0)
                yield break;
            int startIndexOrZero = includeOutsideOfRange ? 0 : StartIndex;
            int totalItems = NumMemoryChunks();
            int numYielded = 0;
            for (int i = StartIndex; i < totalItems; i++)
            {
                var m = MemoryAtIndex(i);
                int startPositionOrZero;
                if (i == StartIndex && !includeOutsideOfRange)
                    startPositionOrZero = StartPosition;
                else
                    startPositionOrZero = 0;
                for (int j = startPositionOrZero; j < m.Memory.Length; j++)
                {
                    yield return m.Memory.Span[j];
                    numYielded++;
                    if (!includeOutsideOfRange && numYielded == Length)
                        yield break;
                }
            }
        }

        public bool Matches(ReadOnlySpan<byte> span)
        {
            int i = 0;
            foreach (byte b in EnumerateBytes(false))
                if (span[i++] != b)
                    return false;
            return true;
        }
        public void CopyToArray(byte[] array)
        {
            int i = 0;
            foreach (byte b in EnumerateBytes(false))
                array[i++] = b;
        }

        public int NumMemoryChunks()
        {
            return 1 + (MoreOwnedMemory == null ? 0 : MoreOwnedMemory.Count);
        }

        public int GetGrossLength()
        {
            int numMemoryChunks = NumMemoryChunks();
            int total = 0;
            for (int i = 0; i < numMemoryChunks; i++)
                total += MemoryAtIndex(i).Memory.Length; // DEBUG -- create IMemoryOwnerWithLength
            return total;
        }

        public Memory<byte> GetConsolidatedMemory(bool includeOutsideOfRange = false)
        {
            if (SingleMemory)
            {
                if (includeOutsideOfRange)
                    return InitialOwnedMemory.Memory;
                else
                    return InitialOwnedMemory.Memory.Slice(StartPosition, Length);
            }

            int totalLength = includeOutsideOfRange ? GetGrossLength() : Length;
            BinaryBufferWriter w = new BinaryBufferWriter(totalLength);
            foreach (byte b in EnumerateBytes(includeOutsideOfRange))
                w.Write(b);
            return w.LazinatorMemory.InitialMemory;
        }

        public LazinatorMemory WithAppendedChunk(IMemoryOwner<byte> chunk)
        {
            var evenMoreOwnedMemory = MoreOwnedMemory?.ToList() ?? new List<IMemoryOwner<byte>>();
            evenMoreOwnedMemory.Add(chunk);
            if (StartIndex == 0 && StartPosition == 0 && Length == GetGrossLength())
                return new LazinatorMemory(InitialOwnedMemory, evenMoreOwnedMemory, 0, 0, Length + chunk.Memory.Length);
            return new LazinatorMemory(InitialOwnedMemory, evenMoreOwnedMemory, StartIndex, StartPosition, Length);
        }

        #endregion

    }
}
