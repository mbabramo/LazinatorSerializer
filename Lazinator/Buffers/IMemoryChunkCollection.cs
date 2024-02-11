using Lazinator.Attributes;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryBlockCollection)]
    public interface IMemoryBlockCollection
    {
        [Uncompressed]
        string BaseBlobPath { get; set; }
        bool ContainedInSingleBlob { get; set; }
        bool IsPersisted { get; set; }
        List<MemoryBlockLoadingInfo> MemoryBlocksLoadingInfo { get; set; }

        long LengthOfMemoryBlocks { get; set; }
        MemoryBlockID HighestMemoryBlockID { get; set; }
    }
}