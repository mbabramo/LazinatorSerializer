using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperFloat, -1)]
    interface ILazinatorWrapperFloat : ILazinatorWrapper<float>
    {
    }
}