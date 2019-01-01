using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWNullableUshort, -1)]
    interface IWNullableUshort : IW<ushort?>
    {
    }
}