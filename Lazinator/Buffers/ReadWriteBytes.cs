using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A memory owner that involves no memory pooling. When this is disposed, the memory will eventually be reclaimed due to garbage collection, which is usually less efficient than memory pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadWriteBytes : IMemoryOwner<byte>, IReadableBytes
    {
        private Memory<byte> _Memory;
        public Memory<byte> Memory { get => Disposed ? throw new ObjectDisposedException("ReadWriteBytes") : _Memory; set => _Memory = value; }
        public bool Disposed { get; set; }
        public long AllocationID => MemoryOwner is ExpandableBytes e ? e.AllocationID : -1;
        public IMemoryOwner<byte> MemoryOwner { get; set; }

        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public ReadWriteBytes(Memory<byte> memory, IMemoryOwner<byte> memoryOwner = null)
        {
            _Memory = memory;
            Disposed = false;
            MemoryOwner = memoryOwner;
        }

        public ReadOnlyBytes ToReadOnlyBytes() => new ReadOnlyBytes(Memory);

        public void Dispose()
        {
            Disposed = true;
            MemoryOwner?.Dispose();
        }
    }
}
