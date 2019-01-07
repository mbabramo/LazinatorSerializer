using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorTree)]
    public interface ILazinatorTree<T> where T : ILazinator
    {
        bool Contains(T item, IComparer<T> comparer);
        bool TryInsertSorted(T item, IComparer<T> comparer);
        bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemoveSorted(T item, IComparer<T> comparer);
        bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        void Clear();
    }
}
