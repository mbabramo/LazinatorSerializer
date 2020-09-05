using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a float. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SmallLazinator]
    [FixedLengthLazinator(4)]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWFloat, -1)]
    interface IWFloat : IW<float>
    {
    }
}