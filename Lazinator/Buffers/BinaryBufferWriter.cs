using System;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Used internally by Lazinator to write data into a binary buffer.
    /// </summary>
    public struct BinaryBufferWriter
    {
        /// <summary>
        /// Indicates whether storage should be in little Endian format. This should be true unless the intent is to use software primarily on big Endian computers, which are comparatively rarer.
        /// </summary>
        public static bool LittleEndianStorage = true;

        public override string ToString()
        {
            return ActiveMemory == null ? "" : "Position " + _Position + " " + ActiveMemory.ToString();
        }

        public ExpandableBytes ActiveMemory { get; private set; }
        public LazinatorMemory CompletedMemory { get; private set; }

        private List<BytesSegment> BytesSegments;

        Span<byte> BufferSpan => ActiveMemory == null ? new Span<byte>() : ActiveMemory.Memory.Span;

        public BinaryBufferWriter(int minimumSize, LazinatorMemory? completedMemory = null)
        {
            if (minimumSize == 0)
                minimumSize = ExpandableBytes.DefaultMinBufferSize;
            ActiveMemory = new ExpandableBytes(minimumSize);
            if (completedMemory == null)
            {
                CompletedMemory = default;
                BytesSegments = null;
            }
            else
            {
                BytesSegments = new List<BytesSegment>();
                CompletedMemory = completedMemory.Value;
            }
            _Position = 0;
        }

        private void InitializeIfNecessary()
        {
            if (ActiveMemory == null)
                ActiveMemory = new ExpandableBytes();
        }

        /// <summary>
        /// Returns the memory in the buffer beginning at the specified position.
        /// </summary>
        /// <param name="position">The start position</param>
        /// <returns></returns>
        public LazinatorMemory Slice(int position) => LazinatorMemory.Slice(position);


        /// <summary>
        /// Returns the memory in the buffer beginning at the specified position and with the specified length
        /// </summary>
        /// <param name="position">The start position</param>
        /// <param name="position">The number of bytes to include</param>
        /// <returns></returns>
        public LazinatorMemory Slice(int position, int length) => LazinatorMemory.Slice(position, length);

        /// <summary>
        /// Creates LazinatorMemory equal to the underlying memory through the current position.
        /// </summary>
        public LazinatorMemory LazinatorMemory
        {
            get
            {
                InitializeIfNecessary();
                return new LazinatorMemory(ActiveMemory, 0, Position);
            }
        }

        /// <summary>
        /// The position within the buffer. This is changed by the client after writing to the buffer.
        /// </summary>
        private int _Position;
        public int Position
        {
            get => _Position;
            set
            {
                EnsureMinBufferSize(value);
                _Position = value;
            }
        }

        /// <summary>
        /// Free bytes that have not been written to. The client can attempt to write to these bytes directly, calling EnsureMinBufferSize if the operation fails and trying again. Then, the client must update the position.
        /// </summary>
        public Span<byte> FreeSpan => BufferSpan.Slice(Position);

        /// <summary>
        /// The bytes written through the current position. Note that the client can change the position within the buffer.
        /// </summary>
        public Span<byte> ActiveMemoryWrittenSpan => BufferSpan.Slice(0, Position);

        /// <summary>
        /// Sets the position to the beginning of the buffer. It does not dispose the underlying memory, but prepares to rewrite it.
        /// </summary>
        public void Clear()
        {
            Position = 0;
        }

        /// <summary>
        /// Ensures that the underlying memory is at least a specified size, copying the current memory if needed.
        /// </summary>
        /// <param name="desiredBufferSize"></param>
        public void EnsureMinBufferSize(int desiredBufferSize = 0)
        {
            InitializeIfNecessary();
            ActiveMemory.EnsureMinBufferSize(desiredBufferSize);
        }

        /// <summary>
        /// Writes from CompletedMemory. Instead of copying the bytes, it simply adds a BytesSegment reference to where those bytes are in the overall sets of bytes.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="numBytes"></param>
        public void WriteFromCompletedMemory(int startIndex, int numBytes)
        {
            if (CompletedMemory == null)
                throw new ArgumentException();
            RecordLastActiveMemoryBytesSegment();
            IEnumerable<BytesSegment> segmentsToAdd = CompletedMemory.EnumerateSubrangeAsSegments(startIndex, numBytes);
            BytesSegment.ExtendBytesSegmentList(BytesSegments, segmentsToAdd);
        }

        public void FinalizeBytesSegments()
        {
            RecordLastActiveMemoryBytesSegment();
            throw new NotImplementedException("DEBUG"); // Here, we need to add the list of bytes segments onto the end of the active memory, followed by the size. 
        }

        internal void RecordLastActiveMemoryBytesSegment()
        {
            int activeMemoryLength = ActiveMemory.Memory.Length;
            if (activeMemoryLength == 0)
                return;
            int activeMemoryVersion = CompletedMemory.MoreOwnedMemory.Last().VersionOfReferencedMemory + 1;
            int firstUnrecordedActiveMemoryByte = GetFirstUnrecordedActiveMemoryByte(activeMemoryVersion);
            if (firstUnrecordedActiveMemoryByte != activeMemoryLength)
                BytesSegment.ExtendBytesSegmentList(BytesSegments, new BytesSegment(activeMemoryVersion, firstUnrecordedActiveMemoryByte, activeMemoryLength));
        }

        private int GetFirstUnrecordedActiveMemoryByte(int memoryChunkVersion)
        {
            for (int i = BytesSegments.Count - 1; i >= 0; i--)
            {
                BytesSegment bytesSegment = BytesSegments[i];
                if (bytesSegment.MemoryChunkVersion == memoryChunkVersion)
                {
                    return bytesSegment.IndexWithinMemoryChunk + bytesSegment.NumBytes;
                }
            }
            return 0;
        }

        public void Write(bool value)
        {
            WriteEnlargingIfNecessary(ref value);
            Position += sizeof(byte);
        }

        public void Write(byte value)
        {
            if (BufferSpan.Length > Position)
                BufferSpan[Position++] = value;
            else
            {
                WriteEnlargingIfNecessary(ref value);
                Position++;
            }
        }

        public void Write(Span<byte> value)
        {
            int originalPosition = Position;
            if (originalPosition + value.Length > BufferSpan.Length)
                EnsureMinBufferSize((originalPosition + value.Length) * 2);
            value.CopyTo(FreeSpan);
            Position += value.Length;
        }

        public void Write(sbyte value)
        {
            WriteEnlargingIfNecessary(ref value);
            Position += sizeof(sbyte);
        }

        public void Write(char value)
        {
            WriteEnlargingIfNecessary(ref value);
            Position += sizeof(char);
        }

        public void Write(float value)
        {
            Write(BitConverter.SingleToInt32Bits(value));
        }

        public void Write(double value)
        {
            Write(BitConverter.DoubleToInt64Bits(value));
        }

        public void Write(decimal value)
        {
            // Note: Decimal.GetBits allocates an array in the heap. To avoid this, we use our CompressedDecimal class.
            CompressedDecimal.DecomposableDecimal cd = new CompressedDecimal.DecomposableDecimal(value);
            Write(cd.DecomposedDecimal.lo);
            Write(cd.DecomposedDecimal.mid);
            Write(cd.DecomposedDecimal.hi);
            WriteAlwaysLittleEndian(cd.DecomposedDecimal.flags); // we always write this in little endian since our read method looks specifically at bits, assuming little endianness
            // The above is equivalent to the following:
            //int[] integerComponents = Decimal.GetBits(value);
            //foreach (var integerComponent in integerComponents)
            //    Write(integerComponent); // advances EndPosition
        }

        public void Write(Guid value)
        {
            bool success = false;
            while (!success)
            {
                success = value.TryWriteBytes(FreeSpan);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += 16; // trywritebytes always writes exactly 16 bytes even though sizeof(Guid) is not defined
        }

        private void WriteEnlargingIfNecessary<T>(ref T value) where T : struct
        {
            InitializeIfNecessary();
            bool success = false;
            while (!success)
            {
                success = MemoryMarshal.TryWrite<T>(FreeSpan, ref value);
                if (!success)
                    EnsureMinBufferSize();
            }
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            bool success = false;
            while (!success)
            {
                success = value.TryCopyTo(FreeSpan);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += value.Length;
        }

        public void Write(short value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteInt16LittleEndian(FreeSpan, value) : TryWriteInt16BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(short);
        }

        public void Write(ushort value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteUInt16LittleEndian(FreeSpan, value) : TryWriteUInt16BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(ushort);
        }

        public void Write(int value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteInt32LittleEndian(FreeSpan, value) : TryWriteInt32BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(int);
        }

        public void WriteAlwaysLittleEndian(int value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteInt32LittleEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(int);
        }

        public void Write(uint value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteUInt32LittleEndian(FreeSpan, value) : TryWriteUInt32BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(uint);
        }

        public void Write(long value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteInt64LittleEndian(FreeSpan, value) : TryWriteInt64BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(long);
        }

        public void Write(ulong value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteUInt64LittleEndian(FreeSpan, value) : TryWriteUInt64BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(ulong);
        }
    }
}
