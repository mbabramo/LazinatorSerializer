using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableFloat, -1)]
    interface IWNullableFloat : IW<float?>
    {
    }
}