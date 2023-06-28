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
        public readonly int MemoryBlockIntID { get; }
        /// <summary>
        /// The offset into the MemoryChunk (not into the underlying MemoryBlock).
        /// </summary>
        public readonly int OffsetIntoMemoryChunk { get; }
        /// <summary>
        /// The length of information in the MemoryChunk.
        /// </summary>
        public readonly int Length { get; }

        public override string ToString()
        {
            return $"{MemoryBlockIntID}, {OffsetIntoMemoryChunk}, {Length}";
        }

        public MemoryBlockID GetMemoryBlockID() => new MemoryBlockID(MemoryBlockIntID);

        public MemorySegmentIDAndSlice(int memoryBlockIntID, int offsetIntoMemoryChunk, int length)
        {
            if (memoryBlockIntID == 2)
            {
                var DEBUG = 0;
            }
            this.MemoryBlockIntID = memoryBlockIntID;
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }

        public MemorySegmentIDAndSlice(MemoryBlockID memoryBlockID, int offsetIntoMemoryChunk, int length)
        {
            this.MemoryBlockIntID = memoryBlockID.GetIntID();
            if (MemoryBlockIntID == 2)
            {
                var DEBUG = 0;
            }
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }

        public MemorySegmentIDAndSlice Slice(int offset, int length) => new MemorySegmentIDAndSlice(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryChunk + offset, length);

        public MemorySegmentIDAndSlice Slice(int offset) => new MemorySegmentIDAndSlice(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryChunk + offset, Length - offset);

        public MemoryChunkSlice GetSlice() => new MemoryChunkSlice(OffsetIntoMemoryChunk, Length);
    }
}
