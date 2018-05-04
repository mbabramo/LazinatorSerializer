using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableShort, -1)]
    public interface ILazinatorWrapperNullableShort : ILazinatorWrapper<short?>
    {
    }
}