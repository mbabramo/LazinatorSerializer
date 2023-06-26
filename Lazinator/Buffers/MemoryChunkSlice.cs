using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryChunkSlice
    {
        public readonly int OffsetIntoMemoryChunk { get; }
        public readonly int Length { get; }

        public MemoryChunkSlice(int offsetIntoMemoryChunk, int length)
        {
            OffsetIntoMemoryChunk = offsetIntoMemoryChunk;
            Length = length;
        }

        public MemoryChunkSlice Slice(int furtherOffset, int finalLength)
        {
            return new MemoryChunkSlice(OffsetIntoMemoryChunk + furtherOffset, finalLength);
        }

        public MemoryChunkSlice Slice(int furtherOffset)
        {
            return new MemoryChunkSlice(OffsetIntoMemoryChunk + furtherOffset, Length - furtherOffset);
        }

        public override string ToString()
        {
            return $"Offset: {OffsetIntoMemoryChunk}; Length: {Length}";
        }
    }
}
