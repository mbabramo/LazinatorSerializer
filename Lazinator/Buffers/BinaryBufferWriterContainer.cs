using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This contains a container for BinaryBufferWriter. This can be used in async methods, where we cannot pass BinaryBufferWriter by reference.
    /// </summary>
    public class BinaryBufferWriterContainer
    {
        public BinaryBufferWriter Writer;
        public override string ToString()
        {
            return Writer.ToString();
        }
        public ExpandableBytes ActiveMemory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Writer.ActiveMemory;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Writer.ActiveMemory = value;
        }
        public LazinatorMemory CompletedMemory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Writer.CompletedMemory;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Writer.CompletedMemory = value;
        }

        public List<BytesSegment> BytesSegments
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Writer.BytesSegments;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Writer.BytesSegments = value;
        }

        public BinaryBufferWriterContainer(int minimumSize, LazinatorMemory? completedMemory = null)
        {
            Writer = new BinaryBufferWriter(minimumSize, completedMemory);
        }
        public LazinatorMemory LazinatorMemory => Writer.LazinatorMemory;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LazinatorMemory Slice(int position) => Writer.Slice(position);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LazinatorMemory Slice(int position, int length) => Writer.Slice(position, length);
        public int Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Writer.ActiveMemoryPosition;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Writer.ActiveMemoryPosition = value;
        }
        public Span<byte> FreeSpan => Writer.FreeSpan;
        public Span<byte> ActiveMemoryWrittenSpan => Writer.ActiveMemoryWrittenSpan;
        public int ActiveMemoryPosition => Writer.ActiveMemoryPosition;
        public int SetLengthsPosition(int bytesToReserve) => Writer.SetLengthsPosition(bytesToReserve);
        public void ResetLengthsPosition(int previousPosition) => Writer.ResetLengthsPosition(previousPosition);
        public void RecordLength(int length) => Writer.RecordLength(length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => Writer.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureMinBufferSize(int desiredBufferSize = 0) => Writer.EnsureMinBufferSize(desiredBufferSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InsertReferenceToCompletedMemory(int startIndex, int numBytes) => Writer.InsertReferenceToCompletedMemory(startIndex, numBytes);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddDiffsInfoToActiveMemory() => Writer.AddDiffsInfoToActiveMemory();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RecordLastActiveMemoryBytesSegment() => Writer.RecordLastActiveMemoryBytesSegment();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(bool value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(Span<byte> value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(sbyte value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(double value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(decimal value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(Guid value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ReadOnlySpan<byte> value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(short value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ushort value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAlwaysLittleEndian(int value) => Writer.WriteAlwaysLittleEndian(value);
        public void Write(uint value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(long value) => Writer.Write(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value) => Writer.Write(value);

    }
}
