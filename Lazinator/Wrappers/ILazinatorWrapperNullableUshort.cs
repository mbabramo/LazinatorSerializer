using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUshort)]
    public interface ILazinatorWrapperNullableUshort : ILazinatorWrapper<ushort?>
    {
    }
}