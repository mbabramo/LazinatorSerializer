using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemorySegmentIDAndSlice
    {
        /// <summary>
        /// The MemoryBlockID, used to find the corresponding MemoryChunk in a MemoryChunkCollection.
        /// </summary>
        public readonly int MemoryBlockID { get; }
        /// <summary>
        /// The offset into the MemoryChunk (not into the underlying MemoryBlock).
        /// </summary>
        public readonly int OffsetIntoMemoryChunk { get; }
        /// <summary>
        /// The length of information in the MemoryChunk.
        /// </summary>
        public readonly int Length { get; }

        public MemorySegmentIDAndSlice(int memoryBlockID, int offsetIntoMemoryChunk, int length)
        {
            this.MemoryBlockID = memoryBlockID;
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }

        public MemorySegmentIDAndSlice Slice(int offset, int length) => new MemorySegmentIDAndSlice(MemoryBlockID, OffsetIntoMemoryChunk + offset, length);

        public MemorySegmentIDAndSlice Slice(int offset) => new MemorySegmentIDAndSlice(MemoryBlockID, OffsetIntoMemoryChunk + offset, Length - offset);

        public MemoryChunkSlice GetSlice() => new MemoryChunkSlice(OffsetIntoMemoryChunk, Length);
    }
}
