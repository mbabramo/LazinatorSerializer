using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// An interface allowing insertion and removal of sorted comparable items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorListable<T> where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
        (long index, bool insertedNotReplaced) InsertSorted(T item);
        (long priorIndex, bool existed) RemoveSorted(T item);
        (long index, bool exists) FindSorted(T target);
        (long index, bool insertedNotReplaced) InsertSorted(T item, IComparer<T> comparer);
        (long priorIndex, bool existed) RemoveSorted(T item, IComparer<T> comparer);
        (long index, bool exists) FindSorted(T target, IComparer<T> comparer);
    }
}
