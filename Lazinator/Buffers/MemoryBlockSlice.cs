using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockSlice
    {
        public readonly int OffsetIntoMemoryBlock { get; }
        public readonly int Length { get; }

        public MemoryBlockSlice(int offsetIntoMemoryBlock, int length)
        {
            OffsetIntoMemoryBlock = offsetIntoMemoryBlock;
            Length = length;
        }

        public MemoryBlockSlice Slice(int furtherOffset, int finalLength)
        {
            return new MemoryBlockSlice(OffsetIntoMemoryBlock + furtherOffset, finalLength);
        }

        public MemoryBlockSlice Slice(int furtherOffset)
        {
            return new MemoryBlockSlice(OffsetIntoMemoryBlock + furtherOffset, Length - furtherOffset);
        }

        public override string ToString()
        {
            return $"Offset: {OffsetIntoMemoryBlock}; Length: {Length}";
        }
    }
}
