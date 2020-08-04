﻿using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWShort, -1)]
    interface IWShort : IW<short>
    {
    }
}