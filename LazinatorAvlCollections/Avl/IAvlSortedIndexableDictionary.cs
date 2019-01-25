using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Interfaces;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableDictionary)]
    public interface IAvlSortedIndexableDictionary<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        [DoNotAutogenerate]
        ISortedIndexableKeyMultivalueContainer<TKey, TValue> UnderlyingIndexableTree { get; }
    }
}