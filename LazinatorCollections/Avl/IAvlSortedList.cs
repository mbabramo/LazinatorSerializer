﻿using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace LazinatorCollections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedList)]
    interface IAvlSortedList<T> where T : ILazinator, IComparable<T>
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        ISortedIndexableMultivalueContainer<T> UnderlyingTree { get; set; }
    }
}
