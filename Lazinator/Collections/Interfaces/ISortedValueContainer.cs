using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedValueContainer)]
    public interface ISortedValueContainer<T> : IValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        bool Contains(T item);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item);
        bool TryRemove(T item);
    }
}
