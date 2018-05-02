using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperOpenGeneric)]
    public interface ILazinatorWrapper<T>
    {
        T Value { get; set; }
    }

    // ReadOnlySpan etc. need their own interfaces. For now, we'll define only ReadOnlySpan<char>
}