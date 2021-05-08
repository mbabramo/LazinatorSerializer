using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemorySegment
    {
        public readonly MemoryChunk MemoryChunk { get; private init; }
        public readonly MemoryBlockSlice SliceInfo { get; private init; }

        public MemorySegment(MemoryChunk memoryChunk, MemoryBlockSlice sliceInfo)
        {
            MemoryChunk = memoryChunk;
            SliceInfo = sliceInfo;
        }
    }
}
