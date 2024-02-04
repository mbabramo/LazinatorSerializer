using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedIndexableDictionary)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlSortedIndexableDictionary<TKey, TValue> : IAvlSortedDictionary<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        [DoNotAutogenerate]
        ISortedIndexableKeyMultivalueContainer<TKey, TValue> UnderlyingIndexableTree { get; }
    }
}