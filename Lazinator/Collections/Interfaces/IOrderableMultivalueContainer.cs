using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IOrderableMultivalueContainer)]
    public interface IOrderableMultivalueContainer<T> where T : ILazinator
    {
        bool GetMatchingItem(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match);
        bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}