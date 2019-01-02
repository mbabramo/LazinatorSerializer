using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedList)]
    interface IAvlSortedList<T> where T : ILazinator, IComparable<T>
    {
        AvlTree<T, Placeholder> UnderlyingTree { get; set; }
    }
}

