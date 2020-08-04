using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A generic Lazinator wrapper. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IW<T> : ILazinatorHasValue
    {
        [SetterAccessibility("private")]
        T WrappedValue { get; }
    }
}