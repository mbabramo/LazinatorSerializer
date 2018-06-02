using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WNullableDecimal, -1)]
    interface IWNullableDecimal : IW<decimal?>
    {
    }
}