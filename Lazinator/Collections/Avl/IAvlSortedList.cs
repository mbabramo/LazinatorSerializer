using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedList)]
    interface IAvlSortedList<T> where T : ILazinator, IComparable<T>
    {
        ISortedIndexableContainer<T> UnderlyingTree { get; set; }
    }
}

