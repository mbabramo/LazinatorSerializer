using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperString, -1)]
    interface ILazinatorWrapperString : ILazinatorWrapper<string>
    {
    }
}