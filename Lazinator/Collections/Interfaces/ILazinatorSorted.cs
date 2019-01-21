using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// An interface allowing insertion and removal of comparable items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSorted)]
    public interface ILazinatorSorted<T> : ILazinatorSortable<T> where T : ILazinator, IComparable<T>
    {
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item);
        bool TryRemove(T item);
        (long index, bool exists) FindIndex(T target);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne);
        bool TryRemove(T item, MultivalueLocationOptions whichOne);
        (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne);
    }
}
