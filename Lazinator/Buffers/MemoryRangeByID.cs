using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryRangeByID
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

        public MemoryRangeByID(int memoryBlockIntID, int offsetIntoMemoryChunk, int length)
        {
            if (memoryBlockIntID == 2)
            {
                var DEBUG = 0;
            }
            this.MemoryBlockIntID = memoryBlockIntID;
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }

        public MemoryRangeByID(MemoryBlockID memoryBlockID, int offsetIntoMemoryChunk, int length)
        {
            this.MemoryBlockIntID = memoryBlockID.GetIntID();
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }

        public MemoryRangeByID SubsegmentSlice(int offset, int length) => new MemoryRangeByID(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryChunk + offset, length);

        public MemoryRangeByID SubsegmentSlice(int offset) => new MemoryRangeByID(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryChunk + offset, Length - offset);

        public MemoryChunkSlice GetChunkSlice() => new MemoryChunkSlice(OffsetIntoMemoryChunk, Length);
    }
}
