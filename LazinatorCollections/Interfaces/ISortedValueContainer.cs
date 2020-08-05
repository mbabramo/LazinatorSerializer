using System;
using System.Collections.Generic;
using Lazinator.Attributes;
using LazinatorCollections.Location;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted containers of values
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedValueContainer)]
    public interface ISortedValueContainer<T> : IValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        bool Contains(T item);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item);
        bool TryRemove(T item);
        IEnumerable<T> AsEnumerable(bool reverse, T startValue);
        IEnumerator<T> GetEnumerator(bool reverse, T startValue);
    }
}
