using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkWithinBlobLoadingInfo)]
    public interface IMemoryChunkWithinBlobLoadingInfo : IMemoryChunkLoadingInfo
    {
        long OffsetWhenLoading { get; set; }
    }
}