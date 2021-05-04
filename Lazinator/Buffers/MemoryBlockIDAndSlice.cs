using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockIDAndSlice
    {
        public readonly int MemoryBlockID { get; }
        public readonly int Offset { get; }
        public readonly int Length { get; }

        public MemoryBlockIDAndSlice(int memoryBlockID, int offset, int length)
        {
            this.MemoryBlockID = memoryBlockID;
            this.Offset = offset;
            this.Length = length;
        }

        public MemoryBlockIDAndSlice Slice(int offset, int length) => new MemoryBlockIDAndSlice(MemoryBlockID, Offset + offset, length);

        public MemoryBlockIDAndSlice Slice(int offset) => new MemoryBlockIDAndSlice(MemoryBlockID, Offset + offset, Length - offset);
    }
}
