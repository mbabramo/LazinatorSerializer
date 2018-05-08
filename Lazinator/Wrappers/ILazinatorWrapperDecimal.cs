using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDecimal, -1)]
    interface ILazinatorWrapperDecimal : ILazinatorWrapper<decimal>
    {
    }
}