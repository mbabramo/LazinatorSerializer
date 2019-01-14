using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedContainer)]
    public interface ISortedContainer<T> : IValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        bool Contains(T item);
        bool TryInsert(T item);
        bool TryRemove(T item);
    }
}
