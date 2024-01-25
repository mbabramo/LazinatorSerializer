using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for indexable key-multivalue containers. This allows finding or replacing items in multivalue containers by key. 
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableKeyMultivalueContainer)]
    public interface IIndexableKeyMultivalueContainer<TKey, TValue> : IIndexableKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue>, ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
    }
}
