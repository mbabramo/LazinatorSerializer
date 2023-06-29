using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryRange
    {
        public readonly MemoryChunk MemoryChunk { get; private init; }
        public readonly MemoryChunkSlice SliceInfo { get; private init; }

        public MemoryRange(MemoryChunk memoryChunk, MemoryChunkSlice sliceInfo)
        {
            MemoryChunk = memoryChunk;
            SliceInfo = sliceInfo;
        }

        public readonly MemoryRange Slice(int furtherOffset, int length) => new MemoryRange(MemoryChunk, SliceInfo.Slice(furtherOffset, length));

        public readonly Memory<byte> Memory => MemoryChunk.ReadWriteMemory.Slice(SliceInfo.OffsetIntoMemoryChunk, SliceInfo.Length);
        public readonly ReadOnlyMemory<byte> ReadOnlyMemory => MemoryChunk.ReadOnlyMemory.Slice(SliceInfo.OffsetIntoMemoryChunk, SliceInfo.Length);
        public int Length => SliceInfo.Length;
        public void Dispose()
        {
            MemoryChunk?.Dispose();
        }
    }
}
