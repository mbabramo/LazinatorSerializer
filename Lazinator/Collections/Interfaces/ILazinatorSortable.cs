using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// An interface allowing insertion and removal of sorted items, using a custom comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorListable<T>, ILazinator where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
        (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer);
        bool TryRemove(T item, IComparer<T> comparer);
        (long index, bool exists) Find(T target, IComparer<T> comparer);

        (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
