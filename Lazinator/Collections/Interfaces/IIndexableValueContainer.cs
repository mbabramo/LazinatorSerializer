using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableValueContainer)]
    public interface IIndexableValueContainer<T> : IValueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        T GetAtIndex(long index);
        void SetAtIndex(long index, T value);
        void InsertAtIndex(long index, T item);
        void RemoveAtIndex(long index);

        (long index, bool exists) FindIndex(T target, IComparer<T> comparer);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer);
    }
}
