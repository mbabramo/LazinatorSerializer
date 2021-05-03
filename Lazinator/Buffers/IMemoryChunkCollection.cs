using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkCollection)]
    public interface IMemoryChunkCollection
    {
        List<MemoryBlockLoadingInfo> MemoryBlockLoadingInfo { get; set; }
    }
}