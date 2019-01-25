using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableDecimal, -1)]
    interface IWNullableDecimal : IW<decimal?>
    {
    }
}