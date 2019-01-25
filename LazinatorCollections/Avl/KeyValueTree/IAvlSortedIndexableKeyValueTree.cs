using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableKeyValueTree)]
    public interface IAvlSortedIndexableKeyValueTree<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
    }
}