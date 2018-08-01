using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A memory owner that involves no memory pooling. When this is disposed, the memory will eventually be reclaimed due to garbage collection, which is usually less efficient than memory pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
