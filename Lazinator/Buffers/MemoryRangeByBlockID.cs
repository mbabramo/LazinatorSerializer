﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryRangeByBlockID
    {
        /// <summary>
        /// The MemoryBlockID, used to find the corresponding MemoryBlock in a MemoryBlockCollection.
        /// </summary>
        public readonly MemoryBlockID MemoryBlockID { get; }
        /// <summary>
        /// The offset into the MemoryBlock (not into the underlying MemoryBlock).
        /// </summary>
        public readonly int OffsetIntoMemoryBlock { get; }
        /// <summary>
        /// The length of the range, which may be smaller than the remaining length of the memory block.
        /// </summary>
        public readonly int Length { get; }

        public override string ToString()
        {
            return $"{MemoryBlockID}, {OffsetIntoMemoryBlock}, {Length}";
        }

        public int MemoryBlockIntID => MemoryBlockID.AsInt;

        // DEBUG -- delete?
        public MemoryRangeByBlockID(int memoryBlockIntID, int offsetIntoMemoryBlock, int length)
        {
            this.MemoryBlockID = new MemoryBlockID(memoryBlockIntID);
            this.OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            this.Length = length;
        }

        public MemoryRangeByBlockID(MemoryBlockID memoryBlockID, int offsetIntoMemoryBlock, int length)
        {
            this.MemoryBlockID = memoryBlockID;
            this.OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            this.Length = length;
        }

        public MemoryRangeByBlockID SubsegmentSlice(int offset, int length) => new MemoryRangeByBlockID(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryBlock + offset, length);

        public MemoryRangeByBlockID SubsegmentSlice(int offset) => new MemoryRangeByBlockID(new MemoryBlockID(MemoryBlockIntID), OffsetIntoMemoryBlock + offset, Length - offset);

        public MemoryBlockSlice GetBlockSlice() => new MemoryBlockSlice(OffsetIntoMemoryBlock, Length);
    }
}
