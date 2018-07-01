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
    public ref struct BinaryBufferWriter
    {
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this

        private bool Initialized;
        private MemoryInBuffer _MemoryInBuffer;
        public MemoryInBuffer MemoryInBuffer
        {
            get
            {
                if (!Initialized)
                {
                    _MemoryInBuffer = new MemoryInBuffer(LazinatorUtilities.GetRentedMemory(MinMinBufferSize), 0);
                    InitializeBufferSpan();
                    Initialized = true;
                }
                return _MemoryInBuffer;
            }
            set
            {
                _MemoryInBuffer = value;
                InitializeBufferSpan();
                Initialized = true;
            }
        }

        Span<byte> BufferSpan;
        private void InitializeBufferSpan()
        {
            BufferSpan = _MemoryInBuffer.OwnedMemory.Memory.Span;
        }

        public ReadOnlyMemory<byte> Slice(int position) => MemoryInBuffer.OwnedMemory.Memory.Slice(position, Position - position);
        public ReadOnlyMemory<byte> Slice(int position, int length) => MemoryInBuffer.OwnedMemory.Memory.Slice(position, length);
        

        public BinaryBufferWriter(int minimumSize)
        {
            if (minimumSize < MinMinBufferSize)
                minimumSize = MinMinBufferSize;
            _MemoryInBuffer = new MemoryInBuffer(LazinatorUtilities.GetRentedMemory(minimumSize), 0);
            BufferSpan = _MemoryInBuffer.OwnedMemory.Memory.Span;
            Initialized = true;
        }

        public Span<byte> Free => BufferSpan.Slice(Position);

        public Span<byte> Written => BufferSpan.Slice(0, Position);

        public int Position
        {
            get { return MemoryInBuffer.BytesFilled; }
            set
            {
                while (value > BufferSpan.Length) Resize(value);
                MemoryInBuffer = MemoryInBuffer.WithBytesFilled(value);
            }
        }

        public void Clear()
            => Position = 0;

        public void Resize(int desiredBufferSize = 0)
        {
            if (desiredBufferSize <= 0)
            {
                desiredBufferSize = BufferSpan.Length * 2;
                if (desiredBufferSize < MinMinBufferSize)
                    desiredBufferSize = MinMinBufferSize;
            }
            else if (desiredBufferSize < BufferSpan.Length + 1) throw new ArgumentOutOfRangeException(nameof(desiredBufferSize));
            var newMemoryInBuffer = LazinatorUtilities.GetRentedMemory(desiredBufferSize);
            Written.CopyTo(newMemoryInBuffer.Memory.Span);
            MemoryInBuffer = new MemoryInBuffer(newMemoryInBuffer, Position); // size written stays the same
        }

        public void Write(bool value)
        {
            WriteEnlargingIfNecessary(ref value);
            Position += sizeof(byte);
        }

        public void Write(byte value)
        {
            if (Free.Length > 0)
                Free[0] = value;
            else
                WriteEnlargingIfNecessary(ref value);
            Position += sizeof(byte);
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
                    Resize();
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
            Position += value.Length;
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
            Position += sizeof(short);
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
            Position += sizeof(ushort);
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
            Position += sizeof(int);
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
            Position += sizeof(uint);
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
            Position += sizeof(long);
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
            Position += sizeof(ulong);
        }

        public class BufferTooSmallException : Exception
        {
            public BufferTooSmallException() : base("Buffer is too small") { }
        }
    }
}
