using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WNullableDecimal, -1)]
    interface IWNullableDecimal : IW<decimal?>
    {
    }
}