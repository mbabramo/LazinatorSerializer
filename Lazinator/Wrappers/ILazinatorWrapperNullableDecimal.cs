﻿using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDecimal, -1)]
    public interface ILazinatorWrapperNullableDecimal : ILazinatorWrapper<decimal?>
    {
    }
}