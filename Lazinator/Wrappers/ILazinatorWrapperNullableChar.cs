using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableChar, -1)]
    public interface ILazinatorWrapperNullableChar : ILazinatorWrapper<char?>
    {
    }
}