using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryRangeByIndex
    {
        public readonly int MemoryBlockIndex { get; }
        public readonly int OffsetIntoMemoryBlock { get; }
        public readonly int Length { get; }

        public MemoryRangeByIndex(int memoryBlockIndex, int offsetIntoMemoryBlock, int length)
        {
            this.MemoryBlockIndex = memoryBlockIndex;
            this.OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            this.Length = length;
        }
        public MemoryRangeByIndex SubsegmentSlice(int offset, int length) => new MemoryRangeByIndex(MemoryBlockIndex, OffsetIntoMemoryBlock + offset, length);

        public MemoryRangeByIndex SubsegmentSlice(int offset) => new MemoryRangeByIndex(MemoryBlockIndex, OffsetIntoMemoryBlock + offset, Length - offset);
        public MemoryBlockSlice GetBlockSlice() => new MemoryBlockSlice(OffsetIntoMemoryBlock, Length);
    }
}
