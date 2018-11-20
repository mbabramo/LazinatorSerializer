using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;

namespace Lazinator.Buffers
{
    public abstract class TrackedMemory : IMemoryOwner<byte>
    {
        public static long NextAllocationID = 0; // we track all allocations to facilitate debugging of memory allocation and disposal
        public long AllocationID;

        public override string ToString()
        {
            return $@"Allocation {AllocationID} Length {Memory.Length} Bytes {String.Join(",", Memory.Span.Slice(0, Math.Min(Memory.Span.Length, 100)).ToArray())}";
        }

        public TrackedMemory()
        {
            unchecked
            {
                AllocationID = Interlocked.Increment(ref NextAllocationID) - 1;
            }
        }

        public bool Disposed { get; protected internal set; }

        public abstract Memory<byte> Memory { get; }


        #region Memory management
        
        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public virtual void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
            }
        }

        #endregion
    }
}
