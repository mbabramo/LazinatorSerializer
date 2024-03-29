﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableKeyValueTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlIndexableKeyValueTree<TKey, TValue> : IAvlKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}