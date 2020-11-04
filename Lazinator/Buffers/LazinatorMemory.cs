﻿using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public readonly List<MemoryReference> MoreOwnedMemory;
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
        public readonly long Length;
        /// <summary>
        /// The number of bytes, as an integer, or null if the number is too large to be stored in an integer.
        /// </summary>
        public int? LengthInt => Length > int.MaxValue ? null : (int)Length;

        public bool IsEmpty => InitialOwnedMemory == null || Length == 0;
        public long? AllocationID => (InitialOwnedMemory as ExpandableBytes)?.AllocationID;
        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(new Memory<byte>());

        /// <summary>
        /// The first chunk of the memory. To obtain all of the memory, use GetConsolidatedMemory(). 
        /// </summary>
        public Memory<byte> Memory => InitialMemory;


        public override string ToString()
        {
            return $@"{(AllocationID != null ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", InitialMemory.Span.Slice(0, Math.Min(InitialMemory.Span.Length, 100)).ToArray())}";
        }

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, long length)
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

        public LazinatorMemory(MemoryReference ownedMemory, List<MemoryReference> moreOwnedMemory, int startIndex, int startPosition, long length) : this(ownedMemory, startPosition, length)
        {
            MoreOwnedMemory = moreOwnedMemory;
            StartIndex = startIndex;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, long length) : this(ownedMemory, 0, length)
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

        public bool Disposed => EnumerateReferencedMemoryOwners().Any(x => x != null && (x is ExpandableBytes e && e.Disposed) || (x is SimpleMemoryOwner<byte> s && s.Disposed));

        public void Dispose()
        {
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

        private int LengthAtIndex(int i)
        {
            var memoryAtIndex = MemoryAtIndex(i);
            return GetMemoryOwnerLength(memoryAtIndex);
        }

        private static int GetMemoryOwnerLength(IMemoryOwner<byte> memoryAtIndex)
        {
            if (memoryAtIndex is MemoryReference memoryReference)
                return memoryReference.Length; // saves us from possibility of loading the memory (if using memory-mapped files)
            else
                return memoryAtIndex.Memory.Length;
        }

        private LazinatorMemory SliceInitial(int relativePositionOfSubrange) => Length - relativePositionOfSubrange is long revisedLength and > 0 ? SliceInitial(relativePositionOfSubrange, revisedLength) : LazinatorMemory.EmptyLazinatorMemory;
        private LazinatorMemory SliceInitial(int relativePositionOfSubrange, long length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(InitialOwnedMemory, StartPosition + relativePositionOfSubrange, length);

        public LazinatorMemory Slice(long relativePositionOfSubrange) => Slice(relativePositionOfSubrange, Length - relativePositionOfSubrange);

        public LazinatorMemory Slice(long relativePositionOfSubrange, long length)
        {
            if (Length == 0)
                return EmptyLazinatorMemory;

            if (SingleMemory)
            {
                return SliceInitial((int) relativePositionOfSubrange, length);
            }

            long positionRemaining = relativePositionOfSubrange;
            // position is relative to StartPosition within memory chunk index StartIndex. 
            // We use up "positionRemaining" by advancing StartPosition up to the end of the Length of the starting index.
            // If we go all the way to the end, then we increment the starting index.
            // Note that we never change the Length (which is the Length of all combined).
            int revisedStartIndex = StartIndex;
            int revisedStartPosition = StartPosition;
            while (positionRemaining > 0)
            {
                IMemoryOwner<byte> current = MemoryAtIndex(revisedStartIndex);
                int remainingBytesThisMemory = GetMemoryOwnerLength(current) - revisedStartPosition;
                if (remainingBytesThisMemory <= positionRemaining)
                {
                    positionRemaining -= remainingBytesThisMemory;
                    revisedStartIndex++;
                    revisedStartPosition = 0;
                }
                else
                {
                    revisedStartPosition += (int) positionRemaining;
                    positionRemaining = 0;
                }
            }

            return new LazinatorMemory((MemoryReference)InitialOwnedMemory, MoreOwnedMemory, revisedStartIndex, revisedStartPosition, length);
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
                    return InitialOwnedMemory.Memory.Slice(StartPosition, (int) Length);
                else
                {
                    IMemoryOwner<byte> memoryOwner = MemoryAtIndex(StartIndex);
                    var memory = memoryOwner.Memory;
                    int overallMemoryLength = memory.Length;
                    int lengthOfMemoryChunkAfterStartPosition = overallMemoryLength - StartPosition;
                    return memoryOwner.Memory.Slice(StartPosition, lengthOfMemoryChunkAfterStartPosition);
                }
            }
        }

        public ReadOnlyMemory<byte> InitialReadOnlyMemory => InitialMemory;

        public Memory<byte> OnlyMemory
        {
            get
            {
                if (!SingleMemory)
                    throw new LazinatorCompoundMemoryException();
                return InitialOwnedMemory.Memory.Slice(StartPosition, (int) Length);
            }
        }

        public IMemoryOwner<byte> LoadInitialMemory()
        {
            if (SingleMemory)
                return InitialOwnedMemory;
            IMemoryOwner<byte> memoryOwner = MemoryAtIndex(StartIndex);
            if (memoryOwner is MemoryReference memoryReference && memoryReference.IsLoaded == false)
            {
                // Unfortunately, we must call an async method synchronously. It would be better for the user
                // to use an asynchronous method.
                var loadMemory = memoryReference.LoadMemoryAsync();
                var task = Task.Run(async () => await loadMemory);
                task.Wait();
            }
            return memoryOwner;
        }

        public void ConsiderUnloadInitialMemory()
        {
            if (SingleMemory)
                return;
            IMemoryOwner<byte> memoryOwner = MemoryAtIndex(StartIndex);
            if (memoryOwner is MemoryReference memoryReference && memoryReference.IsLoaded == true)
            {
                // Unfortunately, we must call an async method synchronously. It would be better for the user
                // to use an asynchronous method.
                var loadMemory = memoryReference.ConsiderUnloadMemoryAsync();
                var task = Task.Run(async () => await loadMemory);
                task.Wait();
            }
        }

        public async ValueTask<IMemoryOwner<byte>> LoadInitialMemoryAsync()
        {
            if (SingleMemory)
                return InitialOwnedMemory;
            IMemoryOwner<byte> memoryOwner = MemoryAtIndex(StartIndex);
            if (memoryOwner is MemoryReference memoryReference && memoryReference.IsLoaded == false)
                await memoryReference.LoadMemoryAsync();
            return memoryOwner;
        }

        public async ValueTask ConsiderUnloadInitialMemoryAsync()
        {
            if (SingleMemory)
                return;
            IMemoryOwner<byte> memoryOwner = MemoryAtIndex(StartIndex);
            if (memoryOwner is MemoryReference memoryReference && memoryReference.IsLoaded == true)
                await memoryReference.ConsiderUnloadMemoryAsync();
        }

        public async ValueTask<Memory<byte>> GetInitialMemoryAsync()
        {
            if (IsEmpty)
                return EmptyMemory;
            if (SingleMemory)
                return InitialOwnedMemory.Memory.Slice(StartPosition, (int) Length);
            else
            {
                IMemoryOwner<byte> memoryOwner = await LoadInitialMemoryAsync();
                var memory = memoryOwner.Memory;
                int overallMemoryLength = memory.Length;
                int lengthOfMemoryChunkAfterStartPosition = overallMemoryLength - StartPosition;
                return memoryOwner.Memory.Slice(StartPosition, lengthOfMemoryChunkAfterStartPosition);
            }
        }

        public async ValueTask<ReadOnlyMemory<byte>> GetInitialReadOnlyMemoryAsync() => await GetInitialMemoryAsync();

        public IEnumerable<(int chunkIndex, int startPosition, int numBytes)> EnumerateMemoryChunkRanges(bool includeOutsideOfRange = false)
        {
            if (!includeOutsideOfRange && Length == 0)
                yield break;
            int startIndexOrZero = includeOutsideOfRange ? 0 : StartIndex;
            int totalItems = NumMemoryChunks();
            long lengthRemaining = Length;
            for (int i = StartIndex; i < totalItems; i++)
            {
                var m = MemoryAtIndex(i);
                int startPositionOrZero;
                if (i == StartIndex && !includeOutsideOfRange)
                    startPositionOrZero = StartPosition;
                else
                    startPositionOrZero = 0;
                int numBytes = GetMemoryOwnerLength(m) - startPositionOrZero;
                if (numBytes > lengthRemaining)
                    numBytes = (int) lengthRemaining;
                yield return (i, startPositionOrZero, numBytes);
                lengthRemaining -= numBytes;
                if (lengthRemaining == 0)
                    yield break;
            }
        }

        public IEnumerable<(int chunkIndex, int startPosition, int numBytes)> EnumerateMemoryChunkSubranges(int relativeStartPositionOfSubrange, int numBytesInSubrange)
        {
            int bytesBeforeSubrangeRemaining = relativeStartPositionOfSubrange;
            int bytesOfSubrangeRemaining = numBytesInSubrange;
            bool withinSubrange = false;
            foreach (var rangeInfo in EnumerateMemoryChunkRanges(false))
            {
                int skipOverBytes = 0;
                if (!withinSubrange)
                {
                    skipOverBytes = Math.Min(bytesBeforeSubrangeRemaining, rangeInfo.numBytes);
                    bytesBeforeSubrangeRemaining -= skipOverBytes;
                    if (bytesBeforeSubrangeRemaining == 0)
                        withinSubrange = true;
                }
                if (withinSubrange)
                {
                    int numBytesToInclude = Math.Min(bytesOfSubrangeRemaining, rangeInfo.numBytes - skipOverBytes);
                    yield return (rangeInfo.chunkIndex, rangeInfo.startPosition + skipOverBytes, numBytesToInclude);
                    bytesOfSubrangeRemaining -= numBytesToInclude;
                    if (bytesOfSubrangeRemaining == 0)
                        yield break;
                }
            }
        }

        // Explanation of how delta serialization works:
        // Assume we have a large LazinatorMemory consisting of big ranges of bytes.
        // Then assume we have a cobbled-together LazinatorMemory with references to these big ranges. This is effectively the result of potentially multiple generations of delta serialization.
        // Now, changes are made to this object graph, and the goal is to do another delta serialization. 
        // BinaryBufferWriter will be compiling a list of the ranges we are writing to in the large LazinatorMemory.
        // When an object has changed, then BinaryBufferWriter writes to a new big range of bytes and can directly record a BytesSegment. 
        // When an object has not changed, then BinaryBufferWriter has a reference to a range of bytes in the cobbled-together LazinatorMemory. 
        // So it needs to translate this range to get a reference to the large LazinatorMemory as a BytesSegment. It does this by calling EnumerateSubrangeAsSegments.
        // The result is that we have a list of BytesSegments. We can save this list of BytesSegments at the end of the latest range of data that we have just written to (plus an indication of the size of this list).
        // That way, we can see what versions we need to have in memory (or lazy loadable) and we can create the large LazinatorMemory, then the next-generation cobbled-together LazinatorMemory.

        public IEnumerable<BytesSegment> EnumerateSubrangeAsSegments(int relativeStartPositionOfSubrange, int numBytesInSubrange)
        {
            foreach ((int chunkIndex, int startPosition, int numBytes) in EnumerateMemoryChunkSubranges(relativeStartPositionOfSubrange, numBytesInSubrange))
            {
                var memoryOwner = MemoryAtIndex(chunkIndex);
                if (memoryOwner is MemoryReference memoryReference)
                {
                    yield return new BytesSegment(memoryReference.VersionOfReferencedMemory, memoryReference.StartIndex + startPosition, numBytes);
                }
                else
                    throw new ArgumentException();
            }
        }

        public Memory<byte> GetMemoryAtBytesSegment(BytesSegment bytesSegment)
        {
            var m = (MemoryReference)MemoryAtIndex(bytesSegment.MemoryChunkVersion);
            var underlyingChunk = m.ReferencedMemory.Memory.Slice(bytesSegment.IndexWithinMemoryChunk, bytesSegment.NumBytes);
            return underlyingChunk;
        }

        public IEnumerable<Memory<byte>> EnumerateMemoryChunks(bool includeOutsideOfRange = false)
        {
            foreach (var rangeInfo in EnumerateMemoryChunkRanges(includeOutsideOfRange))
            {
                var m = MemoryAtIndex(rangeInfo.chunkIndex);
                yield return m.Memory.Slice(rangeInfo.startPosition, rangeInfo.numBytes);
            }
        }

        public async IAsyncEnumerable<Memory<byte>> EnumerateMemoryChunksAsync(bool includeOutsideOfRange = false)
        {
            MemoryReference lastMemoryReferenceLoaded = null;
            foreach (var rangeInfo in EnumerateMemoryChunkRanges(includeOutsideOfRange))
            {
                var m = MemoryAtIndex(rangeInfo.chunkIndex);
                if (m is MemoryReference memoryReference && memoryReference.IsLoaded == false)
                {
                    if (lastMemoryReferenceLoaded != memoryReference)
                    { // consider unloading the last memory reference, before loading this one, so that we don't have too many in memory at the same time
                        await lastMemoryReferenceLoaded.ConsiderUnloadMemoryAsync();
                        lastMemoryReferenceLoaded = memoryReference;
                    }
                    await memoryReference.LoadMemoryAsync();
                }
                yield return m.Memory.Slice(rangeInfo.startPosition, rangeInfo.numBytes);
            }
        }


        public IEnumerable<IMemoryOwner<byte>> EnumerateMemoryOwners()
        {
            if (InitialOwnedMemory != null)
                yield return InitialOwnedMemory;
            if (MoreOwnedMemory != null)
                foreach (var additional in MoreOwnedMemory)
                    yield return additional;
        }

        public IEnumerable<IMemoryOwner<byte>> EnumerateReferencedMemoryOwners()
        {
            foreach (var owner in EnumerateMemoryOwners())
                if (owner is MemoryReference memoryReference)
                    yield return memoryReference.ReferencedMemory;
                else
                    yield return owner;
        }

        public IEnumerable<MemoryReference> EnumerateMemoryReferences()
        {
            if (InitialOwnedMemory != null)
                yield return InitialOwnedMemoryReference;
            if (MoreOwnedMemory != null)
                foreach (var additional in MoreOwnedMemory)
                    yield return additional;
        }

        public async ValueTask WriteToBinaryBufferAsync(BinaryBufferWriterContainer writer, bool includeOutsideOfRange = false)
        {
            await foreach (Memory<byte> memory in EnumerateMemoryChunksAsync(includeOutsideOfRange))
                writer.Write(memory.Span); // DEBUG -- after each write, we should check whether this is getting too large, and if so, move to the next chunk
        }


        public async ValueTask WriteToBinaryBuffer_WithBytePrefixAsync(BinaryBufferWriterContainer writer, bool includeOutsideOfRange = false)
        {
            if (Length > byte.MaxValue)
                ThrowHelper.ThrowTooLargeException(byte.MaxValue);
            writer.Write((byte)Length);
            await WriteToBinaryBufferAsync(writer, includeOutsideOfRange);
        }
        public async ValueTask WriteToBinaryBuffer_WithInt16PrefixAsync(BinaryBufferWriterContainer writer, bool includeOutsideOfRange = false)
        {
            if (Length > Int16.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int16.MaxValue);
            writer.Write((Int16)Length);
            await WriteToBinaryBufferAsync(writer, includeOutsideOfRange);
        }
        public async ValueTask WriteToBinaryBuffer_WithInt32PrefixAsync(BinaryBufferWriterContainer writer, bool includeOutsideOfRange = false)
        {
            if (Length > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            writer.Write((int)Length);
            await WriteToBinaryBufferAsync(writer, includeOutsideOfRange);
        }
        public async ValueTask WriteToBinaryBuffer_WithInt64PrefixAsync(BinaryBufferWriterContainer writer, bool includeOutsideOfRange = false)
        {
            writer.Write((Int64)Length);
            await WriteToBinaryBufferAsync(writer, includeOutsideOfRange);
        }

        public void WriteToBinaryBuffer(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            foreach (Memory<byte> memory in EnumerateMemoryChunks(includeOutsideOfRange))
                writer.Write(memory.Span);
        }
        public void WriteToBinaryBuffer_WithBytePrefix(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            if (Length > byte.MaxValue)
                ThrowHelper.ThrowTooLargeException(byte.MaxValue);
            writer.Write((byte)Length);
            WriteToBinaryBuffer(ref writer, includeOutsideOfRange);
        }
        public void WriteToBinaryBuffer_WithInt16Prefix(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            if (Length > Int16.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int16.MaxValue);
            writer.Write((Int16)Length);
            WriteToBinaryBuffer(ref writer, includeOutsideOfRange);
        }
        public void WriteToBinaryBuffer_WithInt32Prefix(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            if (Length > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            writer.Write((int)Length);
            WriteToBinaryBuffer(ref writer, includeOutsideOfRange);
        }
        public void WriteToBinaryBuffer_WithInt64Prefix(ref BinaryBufferWriter writer, bool includeOutsideOfRange = false)
        {
            writer.Write((Int64)Length);
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
                int memoryOwnerLength = GetMemoryOwnerLength(m);
                for (int j = startPositionOrZero; j < memoryOwnerLength; j++)
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
                total += LengthAtIndex(i); 
            return total;
        }

        public Memory<byte> GetConsolidatedMemory(bool includeOutsideOfRange = false)
        {
            if (SingleMemory)
            {
                if (includeOutsideOfRange)
                    return InitialOwnedMemory.Memory;
                else
                    return InitialOwnedMemory.Memory.Slice(StartPosition, (int) Length);
            }

            if (Length > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            int totalLength = includeOutsideOfRange ? GetGrossLength() : (int) Length;
            BinaryBufferWriter w = new BinaryBufferWriter(totalLength);
            foreach (byte b in EnumerateBytes(includeOutsideOfRange))
                w.Write(b);
            return w.LazinatorMemory.InitialMemory;
        }

        public MemoryReference InitialOwnedMemoryReference
        {
            get
            {
                MemoryReference initialOwnedMemoryReference = InitialOwnedMemory as MemoryReference;
                if (initialOwnedMemoryReference == null)
                    initialOwnedMemoryReference = new MemoryReference(InitialOwnedMemory, 0, 0, InitialOwnedMemory.Memory.Length);
                return initialOwnedMemoryReference;
            }
        }

        public LazinatorMemory WithAppendedChunk(MemoryReference chunk)
        {
            var evenMoreOwnedMemory = MoreOwnedMemory?.ToList() ?? new List<MemoryReference>();

            evenMoreOwnedMemory.Add(chunk);
            if (StartIndex == 0 && StartPosition == 0 && Length == GetGrossLength())
                return new LazinatorMemory(InitialOwnedMemoryReference, evenMoreOwnedMemory, 0, 0, Length + chunk.Length);
            return new LazinatorMemory(InitialOwnedMemoryReference, evenMoreOwnedMemory, StartIndex, StartPosition, Length);
        }

        #endregion

    }
}
