﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public interface IMemoryAllocationInfo
    {
        long AllocationID { get; }
        bool Disposed { get; }
    }
}
