using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperIntArray, -1)]
    interface ILazinatorWrapperIntArray : ILazinatorWrapper<int[]>
    {
    }
}