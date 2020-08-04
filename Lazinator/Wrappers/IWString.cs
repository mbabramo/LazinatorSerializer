using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a string. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [Lazinator((int)LazinatorCoreUniqueIDs.IWString, -1)]
    [NonbinaryHash]
    [GenerateRefStruct]
    interface IWString : IW<string>
    {
    }
}