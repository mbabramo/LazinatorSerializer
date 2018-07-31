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
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this

        private bool Initialized;
        private LazinatorMemory _LazinatorMemory;
        public LazinatorMemory LazinatorMemory
        {
            get
            {
                if (!Initialized)
                {
                    _LazinatorMemory = new LazinatorMemory(LazinatorUtilities.GetRentedMemory(MinMinBufferSize), 0);
                    InitializeBufferSpan();
                    Initialized = true;
                }
                else
                {
                    if (Position != _LazinatorMemory.BytesFilled)
                        _LazinatorMemory.BytesFilled = Position;
                    return _LazinatorMemory;
                }
                return _LazinatorMemory;
            }
            set
            {
                _LazinatorMemory = value;
                InitializeBufferSpan();
                Initialized = true;
            }
        }

        Span<byte> BufferSpan;
        private void InitializeBufferSpan()
        {
            BufferSpan = _LazinatorMemory.OwnedMemory.Memory.Span;
        }

        public LazinatorMemory LazinatorMemorySlice(int position) => LazinatorMemory.Slice(position);
        public ReadOnlyMemory<byte> Slice(int position) => LazinatorMemory.OwnedMemory.Memory.Slice(position, Position - position);
        public ReadOnlyMemory<byte> Slice(int position, int length) => LazinatorMemory.OwnedMemory.Memory.Slice(position, length);
        

        public BinaryBufferWriter(int minimumSize)
        {
            if (minimumSize < MinMinBufferSize)
                minimumSize = MinMinBufferSize;
            _LazinatorMemory = new LazinatorMemory(LazinatorUtilities.GetRentedMemory(minimumSize), 0);
            BufferSpan = _LazinatorMemory.OwnedMemory.Memory.Span;
            Position = 0;
            Initialized = true;
        }

        public Span<byte> Free => BufferSpan.Slice(Position);

        public Span<byte> Written => BufferSpan.Slice(0, Position);
        
        public int Position { get; set; }

        public void Clear()
        {
            Position = 0;
        }

        public void EnsureMinBufferSize(int desiredBufferSize = 0)
        {
            if (desiredBufferSize <= 0)
            {
                desiredBufferSize = BufferSpan.Length * 2;
                if (desiredBufferSize < MinMinBufferSize)
                    desiredBufferSize = MinMinBufferSize;
            }
            else if (BufferSpan.Length >= desiredBufferSize)
                return;
            var newLazinatorMemory = LazinatorUtilities.GetRentedMemory(desiredBufferSize);
            Written.CopyTo(newLazinatorMemory.Memory.Span);
            LazinatorMemory = new LazinatorMemory(newLazinatorMemory, Position);
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
