using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;

namespace Lazinator.Examples.Structs
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.INonComparableWrapperString, -1)]
    [NonbinaryHash]
    interface INonComparableWrapperString : IW<string>
    {
    }
}