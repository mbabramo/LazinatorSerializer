using System;
using System.Buffers;

namespace Lazinator.Buffers
{
    public class LazinatorMemory : JointlyDisposableMemory
    {
        public IMemoryOwner<byte> OwnedMemory;
        public int StartPosition { get; set; }
        public int Length { get; private set; }
        public override Memory<byte> Memory => OwnedMemory.Memory.Slice(StartPosition, Length);
        public ReadOnlyMemory<byte> ReadOnlyMemory => Memory;
        public Span<byte> Span => Memory.Span;
        public ReadOnlySpan<byte> ReadOnlySpan => Memory.Span;

        #region Constructors

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int startPosition, int bytesFilled, JointlyDisposableMemory originalSource) : base()
        {
            OwnedMemory = ownedMemory;
            StartPosition = startPosition;
            Length = bytesFilled;
            OriginalSource = originalSource;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory, int bytesFilled) : base()
        {
            OwnedMemory = ownedMemory;
            Length = bytesFilled;
        }

        public LazinatorMemory(IMemoryOwner<byte> ownedMemory)
        {
            OwnedMemory = ownedMemory;
            Length = ownedMemory.Memory.Length;
        }

        public LazinatorMemory(Memory<byte> memory) : this(new SimpleMemoryOwner<byte>(memory), memory.Length)
        {
        }

        public LazinatorMemory(byte[] array) : this(new SimpleMemoryOwner<byte>(new Memory<byte>(array)), array.Length)
        {
        }

        public void CopyFrom(LazinatorMemory existingMemoryToCopy)
        {
            OwnedMemory = existingMemoryToCopy.OwnedMemory;
            StartPosition = existingMemoryToCopy.StartPosition;
            Length = existingMemoryToCopy.Length;
            OriginalSource = existingMemoryToCopy.OriginalSource;
        }

        public bool IndirectlyDisposed()
        {
            return Disposed || (OwnedMemory != null && OwnedMemory is JointlyDisposableMemory j && j.Disposed);
        }

        public override void Dispose()
        {
            base.Dispose(); // dispose anything that we are supposed to dispose besides the current buffer
            if (!(OwnedMemory is SimpleMemoryOwner<byte>))
                OwnedMemory.Dispose();
        }

        public override void ReplaceWithNewBuffer(IMemoryOwner<byte> newBuffer)
        {
            DoNotDisposeWithThis(OwnedMemory, true);
            DisposeWithThis(newBuffer);
        }

        public void DoNotAutomaticallyReturnToPool()
        {
            if (OwnedMemory is ExpandableBytes e)
            {
                e.DoNotAutomaticallyReturnToPool = true;
            }
        }

        #endregion

        #region Conversions and slicing

        public static implicit operator LazinatorMemory(Memory<byte> memory)
        {
            return new LazinatorMemory(memory);
        }

        public static implicit operator LazinatorMemory(byte[] array)
        {
            return new LazinatorMemory(array);
        }

        public LazinatorMemory Slice(int position) => Slice(position, Length - position);
        public LazinatorMemory Slice(int position, int length) => new LazinatorMemory(OwnedMemory, StartPosition + position, length, OriginalSource);

        #endregion

    }
}
