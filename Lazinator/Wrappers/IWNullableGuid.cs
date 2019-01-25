﻿using System;
using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableGuid, -1)]
    interface IWNullableGuid : IW<Guid?>
    {
    }
}