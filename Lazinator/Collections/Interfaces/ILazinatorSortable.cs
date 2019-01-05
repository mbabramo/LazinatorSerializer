using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorListable<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
        (long location, bool rejectedAsDuplicate) InsertSorted(T item);
        (long location, bool rejectedAsDuplicate) InsertSorted(T item, IComparer<T> comparer);
        (long priorLocation, bool existed) RemoveSorted(T item);
        (long priorLocation, bool existed) RemoveSorted(T item, IComparer<T> comparer);
        (long location, bool exists) FindSorted(T target);
        (long location, bool exists) FindSorted(T target, IComparer<T> comparer);

        // Note: The reason that we pass comparer to individual methods is that one might want to use a comparer that looks for a match based on part of the target (e.g., if target is a Key-Value Pair, only based on the key).
    }
}
