using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUlong, -1)]
    interface ILazinatorWrapperUlong : ILazinatorWrapper<ulong>
    {
    }
}