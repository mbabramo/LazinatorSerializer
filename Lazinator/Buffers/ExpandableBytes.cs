﻿using Lazinator.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This memory owner rents memory and later returns it and rents more. If, when writing, it realizes that it needs more, then it 
    /// rents a bigger buffer and copies the existing buffer into it and returns the original. 
    /// </summary>
    public class ExpandableBytes : IMemoryOwner<byte>, IMemoryAllocationInfo
    {
        public const int DefaultMinBufferSize = 1024; 
        public bool LazinatorShouldNotReturnToPool;
        public IMemoryOwner<byte> CurrentBuffer { get; set; }
        public int UsedBytesInCurrentBuffer { get; set; }
        public Memory<byte> Memory => CurrentBuffer.Memory.Slice(0, UsedBytesInCurrentBuffer);
        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public ReadOnlyBytes ReadOnlyBytes => new ReadOnlyBytes(ReadOnlyMemory, this);
        public ReadWriteBytes ReadWriteBytes => new ReadWriteBytes(Memory, this);

        public bool Disposed { get; protected internal set; }
        public static long NextAllocationID = 0; // we track all allocations to facilitate debugging of memory allocation and disposal
        public long AllocationID { get; private set; }

        public static bool UseMemoryPooling = true;
        public static bool TrackMemoryAllocations = false;
        public static List<WeakReference<IMemoryOwner<byte>>> MemoryAllocations = new List<WeakReference<IMemoryOwner<byte>>>();
        public static List<bool> MemoryAllocationsManuallyReturned = new List<bool>();
        public static HashSet<long> NotReturnedByLazinatorHashSet = new HashSet<long>();

        public override string ToString()
        {
            return $@"Allocation {AllocationID} Length {Memory.Length} Bytes {String.Join(",", Memory.Span.Slice(0, Math.Min(Memory.Span.Length, 100)).ToArray())}";
        }

        public ExpandableBytes() : this(DefaultMinBufferSize)
        {
        }

        public ExpandableBytes(int minBufferSize)
        {
            int minimumSize = Math.Max(minBufferSize, DefaultMinBufferSize);
            if (UseMemoryPooling)
            {
                CurrentBuffer = LazinatorUtilities.GetRentedMemory(minimumSize);
            }
            else
                CurrentBuffer = new ReadWriteBytes(new Memory<byte>(new byte[minimumSize]));

            unchecked
            {
                AllocationID = Interlocked.Increment(ref NextAllocationID) - 1;
            }
            if (TrackMemoryAllocations)
            {
                MemoryAllocations.Add(new WeakReference<IMemoryOwner<byte>>(CurrentBuffer));
                MemoryAllocationsManuallyReturned.Add(false);
            }
        }

        public void EnsureMinBufferSize(int desiredBufferSize = 0)
        {
            if (desiredBufferSize <= 0)
            {
                desiredBufferSize = CurrentBuffer.Memory.Length * 2;
                if (desiredBufferSize < DefaultMinBufferSize)
                    desiredBufferSize = DefaultMinBufferSize;
            }
            else if (CurrentBuffer.Memory.Length >= desiredBufferSize)
                return;
            IMemoryOwner<byte> newBuffer;
            if (UseMemoryPooling)
            {
                newBuffer = LazinatorUtilities.GetRentedMemory(desiredBufferSize);
            }
            else
                newBuffer = new ReadWriteBytes(new Memory<byte>(new byte[desiredBufferSize]));
            if (TrackMemoryAllocations)
                MemoryAllocations[(int) AllocationID].SetTarget(newBuffer);
            CurrentBuffer.Memory.Span.CopyTo(newBuffer.Memory.Span);
            var oldBuffer = CurrentBuffer;
            CurrentBuffer = newBuffer;
            oldBuffer.Dispose();
        }

        public void Dispose()
        {
            if (Disposed)
                return;
            Disposed = true;
            if (TrackMemoryAllocations)
                MemoryAllocationsManuallyReturned[(int)AllocationID] = true;
            if (LazinatorShouldNotReturnToPool)
                return; // no need to dispose current buffer -- garbage collection will handle it
            if (!UseMemoryPooling)
                return;
        }

        /// <summary>
        /// Get a list of which prior allocated items have not been restored.
        /// </summary>
        /// <returns></returns>
        public static string PoolTrackerSummary()
        {
            GC.Collect();
            return $@"{MemoryAllocations.Count()} total allocations; {MemoryAllocations.Where(x => x.TryGetTarget(out _)).Count()} allocations still remain: {String.Join(",", MemoryAllocations.Select((item, index) => new { Index = index, Item = item }).Where(x => x.Item.TryGetTarget(out _)).Select(x => x.Index + (NotReturnedByLazinatorHashSet.Contains((long) x.Index) ? "*" : "") + (MemoryAllocationsManuallyReturned[x.Index] ? "x" : "")).ToArray()) }"; // * means that the reason it hasn't been returned is we're leaving it to the system to do so automatically; x means that we have in fact manually returned this (but it just hasn't registered as returned).
        }
    }
}
