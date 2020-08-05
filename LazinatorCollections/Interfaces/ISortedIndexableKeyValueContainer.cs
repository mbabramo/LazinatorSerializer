using System;
using Lazinator.Attributes;
using LazinatorCollections.Location;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted indexable key-value pairs
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableKeyValueContainer)]
    public interface ISortedIndexableKeyValueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue>, IIndexableKeyValueContainer<TKey, TValue>, ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) FindIndex(TKey key);
        (long index, bool found) FindIndex(TKey key, TValue value);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value);
    }
}
