using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUshort, -1)]
    interface ILazinatorWrapperUshort : ILazinatorWrapper<ushort>
    {
    }
}