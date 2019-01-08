using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorIndexableKeyValueTree)]
    public interface IIndexableKeyValueContainer<TKey, TValue> : IKeyValueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        TValue GetValueAt(long i);
        void SetValueAt(long i, TValue value);
        TKey GetKeyAt(long i);
        void SetKeyAt(long i, TKey key);
        void InsertAt(TKey key, TValue value, long index);
        bool RemoveAt(long i);

        (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, IComparer<TKey> comparer);
        (bool inserted, long index) Insert(TKey key, TValue value, IComparer<TKey> comparer);

        (bool removed, long index) Remove(TKey key, IComparer<TKey> comparer);
        (bool removed, long index) RemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer);

    }
}