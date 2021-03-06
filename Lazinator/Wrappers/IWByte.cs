﻿using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a byte. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SizeOfLength(1)]
    [FixedLengthLazinator(1)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWByte, -1)]
    interface IWByte : IW<byte>
    {
    }
}