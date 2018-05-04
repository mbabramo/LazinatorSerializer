﻿using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableBool, -1)]
    public interface ILazinatorWrapperNullableBool : ILazinatorWrapper<bool?>
    {
    }
}