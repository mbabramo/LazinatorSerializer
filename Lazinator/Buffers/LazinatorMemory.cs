﻿using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// An immutable memory owner used by Lazinator to store and reference serialized Lazinator data. The memory may be split across multiple memory blocks.
    /// The memory referenced is defined by the index of the first memory block, the index of the first byte within that block, and the number of 
    /// bytes altogether. 
    /// </summary>
    public readonly struct LazinatorMemory
    {
        /// <summary>
        /// A single block of memory
        /// </summary>
        public readonly MemoryBlock SingleMemoryBlock;
        /// <summary>
        /// Multiple blocks of memory
        /// </summary>
        public readonly MemoryBlockCollection MultipleMemoryBlocks;
        /// <summary>
        /// The index of the MemoryRange from MultipleMemoryBlocks (or 0 with a single block) for the referenced range. Note that this is not necessarily the index of the memory block, since MemoryRanges may consist of patched ranges of blocks.
        /// </summary>
        public readonly int MemoryRangeIndex;
        /// <summary>
        /// The starting position within the block of memory referred by the MemoryRange at MemoryRangeIndex. With a single memory, this is just an offset into that single block. With MultipleMemoryBlocks, this will be added to the offset of the MemoryRange at MemoryRangeIndex to determine the total offset into the memory block.
        /// </summary>
        public readonly int OffsetIntoMemoryBlock;
        /// <summary>
        /// The total number of bytes in the referenced range, potentially spanning multiple blocks of memory.
        /// </summary>
        public readonly long Length;


        /// <summary>
        /// The number of bytes, as an integer, or null if the number is too large to be stored in an integer.
        /// </summary>
        public int? LengthInt => Length > int.MaxValue ? null : (int)Length;

        public bool IsEmpty => (SingleMemoryBlock == null && MultipleMemoryBlocks == null) || Length == 0;
        public long AllocationID => (SingleMemoryBlock.MemoryAsLoaded as ExpandableBytes)?.AllocationID ?? -1;

        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(EmptyMemory);


        public override string ToString()
        {
            return $@"{(AllocationID != -1 ? $"Allocation {AllocationID} " : "")}Length {Length} Bytes {String.Join(",", EnumerateBytes().Take(2000))}";
        }

        #region Construction

        public LazinatorMemory(MemoryBlock memoryBlock, int startPosition, long length)
        {
            SingleMemoryBlock = memoryBlock;
            MultipleMemoryBlocks = null;
            MemoryRangeIndex = 0;
            if (startPosition < 0)
                throw new ArgumentException();
            OffsetIntoMemoryBlock = startPosition;
            if (length < 0)
                Length = 0;
            else
                Length = length;
        }

        public LazinatorMemory(MemoryBlockCollection memoryBlockCollection)
        {
            MultipleMemoryBlocks = memoryBlockCollection; 
            SingleMemoryBlock = null;
            MemoryRangeIndex = 0;
            OffsetIntoMemoryBlock = 0;
            Length = memoryBlockCollection.Length;
        }

        public LazinatorMemory(MemoryBlockCollection moreMemoryBlocks, int memoryRangeIndex, int startPosition, long length) : this(null, startPosition, length)
        {
            MultipleMemoryBlocks = moreMemoryBlocks;
            MemoryRangeIndex = memoryRangeIndex;
        }

        public LazinatorMemory(IEnumerable<MemoryBlock> moreMemoryBlocks, int memoryRangeIndex, int startPosition, long length) : this(null, startPosition, length)
        {
            MultipleMemoryBlocks = new MemoryBlockCollection(moreMemoryBlocks);
            MemoryRangeIndex = memoryRangeIndex;
        }

        public LazinatorMemory(MemoryBlock memoryBlock, long length) : this(memoryBlock, 0, length)
        {
        }

        public LazinatorMemory(MemoryBlock memoryBlock) : this(memoryBlock, 0, memoryBlock.ReadOnlyMemory.Length)
        {
        }

        public LazinatorMemory(IReadableBytes readOnlyBytes) : this(new MemoryBlock(readOnlyBytes))
        {
        }

        public LazinatorMemory(ReadOnlyBytes readOnlyBytes) : this(new MemoryBlock(readOnlyBytes))
        {
        }

        public LazinatorMemory(ReadOnlyMemory<byte> memory) : this(new MemoryBlock(new ReadOnlyBytes(memory)))
        {
        }

        public LazinatorMemory(byte[] array) : this(new MemoryBlock(new ReadOnlyBytes(array)))
        {
        }

        /// <summary>
        /// Returns a new LazinatorMemory with an appended memory block.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public LazinatorMemory WithAppendedBlock(MemoryBlock block)
        {
            if (IsEmpty)
                return new LazinatorMemory(block);

            var withAppendedBlock = new MemoryRangeCollection();
            withAppendedBlock.SetFromLazinatorMemory(this);
            withAppendedBlock.AppendMemoryBlock(block);
            return new LazinatorMemory(withAppendedBlock, MemoryRangeIndex, OffsetIntoMemoryBlock, Length + block.Length);
        }

        public bool Disposed => EnumerateReadOnlyBytesSegments().Any(x => x != null && (x is IMemoryAllocationInfo info && info.Disposed) || (x is ReadOnlyBytes s && s.Disposed));

        #endregion

        #region Disposal

        public void Dispose()
        {
            SingleMemoryBlock?.Dispose();
            if (MultipleMemoryBlocks != null)
                foreach (var additional in MultipleMemoryBlocks)
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

        private MemoryRange SingleMemoryRange => new MemoryRange(SingleMemoryBlock, new MemoryBlockSlice(0, SingleMemoryBlock.Length));

        public MemoryRange MemoryRangeAtIndex(int i) => MultipleMemoryBlocks == null && i == 0 ? SingleMemoryRange : MultipleMemoryBlocks.MemoryRangeAtIndex(i);

        public async ValueTask<MemoryRange> MemoryRangeAtIndexAsync(int i) => MultipleMemoryBlocks == null && i == 0 ? SingleMemoryRange : await MultipleMemoryBlocks.MemoryRangeAtIndexAsync(i);

        /// <summary>
        /// Slices the first referenced memory block only, producing a new LazinatorMemory.
        /// </summary>
        /// <param name="offset">An offset relative to the existing Offset, which must refer to the initial memory</param>
        /// <param name="length"></param>
        /// <returns></returns>
        private LazinatorMemory SliceSingle(int offset, long length) => length == 0 ? LazinatorMemory.EmptyLazinatorMemory : new LazinatorMemory(SingleMemoryBlock, OffsetIntoMemoryBlock + offset, length);

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

            MemoryRangeByBlockIndex memoryRange = default; // SUPERDEBUG MultipleMemoryBlocks.GetMemoryRangeAtOffsetFromStartPosition(MemoryRangeIndex, OffsetIntoMemoryBlock, furtherOffset);

            return default; // SUPERDEBUG new LazinatorMemory(MultipleMemoryBlocks.DeepCopy(), debug memoryRange.MemoryBlockIndex, memoryRange.OffsetIntoMemoryBlock, length);
        }

        #endregion

        #region Equality

        public override bool Equals(object obj) => obj == null ? throw new LazinatorSerializationException("Invalid comparison of LazinatorMemory to null") :
            obj is LazinatorMemory lm && ( (lm.SingleMemoryBlock != null && lm.SingleMemoryBlock.Equals(SingleMemoryBlock)) || (lm.MultipleMemoryBlocks != null && lm.MultipleMemoryBlocks.Equals(MultipleMemoryBlocks)) ) && lm.OffsetIntoMemoryBlock == OffsetIntoMemoryBlock && lm.Length == Length;

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
        /// True if there is only a single memory block.
        /// </summary>
        public bool SingleMemory => MultipleMemoryBlocks == null;

        /// <summary>
        /// The first referenced memory block
        /// </summary>
        public ReadOnlyMemory<byte> InitialReadOnlyMemory
        {
            get
            {
                if (IsEmpty)
                    return EmptyReadOnlyMemory;
                LoadInitialReadOnlyMemory();
                ReadOnlyMemory<byte> memory = SingleMemoryBlock?.ReadOnlyMemory ?? MemoryRangeAtIndex(MemoryRangeIndex).ReadOnlyMemory;
                int length = (int)Math.Min(memory.Length - OffsetIntoMemoryBlock, Length);
                return memory.Slice(OffsetIntoMemoryBlock, length);
            }
        }


        /// <summary>
        /// Asynchronously returns the first referenced memory block.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<ReadOnlyMemory<byte>> GetInitialReadOnlyMemoryAsync()
        {
            if (IsEmpty)
                return EmptyMemory;
            await LoadInitialReadOnlyMemoryAsync();
            ReadOnlyMemory<byte> memory = SingleMemoryBlock?.ReadOnlyMemory ?? MemoryRangeAtIndex(MemoryRangeIndex).ReadOnlyMemory;
            return memory.Slice(OffsetIntoMemoryBlock, memory.Length - OffsetIntoMemoryBlock);
        }

        /// <summary>
        /// The only memory block. This will throw if there are multiple memory blocks.
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
        /// Loads the first referenced memory block synchronously if it is not loaded.
        /// </summary>
        /// <returns></returns>
        public void LoadInitialReadOnlyMemory()
        {
            if (SingleMemory)
            {
                LoadMemoryBlock(SingleMemoryBlock);
                return;
            }
            MemoryRange memoryRange = MemoryRangeAtIndex(MemoryRangeIndex);
            LoadMemoryBlock(memoryRange.MemoryBlock);
        }

        public async ValueTask LoadInitialReadOnlyMemoryAsync()
        {
            if (SingleMemory)
            {
                await LoadMemoryBlockAsync(SingleMemoryBlock);
                return;
            }
            MemoryRange memorySegment = MemoryRangeAtIndex(MemoryRangeIndex);
            await LoadMemoryBlockAsync(memorySegment.MemoryBlock);
        }

        private static void LoadMemoryBlock(MemoryBlock memoryBlock)
        {
            if (memoryBlock.IsLoaded == false)
            {
                memoryBlock.LoadMemory();
            }
        }

        private static async ValueTask LoadMemoryBlockAsync(MemoryBlock memoryBlock)
        {
            if (memoryBlock.IsLoaded == false)
            {
                await memoryBlock.LoadMemoryAsync();
            }
        }

        /// <summary>
        /// Loads all memory.
        /// </summary>
        public void LoadAllMemory()
        {
            LoadInitialReadOnlyMemory();
            if (MultipleMemoryBlocks != null)
                foreach (var additional in MultipleMemoryBlocks)
                    LoadMemoryBlock(additional.MemoryBlock);
        }

        public async ValueTask LoadAllMemoryAsync()
        {
            await LoadInitialReadOnlyMemoryAsync();
            if (MultipleMemoryBlocks != null)
                foreach (var additional in MultipleMemoryBlocks)
                    await LoadMemoryBlockAsync(additional.MemoryBlock);
        }

        /// <summary>
        /// Allows for unloading the first referenced memory block, if it is loaded. The memory can be unloaded only if the owner of the first memory
        /// block is a MemoryReference that supports this functionality.
        /// </summary>
        public void ConsiderUnloadInitialReadOnlyMemory()
        {
            if (SingleMemory)
                return;
            MemoryRange memorySegment = MemoryRangeAtIndex(MemoryRangeIndex);
            if (memorySegment.MemoryBlock.IsLoaded == true)
            {
                memorySegment.MemoryBlock.ConsiderUnloadMemory();
            }
        }

        /// <summary>
        /// Allows for asynchronously unloading the first referenced memory block, if it is loaded. The memory can be unloaded only if the owner of the first memory
        /// block is a MemoryReference that supports this functionality.
        /// </summary>
        /// <returns></returns>
        public async ValueTask ConsiderUnloadReadOnlyMemoryAsync()
        {
            if (SingleMemory)
                return;
            MemoryRange memorySegment = MemoryRangeAtIndex(MemoryRangeIndex);
            if (memorySegment.MemoryBlock.IsLoaded == true)
                await memorySegment.MemoryBlock.ConsiderUnloadMemoryAsync();
        }

        #endregion

        #region Multiple memory management

        internal MemoryBlockID GetNextMemoryBlockID()
        {
            if (IsEmpty)
                return new MemoryBlockID(0);
            MemoryBlockID maxMemoryBlockID = SingleMemoryBlock == null ? MultipleMemoryBlocks.HighestMemoryBlockID : SingleMemoryBlock.MemoryBlockID; // not always the ID of the last block, because patching may reassemble into a different order. We are guaranteed, however, that if we're doing versioning, the most recent memory block ID will be included.
            return maxMemoryBlockID.Next();
        }

        /// <summary>
        /// Enumerates references to memory ranges in this LazinatorMemory, by block ID.
        /// </summary>
        /// <returns>An enumerable where each element refers to a particular memory range.</returns>
        public IEnumerable<MemoryRangeByBlockIndex> EnumerateMemoryRangesByBlockIndex()
        {
            if (SingleMemoryBlock != null)
            {
                yield return new MemoryRangeByBlockIndex(0, OffsetIntoMemoryBlock, (int)Length);
            }
            else 
                foreach (MemoryRangeByBlockIndex indexAndSlice in MultipleMemoryBlocks.EnumerateMemoryRangesByBlockIndex(MemoryRangeIndex, OffsetIntoMemoryBlock, Length))
                    yield return indexAndSlice;
        }

        /// <summary>
        /// Enumerates memory ranges corresponding to this LazinatorMemory, with reference by block ID.
        /// </summary>
        /// <returns>An enumerable where each element consists of the memory block ID, the start position, and the number of bytes</returns>
        public IEnumerable<MemoryRangeByBlockID> EnumerateMemoryRangesByID()
        {
            if (SingleMemoryBlock != null)
            {
                yield return new MemoryRangeByBlockID(new MemoryBlockID(0), OffsetIntoMemoryBlock, (int)Length);
            }
            else
                foreach (MemoryRangeByBlockID idAndSlice in MultipleMemoryBlocks.EnumerateMemoryRangesByBlockID(MemoryRangeIndex, OffsetIntoMemoryBlock, Length))
                    yield return idAndSlice;
        }

        /// <summary>
        /// Enumerates memory segments.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryRange> EnumerateMemorySegments()
        {
            if (SingleMemoryBlock != null)
            {
                yield return SingleMemoryRange.Slice(OffsetIntoMemoryBlock, (int) Length);
            }
            else
                foreach (MemoryRange memorySegment in MultipleMemoryBlocks.EnumerateMemoryRanges(MemoryRangeIndex, OffsetIntoMemoryBlock, Length))
                    yield return memorySegment;
        }

        /// <summary>
        /// Enumerates the memory segments as ReadOnlyMemory<byte>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReadOnlyMemory<byte>> EnumerateReadOnlyMemory()
        {
            foreach (MemoryRange memorySegment in EnumerateMemorySegments())
                yield return memorySegment.ReadOnlyMemory;
        }

        /// <summary>
        /// Enumerates all memory blocks asynchronously, asynchronously loading memory if needed.
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<ReadOnlyMemory<byte>> EnumerateReadOnlyMemoryAsync()
        {
            foreach (var memoryBlockIndexAndSlice in EnumerateMemoryRangesByBlockIndex())
            {
                var memorySegment = await MemoryRangeAtIndexAsync(memoryBlockIndexAndSlice.MemoryBlockIndex);
                yield return memorySegment.ReadOnlyMemory;
            }
        }

        /// <summary>
        /// Enumerates each of the memory blocks, whether included within the referenced range or not.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryBlock> EnumerateMemoryBlocks()
        {
            if (SingleMemoryBlock != null)
                yield return SingleMemoryBlock;
            else if (MultipleMemoryBlocks != null)
                foreach (var additional in MultipleMemoryBlocks.EnumerateMemoryBlocks())
                    yield return additional;
        }

        /// <summary>
        /// Enumerates the referenced memory owners, whether included within the referenced range or not.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IReadableBytes> EnumerateReadOnlyBytesSegments()
        {
            foreach (var memoryBlock in EnumerateMemoryBlocks())
                yield return memoryBlock.MemoryAsLoaded;
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
        /// Returns a single memory block consolidating all of the memory from the LazinatorMemory. If there is only a single memory block, 
        /// then the memory is not copied.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyMemory<byte> GetConsolidatedMemory()
        {
            if (SingleMemory)
            {
                SingleMemoryBlock.LoadMemory();
                return SingleMemoryBlock.ReadOnlyMemory.Slice(OffsetIntoMemoryBlock, (int) Length);
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
                await SingleMemoryBlock.LoadMemoryAsync();
                return SingleMemoryBlock.ReadOnlyMemory.Slice(OffsetIntoMemoryBlock, (int)Length);
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
        public string ToStringByBlock()
        {
            var blocks = EnumerateMemoryBlocks().ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var block in blocks)
            {
                sb.AppendLine(block.ToString());
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
