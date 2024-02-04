using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedIndexableKeyValueTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlSortedIndexableKeyValueTree<TKey, TValue> : IAvlIndexableKeyValueTree<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
    }
}