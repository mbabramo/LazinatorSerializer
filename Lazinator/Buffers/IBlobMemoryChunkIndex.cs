using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int) LazinatorCoreUniqueIDs.IBlobMemoryChunkIndex)]
    public interface IBlobMemoryChunkIndex
    {
        bool ContainedInSingleBlob { get; set; }
        bool MaintainOldVersions { get; set; }
        bool IsPersisted { get; set; }
        string BlobPath { get; set; }
        List<MemoryChunkReference> MemoryChunkReferences { get; set; }
        List<int> NoLongerNeededChunks { get; set; }
    }
}