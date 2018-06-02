using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WDecimal, -1)]
    interface IWDecimal : IW<decimal>
    {
    }
}