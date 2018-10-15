using Lazinator.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This memory owner rents memory and then returns it and rents more, copying what it has written, when more space is needed.
    /// </summary>
    public class ExpandableBytes : JointlyDisposableMemory
    {
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this
        public static ulong NextAllocationID = 0; // we track all allocations to facilitate debugging of memory allocation and disposal
        public static bool UseMemoryPooling = false; // DEBUG
        public ulong AllocationID;

        IMemoryOwner<byte> CurrentBuffer { get; set; }

        public override Memory<byte> Memory => CurrentBuffer.Memory;
        
        public ExpandableBytes() : this(MinMinBufferSize, null)
        {
        }

        public ExpandableBytes(int minBufferSize, JointlyDisposableMemory originalSource)
        {
            int minimumSize = Math.Max(minBufferSize, MinMinBufferSize);
            if (UseMemoryPooling)
            {
                CurrentBuffer = LazinatorUtilities.GetRentedMemory(minimumSize);
            }
            else
                CurrentBuffer = new SimpleMemoryOwner<byte>(new Memory<byte>(new byte[minimumSize]));
            unchecked
            {
                AllocationID = NextAllocationID++;
            }

            OriginalSource = originalSource;
            if (OriginalSource != null)
                OriginalSource.DisposeWithThis(this);
        }

        public ExpandableBytes(IMemoryOwner<byte> initialBuffer)
        {
            CurrentBuffer = initialBuffer;
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
            CurrentBuffer.Memory.Span.CopyTo(newBuffer.Memory.Span);
            var oldBuffer = CurrentBuffer;
            CurrentBuffer = newBuffer;
            DisposeWithThis(oldBuffer); // keep the old buffer around for now, because we might already have saved memory from it, but when this is disposed, we'll dispose the old buffer as well
        }

        public override void Dispose()
        {
            if (!UseMemoryPooling)
                return; // no need to dispose -- garbage collection will handle it
            base.Dispose(); // dispose anything that we are supposed to dispose besides the current buffer
            CurrentBuffer.Dispose();
        }
    }
}
