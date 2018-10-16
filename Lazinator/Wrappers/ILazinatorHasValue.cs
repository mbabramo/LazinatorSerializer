using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    public interface ILazinatorHasValue
    {
        [DoNotAutogenerate]
        bool HasValue { get; }
    }
}
