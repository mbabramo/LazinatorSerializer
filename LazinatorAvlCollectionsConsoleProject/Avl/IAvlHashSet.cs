﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Wrappers;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlHashSet)]
    [SingleParent]
    public interface IAvlHashSet<T> where T : ILazinator
    {
        AvlDictionary<T, Placeholder> UnderlyingDictionary { get; set; }
    }
}