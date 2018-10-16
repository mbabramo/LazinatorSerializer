using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    interface IW<T> : ILazinatorHasValue
    {
        [SetterAccessibility("private")]
        T WrappedValue { get; }
    }

    // ReadOnlySpan etc. need their own interfaces. For now, we'll define only ReadOnlySpan<char>
}