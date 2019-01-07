﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedIndexableTree)]
    public interface ISortedIndexableContainer<T> : ISortedContainer<T>, IIndexableContainer<T> where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) FindSorted(T target);
        (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne);
        (long index, bool insertedNotReplaced) InsertSorted(T item);
        (long index, bool insertedNotReplaced) InsertSorted(T item, MultivalueLocationOptions whichOne);
        (long priorIndex, bool existed) RemoveSorted(T item);
        (long priorIndex, bool existed) RemoveSorted(T item, MultivalueLocationOptions whichOne);
    }
}