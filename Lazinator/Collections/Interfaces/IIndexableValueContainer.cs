using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableContainer)]
    public interface IIndexableValueContainer<T> : IValueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        T GetAt(long index);
        void SetAt(long index, T value);
        void InsertAt(long index, T item);
        void RemoveAt(long index);

        (long index, bool exists) Find(T target, IComparer<T> comparer);
        (long index, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer);
    }
}
