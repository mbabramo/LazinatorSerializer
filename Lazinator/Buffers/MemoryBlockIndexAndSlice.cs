using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockIndexAndSlice
    {
        public readonly int MemoryBlockIndex { get; }
        public readonly int Offset { get; }
        public readonly int Length { get; }

        public MemoryBlockIndexAndSlice(int memoryBlockIndex, int offset, int length)
        {
            this.MemoryBlockIndex = memoryBlockIndex;
            this.Offset = offset;
            this.Length = length;
        }
        public MemoryBlockIndexAndSlice Slice(int offset, int length) => new MemoryBlockIndexAndSlice(MemoryBlockIndex, Offset + offset, length);

        public MemoryBlockIndexAndSlice Slice(int offset) => new MemoryBlockIndexAndSlice(MemoryBlockIndex, Offset + offset, Length - offset);
        public MemoryBlockSlice GetSlice() => new MemoryBlockSlice(Offset, Length);
    }
}
