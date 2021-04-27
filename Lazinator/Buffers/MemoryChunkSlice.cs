using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryChunkSlice
    {
        public readonly int AdditionalOffset;
        public readonly int FinalLength;

        public MemoryChunkSlice(int additionalOffset, int finalLength)
        {
            AdditionalOffset = additionalOffset;
            FinalLength = finalLength;
        }
    }
}
