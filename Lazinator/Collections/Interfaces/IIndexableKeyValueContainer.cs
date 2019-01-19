using Lazinator.Attributes;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableKeyValueTree)]
    public interface IIndexableKeyValueContainer<TKey, TValue> : IKeyValueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        TValue GetValueAt(long index);
        void SetValueAt(long index, TValue value);
        TKey GetKeyAt(long index);
        void SetKeyAt(long index, TKey key);
        LazinatorKeyValue<TKey, TValue> GetKeyValueAt(long index);
        void SetKeyValueAt(long index, TKey key, TValue value);
        void InsertAt(long index, TKey key, TValue value);
        void RemoveAt(long index);

        (TValue valueIfFound, long index, bool found) Find(TKey key, IComparer<TKey> comparer);
        (long index, bool found) Find(TKey key, TValue value, IComparer<TKey> comparer);
        (long index, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, IComparer<TKey> comparer);

    }
}