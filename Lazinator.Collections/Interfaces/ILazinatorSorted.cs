﻿using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    /// <summary>
    /// A nonexclusive interface allowing insertion and removal of comparable items, using the default comparer for a type implementing IComparable<typeparamref name="T"/>
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
