using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryBlockLoadingInfo)]
    public interface IMemoryBlockLoadingInfo
    {
        int MemoryBlockID { get; set; }
        int PreTruncationLength { get; set; }
    }
}