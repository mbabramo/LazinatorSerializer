﻿using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [FixedLengthLazinator(1)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableBool, -1)]
    interface IWNullableBool : IW<bool?>
    {
    }
}