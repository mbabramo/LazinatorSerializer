using Lazinator.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This memory owner rents memory and then returns it and rents more, copying what it has written, when more space is needed.
    /// </summary>
    public class ExpandableBytes : JointlyDisposableMemory
    {
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this
        public static ulong NextAllocationID = 0; // we track all allocations to facilitate debugging of memory allocation and disposal
        public bool LazinatorShouldNotReturnToPool;
        public ulong AllocationID;

        IMemoryOwner<byte> CurrentBuffer { get; set; }
        public override Memory<byte> Memory => CurrentBuffer.Memory;

        public static bool UseMemoryPooling = true;
        public static bool TrackMemoryAllocations = false;
        public static List<WeakReference<IMemoryOwner<byte>>> MemoryAllocations = new List<WeakReference<IMemoryOwner<byte>>>();
        public static HashSet<ulong> NotReturnedByLazinatorHashSet = new HashSet<ulong>(); // DEBUG -- change all DoNotAutomaticallyReturn to something like LazinatorWontReturn
        
        public ExpandableBytes() : this(MinMinBufferSize, null)
        {
        }

        public ExpandableBytes(int minBufferSize, JointlyDisposableMemory originalSource)
        {
            unchecked
            {
                AllocationID = NextAllocationID++;
            }
            int minimumSize = Math.Max(minBufferSize, MinMinBufferSize);
            if (UseMemoryPooling)
            {
                CurrentBuffer = LazinatorUtilities.GetRentedMemory(minimumSize);
            }
            else
                CurrentBuffer = new SimpleMemoryOwner<byte>(new Memory<byte>(new byte[minimumSize]));

            if (TrackMemoryAllocations)
                MemoryAllocations.Add(new WeakReference<IMemoryOwner<byte>>(CurrentBuffer));

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
            var newBuffer = LazinatorUtilities.GetRentedMemory(desiredBufferSize);
            if (TrackMemoryAllocations)
                MemoryAllocations[(int) AllocationID].SetTarget(newBuffer);
            CurrentBuffer.Memory.Span.CopyTo(newBuffer.Memory.Span);
            var oldBuffer = CurrentBuffer;
            CurrentBuffer = newBuffer;
            oldBuffer.Dispose(); // DEBUG? Better if we can leave this
            // DEBUG -- not necessary? DisposeWithThis(oldBuffer); // keep the old buffer around for now, because we might already have saved memory from it, but when this is disposed, we'll dispose the old buffer as well
        }

        public override void Dispose()
        {
            if (!UseMemoryPooling || LazinatorShouldNotReturnToPool)
                return; // no need to dispose -- garbage collection will handle it
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
            return $@"{MemoryAllocations.Count()} total allocations; {MemoryAllocations.Where(x => x.TryGetTarget(out _)).Count()} allocations still remain: {String.Join(",", MemoryAllocations.Select((item, index) => new { Index = index, Item = item }).Where(x => x.Item.TryGetTarget(out _)).Select(x => x.Index + (NotReturnedByLazinatorHashSet.Contains((ulong) x.Index) ? "*" : "")).ToArray())}";
        }
    }
}
