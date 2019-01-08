using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IOrderableContainer)]
    public interface IValueContainer<T> where T : ILazinator
    {
        IValueContainer<T> CreateNewWithSameSettings();
        bool Contains(T item, IComparer<T> comparer);
        /// <summary>
        /// Gets a matching value using a custom comparer, which may match on only part of the item.
        /// </summary>
        bool GetValue(T item, IComparer<T> comparer, out T match);
        bool TryInsert(T item, IComparer<T> comparer);
        bool TryRemove(T item, IComparer<T> comparer);
        void Clear();
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
        IEnumerator<T> GetEnumerator();
    }
}
