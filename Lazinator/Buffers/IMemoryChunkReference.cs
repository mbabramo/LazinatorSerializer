using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryChunkReference)]
    public interface IMemoryChunkReference
    {
        int MemoryChunkID { get; set; }
        int OffsetForLoading { get; set; }
        int LengthForLoading { get; set; }
        int AdditionalOffset { get; set; }
        int FinalLength { get; set; }
    }
}