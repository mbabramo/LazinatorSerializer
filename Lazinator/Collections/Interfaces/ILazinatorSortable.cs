using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorCountableListable<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
        (long location, bool rejectedAsDuplicate) InsertSorted(T item);
        (long location, bool rejectedAsDuplicate) InsertSorted(T item, IComparer<T> comparer);
        (long priorLocation, bool existed) RemoveSorted(T item);
        (long location, bool exists) FindSorted(T target);
        (long location, bool exists) FindSorted(T target, IComparer<T> comparer);
    }
}
