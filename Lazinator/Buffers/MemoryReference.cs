using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MemoryReference : IMemoryOwner<byte>
    {
        public IMemoryOwner<byte> ReferencedMemory { get; set; }

        public int VersionOfReferencedMemory { get; set; }

        public int StartIndex;

        public int Length;

        public Memory<byte> Memory => ReferencedMemory.Memory.Slice(StartIndex, Length);

        public MemoryReference Slice(int startPosition, int length) => new MemoryReference()
        {
            ReferencedMemory = ReferencedMemory,
            VersionOfReferencedMemory = VersionOfReferencedMemory,
            StartIndex = StartIndex + startPosition,
            Length = length
        };

        public void Dispose()
        {
        }
    }
}
