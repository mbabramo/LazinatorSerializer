using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableUint, -1)]
    interface IWNullableUint : IW<uint?>
    {
    }
}