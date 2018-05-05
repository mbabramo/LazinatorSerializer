﻿using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperLong, -1)]
    public interface ILazinatorWrapperLong : ILazinatorWrapper<long>
    {
    }
}