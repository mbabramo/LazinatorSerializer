using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryRangeByBlockIndex
    {
        public readonly int MemoryBlockIndex { get; }
        public readonly int OffsetIntoMemoryBlock { get; }
        public readonly int Length { get; }

        public MemoryRangeByBlockIndex(int memoryBlockIndex, int offsetIntoMemoryBlock, int length)
        {
            this.MemoryBlockIndex = memoryBlockIndex;
            this.OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            this.Length = length;
        }
        public MemoryRangeByBlockIndex SubsegmentSlice(int offset, int length) => new MemoryRangeByBlockIndex(MemoryBlockIndex, OffsetIntoMemoryBlock + offset, length);

        public MemoryRangeByBlockIndex SubsegmentSlice(int offset) => new MemoryRangeByBlockIndex(MemoryBlockIndex, OffsetIntoMemoryBlock + offset, Length - offset);
        public MemoryBlockSlice GetBlockSlice() => new MemoryBlockSlice(OffsetIntoMemoryBlock, Length);
    }
}
