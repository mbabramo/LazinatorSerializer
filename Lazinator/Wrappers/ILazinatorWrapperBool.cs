﻿using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperBool, -1)]
    interface ILazinatorWrapperBool : ILazinatorWrapper<bool>
    {
    }
}