using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IOrderableContainer)]
    public interface IValueContainer<T> : IEnumerable<T> where T : ILazinator
    {
        bool Unbalanced { get; set; }
        bool AllowDuplicates { get; set; }
        IValueContainer<T> CreateNewWithSameSettings();
        bool Contains(T item, IComparer<T> comparer);
        /// <summary>
        /// Gets a matching value using a custom comparer, which may match on only part of the item.
        /// </summary>
        bool GetValue(T item, IComparer<T> comparer, out T match);
        /// <summary>
        /// Inserts a matching value, replacing any existing match.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
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
