using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    interface ILazinatorWrapper<T> : ILazinatorHasValue
    {
        [SetterAccessibility("private")]
        T Value { get; }
    }

    // ReadOnlySpan etc. need their own interfaces. For now, we'll define only ReadOnlySpan<char>
}