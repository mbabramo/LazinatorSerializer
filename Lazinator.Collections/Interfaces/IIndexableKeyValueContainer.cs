using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System.Collections.Generic;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for indexable key-value containers, allowing getting and setting keys and values and insertions and removals at a particular index.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableKeyValueTree)]
    public interface IIndexableKeyValueContainer<TKey, TValue> : IKeyValueContainer<TKey, TValue>, ICountableContainer where TKey : ILazinator where TValue : ILazinator
    {
        TValue GetValueAtIndex(long index);
        void SetValueAtIndex(long index, TValue value);
        TKey GetKeyAtIndex(long index);
        void SetKeyAtIndex(long index, TKey key);
        LazinatorKeyValue<TKey, TValue> GetKeyValueAtIndex(long index);
        void SetKeyValueAtIndex(long index, TKey key, TValue value);
        void InsertAtIndex(long index, TKey key, TValue value);
        void RemoveAtIndex(long index);

        (TValue valueIfFound, long index, bool found) FindIndex(TKey key, IComparer<TKey> comparer);
        (long index, bool found) FindIndex(TKey key, TValue value, IComparer<TKey> comparer);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, IComparer<TKey> comparer);

    }
}