using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedList)]
    [SingleParent]
    [AsyncLazinatorMemory]
    interface IAvlSortedList<T> where T : ILazinator, IComparable<T>
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        ISortedIndexableMultivalueContainer<T> UnderlyingTree { get; set; }
    }
}

