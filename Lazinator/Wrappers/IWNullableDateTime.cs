using System;
using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a nullable DateTime. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SingleByteLength]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableDateTime, -1)]
    interface IWNullableDateTime : IW<DateTime?>
    {
    }
}