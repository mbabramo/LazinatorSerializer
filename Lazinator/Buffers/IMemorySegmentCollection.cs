using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemorySegmentCollection)]
    public interface IMemorySegmentCollection : IMemoryChunkCollection
    {
        List<(int memoryBlockID, int offset, int length)> Segments { get; set; }
    }
}