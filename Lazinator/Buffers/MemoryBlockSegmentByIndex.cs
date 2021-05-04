using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockSegmentByIndex
    {
        public readonly int MemoryBlockIndex { get; }
        public readonly int Offset { get; }
        public readonly int Length { get; }

        public MemoryBlockSegmentByIndex(int memoryBlockIndex, int offset, int length)
        {
            this.MemoryBlockIndex = memoryBlockIndex;
            this.Offset = offset;
            this.Length = length;
        }
    }
}
