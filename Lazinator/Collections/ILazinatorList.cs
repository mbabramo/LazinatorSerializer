﻿using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorList)]
    interface ILazinatorList<T> where T : ILazinator, new()
    {
        LazinatorOffsetList Offsets { get; set; }
    }
}