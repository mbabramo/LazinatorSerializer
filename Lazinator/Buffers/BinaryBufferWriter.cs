using System;
using System.Collections.Generic;
using System.Buffers.Text;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Runtime.InteropServices;
using System.Buffers;
using Lazinator.Support;
using Lazinator.Core;
using System.Runtime.CompilerServices;

namespace Lazinator.Buffers
{
    public ref struct BinaryBufferWriter
    {
        ExpandableBytes UnderlyingMemory { get; set; }
        Span<byte> BufferSpan => UnderlyingMemory.Memory.Span;

        public BinaryBufferWriter(int minimumSize)
        {
            UnderlyingMemory = new ExpandableBytes(minimumSize);
            _Position = 0;
        }

        /// <summary>
        /// Returns the memory in the buffer beginning at the specified position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public LazinatorMemory Slice(int position) => LazinatorMemory.Slice(position);

        /// <summary>
        /// Creates LazinatorMemory equal to the underlying memory through the current position.
        /// </summary>
        public LazinatorMemory LazinatorMemory => new LazinatorMemory(UnderlyingMemory, 0, Position);

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
        public Span<byte> Free => BufferSpan.Slice(Position);

        /// <summary>
        /// The bytes written through the current position. Note that the client can change the position within the buffer.
        /// </summary>
        public Span<byte> Written => BufferSpan.Slice(0, Position);

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
            UnderlyingMemory.EnsureMinBufferSize(desiredBufferSize);
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
            value.CopyTo(Free);
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
            WriteEnlargingIfNecessary(ref value);
            Position += sizeof(float);
        }

        public void Write(double value)
        {
            WriteEnlargingIfNecessary(ref value);
            Position += sizeof(double);
        }

        public void Write(decimal value)
        {
            // Note: Decimal.GetBits allocates an array in the heap. To avoid this, we use our CompressedDecimal class.
            CompressedDecimal.DecomposableDecimal cd = new CompressedDecimal.DecomposableDecimal(value);
            Write(cd.DecomposedDecimal.lo);
            Write(cd.DecomposedDecimal.mid);
            Write(cd.DecomposedDecimal.hi);
            Write(cd.DecomposedDecimal.flags);
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
                success = value.TryWriteBytes(Free);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += 16; // trywritebytes always writes exactly 16 bytes even though sizeof(Guid) is not defined
        }

        private void WriteEnlargingIfNecessary<T>(ref T value) where T : struct
        {
            bool success = false;
            while (!success)
            {
                success = MemoryMarshal.TryWrite<T>(Free, ref value);
                if (!success)
                    EnsureMinBufferSize();
            }
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            bool success = false;
            while (!success)
            {
                success = value.TryCopyTo(Free);
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
                success = TryWriteInt16LittleEndian(Free, value);
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
                success = TryWriteUInt16LittleEndian(Free, value);
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
                success = TryWriteInt32LittleEndian(Free, value);
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
                success = TryWriteUInt32LittleEndian(Free, value);
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
                success = TryWriteInt64LittleEndian(Free, value);
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
                success = TryWriteUInt64LittleEndian(Free, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            Position += sizeof(ulong);
        }

        public class BufferTooSmallException : Exception
        {
            public BufferTooSmallException() : base("Buffer is too small") { }
        }
    }
}
