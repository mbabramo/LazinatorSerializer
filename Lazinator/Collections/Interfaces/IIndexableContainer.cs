﻿using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableContainer)]
    public interface IIndexableContainer<T> : IOrderableContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        T GetAt(long index);
        void SetAt(long index, T value);
        void InsertAt(long index, T item);
        void RemoveAt(long index);

        (long index, bool exists) FindSorted(T target, IComparer<T> comparer);
        (long index, bool insertedNotReplaced) InsertSorted(T item, IComparer<T> comparer);
        bool RemoveSorted(T item, IComparer<T> comparer);
    }
}
