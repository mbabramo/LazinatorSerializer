using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedIndexableDictionary)]
    public interface IAvlSortedIndexableDictionary<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        [DoNotAutogenerate]
        ISortedIndexableKeyMultivalueContainer<TKey, TValue> UnderlyingIndexableTree { get; }
    }
}