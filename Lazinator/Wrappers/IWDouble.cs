using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a double. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SingleByteLength]
    [FixedLengthLazinator(8)]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWDouble, -1)]
    interface IWDouble : IW<double>
    {
    }
}