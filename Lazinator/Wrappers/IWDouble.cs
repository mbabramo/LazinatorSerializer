﻿using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWDouble, -1)]
    interface IWDouble : IW<double>
    {
    }
}