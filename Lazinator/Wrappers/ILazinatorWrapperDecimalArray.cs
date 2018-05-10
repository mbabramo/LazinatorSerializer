using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDecimalArray, -1)]
    interface ILazinatorWrapperDecimalArray : ILazinatorWrapper<decimal[]>
    {
    }
}