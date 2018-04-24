using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUlong)]
    public interface ILazinatorWrapperNullableUlong : ILazinatorWrapper<ulong?>
    {
    }
}