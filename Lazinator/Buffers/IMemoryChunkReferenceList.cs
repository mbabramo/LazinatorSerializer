using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int) LazinatorCoreUniqueIDs.IMemoryChunkReferenceList)]
    public interface IMemoryChunkReferenceList
    {
        List<MemoryChunkReference> MemoryChunkReferences { get; set; }
    }
}