using Lazinator.Attributes;

namespace Lazinator.Buffers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IMemoryBlockInsetLoadingInfo)]
    public interface IMemoryBlockInsetLoadingInfo : IMemoryBlockLoadingInfo
    {
        long OffsetWhenLoading { get; set; }
    }
}