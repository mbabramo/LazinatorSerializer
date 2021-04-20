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
    public readonly struct LazinatorMemory : IReadOnlyBytes
    {
        /// <summary>
        /// The first chunk of owned memory (and in ordinary usage, the only chunk).
        /// </summary>
        public readonly MemoryChunk InitialMemoryChunk;
        /// <summary>
        /// Additional chunks of owned memory, where the memory storage is split across chunks.
        /// </summary>
        public readonly IMemoryChunkCollection MoreMemoryChunks; 
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

        public bool IsEmpty => InitialMemoryChunk == null || Length == 0;
        public long? AllocationID => (InitialMemoryChunk.MemoryAsLoaded as ExpandableBytes)?.AllocationID;

        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(new Memory<byte>());


        public override string ToString()
        {
            return $@"{(AllocationID != null ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", EnumerateBytes().Take(2000))}";
        }

        #region Construction

        public LazinatorMemory(MemoryChunk memoryChunk, int startPosition, long length)
        {
            InitialMemoryChunk = memoryChunk;
            MoreMemoryChunks = null;
            StartIndex = 0;
            if (startPosition < 0)
                throw new ArgumentException();
            Offset = startPosition;
            if (length < 0)
                Length = 0;
            else
                Length = length;
        }

        public LazinatorMemory(MemoryChunk memoryChunk, IMemoryChunkCollection moreMemoryChunks, int startIndex, int startPosition, long length) : this(memoryChunk, startPosition, length)
        {
            MoreMemoryChunks = moreMemoryChunks;
            StartIndex = startIndex;
        }

        public LazinatorMemory(MemoryChunk memoryChunk, IEnumerable<MemoryChunk> moreMemoryChunks, int startIndex, int startPosition, long length) : this(memoryChunk, startPosition, length)
        {
            MoreMemoryChunks = new MemoryChunkCollection();
            MoreMemoryChunks.SetContents(moreMemoryChunks);
            StartIndex = startIndex;
        }

        public LazinatorMemory(MemoryChunk memoryChunk, long length) : this(memoryChunk, 0, length)
        {
        }

        public LazinatorMemory(MemoryChunk memoryChunk) : this(memoryChunk, 0, memoryChunk.ReadOnlyMemory.Length)
        {
        }

        public LazinatorMemory(IReadOnlyBytes readOnlyBytes) : this(new MemoryChunk(readOnlyBytes))
        {
        }

        public LazinatorMemory(Memory<byte> memory) : this(new MemoryChunk(new ReadOnlyBytes(memory)))
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
                var evenMoreOwnedMemory = MoreMemoryChunks?.WithAppendedMemoryChunk(chunk) ?? new MemoryChunkCollection(new List<MemoryChunk>() { chunk });
                return new LazinatorMemory(InitialMemoryChunk.WithPreTruncationLengthIncreasedIfNecessary(chunk), evenMoreOwnedMemory, StartIndex, Offset, Length + chunk.Reference.FinalLength);
            }

            // The current LazinatorMemory does not terminate at the end of the last chunk. If we just added a chunk, then
            // the range would include the chunks at the end that we do not reference.

            // We can't just return the existing memory plus the new memory, because the existing memory might include
            // some memory that isn't referenced. If, for example, that memory is at the end of the range, then adding
            // an additional chunk will not allow us to reference that memory. 

            MemoryChunk initialMemoryChunk;
            List<MemoryChunk> additionalMemoryChunks;
            GetReferencedMemoryChunks(chunk, out initialMemoryChunk, out additionalMemoryChunks);
            additionalMemoryChunks.Add(chunk);
            MemoryChunkCollection memoryChunkCollection = new MemoryChunkCollection();
            memoryChunkCollection.SetContents(additionalMemoryChunks);
            return new LazinatorMemory(initialMemoryChunk, memoryChunkCollection, StartIndex, Offset, Length);
        }

        private void GetReferencedMemoryChunks(MemoryChunk chunkBeingAdded, out MemoryChunk initialMemoryChunk, out List<MemoryChunk> additionalMemoryChunks)
        {
            List<MemoryChunkIndexReference> memoryChunkIndexReferences = EnumerateMemoryChunksByIndex().ToList();
            initialMemoryChunk = GetMemoryChunkFromMemoryChunkIndexReference(memoryChunkIndexReferences[0]).WithPreTruncationLengthIncreasedIfNecessary(chunkBeingAdded);
            additionalMemoryChunks = new List<MemoryChunk>();
            foreach (var indexReference in memoryChunkIndexReferences.Skip(1))
                additionalMemoryChunks.Add(GetMemoryChunkFromMemoryChunkIndexReference(indexReference).WithPreTruncationLengthIncreasedIfNecessary(chunkBeingAdded));
        }

        public bool Disposed => EnumerateReadOnlyBytesSegments(true).Any(x => x != null && (x is ExpandableBytes e && e.Disposed) || (x is ReadOnlyBytes s && s.Disposed));

        #endregion

        #region Disposal

        public void Dispose()
        {
            InitialMemoryChunk?.Dispose();
            if (MoreMemoryChunks != null)
                foreach (var additional in MoreMemoryChunks)
                    additional?.Dispose();
        }

        public void LazinatorShouldNotReturnToPool()
        {
            Helper(InitialMemoryChunk);

            if (MoreMemoryChunks != null)
                foreach (var additional in MoreMemoryChunks)
                {
                    Helper(additional);
                }

            static void Helper(MemoryChunk memoryChunk)
            {
                if (memoryChunk.MemoryAsLoaded is ExpandableBytes e)
                {
                    throw new Exception("DEBUG");
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

        private MemoryChunk MemoryAtIndex(int i) => i == 0 ? InitialMemoryChunk : MoreMemoryChunks.MemoryAtIndex(i - 1);

        /// <summary>
        /// Gets the final length of the specified memory chunk. It avoids loading the memory if possible.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int LengthAtIndex(int i)
        {
            var memoryAtIndex = MemoryAtIndex(i);
            return memoryAtIndex.Reference.FinalLength;
        }

        public MemoryChunk GetFirstMemoryChunkWithID(int memoryChunkID)
        {
            int? index = GetFirstIndexOfMemoryChunkID(memoryChunkID);
            if (index == null)
                return null;
            return (MemoryChunk) MemoryAtIndex((int)index);
        }

        public int? GetFirstIndexOfMemoryChunkID(int memoryChunkID)
        {
            if (MemoryAtIndex(0).Reference.MemoryChunkID == memoryChunkID)
                return 0;
            if (MoreMemoryChunks == null)
                return null;
            var index = MoreMemoryChunks.GetFirstIndexOfMemoryChunkID(memoryChunkID);
            if (index == null)
                return null;
            return index + 1;
        }

        /// <summary>
        /// Slices the initial referenced memory chunk only, producing a new LazinatorMemory.
        /// </summary>
        /// <param name="offset">An offset relative to the existing offset</param>
        /// <returns>The memory sliced, if the offset is valid, or empty memory otherwise</returns>
        private LazinatorMemory SliceInitial(int offset) => Length - offset is long revisedLength and > 0 ? SliceInitial(offset, revisedLength) : LazinatorMemory.EmptyLazinatorMemory;

        /// <summary>
        /// Slices the first referenced memory chunk only, producing a new LazinatorMemory.
        /// </summary>
        /// <param name="offset">An offset relative to the existing Offset, which must refer to the initial memory</param>
        /// <param name="length"></param>
        /// <returns></returns>
        private LazinatorMemory SliceInitial(int offset, long length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(InitialMemoryChunk, Offset + offset, length);

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
                return SliceInitial((int) offset, length);
            }

            // relativePositionOfSubrange is relative to the total offset within memory chunk index StartIndex. 
            // We use up "positionRemaining" by advancing StartPosition up to the end of the length of the starting index.
            // If we go all the way to the end, then we increment the starting index.
            // Note that we never change the Length (which is the Length of all combined).
            long positionRemaining = offset;
            int revisedStartIndex = StartIndex;
            int revisedStartPosition = Offset;
            int moreMemoryCount = MoreMemoryChunks?.Count ?? 0;
            while (positionRemaining > 0)
            {
                MemoryChunk current = MemoryAtIndex(revisedStartIndex);
                int remainingBytesThisMemory = current.Reference.FinalLength - revisedStartPosition;
                if (remainingBytesThisMemory <= positionRemaining)
                {
                    positionRemaining -= remainingBytesThisMemory;
                    if (positionRemaining == 0 && revisedStartIndex == moreMemoryCount)
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

            return new LazinatorMemory((MemoryChunk)InitialMemoryChunk, MoreMemoryChunks.DeepCopy(), revisedStartIndex, revisedStartPosition, length);
        }

        #endregion

        #region Equality

        public override bool Equals(object obj) => obj == null ? throw new LazinatorSerializationException("Invalid comparison of LazinatorMemory to null") :
            obj is LazinatorMemory lm && lm.InitialMemoryChunk.Equals(InitialMemoryChunk) && lm.Offset == Offset && lm.Length == Length;

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

        #region Initial memory

        /// <summary>
        /// True if there is only a single memory chunk.
        /// </summary>
        public bool SingleMemory => MoreMemoryChunks == null || MoreMemoryChunks.Count() == 0;
        /// <summary>
        /// The first referenced memory chunk
        /// </summary>
        public ReadOnlyMemory<byte> ReadOnlyMemory
        {
            get
            {
                if (IsEmpty)
                    return EmptyReadOnlyMemory;
                LoadInitialMemory();
                if (SingleMemory)
                    return InitialMemoryChunk.ReadOnlyMemory.Slice(Offset, (int)Length);
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
        /// A read-only version of the first referenced memory chunk.
        /// </summary>
        public ReadOnlyMemory<byte> InitialReadOnlyMemory => ReadOnlyMemory;


        /// <summary>
        /// Asynchronously returns the first referenced memory chunk.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<ReadOnlyMemory<byte>> GetInitialMemoryAsync()
        {
            if (IsEmpty)
                return EmptyMemory;
            await LoadInitialMemoryAsync();
            if (SingleMemory)
                return InitialMemoryChunk.ReadOnlyMemory.Slice(Offset, (int)Length);
            else
            {
                var readOnlyMemory = InitialMemoryChunk.ReadOnlyMemory;
                int overallMemoryLength = readOnlyMemory.Length;
                int lengthOfMemoryChunkAfterStartPosition = overallMemoryLength - Offset;
                return readOnlyMemory.Slice(Offset, lengthOfMemoryChunkAfterStartPosition);
            }
        }

        /// <summary>
        /// A read-only version of the first referenced memory chunk, returned asynchronously.
        /// </summary>
        public ReadOnlyMemory<byte> GetInitialReadOnlyMemory() => ReadOnlyMemory;

        /// <summary>
        /// A read-only version of the first referenced memory chunk, returned asynchronously.
        /// </summary>
        public async ValueTask<ReadOnlyMemory<byte>> GetInitialReadOnlyMemoryAsync() => await GetInitialMemoryAsync();

        /// <summary>
        /// The only memory chunk. This will throw if there are multiple memory chunks.
        /// </summary>
        public ReadOnlyMemory<byte> OnlyMemory
        {
            get
            {
                if (!SingleMemory)
                    throw new LazinatorCompoundMemoryException();
                return InitialMemoryChunk.ReadOnlyMemory.Slice(Offset, (int) Length);
            }
        }

        /// <summary>
        /// Loads the first referenced memory chunk synchronously if it is not loaded.
        /// </summary>
        /// <returns></returns>
        public void LoadInitialMemory()
        {
            if (SingleMemory)
            {
                LoadMemoryChunk(InitialMemoryChunk);
                return;
            }
            MemoryChunk memoryChunk = MemoryAtIndex(StartIndex);
            LoadMemoryChunk(memoryChunk);
        }

        public async ValueTask LoadInitialMemoryAsync()
        {
            if (SingleMemory)
            {
                await LoadMemoryChunkAsync(InitialMemoryChunk);
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
            LoadInitialMemory();
            if (MoreMemoryChunks != null)
                foreach (var additional in MoreMemoryChunks)
                    LoadMemoryChunk(additional);
        }

        public async ValueTask LoadAllMemoryAsync()
        {
            await LoadInitialMemoryAsync();
            if (MoreMemoryChunks != null)
                foreach (var additional in MoreMemoryChunks)
                    await LoadMemoryChunkAsync(additional);
        }

        /// <summary>
        /// Allows for unloading the first referenced memory chunk, if it is loaded. The memory can be unloaded only if the owner of the first memory
        /// chunk is a MemoryReference that supports this functionality.
        /// </summary>
        public void ConsiderUnloadInitialMemory()
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
        public async ValueTask ConsiderUnloadInitialMemoryAsync()
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
            if (InitialMemoryChunk == null)
                return 0;
            return 1 + (MoreMemoryChunks == null ? 0 : MoreMemoryChunks.Count);
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

        /// <summary>
        /// A reference to a portion of a stored memory chunk. Unlike MemoryChunkReference, each reference refers
        /// to a particular memory chunk by index instead of memory chunk ID.
        /// </summary>
        private readonly struct MemoryChunkIndexReference
        {
            public readonly int MemoryChunkIndex;
            public readonly int Offset;
            public readonly int Length;

            public MemoryChunkIndexReference(int memoryChunkIndex, int offset, int length)
            {
                this.MemoryChunkIndex = memoryChunkIndex;
                this.Offset = offset;
                this.Length = length;
            }
        }

        private MemoryChunk GetMemoryChunkFromMemoryChunkIndexReference(MemoryChunkIndexReference memoryChunkIndexReference) => MemoryAtIndex(memoryChunkIndexReference.MemoryChunkIndex);

        internal int GetNextMemoryChunkID()
        {
            if (IsEmpty)
                return 0;
            int maxMemoryChunkID = Math.Max(InitialMemoryChunk.MemoryChunkID, MoreMemoryChunks?.MaxMemoryChunkID ?? 0); // not always the ID of the last chunk, because patching may reassemble into a different order. We are guaranteed, however, that if we're doing versioning, the most recent memory chunk ID will be included.
            return maxMemoryChunkID + 1;
        }

        /// <summary>
        /// Enumerates memory chunk ranges corresponding to this LazinatorMemory. Note that memory chunks are referred to by index instead of by ID.
        /// </summary>
        /// <param name="includeOutsideOfRange">If true, then the full range of bytes in all contained memory chunks is included; if false, only those bytes referenced by this LazinatorMemory are included</param>
        /// <returns>An enumerable where each element consists of the chunk index, the start position, and the number of bytes</returns>
        private IEnumerable<MemoryChunkIndexReference> EnumerateMemoryChunksByIndex(bool includeOutsideOfRange = false)
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
                    startPositionOrZero = Offset;
                else
                    startPositionOrZero = 0;
                int numBytes = m.Reference.FinalLength - startPositionOrZero;
                if (numBytes > lengthRemaining)
                    numBytes = (int) lengthRemaining;
                yield return new MemoryChunkIndexReference(i, startPositionOrZero, numBytes);
                lengthRemaining -= numBytes;
                if (lengthRemaining == 0)
                    yield break;
            }
        }

        // Explanation of how delta serialization works:
        // Assume we have a large LazinatorMemory consisting of a set of MemoryReferences, each a lazy loadable long range of bytes. As we go from one version to the next, we will be adding additional ranges.
        // Then assume we have a cobbled-together LazinatorMemory with references to these big ranges. This is effectively the result of potentially multiple generations of delta serialization.
        // Eventually, some parts of the large LazinatorMemory may be deleted, because the more recent delta serializations don't refer to them anymore.
        // Now, changes are made to this object graph, and the goal is to do another delta serialization. 
        // BufferWriter will be compiling a list of the ranges we are writing to in the large LazinatorMemory.
        // When an object has changed, then BufferWriter's action depends on whether it is writing some entirely new data or repeating previously serialized data.
        // When writing previously serialized data, it simply records a new MemoryChunkReference, which points to the location in this range of bytes in some
        // existing memory chunk from the original LazinatorMemory. When writing new data, it adds to its ActiveMemory but also adds a MemoryChunkReference
        // (or extends an existing such reference) so that the set of MemoryChunkReferences will always encompass all of the data.
        // Note that as usual, if BufferWriter needs to, it will append its active memory chunk to the LazinatorMemory and move to a new active chunk.

        /// <summary>
        /// Enumerates memory chunk references based on an initial memory chunk index (not an ID), an offset into that memory chunk, and a length.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryChunkReference> EnumerateMemoryChunkReferences(int initialMemoryChunkIndex, int offset, long length)
        {
            int memoryChunkIndex = initialMemoryChunkIndex;
            long numBytesOfLengthRemaining=length;
            while (numBytesOfLengthRemaining > 0)
            {
                var memoryChunk = MemoryAtIndex(memoryChunkIndex);

                int numBytesThisChunk = memoryChunk.Reference.FinalLength;
                int bytesToUseThisChunk = (int)Math.Min(numBytesThisChunk - offset, numBytesOfLengthRemaining);
                yield return memoryChunk.Reference.Slice(offset, bytesToUseThisChunk);

                numBytesOfLengthRemaining -= bytesToUseThisChunk;
                memoryChunkIndex++;
                offset = 0;
            }
        }

        /// <summary>
        /// Enumerates memory chunk ranges corresponding to a subset of the bytes referenced by this LazinatorMemory
        /// </summary>
        /// <param name="offset">The byte index at which to start enumerating</param>
        /// <param name="length">The number of bytes to include</param>
        /// <returns>An enumerable where each element consists of the chunk index, the start position, and the number of bytes</returns>
        public IEnumerable<MemoryChunkReference> EnumerateMemoryChunkReferences(long offset, long length)
        {
            foreach (MemoryChunkIndexReference memoryChunkIndexReference in EnumerateMemoryChunkIndices(offset, length))
            {
                MemoryChunk memoryChunk = MemoryAtIndex(memoryChunkIndexReference.MemoryChunkIndex);
                yield return memoryChunk.Reference.Slice(memoryChunkIndexReference.Offset, memoryChunkIndexReference.Length);
            }
        }

        /// <summary>
        /// Enumerate ranges of bytes, referring to each range by identifying a particular memory chunk by index instead of ID, as well as an offset and length.
        /// </summary>
        /// <param name="relativeStartPositionOfSubrange"></param>
        /// <param name="numBytesInSubrange"></param>
        /// <returns></returns>
        private IEnumerable<MemoryChunkIndexReference> EnumerateMemoryChunkIndices(long relativeStartPositionOfSubrange, long numBytesInSubrange)
        {
            long bytesBeforeSubrangeRemaining = relativeStartPositionOfSubrange;
            long bytesOfSubrangeRemaining = numBytesInSubrange;
            bool withinSubrange = false;
            foreach (var rangeInfo in EnumerateMemoryChunksByIndex(false))
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
                    yield return new MemoryChunkIndexReference(rangeInfo.MemoryChunkIndex, (int)(rangeInfo.Offset + skipOverBytes), numBytesToInclude);
                    bytesOfSubrangeRemaining -= numBytesToInclude;
                    if (bytesOfSubrangeRemaining == 0)
                        yield break;
                }
            }
        }

        /// <summary>
        /// Returns the Memory block of bytes corresponding to a memory chunk reference. It is required that each memory owner be a MemoryChunk.
        /// </summary>
        /// <param name="memoryChunkReference">The memory chunk reference</param>
        /// <returns></returns>
        public ReadOnlyMemory<byte> GetMemoryAtMemoryChunkReference(MemoryChunkReference memoryChunkReference)
        {
            var memoryChunk = GetFirstMemoryChunkWithID(memoryChunkReference.MemoryChunkID);
            memoryChunk.LoadMemory();
            var underlyingReadOnlyMemory = memoryChunk.MemoryAsLoaded.ReadOnlyMemory.Slice(memoryChunkReference.AdditionalOffset, memoryChunkReference.FinalLength);
            return underlyingReadOnlyMemory;
        }

        /// <summary>
        /// Enumerates all memory blocks.
        /// </summary>
        /// <param name="includeOutsideOfRange">If true, includes all memory blocks, including those beyond the range referenced in this LazinatorMemory; if false, includes only the portion of memory represented by the range referenced in this LazinatorMemory </param>
        /// <returns></returns>
        public IEnumerable<ReadOnlyMemory<byte>> EnumerateRawMemory(bool includeOutsideOfRange = false)
        {
            MemoryChunk lastMemoryReferenceLoaded = null;
            foreach (var rangeInfo in EnumerateMemoryChunksByIndex(includeOutsideOfRange))
            {
                var memoryChunk = MemoryAtIndex(rangeInfo.MemoryChunkIndex);
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
        /// <param name="includeOutsideOfRange">If true, includes all memory blocks, including those beyond the range referenced in this LazinatorMemory; if false, includes only the portion of memory represented by the range referenced in this LazinatorMemory </param>
        /// <returns></returns>
        public async IAsyncEnumerable<ReadOnlyMemory<byte>> EnumerateRawMemoryAsync(bool includeOutsideOfRange = false)
        {
            MemoryChunk lastMemoryReferenceLoaded = null;
            foreach (var rangeInfo in EnumerateMemoryChunksByIndex(includeOutsideOfRange))
            {
                var m = MemoryAtIndex(rangeInfo.MemoryChunkIndex);
                if (m is MemoryChunk memoryChunk && memoryChunk.IsLoaded == false)
                {
                    if (lastMemoryReferenceLoaded != null && lastMemoryReferenceLoaded != memoryChunk)
                    { // consider unloading the last memory reference, before loading this one, so that we don't have too many in memory at the same time
                        await lastMemoryReferenceLoaded.ConsiderUnloadMemoryAsync();
                        lastMemoryReferenceLoaded = memoryChunk;
                    }
                    await memoryChunk.LoadMemoryAsync();
                }
                yield return m.ReadOnlyMemory.Slice(rangeInfo.MemoryChunkIndex, rangeInfo.Length);
            }
        }

        /// <summary>
        /// Enumerates each of the memory owners, whether included within the referenced range or not.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryChunk> EnumerateMemoryChunks(bool includeOutsideOfRange = false)
        {
            if (includeOutsideOfRange)
            {
                if (InitialMemoryChunk != null)
                    yield return InitialMemoryChunk;
                if (MoreMemoryChunks != null)
                    foreach (var additional in MoreMemoryChunks)
                        yield return additional;
            }
            else
            {
                int index = 0;
                long bytesRemaining = Length;
                foreach (var memoryChunk in EnumerateMemoryChunks(true))
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
        }

        /// <summary>
        /// Enumerates the referenced memory owners (including portions of referenced memory chunks not referenced).
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IReadOnlyBytes> EnumerateReadOnlyBytesSegments(bool includeOutsideOfRange = false)
        {
            foreach (var memoryChunk in EnumerateMemoryChunks(false))
                yield return memoryChunk.MemoryAsLoaded;
        }

        public Dictionary<int, MemoryChunk> GetMemoryChunksByID()
        {
            Dictionary<int, MemoryChunk> d = new Dictionary<int, MemoryChunk>();
            foreach (MemoryChunk memoryChunk in EnumerateMemoryChunks(true))
            {
                int chunkID = memoryChunk.Reference.MemoryChunkID;
                if (!d.ContainsKey(chunkID))
                    d[chunkID] = memoryChunk;
            }
            return d;
        }

        public List<MemoryChunk> GetUnpersistedMemoryChunks()
        {
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
            HashSet<int> ids = new HashSet<int>();
            foreach (MemoryChunk memoryChunk in EnumerateMemoryChunks(true))
            {
                if (memoryChunk.IsPersisted)
                    continue;
                int chunkID = memoryChunk.Reference.MemoryChunkID;
                if (!ids.Contains(chunkID))
                {
                    ids.Add(chunkID);
                    memoryChunks.Add(memoryChunk);
                }
            }
            return memoryChunks;
        }

        /// <summary>
        /// Writes the memory to the binary buffer writer asynchronously.
        /// </summary>
        /// <param name="writer">The binary buffer writer container</param>
        /// <param name="includeOutsideOfRange">True if contained memory that is NOT written should be written.</param>
        /// <returns></returns>
        public async ValueTask WriteToBinaryBufferAsync(BufferWriterContainer writer, bool includeOutsideOfRange = false)
        {
            await foreach (ReadOnlyMemory<byte> memory in EnumerateRawMemoryAsync(includeOutsideOfRange))
                writer.Write(memory.Span);
        }


        /// <summary>
        /// Writes the memory to the binary buffer writer 
        /// </summary>
        /// <param name="writer">The binary buffer writer </param>
        /// <param name="includeOutsideOfRange">True if contained memory that is NOT written should be written.</param>
        /// <returns></returns>
        public void WriteToBinaryBuffer(ref BufferWriter writer, bool includeOutsideOfRange = false)
        {
            foreach (ReadOnlyMemory<byte> memory in EnumerateRawMemory(includeOutsideOfRange))
                writer.Write(memory.Span);
        }

        /// <summary>
        /// Enumerates individual bytes referenced by this LazinatorMemory.
        /// </summary>
        /// <param name="includeOutsideOfRange">If true, bytes outside the referenced range are included.</param>
        /// <returns></returns>
        public IEnumerable<byte> EnumerateBytes(bool includeOutsideOfRange = false)
        {
            if (!includeOutsideOfRange && Length == 0)
                yield break;
            int startIndexOrZero = includeOutsideOfRange ? 0 : StartIndex;
            int totalItems = NumMemoryChunks();
            int numYielded = 0;
            for (int i = StartIndex; i < totalItems; i++)
            {
                var memoryChunk = MemoryAtIndex(i);
                memoryChunk.LoadMemory();
                int startPositionOrZero;
                if (i == StartIndex && !includeOutsideOfRange)
                    startPositionOrZero = Offset;
                else
                    startPositionOrZero = 0;
                int memoryChunkLength = memoryChunk.Reference.FinalLength;
                for (int j = startPositionOrZero; j < memoryChunkLength; j++)
                {
                    yield return memoryChunk.ReadOnlyMemory.Span[j];
                    numYielded++;
                    if (!includeOutsideOfRange && numYielded == Length)
                        yield break;
                }
            }
        }

        /// <summary>
        /// Checks whether the referenced memory matches a span
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public bool Matches(ReadOnlySpan<byte> span)
        {
            int i = 0;
            foreach (byte b in EnumerateBytes(false))
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
            foreach (byte b in EnumerateBytes(false))
                array[i++] = b;
        }

        /// <summary>
        /// Returns a single memory block consolidating all of the memory from the LazinatorMemory. If there is only a single memory chunk, 
        /// then the memory is not copied.
        /// </summary>
        /// <param name="includeOutsideOfRange"></param>
        /// <returns></returns>
        public ReadOnlyMemory<byte> GetConsolidatedMemory(bool includeOutsideOfRange = false)
        {
            if (SingleMemory)
            {
                InitialMemoryChunk.LoadMemory();
                if (includeOutsideOfRange)
                    return InitialMemoryChunk.ReadOnlyMemory;
                else
                    return InitialMemoryChunk.ReadOnlyMemory.Slice(Offset, (int) Length);
            }

            long totalLength = includeOutsideOfRange ? GetGrossLength() : Length;
            if (totalLength > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            BufferWriter w = new BufferWriter((int) totalLength);
            foreach (byte b in EnumerateBytes(includeOutsideOfRange))
                w.Write(b);
            return w.LazinatorMemory.ReadOnlyMemory;
        }

        public async ValueTask<ReadOnlyMemory<byte>> GetConsolidatedMemoryAsync(bool includeOutsideOfRange = false)
        {
            if (SingleMemory)
            {
                await InitialMemoryChunk.LoadMemoryAsync();
                if (includeOutsideOfRange)
                    return InitialMemoryChunk.ReadOnlyMemory;
                else
                    return InitialMemoryChunk.ReadOnlyMemory.Slice(Offset, (int)Length);
            }
            await LoadAllMemoryAsync();
            long totalLength = includeOutsideOfRange ? GetGrossLength() : Length;
            if (totalLength > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            BufferWriter w = new BufferWriter((int)totalLength);
            foreach (byte b in EnumerateBytes(includeOutsideOfRange))
                w.Write(b);
            return w.LazinatorMemory.ReadOnlyMemory;
        }

        public string ToStringByChunk()
        {
            var chunks = EnumerateMemoryChunks(true).ToList();
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

        #region Defragmenting


        internal List<(int memoryChunkID, int totalBytesReferenced)> GetMemoryChunksByUsedSize(int maxTotalBytesReference = int.MaxValue)
        {
            return EnumerateMemoryChunks(false)
                .Select(x => x.Reference)
                .GroupBy(x => x.MemoryChunkID)
                .Select(x => (x.Key, x.Sum(y => y.FinalLength)))
                .Where(x => x.Item2 <= maxTotalBytesReference)
                .OrderBy(x => x.Item2)
                .ToList();
        }

        public LazinatorMemory WithConsolidatedMemoryRanges(int maxUsedSizeOldChunk, int maxSizePerNewChunk, int maxNumNewChunks)
        {
            List<List<int>> packingPlan = new List<List<int>>(); // lists the set of chunks to be added to new memory chunks. 
            Dictionary<int, int> memoryChunkIDMap = new Dictionary<int, int>(); // maps the source memory chunk ID to a new target memory chunk ID
            Dictionary<int, int> totalBytesInTargetMemoryChunkID = new Dictionary<int, int>(); // tracks the total bytes in each target memory chunk ID
            var candidateChunks = GetMemoryChunksByUsedSize(maxUsedSizeOldChunk); // chunks where there are sufficiently few bytes referenced so that we might be able to combine them
            int firstPlannedMemoryChunkID = GetNextMemoryChunkID();
            int currentPlannedMemoryChunkID = firstPlannedMemoryChunkID;
            int bytesInCurrentChunk = 0;
            packingPlan.Add(new List<int>());
            int numChunksAdded = 1;
            foreach (var candidateChunk in candidateChunks)
            {
                // See if the candidate chunk can fit in the current chunk. If not, move to the next chunk, or return if max new chunks is hit.
                int bytesRemainingInCurrentChunk = maxSizePerNewChunk - bytesInCurrentChunk;
                if (candidateChunk.totalBytesReferenced > bytesRemainingInCurrentChunk)
                { // too big
                    bool chunkIsEmpty = bytesRemainingInCurrentChunk == maxSizePerNewChunk;
                    if (numChunksAdded == maxNumNewChunks || chunkIsEmpty)
                    {
                        if (chunkIsEmpty)
                        {
                            numChunksAdded--;
                            packingPlan.RemoveAt(packingPlan.Count - 1);
                        }
                        break; // can't add anything else
                    }
                    // prepare for next chunk
                    packingPlan.Add(new List<int>()); 
                    bytesInCurrentChunk = 0;
                    currentPlannedMemoryChunkID++;
                    numChunksAdded++;
                }
                // Plan to add the candidate chunk.
                packingPlan.Last().Add(candidateChunk.memoryChunkID);
                bytesInCurrentChunk += candidateChunk.totalBytesReferenced;
                int targetChunkID = firstPlannedMemoryChunkID + numChunksAdded;
                memoryChunkIDMap[candidateChunk.memoryChunkID] = targetChunkID;
                totalBytesInTargetMemoryChunkID[targetChunkID] = bytesInCurrentChunk;
            }

            if (!packingPlan.Any() || (packingPlan.Count() == 1 && packingPlan[0].Count() == 1 && packingPlan[0][0] == InitialMemoryChunk.MemoryChunkID))
                return this; // avoid repacking the initial memory chunk without anything else, since that accomplishes nothing.

            Dictionary<int, int> memoryChunkIDsToPack = new Dictionary<int, int>();
            for (int i = 0; i < packingPlan.Count; i++)
            {
                int targetChunkID = firstPlannedMemoryChunkID + i;
                List<int> existingChunks = packingPlan[i];
                foreach (var existingChunk in existingChunks)
                    memoryChunkIDMap[existingChunk] = targetChunkID;
            }

            List<MemoryChunk> sourceMemoryChunksToInclude = new List<MemoryChunk>();
            List<MemoryChunk> targetMemoryChunksToInclude = new List<MemoryChunk>();
            Dictionary<int, MemoryChunk> memoryChunksIncludedByID = new Dictionary<int, MemoryChunk>();
            Dictionary<int, int> bytesSoFarInTargetMemoryChunkID = new Dictionary<int, int>();
            foreach (var sourceMemoryChunk in EnumerateMemoryChunks(false))
            {
                if (memoryChunkIDMap.ContainsKey(sourceMemoryChunk.MemoryChunkID))
                {
                    int targetMemoryChunkID = memoryChunkIDMap[sourceMemoryChunk.MemoryChunkID];
                    int totalBytesInTarget = totalBytesInTargetMemoryChunkID[targetMemoryChunkID];
                    int bytesInTargetSoFar = bytesSoFarInTargetMemoryChunkID.GetValueOrDefault(targetMemoryChunkID, 0);
                    int bytesToCopyToTarget = sourceMemoryChunk.Reference.FinalLength;
                    MemoryChunkReference reference = new MemoryChunkReference(targetMemoryChunkID, 0, totalBytesInTarget, bytesInTargetSoFar, bytesToCopyToTarget);
                    MemoryChunk chunk;
                    if (!memoryChunksIncludedByID.ContainsKey(targetMemoryChunkID))
                    {
                        chunk = memoryChunksIncludedByID[targetMemoryChunkID] = new MemoryChunk(new ReadOnlyBytes(new byte[totalBytesInTarget]));
                        targetMemoryChunksToInclude.Add(sourceMemoryChunk);
                    }
                    else
                        chunk = memoryChunksIncludedByID[targetMemoryChunkID];
                    Memory<byte> targetBytes = chunk.ReadOnlyMemory.Slice(bytesInTargetSoFar, bytesToCopyToTarget);
                    sourceMemoryChunk.ReadOnlyMemory.CopyTo(targetBytes);
                    bytesSoFarInTargetMemoryChunkID[targetMemoryChunkID] = bytesInTargetSoFar + bytesToCopyToTarget;
                }
                else
                {
                    if (!memoryChunksIncludedByID.ContainsKey(sourceMemoryChunk.MemoryChunkID))
                    {
                        sourceMemoryChunksToInclude.Add(sourceMemoryChunk);
                        memoryChunksIncludedByID[sourceMemoryChunk.MemoryChunkID] = sourceMemoryChunk;
                    }
                }
            }
            List<MemoryChunk> moreMemoryChunks = new List<MemoryChunk>();
            moreMemoryChunks.AddRange(sourceMemoryChunksToInclude.Skip(1));
            moreMemoryChunks.AddRange(targetMemoryChunksToInclude);
            LazinatorMemory replacement = new LazinatorMemory(sourceMemoryChunksToInclude.First(), moreMemoryChunks, 0, 0, Length);
            return replacement;
        }

        #endregion

    }
}
