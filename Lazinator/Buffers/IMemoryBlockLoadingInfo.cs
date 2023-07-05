using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryBlockLoadingInfo)]
    public interface IMemoryBlockLoadingInfo
    {
        MemoryBlockID MemoryBlockID { get; set; }
        int MemoryBlockLength { get; set; }
    }
}