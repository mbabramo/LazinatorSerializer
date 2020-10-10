using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SingleByteLength]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWInt64, -1)]
    interface IWInt64 : IW<long>
    {
    }
}