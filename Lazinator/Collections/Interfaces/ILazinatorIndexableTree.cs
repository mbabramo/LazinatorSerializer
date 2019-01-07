﻿using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorIndexableTree)]
    public interface ILazinatorIndexableTree<T> : ILazinatorTree<T> where T : ILazinator
    {
        T GetAt(long index);
        void SetAt(long index, T value);
        void InsertAt(long index, T item);
        void RemoveAt(long index);
        IEnumerable<T> AsEnumerable(long skip = 0);

        (long location, bool exists) FindSorted(T target, IComparer<T> comparer);
        (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        (long index, bool rejectedAsDuplicate) InsertSorted(T item, IComparer<T> comparer);
        (long index, bool rejectedAsDuplicate) InsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        (long priorIndex, bool existed) RemoveSorted(T item, IComparer<T> comparer);
        (long priorIndex, bool existed) RemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
