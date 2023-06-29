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
        public readonly int OffsetIntoMemoryChunk { get; }
        public readonly int Length { get; }

        public MemoryRangeByIndex(int memoryBlockIndex, int offsetIntoMemoryChunk, int length)
        {
            this.MemoryBlockIndex = memoryBlockIndex;
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }
        public MemoryRangeByIndex SubsegmentSlice(int offset, int length) => new MemoryRangeByIndex(MemoryBlockIndex, OffsetIntoMemoryChunk + offset, length);

        public MemoryRangeByIndex SubsegmentSlice(int offset) => new MemoryRangeByIndex(MemoryBlockIndex, OffsetIntoMemoryChunk + offset, Length - offset);
        public MemoryChunkSlice GetChunkSlice() => new MemoryChunkSlice(OffsetIntoMemoryChunk, Length);
    }
}
