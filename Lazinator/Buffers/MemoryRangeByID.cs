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
        /// The MemoryBlockID, used to find the corresponding MemoryBlock in a MemoryBlockCollection.
        /// </summary>
        public readonly int MemoryBlockIntID { get; }
        /// <summary>
        /// The offset into the MemoryBlock (not into the underlying MemoryBlock).
        /// </summary>
        public readonly int OffsetIntoMemoryBlock { get; }
        /// <summary>
        /// The length of information in the MemoryBlock.
        /// </summary>
        public readonly int Length { get; }

        public override string ToString()
        {
            return $"{MemoryBlockIntID}, {OffsetIntoMemoryBlock}, {Length}";
        }

        public MemoryBlockID GetMemoryBlockID() => new MemoryBlockID(MemoryBlockIntID);

        public MemoryRangeByID(int memoryBlockIntID, int offsetIntoMemoryBlock, int length)
        {
            if (memoryBlockIntID == 2)
            {
                var DEBUG = 0;
            }
            this.MemoryBlockIntID = memoryBlockIntID;
            this.OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            this.Length = length;
        }

        public MemoryRangeByID(MemoryBlockID memoryBlockID, int offsetIntoMemoryBlock, int length)
        {
            this.MemoryBlockIntID = memoryBlockID.GetIntID();
            this.OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            this.Length = length;
        }

        public MemoryRangeByID SubsegmentSlice(int offset, int length) => new MemoryRangeByID(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryBlock + offset, length);

        public MemoryRangeByID SubsegmentSlice(int offset) => new MemoryRangeByID(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryBlock + offset, Length - offset);

        public MemoryBlockSlice GetBlockSlice() => new MemoryBlockSlice(OffsetIntoMemoryBlock, Length);
    }
}
