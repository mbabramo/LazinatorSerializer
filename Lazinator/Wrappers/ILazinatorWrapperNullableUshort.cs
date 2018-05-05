using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUshort, -1)]
    public interface ILazinatorWrapperNullableUshort : ILazinatorWrapper<ushort?>
    {
    }
}