using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A reference to a memory range within a memory range collection. This thus points to a particular memory range, which in turn points to a particular memory block.
    /// </summary>
    /// <param name="MemoryRangeIndex">The index of the range within the list of ranges (which may or may not correspond to the indices of blocks)</param>
    /// <param name="FurtherOffsetIntoMemoryBlock">An offset into the memory block referred to by the memory range at MemoryRangeIndex, in addition to any offset specified within that memory range.</param>
    public readonly record struct MemoryRangeReference(int MemoryRangeIndex, int FurtherOffsetIntoMemoryBlock)
    {
    }
}
