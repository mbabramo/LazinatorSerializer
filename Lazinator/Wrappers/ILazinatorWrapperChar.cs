using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperChar, -1)]
    interface ILazinatorWrapperChar : ILazinatorWrapper<char>
    {
    }
}