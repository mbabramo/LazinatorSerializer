using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUlong, -1)]
    interface ILazinatorWrapperUlong : ILazinatorWrapper<ulong>
    {
    }
}