using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUshort, -1)]
    interface ILazinatorWrapperNullableUshort : ILazinatorWrapper<ushort?>
    {
    }
}