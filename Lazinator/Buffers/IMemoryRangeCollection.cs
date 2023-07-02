using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemorySegmentCollection)]
    public interface IMemoryRangeCollection : IMemoryBlockCollection
    {
        List<MemoryRangeByBlockID> Ranges { get; set; }
        long PatchesTotalLength { get; set; }
    }
}