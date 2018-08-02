﻿using Lazinator.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    /// <summary>
    /// This memory owner rents memory and then returns it and rents more, copying what it has written, when more space is needed.
    /// </summary>
    public class ExpandableBytes : IMemoryOwner<byte>
    {
        public const int MinMinBufferSize = 1024; // never allocate a pooled buffer smaller than this

        IMemoryOwner<byte> CurrentBuffer { get; set; }
        
        public ExpandableBytes() : this(MinMinBufferSize)
        {
        }

        public ExpandableBytes(int minBufferSize)
        {
            CurrentBuffer = LazinatorUtilities.GetRentedMemory(Math.Max(minBufferSize, MinMinBufferSize));
        }

        public ExpandableBytes(IMemoryOwner<byte> initialBuffer)
        {
            CurrentBuffer = initialBuffer;
        }

        public Memory<byte> Memory => CurrentBuffer.Memory;

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
            oldBuffer.Dispose();
        }

        public void Dispose()
        {
            CurrentBuffer.Dispose();
        }
    }
}