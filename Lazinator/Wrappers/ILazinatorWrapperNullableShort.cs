using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableShort)]
    public interface ILazinatorWrapperNullableShort : ILazinatorWrapper<short?>
    {
    }
}