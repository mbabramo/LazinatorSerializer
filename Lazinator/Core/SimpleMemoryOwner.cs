using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    public struct SimpleMemoryOwner<T> : IMemoryOwner<T>
    {
        public Memory<T> Memory { get; set; }

        public SimpleMemoryOwner(Memory<T> memory)
        {
            Memory = memory;
        }

        public void Dispose()
        {
        }
    }
}
