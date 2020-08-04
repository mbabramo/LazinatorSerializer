using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a nullable unsigned long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableUlong, -1)]
    interface IWNullableUlong : IW<ulong?>
    {
    }
}