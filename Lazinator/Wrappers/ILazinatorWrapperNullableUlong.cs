using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUlong, -1)]
    public interface ILazinatorWrapperNullableUlong : ILazinatorWrapper<ulong?>
    {
    }
}