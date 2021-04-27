using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    [NonbinaryHash]
    public interface IMemoryChunkReference
    {
        int MemoryBlockID { get; set; }
        long LoadingOffset { get; set; }
        int PreTruncationLength { get; set; }
        int AdditionalOffset { get; set; }
        int FinalLength { get; set; }
    }
}