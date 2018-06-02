using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WNullableUshort, -1)]
    interface IWNullableUshort : IW<ushort?>
    {
    }
}