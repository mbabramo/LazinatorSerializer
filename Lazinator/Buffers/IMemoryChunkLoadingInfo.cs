using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkLoadingInfo)]
    public interface IMemoryChunkLoadingInfo
    {
        int MemoryChunkID { get; set; }
        int PreTruncationLength { get; set; }
    }
}