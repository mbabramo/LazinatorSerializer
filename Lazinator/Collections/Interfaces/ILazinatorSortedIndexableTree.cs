using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedIndexableTree)]
    interface ILazinatorSortedIndexableTree<T> where T : ILazinator
    {
        (long index, bool exists) FindSorted(T target);
        (long index, bool rejectedAsDuplicate) InsertSorted(T item);
        (long priorIndex, bool existed) RemoveSorted(T item);
        (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne);
        (long index, bool rejectedAsDuplicate) InsertSorted(T item, MultivalueLocationOptions whichOne);
        (long priorIndex, bool existed) RemoveSorted(T item, MultivalueLocationOptions whichOne);
    }
}
