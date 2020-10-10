using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for an unsigned short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SingleByteLength]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWUInt16, -1)]
    interface IWUInt16 : IW<ushort>
    {
    }
}