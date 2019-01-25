﻿using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [FixedLengthLazinator(2)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWChar, -1)]
    interface IWChar : IW<char>
    {
    }
}