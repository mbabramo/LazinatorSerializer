using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockSlice
    {
        public readonly int Offset;
        public readonly int Length;

        public MemoryBlockSlice(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public MemoryBlockSlice Slice(int furtherOffset, int finalLength)
        {
            return new MemoryBlockSlice(Offset + furtherOffset, finalLength);
        }
    }
}
