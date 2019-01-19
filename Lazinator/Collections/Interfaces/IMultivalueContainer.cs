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
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        /// <summary>
        /// Finds the item in the sorted container, using the comparer. If the item is found, the exact location is returned. Otherwise,
        /// the next location is returned (or null, if it would be after the end of the list).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="whichOne"></param>
        /// <param name="comparer"></param>
        /// <returns>The location, along with an indication of whether an exact match was found</returns>
        (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match);
        bool TryInsert(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemoveAll(T item, IComparer<T> comparer);
        long Count(T item, IComparer<T> comparer);
    }
}