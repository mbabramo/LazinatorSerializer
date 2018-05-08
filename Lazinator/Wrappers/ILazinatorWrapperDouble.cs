using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDouble, -1)]
    interface ILazinatorWrapperDouble : ILazinatorWrapper<double>
    {
    }
}