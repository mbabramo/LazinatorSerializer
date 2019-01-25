﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableKeyValueTree)]
    internal interface IAvlIndexableKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}