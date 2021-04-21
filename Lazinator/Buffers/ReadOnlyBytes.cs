using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A memory owner that involves no memory pooling. When this is disposed, the memory will eventually be reclaimed due to garbage collection, which is usually less efficient than memory pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadOnlyBytes : IReadableBytes, IMemoryAllocationInfo
    {
        private ReadOnlyMemory<byte> _Memory;
        public ReadOnlyMemory<byte> ReadOnlyMemory { get => Disposed || (MemoryOwner is IMemoryAllocationInfo info && info.Disposed) ? throw new ObjectDisposedException("ReadOnlyBytes") : _Memory; set => _Memory = value; }
        public bool Disposed { get; set; }
        public IMemoryOwner<byte> MemoryOwner { get; set; }
        public long AllocationID => MemoryOwner is IMemoryAllocationInfo info ? info.AllocationID : -1;

        public ReadOnlyBytes(ReadOnlyMemory<byte> memory, IMemoryOwner<byte> memoryOwner = null)
        {
            _Memory = memory;
            Disposed = false;
            MemoryOwner = memoryOwner;
        }

        public void Dispose()
        {
            Disposed = true;
            MemoryOwner?.Dispose();
        }
    }
}
