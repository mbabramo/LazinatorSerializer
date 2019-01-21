using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedMultivalueContainer)]
    public interface ISortedMultivalueContainer<T> : IMultivalueContainer<T>, ISortedValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne);
        bool TryRemove(T item, MultivalueLocationOptions whichOne);
        long Count(T item);
        bool TryRemoveAll(T item);
    }
}
