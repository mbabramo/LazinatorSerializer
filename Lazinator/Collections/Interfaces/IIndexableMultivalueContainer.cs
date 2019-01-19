using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableMultivalueContainer)]
    public interface IIndexableMultivalueContainer<T> : IIndexableValueContainer<T>, IMultivalueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
