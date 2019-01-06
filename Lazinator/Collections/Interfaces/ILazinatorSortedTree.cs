using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedTree)]
    public interface ILazinatorSortedTree<T> where T : ILazinator, IComparable<T>
    {
        bool Contains(T item);
        bool TryInsertSorted(T item);
        bool TryRemoveSorted(T item);
        bool TryInsertSorted(T item, MultivalueLocationOptions whichOne);
        bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne);
        bool AllowDuplicates { get; set; }
    }
}
