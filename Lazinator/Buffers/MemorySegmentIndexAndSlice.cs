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
        public readonly int Offset { get; }
        public readonly int Length { get; }

        public MemorySegmentIndexAndSlice(int memoryBlockIndex, int offset, int length)
        {
            this.MemorySegmentIndex = memoryBlockIndex;
            this.Offset = offset;
            this.Length = length;
        }
        public MemorySegmentIndexAndSlice Slice(int offset, int length) => new MemorySegmentIndexAndSlice(MemorySegmentIndex, Offset + offset, length);

        public MemorySegmentIndexAndSlice Slice(int offset) => new MemorySegmentIndexAndSlice(MemorySegmentIndex, Offset + offset, Length - offset);
        public MemoryBlockSlice GetSlice() => new MemoryBlockSlice(Offset, Length);
    }
}
