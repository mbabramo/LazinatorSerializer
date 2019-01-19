﻿using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IValueContainer)]
    public interface IValueContainer<T> : IEnumerable<T>, ILazinator where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        IValueContainer<T> CreateNewWithSameSettings();
        IValueContainer<T> SplitOff(IComparer<T> comparer);
        bool Any();
        T First();
        T FirstOrDefault();
        T Last();
        T LastOrDefault();
        /// <summary>
        /// Finds the item in the sorted container, using the comparer. If the item is found, the exact location is returned. Otherwise,
        /// the next location is returned (or null, if it would be after the end of the list).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns>The location, along with an indication of whether an exact match was found</returns>
        (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer);
        /// <summary>
        /// Gets a matching value using a custom comparer, which may match on only part of the item.
        /// </summary>
        bool GetValue(T item, IComparer<T> comparer, out T match);
        /// <summary>
        /// Inserts a matching value, replacing any existing match.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns>True if the result is an insertion rather than a replacement</returns>
        bool TryInsert(T item, IComparer<T> comparer);
        /// <summary>
        /// Removes a matching value.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        bool TryRemove(T item, IComparer<T> comparer);
        void Clear();
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
        IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0);
    }
}
