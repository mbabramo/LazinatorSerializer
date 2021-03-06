﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockSegmentByID
    {
        public readonly int MemoryBlockID { get; }
        public readonly int Offset { get; }
        public readonly int Length { get; }

        public MemoryBlockSegmentByID(int memoryBlockID, int offset, int length)
        {
            this.MemoryBlockID = memoryBlockID;
            this.Offset = offset;
            this.Length = length;
        }
    }
}
