using System;
using System.Collections.Generic;
using System.Buffers.Text;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Runtime.InteropServices;
using System.Buffers;
using Lazinator.Support;
using Lazinator.Core;

namespace Lazinator.Buffers
{
    public class BinaryBufferWriter : IDisposable
    {
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this
        
        public MemoryInBuffer MemoryInBuffer { get; set; }
        Span<byte> _buffer => MemoryInBuffer.OwnedMemory.Memory.Span;

        public BinaryBufferWriter() : this(MinMinBufferSize)
        {
        }

        public BinaryBufferWriter(int minimumSize)
        {
            if (minimumSize < MinMinBufferSize)
                minimumSize = MinMinBufferSize;
            MemoryInBuffer = new MemoryInBuffer(LazinatorUtilities.GetRentedMemory(minimumSize), 0);
        }

        public Span<byte> Free => _buffer.Slice(EndPosition);

        public Span<byte> Written => _buffer.Slice(0, EndPosition);

        public int EndPosition
        {
            get { return MemoryInBuffer.BytesFilled; }
            set
            {
                while (value > _buffer.Length) Resize(value);
                MemoryInBuffer = MemoryInBuffer.WithBytesFilled(value);
            }
        }

        public void Clear()
            => EndPosition = 0;

        private void Resize(int desiredBufferSize = 0)
        {
            if (desiredBufferSize <= 0)
            {
                desiredBufferSize = _buffer.Length * 2;
                if (desiredBufferSize < MinMinBufferSize)
                    desiredBufferSize = MinMinBufferSize;
            }
            else if (desiredBufferSize < _buffer.Length + 1) throw new ArgumentOutOfRangeException(nameof(desiredBufferSize));
            var newMemoryInBuffer = LazinatorUtilities.GetRentedMemory(desiredBufferSize);
            Written.CopyTo(newMemoryInBuffer.Memory.Span);
            MemoryInBuffer = new MemoryInBuffer(newMemoryInBuffer, EndPosition); // size written stays the same
        }

        public void Write(bool value)
        {
            WriteEnlargingIfNecessary(ref value);
            EndPosition += sizeof(byte);
        }

        public void Write(byte value)
        {
            WriteEnlargingIfNecessary(ref value);
            EndPosition += sizeof(byte);
        }

        public void Write(sbyte value)
        {
            WriteEnlargingIfNecessary(ref value);
            EndPosition += sizeof(sbyte);
        }

        public void Write(char value)
        {
            WriteEnlargingIfNecessary(ref value);
            EndPosition += sizeof(char);
        }

        public void Write(float value)
        {
            WriteEnlargingIfNecessary(ref value);
            EndPosition += sizeof(float);
        }

        public void Write(double value)
        {
            WriteEnlargingIfNecessary(ref value);
            EndPosition += sizeof(double);
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
                    Resize();
            }
            EndPosition += 16; // trywritebytes always writes exactly 16 bytes even though sizeof(Guid) is not defined
        }

        private void WriteEnlargingIfNecessary<T>(ref T value) where T : struct
        {
            bool success = false;
            while (!success)
            {
                success = MemoryMarshal.TryWrite<T>(Free, ref value);
                if (!success)
                    Resize();
            }
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            bool success = false;
            while (!success)
            {
                success = value.TryCopyTo(Free);
                if (!success)
                    Resize();
            }
            EndPosition += value.Length;
        }

        public void Write(short value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteInt16LittleEndian(Free, value);
                if (!success)
                    Resize();
            }
            EndPosition += sizeof(short);
        }

        public void Write(ushort value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteUInt16LittleEndian(Free, value);
                if (!success)
                    Resize();
            }
            EndPosition += sizeof(ushort);
        }

        public void Write(int value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteInt32LittleEndian(Free, value);
                if (!success)
                    Resize();
            }
            EndPosition += sizeof(int);
        }

        public void Write(uint value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteUInt32LittleEndian(Free, value);
                if (!success)
                    Resize();
            }
            EndPosition += sizeof(uint);
        }

        public void Write(long value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteInt64LittleEndian(Free, value);
                if (!success)
                    Resize();
            }
            EndPosition += sizeof(long);
        }

        public void Write(ulong value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteUInt64LittleEndian(Free, value);
                if (!success)
                    Resize();
            }
            EndPosition += sizeof(ulong);
        }

        public void Dispose()
        {
            // Right now, we're not doing anything with this, because we're using managed memory. But if we switch to allowing use of native memory, we'll have to release that. 
        }

        public class BufferTooSmallException : Exception
        {
            public BufferTooSmallException() : base("Buffer is too small") { }
        }
    }
}
