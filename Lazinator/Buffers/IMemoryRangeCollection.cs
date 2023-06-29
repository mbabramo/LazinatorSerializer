using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemorySegmentCollection)]
    public interface IMemoryRangeCollection : IMemoryBlockCollection
    {
        List<MemoryRangeByID> Ranges { get; set; }
    }
}