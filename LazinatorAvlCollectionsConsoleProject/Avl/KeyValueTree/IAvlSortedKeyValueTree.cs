using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedKeyValueTree)]
    public interface IAvlSortedKeyValueTree<TKey, TValue> : IAvlKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}