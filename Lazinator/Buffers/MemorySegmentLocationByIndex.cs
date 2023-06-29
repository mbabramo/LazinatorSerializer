using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemorySegmentLocationByIndex
    {
        // DEBUG5 // maybe this should be MemoryChunkIndex, because when we write into completed memory, we need to be referring to a MemoryChunkIndex.
        public readonly int MemorySegmentIndex { get; }
        public readonly int OffsetIntoMemoryChunk { get; }
        public readonly int Length { get; }

        public MemorySegmentLocationByIndex(int memoryBlockIndex, int offsetIntoMemoryChunk, int length)
        {
            this.MemorySegmentIndex = memoryBlockIndex;
            this.OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            this.Length = length;
        }
        public MemorySegmentLocationByIndex SubsegmentSlice(int offset, int length) => new MemorySegmentLocationByIndex(MemorySegmentIndex, OffsetIntoMemoryChunk + offset, length);

        public MemorySegmentLocationByIndex SubsegmentSlice(int offset) => new MemorySegmentLocationByIndex(MemorySegmentIndex, OffsetIntoMemoryChunk + offset, Length - offset);
        public MemoryChunkSlice GetChunkSlice() => new MemoryChunkSlice(OffsetIntoMemoryChunk, Length);
    }
}
