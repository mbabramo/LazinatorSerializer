using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedList)]
    interface IAvlSortedList<T> where T : ILazinator, IComparable<T>
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        ISortedIndexableMultivalueContainer<T> UnderlyingTree { get; set; }
    }
}

