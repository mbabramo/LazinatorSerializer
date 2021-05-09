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
        }

        public LazinatorMemory(IEnumerable<MemoryChunk> moreMemoryChunks, int startIndex, int startPosition, long length) : this(null, startPosition, length)
        {
            MultipleMemoryChunks = new MemoryChunkCollection(moreMemoryChunks);
            StartIndex = startIndex;
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

            var withAppendedChunk = new MemorySegmentCollection();
            withAppendedChunk.SetFromLazinatorMemory(this);
            withAppendedChunk.AppendMemoryChunk(chunk);
            return new LazinatorMemory(withAppendedChunk, StartIndex, Offset, Length + chunk.Length);
        }

        public bool Disposed => EnumerateReadOnlyBytesSegments().Any(x => x != null && (x is IMemoryAllocationInfo info && info.Disposed) || (x is ReadOnlyBytes s && s.Disposed));

        #endregion

        #region Disposal

        public void Dispose()
        {
            SingleMemoryChunk?.Dispose();
            if (MultipleMemoryChunks != null)
                foreach (var additional in MultipleMemoryChunks)
                    additional.Dispose();
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

        private MemorySegment SingleMemorySegment => new MemorySegment(SingleMemoryChunk, new MemoryBlockSlice(0, SingleMemoryChunk.Length));

        public MemorySegment MemorySegmentAtIndex(int i) => MultipleMemoryChunks == null && i == 0 ? SingleMemorySegment : MultipleMemoryChunks.MemoryAtIndex(i);

        public async ValueTask<MemorySegment> MemorySegmentAtIndexAsync(int i) => MultipleMemoryChunks == null && i == 0 ? SingleMemorySegment : await MultipleMemoryChunks.MemoryAtIndexAsync(i);

        /// <summary>
        /// Slices the first referenced memory chunk only, producing a new LazinatorMemory.
        /// </summary>
        /// <param name="offset">An offset relative to the existing Offset, which must refer to the initial memory</param>
        /// <param name="length"></param>
        /// <returns></returns>
        private LazinatorMemory SliceSingle(int offset, long length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(SingleMemoryChunk, Offset + offset, length);

        /// <summary>
        /// Slices the memory, returning a new LazinatorMemory beginning at the specified offset, beyond the offset already existing in this LazinatorMemory.
        /// </summary>
        /// <param name="offset">The first byte of the sliced memory, relative to the first byte of this LazinatorMemory</param>
        /// <returns></returns>
        public LazinatorMemory Slice(long offset) => Slice(offset, Length - offset);

        /// <summary>
        /// Slices the memory, returning a new LazinatorMemory beginning at the specified offset, beyond the offset already existing in this LazinatorMemory.
        /// </summary>
        /// <param name="furtherOffset">The first byte of the sliced memory, relative to the first byte specified by the offset of this LazinatorMemory</param>
        /// <param name="length">The number of bytes to include in the slice</param>
        /// <returns></returns>
        public LazinatorMemory Slice(long furtherOffset, long length)
        {
            if (Length == 0)
                return EmptyLazinatorMemory;

            if (SingleMemory)
            {
                return SliceSingle((int) furtherOffset, length);
            }

            MemorySegmentIndexAndSlice segmentInfo = MultipleMemoryChunks.GetMemorySegmentAtOffsetFromStartPosition(StartIndex, Offset, furtherOffset);

            return new LazinatorMemory(MultipleMemoryChunks.DeepCopy(), segmentInfo.MemorySegmentIndex, segmentInfo.Offset, length);
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
                ReadOnlyMemory<byte> memory = SingleMemoryChunk?.ReadOnlyMemory ?? MemorySegmentAtIndex(StartIndex).ReadOnlyMemory;
                return memory.Slice(Offset, memory.Length - Offset);
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
            ReadOnlyMemory<byte> memory = SingleMemoryChunk?.ReadOnlyMemory ?? MemorySegmentAtIndex(StartIndex).ReadOnlyMemory;
            return memory.Slice(Offset, memory.Length - Offset);
        }

        /// <summary>
        /// The only memory chunk. This will throw if there are multiple memory chunks.
        /// </summary>
        public ReadOnlyMemory<byte> OnlyMemory
        {
            get
            {
                if (!SingleMemory)
                    throw new LazinatorCompoundMemoryException();
                return InitialReadOnlyMemory;
            }
        }

        #endregion

        #region Loading

        // DEBUG -- remove all the Load stuff once we can make it transparent within the memory

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
            MemorySegment memorySegment = MemorySegmentAtIndex(StartIndex);
            LoadMemoryChunk(memorySegment.MemoryChunk);
        }

        public async ValueTask LoadInitialReadOnlyMemoryAsync()
        {
            if (SingleMemory)
            {
                await LoadMemoryChunkAsync(SingleMemoryChunk);
                return;
            }
            MemorySegment memorySegment = MemorySegmentAtIndex(StartIndex);
            await LoadMemoryChunkAsync(memorySegment.MemoryChunk);
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
                    LoadMemoryChunk(additional.MemoryChunk);
        }

        public async ValueTask LoadAllMemoryAsync()
        {
            await LoadInitialReadOnlyMemoryAsync();
            if (MultipleMemoryChunks != null)
                foreach (var additional in MultipleMemoryChunks)
                    await LoadMemoryChunkAsync(additional.MemoryChunk);
        }

        /// <summary>
        /// Allows for unloading the first referenced memory chunk, if it is loaded. The memory can be unloaded only if the owner of the first memory
        /// chunk is a MemoryReference that supports this functionality.
        /// </summary>
        public void ConsiderUnloadInitialReadOnlyMemory()
        {
            if (SingleMemory)
                return;
            MemorySegment memorySegment = MemorySegmentAtIndex(StartIndex);
            if (memorySegment.MemoryChunk.IsLoaded == true)
            {
                memorySegment.MemoryChunk.ConsiderUnloadMemory();
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
            MemorySegment memorySegment = MemorySegmentAtIndex(StartIndex);
            if (memorySegment.MemoryChunk.IsLoaded == true)
                await memorySegment.MemoryChunk.ConsiderUnloadMemoryAsync();
        }

        #endregion

        #region Multiple memory management

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
        public IEnumerable<MemorySegmentIndexAndSlice> EnumerateMemorySegmentIndexAndSlices()
        {
            if (SingleMemoryChunk != null)
            {
                yield return new MemorySegmentIndexAndSlice(0, Offset, (int)Length);
            }
            else 
                foreach (MemorySegmentIndexAndSlice indexAndSlice in MultipleMemoryChunks.EnumerateMemorySegmentIndexAndSlices(StartIndex, Offset, Length))
                    yield return indexAndSlice;
        }

        /// <summary>
        /// Enumerates memory chunk ranges corresponding to this LazinatorMemory.
        /// </summary>
        /// <returns>An enumerable where each element consists of the chunk ID, the start position, and the number of bytes</returns>
        public IEnumerable<MemoryBlockIDAndSlice> EnumerateMemoryBlockIDsAndSlices()
        {
            if (SingleMemoryChunk != null)
            {
                yield return new MemoryBlockIDAndSlice(0, Offset, (int)Length);
            }
            else
                foreach (MemoryBlockIDAndSlice idAndSlice in MultipleMemoryChunks.EnumerateMemoryBlockIDAndSlices(StartIndex, Offset, Length))
                    yield return idAndSlice;
        }

        public IEnumerable<MemorySegment> EnuemrateMemorySegments()
        {
            if (SingleMemoryChunk != null)
            {
                yield return SingleMemorySegment.Slice(Offset, (int) Length);
            }
            else
                foreach (MemorySegment memorySegment in MultipleMemoryChunks.EnumerateMemorySegments(StartIndex, Offset, Length))
                    yield return memorySegment;
        }

        /// <summary>
        /// Enumerates all memory blocks.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReadOnlyMemory<byte>> EnumerateReadOnlyMemory()
        {
            foreach (var memoryBlockIndexAndSlice in EnumerateMemorySegmentIndexAndSlices())
            {
                var memorySegment = MemorySegmentAtIndex(memoryBlockIndexAndSlice.MemorySegmentIndex);
                yield return memorySegment.ReadOnlyMemory.Slice(memoryBlockIndexAndSlice.Offset, memoryBlockIndexAndSlice.Length);
            }
        }

        /// <summary>
        /// Enumerates all memory blocks asynchronously, asynchronously loading and unloading blocks of memory as needed.
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<ReadOnlyMemory<byte>> EnumerateReadOnlyMemoryAsync()
        {
            foreach (var memoryBlockIndexAndSlice in EnumerateMemorySegmentIndexAndSlices())
            {
                var memorySegment = await MemorySegmentAtIndexAsync(memoryBlockIndexAndSlice.MemorySegmentIndex);
                yield return memorySegment.ReadOnlyMemory.Slice(memoryBlockIndexAndSlice.Offset, memoryBlockIndexAndSlice.Length);
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
                    yield return additional.MemoryChunk;
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
            await foreach (ReadOnlyMemory<byte> memory in EnumerateReadOnlyMemoryAsync())
                writer.Write(memory.Span);
        }


        /// <summary>
        /// Writes the memory to the binary buffer writer 
        /// </summary>
        /// <param name="writer">The binary buffer writer </param>
        /// <returns></returns>
        public void WriteToBuffer(ref BufferWriter writer)
        {
            foreach (ReadOnlyMemory<byte> memory in EnumerateReadOnlyMemory())
                writer.Write(memory.Span);
        }

        /// <summary>
        /// Enumerates individual bytes referenced by this LazinatorMemory.
        /// </summary>
        /// <returns></returns>

        public IEnumerable<byte> EnumerateBytes()
        {
            foreach (ReadOnlyMemory<byte> memory in EnumerateReadOnlyMemory())
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

        // DEBUG -- delete this
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
