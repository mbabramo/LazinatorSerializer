﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlHashSet)]
    public interface IAvlHashSet<T> where T : ILazinator
    {
        AvlDictionary<T, Placeholder> UnderlyingDictionary { get; set; }
    }
}