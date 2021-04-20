using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A memory owner that involves no memory pooling. When this is disposed, the memory will eventually be reclaimed due to garbage collection, which is usually less efficient than memory pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadWriteBytes : IMemoryOwner<byte>
    {
        private Memory<byte> _Memory;
        public Memory<byte> Memory { get => Disposed ? throw new ObjectDisposedException("SimpleMemoryOwner") : _Memory; set => _Memory = value; }
        public bool Disposed { get; set; }

        public ReadWriteBytes(Memory<byte> memory)
        {
            _Memory = memory;
            Disposed = false;
        }

        public void Dispose()
        {
            Disposed = true;
            // Note: This doesn't actually dispose of the underlying memory, since Memory<T> does not define Dispose. In general, you should not manually call Dispose() on SimpleMemoryOwner. The memory will be automatically garbage collected.
        }
    }
}
