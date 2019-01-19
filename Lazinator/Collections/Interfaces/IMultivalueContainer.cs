using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultivalueContainer)]
    public interface IMultivalueContainer<T> : IValueContainer<T>, ILazinator where T : ILazinator
    {
        bool AllowDuplicates { get; }
        bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match);
        bool TryInsert(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemoveAll(T item, IComparer<T> comparer);
        long Count(T item, IComparer<T> comparer);
    }
}