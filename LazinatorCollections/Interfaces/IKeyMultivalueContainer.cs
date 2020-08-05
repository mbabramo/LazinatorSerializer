using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive interface for Lazinator key-multivalue containers.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IKeyMultivalueContainer)]
    public interface IKeyMultivalueContainer<TKey, TValue> : IKeyValueContainer<TKey, TValue>, ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        IEnumerable<TValue> GetAllValues(TKey key, IComparer<TKey> comparer);
        bool TryRemoveAll(TKey key, IComparer<TKey> comparer);
        
        TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        void AddValueForKey(TKey key, TValue value, IComparer<TKey> comparer);

        bool TryRemove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
    }
}