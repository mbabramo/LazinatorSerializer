using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryRange
    {
        public readonly MemoryBlock MemoryBlock { get; private init; }
        public readonly MemoryBlockSlice SliceInfo { get; private init; }

        public MemoryRange(MemoryBlock memoryBlock, MemoryBlockSlice sliceInfo)
        {
            MemoryBlock = memoryBlock;
            SliceInfo = sliceInfo;
        }

        public readonly MemoryRange Slice(int furtherOffset, int length) => new MemoryRange(MemoryBlock, SliceInfo.Slice(furtherOffset, length));

        public readonly Memory<byte> Memory => MemoryBlock.ReadWriteMemory.Slice(SliceInfo.OffsetIntoMemoryBlock, SliceInfo.Length);
        public readonly ReadOnlyMemory<byte> ReadOnlyMemory => MemoryBlock.ReadOnlyMemory.Slice(SliceInfo.OffsetIntoMemoryBlock, SliceInfo.Length);
        
        public string ToMemoryString() => String.Join(",", ReadOnlyMemory.ToArray().Select(x => x.ToString().PadLeft(3, '0')).ToArray());
        public int Length => SliceInfo.Length;
        public void Dispose()
        {
            MemoryBlock?.Dispose();
        }
    }
}
