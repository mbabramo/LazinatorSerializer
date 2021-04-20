using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A memory owner that involves no memory pooling. When this is disposed, the memory will eventually be reclaimed due to garbage collection, which is usually less efficient than memory pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadOnlyBytes : IReadOnlyBytes
    {
        private ReadOnlyMemory<byte> _Memory;
        public ReadOnlyMemory<byte> ReadOnlyMemory { get => Disposed ? throw new ObjectDisposedException("ReadOnlyBytes") : _Memory; set => _Memory = value; }
        public bool Disposed { get; set; }

        public ReadOnlyBytes(ReadOnlyMemory<byte> memory)
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
