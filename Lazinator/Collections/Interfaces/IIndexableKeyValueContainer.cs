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
        TValue GetValueAt(long index);
        void SetValueAt(long index, TValue value);
        TKey GetKeyAt(long index);
        void SetKeyAt(long index, TKey key);
        void InsertAt(TKey key, TValue value, long index);
        bool RemoveAt(long index);

        (TValue valueIfFound, long index, bool found) Find(TKey key, IComparer<TKey> comparer);
        (bool inserted, long index) InsertGetIndex(TKey key, TValue value, IComparer<TKey> comparer);

    }
}