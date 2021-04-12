using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This contains a container for BufferWriter. This can be used in async methods, where we cannot pass BufferWriter by reference.
    /// </summary>
    public class BufferWriterContainer
    {
        public BufferWriter Writer;
        public override string ToString()
        {
            return Writer.ToString();
        }
        public BufferWriterContainer(int minimumSize, LazinatorMemory? completedMemory = null)
        {
            Writer = new BufferWriter(minimumSize, completedMemory);
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
        public long OverallMemoryPosition => Writer.OverallMemoryPosition;
        public long SetLengthsPosition(int bytesToReserve) => Writer.SetLengthsPosition(bytesToReserve);
        public void ResetLengthsPosition(long previousPosition) => Writer.ResetLengthsPosition(previousPosition);
        public void RecordLength(byte length) => Writer.RecordLength(length);
        public void RecordLength(Int16 length) => Writer.RecordLength(length);
        public void RecordLength(int length) => Writer.RecordLength(length);
        public void RecordLength(Int64 length) => Writer.RecordLength(length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureMinBufferSize(int desiredBufferSize = 0) => Writer.EnsureMinBufferSize(desiredBufferSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ConsiderSwitchToNextBuffer(ref LazinatorSerializationOptions options) => Writer.ConsiderSwitchToNextBuffer(ref options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InsertReferenceToCompletedMemory(int memoryChunkIndex, int startPosition, long numBytes) => Writer.InsertReferenceToCompletedMemory(memoryChunkIndex, startPosition, numBytes);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RecordLastActiveMemoryChunkReferenceIfAny() => Writer.RecordLastActiveMemoryChunkReferenceIfAny();
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
