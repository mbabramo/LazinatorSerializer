using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a nullable decimal. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableDecimal, -1)]
    interface IWNullableDecimal : IW<decimal?>
    {
    }
}