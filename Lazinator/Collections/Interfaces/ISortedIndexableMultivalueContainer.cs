using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableMultivalueContainer)]
    public interface ISortedIndexableMultivalueContainer<T> : ISortedContainer<T>, IIndexableContainer<T>, ISortedIndexableContainer<T>, IIndexableMultivalueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne);
        (long index, bool insertedNotReplaced) InsertSorted(T item, MultivalueLocationOptions whichOne);
        bool RemoveSorted(T item, MultivalueLocationOptions whichOne);
    }
}
