﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IOrderableMultivalueContainer)]
    public interface IMultivalueContainer<T> where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
        bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match);
        bool TryInsert(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}