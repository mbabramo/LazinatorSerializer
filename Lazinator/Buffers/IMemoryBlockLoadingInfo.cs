using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryBlockLoadingInfo)]
    public interface IMemoryBlockLoadingInfo
    {
        int MemoryBlockIntID { get; set; }
        int MemoryBlockLength { get; set; }
    }
}