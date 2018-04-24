using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableChar)]
    public interface ILazinatorWrapperNullableChar : ILazinatorWrapper<char?>
    {
    }
}