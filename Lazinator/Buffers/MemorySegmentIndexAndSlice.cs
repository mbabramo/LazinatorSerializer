using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemorySegmentIndexAndSlice
    {
        public readonly int MemorySegmentIndex { get; }
        public readonly int OffsetIntoMemoryChunk { get; }
        public readonly int Length { get; }

        public MemorySegmentIndexAndSlice(int memoryBlockIndex, int offsetIntoMemoryChunk, int length)
        {
            this.MemorySegmentIndex = memoryBlockIndex;
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }
        public MemorySegmentIndexAndSlice Slice(int offset, int length) => new MemorySegmentIndexAndSlice(MemorySegmentIndex, OffsetIntoMemoryChunk + offset, length);

        public MemorySegmentIndexAndSlice Slice(int offset) => new MemorySegmentIndexAndSlice(MemorySegmentIndex, OffsetIntoMemoryChunk + offset, Length - offset);
        public MemoryChunkSlice GetSlice() => new MemoryChunkSlice(OffsetIntoMemoryChunk, Length);
    }
}
