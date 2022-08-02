using System;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted indexable key-multivalue pairs
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableKeyMultivalueContainer)]
    public interface ISortedIndexableKeyMultivalueContainer<TKey, TValue> : ISortedIndexableKeyValueContainer<TKey, TValue>, ISortedKeyMultivalueContainer<TKey, TValue>, IIndexableKeyMultivalueContainer<TKey, TValue>, ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne);

        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, MultivalueLocationOptions whichOne);
    }
}
