using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableMultivalueContainer)]
    public interface IIndexableMultivalueContainer<T> : IOrderableContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        (long index, bool insertedNotReplaced) InsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool RemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
