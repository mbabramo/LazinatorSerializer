using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedIndexableKeyValueTree)]
    public interface IAvlSortedIndexableKeyValueTree<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
    }
}