using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryChunkSlice
    {
        public readonly int Offset;
        public readonly int Length;

        public MemoryChunkSlice(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }
    }
}
