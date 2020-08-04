using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for an unsigned short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWUshort, -1)]
    interface IWUshort : IW<ushort>
    {
    }
}