using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    public interface IMemoryChunkReference
    {
        int MemoryChunkID { get; set; }
        long OffsetForLoading { get; set; }
        int PreTruncationLength { get; set; }
        int AdditionalOffset { get; set; }
        int FinalLength { get; set; }
    }
}