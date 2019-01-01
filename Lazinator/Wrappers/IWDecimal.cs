using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWDecimal, -1)]
    interface IWDecimal : IW<decimal>
    {
    }
}