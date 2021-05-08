using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace Lazinator.Buffers
{
    /// <summary>
    /// An immutable memory owner used by Lazinator to store and reference serialized Lazinator data. The memory may be split across multiple memory chunks.
    /// The memory referenced is defined by the index of the first memory chunk, the index of the first byte within that chunk, and the number of 
    /// bytes altogether. 
    /// </summary>
    public readonly struct LazinatorMemory
    {
        /// <summary>
        /// A single chunk of memory
        /// </summary>
        public readonly MemoryChunk SingleMemoryChunk;
        /// <summary>
        /// Multiple chunks of memory
        /// </summary>
        public readonly MemoryChunkCollection MultipleMemoryChunks;
        /// <summary>
        /// The starting index from the set consisting of InitialOwnedMemory and MoreOwnedMemory for the referenced range.
        /// </summary>
        public readonly int StartIndex;
        /// <summary>
        /// The starting position within the chunk of memory referred to by StartIndex of the referenced range.
        /// </summary>
        public readonly int Offset;
        /// <summary>
        /// The total number of bytes in the referenced range, potentially spanning multiple chunks of memory.
        /// </summary>
        public readonly long Length;

        /// <summary>
        /// The number of bytes, as an integer, or null if the number is too large to be stored in an integer.
        /// </summary>
        public int? LengthInt => Length > int.MaxValue ? null : (int)Length;

        public bool IsEmpty => (SingleMemoryChunk == null && MultipleMemoryChunks == null) || Length == 0;
        public long AllocationID => (SingleMemoryChunk.MemoryAsLoaded as ExpandableBytes)?.AllocationID ?? -1;

        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(EmptyMemory);


        public override string ToString()
        {
            return $@"{(AllocationID != -1 ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", EnumerateBytes().Take(2000))}";
        }

        #region Construction

        public LazinatorMemory(MemoryChunk memoryChunk, int startPosition, long length)
        {
            SingleMemoryChunk = memoryChunk;
            MultipleMemoryChunks = null;
            StartIndex = 0;
            if (startPosition < 0)
                throw new ArgumentException();
            Offset = startPosition;
            if (length < 0)
                Length = 0;
            else
                Length = length;
        }

        public LazinatorMemory(MemoryChunkCollection memoryChunkCollection)
        {
            MultipleMemoryChunks = memoryChunkCollection; 
            SingleMemoryChunk = null;
            StartIndex = 0;
            Offset = 0;
            Length = memoryChunkCollection.Length;
        }

        public LazinatorMemory(MemoryChunkCollection moreMemoryChunks, int startIndex, int startPosition, long length) : this(null, startPosition, length)
        {
            MultipleMemoryChunks = moreMemoryChunks;
            StartIndex = startIndex;
            if (StartIndex != 0 || Offset != 0)
                throw new Exception("DEBUG");
        }

        public LazinatorMemory(IEnumerable<MemoryChunk> moreMemoryChunks, int startIndex, int startPosition, long length) : this(null, startPosition, length)
        {
            MultipleMemoryChunks = new MemoryChunkCollection();
            MultipleMemoryChunks.SetChunks(moreMemoryChunks);
            StartIndex = startIndex;
            if (StartIndex != 0 || Offset != 0)
                throw new Exception("DEBUG");
        }

        public LazinatorMemory(MemoryChunk memoryChunk, long length) : this(memoryChunk, 0, length)
        {
        }

        public LazinatorMemory(MemoryChunk memoryChunk) : this(memoryChunk, 0, memoryChunk.ReadOnlyMemory.Length)
        {
        }

        public LazinatorMemory(IReadableBytes readOnlyBytes) : this(new MemoryChunk(readOnlyBytes))
        {
        }

        public LazinatorMemory(ReadOnlyBytes readOnlyBytes) : this(new MemoryChunk(readOnlyBytes))
        {
        }

        public LazinatorMemory(ReadOnlyMemory<byte> memory) : this(new MemoryChunk(new ReadOnlyBytes(memory)))
        {
        }

        public LazinatorMemory(byte[] array) : this(new MemoryChunk(new ReadOnlyBytes(array)))
        {
        }

        /// <summary>
        /// Returns a new LazinatorMemory with an appended memory chunk.
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public LazinatorMemory WithAppendedChunk(MemoryChunk chunk)
        {
            if (IsEmpty)
                return new LazinatorMemory(chunk);

            if (SpansLastChunk)
            {
                var revisedMemoryChunks = MultipleMemoryChunks?.WithAppendedMemoryChunk(chunk) ?? new MemoryChunkCollection(new List<MemoryChunk>() { SingleMemoryChunk.WithPreTruncationLengthIncreasedIfNecessary(chunk), chunk });
                return new LazinatorMemory(revisedMemoryChunks, StartIndex, Offset, Length + chunk.Length);
            }

            // The current LazinatorMemory does not terminate at the end of the last chunk. If we just added a chunk, then
            // the range would include the chunks at the end that we do not reference.

            // We can't just return the existing memory plus the new memory, because the existing memory might include
            // some memory that isn't referenced. If, for example, that memory is at the end of the range, then adding
            // an additional chunk will not allow us to reference that memory. 

            List<MemoryChunk> additionalMemoryChunks = GetMemoryChunksWithinLimits(chunk);
            additionalMemoryChunks.Add(chunk);
            MemoryChunkCollection memoryChunkCollection = new MemoryChunkCollection();
            memoryChunkCollection.SetChunks(additionalMemoryChunks);
            return new LazinatorMemory(memoryChunkCollection, StartIndex, Offset, Length);
        }

        /// <summary>
        /// Returns memory chunks within the region specified by StartIndex, Offset, and Length. This is not affected by any 
        /// recycled references specified in MultipleMemoryChunks.
        /// </summary>
        /// <param name="chunkBeingAdded"></param>
        /// <returns></returns>
        private List<MemoryChunk> GetMemoryChunksWithinLimits(MemoryChunk chunkBeingAdded)
        {
            List<MemoryBlockIndexAndSlice> memoryChunkIndexReferences = EnumerateMemoryBlockIndexAndSlices().ToList();
            var additionalMemoryChunks = new List<MemoryChunk>();
            foreach (var indexReference in memoryChunkIndexReferences)
                additionalMemoryChunks.Add(GetMemoryChunkFromMemorySegmentByIndex(indexReference).WithPreTruncationLengthIncreasedIfNecessary(chunkBeingAdded));
            return additionalMemoryChunks;
        }

        public bool Disposed => EnumerateReadOnlyBytesSegments().Any(x => x != null && (x is IMemoryAllocationInfo info && info.Disposed) || (x is ReadOnlyBytes s && s.Disposed));

        #endregion

        #region Disposal

        public void Dispose()
        {
            SingleMemoryChunk?.Dispose();
            if (MultipleMemoryChunks != null)
                foreach (var additional in MultipleMemoryChunks)
                    additional?.Dispose();
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

        public MemoryChunk MemoryAtIndex(int i) => MultipleMemoryChunks == null && i == 0 ? SingleMemoryChunk : MultipleMemoryChunks.MemoryAtIndex(i);

        /// <summary>
        /// Gets the final length of the specified memory chunk. It avoids loading the memory if possible.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int LengthAtIndex(int i)
        {
            var memoryAtIndex = MemoryAtIndex(i);
            return memoryAtIndex.Length;
        }

        /// <summary>
        /// Slices the first referenced memory chunk only, producing a new LazinatorMemory.
        /// </summary>
        /// <param name="offset">An offset relative to the existing Offset, which must refer to the initial memory</param>
        /// <param name="length"></param>
        /// <returns></returns>
        private LazinatorMemory SliceSingle(int offset, long length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(SingleMemoryChunk, Offset + offset, length);

        /// <summary>
        /// Slices the memory, returning a new LazinatorMemory beginning at the specified index. The returned memory will include all of the same memory
        /// chunks as this LazinatorMemory, but will refer to a subset of the bytes.
        /// </summary>
        /// <param name="offset">The first byte of the sliced memory, relative to the first byte of this LazinatorMemory</param>
        /// <returns></returns>
        public LazinatorMemory Slice(long offset) => Slice(offset, Length - offset);

        /// <summary>
        /// Slices the memory, returning a new LazinatorMemory beginning at the specified index. The returned memory will include all of the same memory
        /// chunks as this LazinatorMemory, but will refer to a subset of the bytes.
        /// </summary>
        /// <param name="offset">The first byte of the sliced memory, relative to the first byte specified by the offset of this LazinatorMemory</param>
        /// <param name="length">The number of bytes to include in the slice</param>
        /// <returns></returns>
        public LazinatorMemory Slice(long offset, long length)
        {
            if (Length == 0)
                return EmptyLazinatorMemory;

            if (SingleMemory)
            {
                return SliceSingle((int) offset, length);
            }

            // relativePositionOfSubrange is relative to the total offset within memory chunk index StartIndex. 
            // We use up "positionRemaining" by advancing StartPosition up to the end of the length of the starting index.
            // If we go all the way to the end, then we increment the starting index.
            // Note that we never change the Length (which is the Length of all combined).
            long positionRemaining = offset;
            int revisedStartIndex = StartIndex;
            int revisedStartPosition = Offset;
            int moreMemoryCount = MultipleMemoryChunks?.NumMemoryChunks ?? 0;
            while (positionRemaining > 0)
            {
                MemoryChunk current = MemoryAtIndex(revisedStartIndex);
                int remainingBytesThisMemory = current.Length - revisedStartPosition;
                if (remainingBytesThisMemory <= positionRemaining)
                {
                    positionRemaining -= remainingBytesThisMemory;
                    if (positionRemaining == 0 && revisedStartIndex == moreMemoryCount - 1)
                        break; // we are at the very end of the last LazinatorMemory
                    revisedStartIndex++;
                    revisedStartPosition = 0;
                }
                else
                {
                    revisedStartPosition += (int) positionRemaining;
                    positionRemaining = 0;
                }
            }

            return new LazinatorMemory(MultipleMemoryChunks.DeepCopy(), revisedStartIndex, revisedStartPosition, length);
        }

        #endregion

        #region Equality

        public override bool Equals(object obj) => obj == null ? throw new LazinatorSerializationException("Invalid comparison of LazinatorMemory to null") :
            obj is LazinatorMemory lm && ( (lm.SingleMemoryChunk != null && lm.SingleMemoryChunk.Equals(SingleMemoryChunk)) || (lm.MultipleMemoryChunks != null && lm.MultipleMemoryChunks.Equals(MultipleMemoryChunks)) ) && lm.Offset == Offset && lm.Length == Length;

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

        #region Single and initial memory

        /// <summary>
        /// True if there is only a single memory chunk.
        /// </summary>
        public bool SingleMemory => MultipleMemoryChunks == null;

        /// <summary>
        /// The first referenced memory chunk
        /// </summary>
        public ReadOnlyMemory<byte> InitialReadOnlyMemory
        {
            get
            {
                if (IsEmpty)
                    return EmptyReadOnlyMemory;
                LoadInitialReadOnlyMemory();
                if (SingleMemory)
                    return SingleMemoryChunk.ReadOnlyMemory.Slice(Offset, (int)Length);
                else
                {
                    MemoryChunk memoryOwner = MemoryAtIndex(StartIndex);
                    var memory = memoryOwner.ReadOnlyMemory;
                    int overallMemoryLength = memory.Length;
                    int lengthOfMemoryChunkAfterStartPosition = overallMemoryLength - Offset;
                    return memoryOwner.ReadOnlyMemory.Slice(Offset, lengthOfMemoryChunkAfterStartPosition);
                }
            }
        }


        /// <summary>
        /// Asynchronously returns the first referenced memory chunk.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<ReadOnlyMemory<byte>> GetInitialReadOnlyMemoryAsync()
        {
            if (IsEmpty)
                return EmptyMemory;
            await LoadInitialReadOnlyMemoryAsync();
            if (SingleMemory)
                return SingleMemoryChunk.ReadOnlyMemory.Slice(Offset, (int)Length);
            else
            {
                MemoryChunk memoryOwner = MemoryAtIndex(StartIndex);
                var memory = memoryOwner.ReadOnlyMemory;
                int overallMemoryLength = memory.Length;
                int lengthOfMemoryChunkAfterStartPosition = overallMemoryLength - Offset;
                return memoryOwner.ReadOnlyMemory.Slice(Offset, lengthOfMemoryChunkAfterStartPosition);
            }
        }

        /// <summary>
        /// A read-only version of the first referenced memory chunk, returned asynchronously.
        /// </summary>
        public ReadOnlyMemory<byte> GetInitialReadOnlyMemory() => InitialReadOnlyMemory;

        /// <summary>
        /// The only memory chunk. This will throw if there are multiple memory chunks.
        /// </summary>
        public ReadOnlyMemory<byte> OnlyMemory
        {
            get
            {
                if (!SingleMemory)
                    throw new LazinatorCompoundMemoryException();
                return SingleMemoryChunk.ReadOnlyMemory.Slice(Offset, (int) Length);
            }
        }

        #endregion

        #region Loading

        /// <summary>
        /// Loads the first referenced memory chunk synchronously if it is not loaded.
        /// </summary>
        /// <returns></returns>
        public void LoadInitialReadOnlyMemory()
        {
            if (SingleMemory)
            {
                LoadMemoryChunk(SingleMemoryChunk);
                return;
            }
            MemoryChunk memoryChunk = MemoryAtIndex(StartIndex);
            LoadMemoryChunk(memoryChunk);
        }

        public async ValueTask LoadInitialReadOnlyMemoryAsync()
        {
            if (SingleMemory)
            {
                await LoadMemoryChunkAsync(SingleMemoryChunk);
                return;
            }
            MemoryChunk memoryChunk = MemoryAtIndex(StartIndex);
            await LoadMemoryChunkAsync(memoryChunk);
        }

        private static void LoadMemoryChunk(MemoryChunk memoryChunk)
        {
            if (memoryChunk.IsLoaded == false)
            {
                memoryChunk.LoadMemory();
            }
        }

        private static async ValueTask LoadMemoryChunkAsync(MemoryChunk memoryChunk)
        {
            if (memoryChunk.IsLoaded == false)
            {
                await memoryChunk.LoadMemoryAsync();
            }
        }

        /// <summary>
        /// Loads all memory.
        /// </summary>
        public void LoadAllMemory()
        {
            LoadInitialReadOnlyMemory();
            if (MultipleMemoryChunks != null)
                foreach (var additional in MultipleMemoryChunks)
                    LoadMemoryChunk(additional);
        }

        public async ValueTask LoadAllMemoryAsync()
        {
            await LoadInitialReadOnlyMemoryAsync();
            if (MultipleMemoryChunks != null)
                foreach (var additional in MultipleMemoryChunks)
                    await LoadMemoryChunkAsync(additional);
        }

        /// <summary>
        /// Allows for unloading the first referenced memory chunk, if it is loaded. The memory can be unloaded only if the owner of the first memory
        /// chunk is a MemoryReference that supports this functionality.
        /// </summary>
        public void ConsiderUnloadInitialReadOnlyMemory()
        {
            if (SingleMemory)
                return;
            MemoryChunk memoryChunk = MemoryAtIndex(StartIndex);
            if (memoryChunk.IsLoaded == true)
            {
                memoryChunk.ConsiderUnloadMemory();
            }
        }

        /// <summary>
        /// Allows for asynchronously unloading the first referenced memory chunk, if it is loaded. The memory can be unloaded only if the owner of the first memory
        /// chunk is a MemoryReference that supports this functionality.
        /// </summary>
        /// <returns></returns>
        public async ValueTask ConsiderUnloadReadOnlyMemoryAsync()
        {
            if (SingleMemory)
                return;
            MemoryChunk memoryChunk = MemoryAtIndex(StartIndex);
            if (memoryChunk.IsLoaded == true)
                await memoryChunk.ConsiderUnloadMemoryAsync();
        }

        #endregion

        #region Multiple memory chunks

        /// <summary>
        /// The number of memory chunks stored. Note that the referenced memory may span only some subset of these chunks.
        /// </summary>
        /// <returns></returns>
        public int NumMemoryChunks()
        {
            if (SingleMemoryChunk != null)
                return 1;
            return (MultipleMemoryChunks == null ? 0 : MultipleMemoryChunks.NumMemoryChunks);
        }

        /// <summary>
        /// Returns the length of all memory chunks, including those outside the range of this LazinatorMemory.
        /// </summary>
        /// <returns></returns>
        public long GetGrossLength()
        {
            int numMemoryChunks = NumMemoryChunks();
            long total = 0;
            for (int i = 0; i < numMemoryChunks; i++)
                total += LengthAtIndex(i);
            return total;
        }

        public bool SpansAllIncludedChunks => StartIndex == 0 && Offset == 0 && Length == GetGrossLength();

        public bool SpansLastChunk => Offset + Length == GetGrossLength();

        private MemoryChunk GetMemoryChunkFromMemorySegmentByIndex(MemoryBlockIndexAndSlice memoryChunkIndexReference) => MemoryAtIndex(memoryChunkIndexReference.MemoryBlockIndex);

        internal int GetNextMemoryBlockID()
        {
            if (IsEmpty)
                return 0;
            int maxMemoryBlockID = SingleMemoryChunk == null ? MultipleMemoryChunks.MaxMemoryBlockID : SingleMemoryChunk.MemoryBlockID; // not always the ID of the last chunk, because patching may reassemble into a different order. We are guaranteed, however, that if we're doing versioning, the most recent memory chunk ID will be included.
            return maxMemoryBlockID + 1;
        }

        /// <summary>
        /// Enumerates memory chunk ranges corresponding to the MemoryChunks in this LazinatorMemory. Note that memory chunks are referred to by index instead of by ID. 
        /// </summary>
        /// <returns>An enumerable where each element consists of the chunk index, the start position, and the number of bytes</returns>
        private IEnumerable<MemoryBlockIndexAndSlice> EnumerateMemoryBlockIndexAndSlices()
        {

            if (MultipleMemoryChunks != null && MultipleMemoryChunks is MemorySegmentCollection segmentCollection && segmentCollection.Recycling)
            {
                var segments = segmentCollection.Segments.ToList();
                foreach (var segment in segments)
                {
                    int index = segmentCollection.GetIndexFromMemoryBlockID(segment.MemoryBlockID);
                    yield return new MemoryBlockIndexAndSlice(index, segment.Offset, segment.Length);
                }
                yield break;
            }

            int startIndexOrZero = StartIndex;
            int totalItems = NumMemoryChunks();
            long lengthRemaining = Length;
            for (int i = startIndexOrZero; i < totalItems; i++)
            {
                var m = MemoryAtIndex(i);
                int startPositionOrZero;
                if (i == StartIndex)
                    startPositionOrZero = Offset;
                else
                    startPositionOrZero = 0;
                int numBytes = m.Length - startPositionOrZero;
                if (numBytes > lengthRemaining)
                    numBytes = (int) lengthRemaining;
                yield return new MemoryBlockIndexAndSlice(i, startPositionOrZero, numBytes);
                lengthRemaining -= numBytes;
                if (lengthRemaining == 0)
                    yield break;
            }
        }

        /// <summary>
        /// Enumerates memory chunk ranges corresponding to this LazinatorMemory.
        /// </summary>
        /// <returns>An enumerable where each element consists of the chunk ID, the start position, and the number of bytes</returns>
        public IEnumerable<MemoryBlockIDAndSlice> EnumerateMemoryBlockIDsAndSlices()
        {
            if (MultipleMemoryChunks != null && MultipleMemoryChunks is MemorySegmentCollection segmentCollection && segmentCollection.Recycling)
            {
                var segments = segmentCollection.Segments.ToList();
                foreach (var segment in segments)
                    yield return segment;
                yield break;
            }
            foreach (MemoryBlockIndexAndSlice indexAndSlice in EnumerateMemoryBlockIndexAndSlices())
            {
                MemoryChunk memoryBlock = MemoryAtIndex(indexAndSlice.MemoryBlockIndex);
                MemoryBlockIDAndSlice result = new MemoryBlockIDAndSlice(memoryBlock.MemoryBlockID, indexAndSlice.Offset, indexAndSlice.Length);
                yield return result;
            }
        }

        /// <summary>
        /// Enumerate ranges of bytes, referring to each range by identifying a particular memory chunk by index instead of ID, as well as an offset and length.
        /// </summary>
        /// <param name="relativeStartPositionOfSubrange"></param>
        /// <param name="numBytesInSubrange"></param>
        /// <returns></returns>
        public IEnumerable<MemoryBlockIndexAndSlice> EnumeratePortionOfMemoryBlockIndexAndSlices(long relativeStartPositionOfSubrange, long numBytesInSubrange)
        {
            long bytesBeforeSubrangeRemaining = relativeStartPositionOfSubrange;
            long bytesOfSubrangeRemaining = numBytesInSubrange;
            bool withinSubrange = false;
            foreach (var rangeInfo in EnumerateMemoryBlockIndexAndSlices())
            {
                long skipOverBytes = 0;
                if (!withinSubrange)
                {
                    skipOverBytes = Math.Min(bytesBeforeSubrangeRemaining, rangeInfo.Length);
                    bytesBeforeSubrangeRemaining -= skipOverBytes;
                    if (bytesBeforeSubrangeRemaining == 0)
                        withinSubrange = true;
                }
                if (withinSubrange)
                {
                    int numBytesToInclude = (int)Math.Min(bytesOfSubrangeRemaining, rangeInfo.Length - skipOverBytes);
                    yield return new MemoryBlockIndexAndSlice(rangeInfo.MemoryBlockIndex, (int)(rangeInfo.Offset + skipOverBytes), numBytesToInclude);
                    bytesOfSubrangeRemaining -= numBytesToInclude;
                    if (bytesOfSubrangeRemaining == 0)
                        yield break;
                }
            }
        }

        /// <summary>
        /// Enumerates all memory blocks.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReadOnlyMemory<byte>> EnumerateReadOnlyMemorySegments()
        {
            MemoryChunk lastMemoryReferenceLoaded = null;
            foreach (var rangeInfo in EnumerateMemoryBlockIndexAndSlices())
            {
                var memoryChunk = MemoryAtIndex(rangeInfo.MemoryBlockIndex);
                if (memoryChunk.IsLoaded == false)
                {
                    if (lastMemoryReferenceLoaded != null && lastMemoryReferenceLoaded != memoryChunk)
                    { // consider unloading the last memory reference, before loading this one, so that we don't have too many in memory at the same time
                        lastMemoryReferenceLoaded.ConsiderUnloadMemory();
                        lastMemoryReferenceLoaded = memoryChunk;
                    }
                    memoryChunk.LoadMemory();
                }
                yield return memoryChunk.ReadOnlyMemory.Slice(rangeInfo.Offset, rangeInfo.Length);
            }
        }

        /// <summary>
        /// Enumerates all memory blocks asynchronously, asynchronously loading and unloading blocks of memory as needed.
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<ReadOnlyMemory<byte>> EnumerateReadOnlyMemorySegmentsAsync()
        {
            MemoryChunk lastMemoryReferenceLoaded = null;
            foreach (var rangeInfo in EnumerateMemoryBlockIndexAndSlices())
            {
                var m = MemoryAtIndex(rangeInfo.MemoryBlockIndex);
                if (m is MemoryChunk memoryChunk && memoryChunk.IsLoaded == false)
                {
                    if (lastMemoryReferenceLoaded != null && lastMemoryReferenceLoaded != memoryChunk)
                    { // consider unloading the last memory reference, before loading this one, so that we don't have too many in memory at the same time
                        await lastMemoryReferenceLoaded.ConsiderUnloadMemoryAsync();
                        lastMemoryReferenceLoaded = memoryChunk;
                    }
                    await memoryChunk.LoadMemoryAsync();
                }
                yield return m.ReadOnlyMemory.Slice(rangeInfo.MemoryBlockIndex, rangeInfo.Length);
            }
        }

        /// <summary>
        /// Enumerates each of the memory chunks, whether included within the referenced range or not.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryChunk> EnumerateMemoryChunks()
        {
            if (SingleMemoryChunk != null)
                yield return SingleMemoryChunk;
            else if (MultipleMemoryChunks != null)
                foreach (var additional in MultipleMemoryChunks)
                    yield return additional;
        }

        public IEnumerable<MemoryChunk> DEBUG()
        {
            int index = 0;
            long bytesRemaining = Length;
            foreach (var memoryChunk in EnumerateMemoryChunks())
            {
                if (index == StartIndex)
                {
                    int lengthToInclude = (int)Math.Min(memoryChunk.Length - Offset, bytesRemaining);
                    var toInclude = memoryChunk.Slice(Offset, lengthToInclude);
                    bytesRemaining -= lengthToInclude;
                    yield return toInclude;
                }
                else if (index > StartIndex && bytesRemaining > 0)
                {
                    int lengthToInclude = (int)Math.Min(memoryChunk.Length, bytesRemaining);
                    var toInclude = memoryChunk.Slice(Offset, lengthToInclude);
                    bytesRemaining -= lengthToInclude;
                    yield return toInclude;
                }
            }
        }

        /// <summary>
        /// Enumerates the referenced memory owners (including portions of referenced memory chunks not referenced).
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IReadableBytes> EnumerateReadOnlyBytesSegments()
        {
            foreach (var memoryChunk in EnumerateMemoryChunks())
                yield return memoryChunk.MemoryAsLoaded;
        }

        /// <summary>
        /// Writes the memory to the binary buffer writer asynchronously.
        /// </summary>
        /// <param name="writer">The binary buffer writer container</param>
        /// <returns></returns>
        public async ValueTask WriteToBufferAsync(BufferWriterContainer writer)
        {
            await foreach (ReadOnlyMemory<byte> memory in EnumerateReadOnlyMemorySegmentsAsync())
                writer.Write(memory.Span);
        }


        /// <summary>
        /// Writes the memory to the binary buffer writer 
        /// </summary>
        /// <param name="writer">The binary buffer writer </param>
        /// <returns></returns>
        public void WriteToBuffer(ref BufferWriter writer)
        {
            foreach (ReadOnlyMemory<byte> memory in EnumerateReadOnlyMemorySegments())
                writer.Write(memory.Span);
        }

        /// <summary>
        /// Enumerates individual bytes referenced by this LazinatorMemory.
        /// </summary>
        /// <returns></returns>

        public IEnumerable<byte> EnumerateBytes()
        {
            foreach (ReadOnlyMemory<byte> memory in EnumerateReadOnlyMemorySegments())
                for (int i = 0; i < memory.Length; i++)
                    yield return memory.Span[i];
        }

        /// <summary>
        /// Checks whether the referenced memory matches a span
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public bool Matches(ReadOnlySpan<byte> span)
        {
            int i = 0;
            foreach (byte b in EnumerateBytes())
                if (span[i++] != b)
                    return false;
            return true;
        }

        /// <summary>
        /// Copies the referenced memory to an array
        /// </summary>
        /// <param name="array"></param>
        public void CopyToArray(byte[] array)
        {
            int i = 0;
            foreach (byte b in EnumerateBytes())
                array[i++] = b;
        }

        /// <summary>
        /// Returns a single memory block consolidating all of the memory from the LazinatorMemory. If there is only a single memory chunk, 
        /// then the memory is not copied.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyMemory<byte> GetConsolidatedMemory()
        {
            if (SingleMemory)
            {
                SingleMemoryChunk.LoadMemory();
                return SingleMemoryChunk.ReadOnlyMemory.Slice(Offset, (int) Length);
            }

            long totalLength = Length;
            if (totalLength > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            BufferWriter w = new BufferWriter((int) totalLength);
            foreach (byte b in EnumerateBytes())
                w.Write(b);
            return w.LazinatorMemory.InitialReadOnlyMemory;
        }

        public async ValueTask<ReadOnlyMemory<byte>> GetConsolidatedMemoryAsync()
        {
            if (SingleMemory)
            {
                await SingleMemoryChunk.LoadMemoryAsync();
                return SingleMemoryChunk.ReadOnlyMemory.Slice(Offset, (int)Length);
            }
            await LoadAllMemoryAsync();
            long totalLength = Length;
            if (totalLength > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            BufferWriter w = new BufferWriter((int)totalLength);
            foreach (byte b in EnumerateBytes())
                w.Write(b);
            return w.LazinatorMemory.InitialReadOnlyMemory;
        }

        public string ToStringByChunk()
        {
            var chunks = EnumerateMemoryChunks().ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var chunk in chunks)
            {
                sb.AppendLine(chunk.ToString());
            }
            return sb.ToString();
        }

        public string ToStringConsolidated()
        {
            return String.Join(",", EnumerateBytes().Select(x => x.ToString().PadLeft(3, '0')));
        }

        #endregion

    }
}
