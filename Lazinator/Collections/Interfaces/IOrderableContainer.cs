using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IOrderableContainer)]
    public interface IOrderableContainer<T> where T : ILazinator
    {
        IOrderableContainer<T> CreateNewWithSameSettings();
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
        IEnumerator<T> GetEnumerator();
        bool Contains(T item, IComparer<T> comparer);
        bool GetMatchingItem(T item, IComparer<T> comparer, out T match);
        bool TryInsertSorted(T item, IComparer<T> comparer);
        bool TryRemoveSorted(T item, IComparer<T> comparer);
        void Clear();
    }
}
