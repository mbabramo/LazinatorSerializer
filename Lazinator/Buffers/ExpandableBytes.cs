using Lazinator.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This memory owner rents memory and then returns it and rents more, copying what it has written, when more space is needed.
    /// </summary>
    public class ExpandableBytes : JointlyDisposableMemory
    {
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this
        public static long NextAllocationID = 0; // we track all allocations to facilitate debugging of memory allocation and disposal
        public bool LazinatorShouldNotReturnToPool;
        public long AllocationID;

        IMemoryOwner<byte> CurrentBuffer { get; set; }
        public override Memory<byte> Memory => CurrentBuffer.Memory;

        public static bool UseMemoryPooling = true;
        public static bool TrackMemoryAllocations = false;
        public static List<WeakReference<IMemoryOwner<byte>>> MemoryAllocations = new List<WeakReference<IMemoryOwner<byte>>>();
        public static List<bool> MemoryAllocationsManuallyReturned = new List<bool>();
        public static HashSet<long> NotReturnedByLazinatorHashSet = new HashSet<long>();
        
        public ExpandableBytes() : this(MinMinBufferSize, null)
        {
        }

        public ExpandableBytes(int minBufferSize, JointlyDisposableMemory originalSource)
        {
            unchecked
            {
                AllocationID = Interlocked.Increment(ref NextAllocationID) - 1;
            }
            int minimumSize = Math.Max(minBufferSize, MinMinBufferSize);
            if (UseMemoryPooling)
            {
                CurrentBuffer = LazinatorUtilities.GetRentedMemory(minimumSize);
            }
            else
                CurrentBuffer = new SimpleMemoryOwner<byte>(new Memory<byte>(new byte[minimumSize]));

            if (TrackMemoryAllocations)
            {
                MemoryAllocations.Add(new WeakReference<IMemoryOwner<byte>>(CurrentBuffer));
                MemoryAllocationsManuallyReturned.Add(false);
            }


            OriginalSource = originalSource;
            if (OriginalSource != null)
                OriginalSource.DisposeWithThis(this);
        }

        public ExpandableBytes(IMemoryOwner<byte> initialBuffer, JointlyDisposableMemory originalSource)
        {
            CurrentBuffer = initialBuffer;
            OriginalSource = originalSource;
            if (OriginalSource != null)
                OriginalSource.DisposeWithThis(this);
        }

        public void EnsureMinBufferSize(int desiredBufferSize = 0)
        {
            if (desiredBufferSize <= 0)
            {
                desiredBufferSize = CurrentBuffer.Memory.Length * 2;
                if (desiredBufferSize < MinMinBufferSize)
                    desiredBufferSize = MinMinBufferSize;
            }
            else if (CurrentBuffer.Memory.Length >= desiredBufferSize)
                return;
            IMemoryOwner<byte> newBuffer;
            if (UseMemoryPooling)
            {
                newBuffer = LazinatorUtilities.GetRentedMemory(desiredBufferSize);
            }
            else
                newBuffer = new SimpleMemoryOwner<byte>(new Memory<byte>(new byte[desiredBufferSize]));
            if (TrackMemoryAllocations)
                MemoryAllocations[(int) AllocationID].SetTarget(newBuffer);
            CurrentBuffer.Memory.Span.CopyTo(newBuffer.Memory.Span);
            var oldBuffer = CurrentBuffer;
            CurrentBuffer = newBuffer;
            oldBuffer.Dispose();
        }

        public override void Dispose()
        {
            if (TrackMemoryAllocations)
                MemoryAllocationsManuallyReturned[(int)AllocationID] = true;
            if (LazinatorShouldNotReturnToPool)
                return; // no need to dispose -- garbage collection will handle it
            // DEBUG -- uncomment
            //if (!UseMemoryPooling)
            //    return; 
            base.Dispose(); // dispose anything that we are supposed to dispose besides the current buffer
            if (!(CurrentBuffer is SimpleMemoryOwner<byte>)) // SimpleMemoryOwner manages its own memory and should thus not be disposed
                CurrentBuffer.Dispose();
        }

        /// <summary>
        /// Get a list of which prior allocated items have not been restored.
        /// </summary>
        /// <returns></returns>
        public static string PoolTrackerSummary()
        {
            GC.Collect();
            return $@"{MemoryAllocations.Count()} total allocations; {MemoryAllocations.Where(x => x.TryGetTarget(out _)).Count()} allocations still remain: {String.Join(",", MemoryAllocations.Select((item, index) => new { Index = index, Item = item }).Where(x => x.Item.TryGetTarget(out _)).Select(x => x.Index + (NotReturnedByLazinatorHashSet.Contains((long) x.Index) ? "*" : "") + (MemoryAllocationsManuallyReturned[x.Index] ? "x" : "")).ToArray()) }"; // * means that the reason it hasn't been returned is we're leaving it to the system to do so automatically; x means that we have in fact manually returned this (but it just hasn't registered as returned). DEBUG
        }
    }
}
