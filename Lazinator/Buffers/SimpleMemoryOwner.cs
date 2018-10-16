using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A memory owner that involves no memory pooling. When this is disposed, the memory will eventually be reclaimed due to garbage collection, which is usually less efficient than memory pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SimpleMemoryOwner<T> : IMemoryOwner<T>
    {
        private Memory<T> _Memory;
        public Memory<T> Memory { get => Disposed ? throw new ObjectDisposedException("SimpleMemoryOwner") : _Memory; set => _Memory = value; }
        public bool Disposed { get; set; }

        public SimpleMemoryOwner(Memory<T> memory)
        {
            _Memory = memory;
            Disposed = false;
        }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}
