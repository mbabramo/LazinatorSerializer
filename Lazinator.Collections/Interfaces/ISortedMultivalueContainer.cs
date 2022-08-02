using System;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted containers of multiple values
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedMultivalueContainer)]
    public interface ISortedMultivalueContainer<T> : IMultivalueContainer<T>, ISortedValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne);
        bool TryRemove(T item, MultivalueLocationOptions whichOne);
        long Count(T item);
        bool TryRemoveAll(T item);
    }
}
