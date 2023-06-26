using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemorySegmentCollection)]
    public interface IMemorySegmentCollection : IMemoryChunkCollection
    {
        List<MemorySegmentIDAndSlice> Segments { get; set; }
    }
}