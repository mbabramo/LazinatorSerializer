using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedDictionary)]
    public interface IAvlSortedDictionary<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        ISortedKeyMultivalueContainer<TKey, TValue> UnderlyingTree { get; set; }
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
    }
}