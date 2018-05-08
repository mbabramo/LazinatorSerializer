using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperShort, -1)]
    interface ILazinatorWrapperShort : ILazinatorWrapper<short>
    {
    }
}