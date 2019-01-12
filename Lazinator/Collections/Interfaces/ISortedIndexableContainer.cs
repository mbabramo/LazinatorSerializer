using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableContainer)]
    public interface ISortedIndexableContainer<T> : ISortedContainer<T>, IIndexableContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) Find(T target);
        (long index, bool insertedNotReplaced) InsertGetIndex(T item);
    }
}
