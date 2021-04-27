using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    [NonbinaryHash]
    public interface IMemoryChunkReference
    {
        int MemoryBlockID { get; set; }
        long OffsetForLoading { get; set; }
        int PreTruncationLength { get; set; }
        int AdditionalOffset { get; set; }
        int FinalLength { get; set; }
    }
}