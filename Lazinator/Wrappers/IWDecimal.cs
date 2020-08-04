using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a decimal. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWDecimal, -1)]
    interface IWDecimal : IW<decimal>
    {
    }
}