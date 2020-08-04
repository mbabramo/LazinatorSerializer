using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A wrapper for a Lazinator type that may or may not automatically have a value.
    /// </summary>
    public interface ILazinatorHasValue
    {
        [DoNotAutogenerate]
        bool HasValue { get; }
    }
}
