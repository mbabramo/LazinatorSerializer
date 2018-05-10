using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperLongArray, -1)]
    interface ILazinatorWrapperLongArray : ILazinatorWrapper<long[]>
    {
    }
}