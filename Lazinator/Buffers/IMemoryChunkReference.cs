using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    [NonbinaryHash]
    public interface IMemoryChunkReference : IMemoryBlockInsetLoadingInfo
    {
        int AdditionalOffset { get; set; }
        int FinalLength { get; set; }
    }
}