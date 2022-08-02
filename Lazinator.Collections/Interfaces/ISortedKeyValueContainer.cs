using System;
using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted containers of key-value pairs
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedKeyValueContainer)]
    public interface ISortedKeyValueContainer<TKey, TValue> : IKeyValueContainer<TKey, TValue>, ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        bool ContainsKey(TKey key);
        bool ContainsKeyValue(TKey key, TValue value);

        TValue GetValueForKey(TKey key);
        bool SetValueForKey(TKey key, TValue value);

        bool TryRemove(TKey key);
        bool TryRemoveKeyValue(TKey key, TValue value);

        IEnumerable<TKey> KeysAsEnumerable(bool reverse, TKey startKey);
        IEnumerable<TValue> ValuesAsEnumerable(bool reverse, TKey startKey);
        IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairsAsEnumerable(bool reverse, TKey startKey);
        IEnumerator<TKey> GetKeyEnumerator(bool reverse, TKey startKey);
        IEnumerator<TValue> GetValueEnumerator(bool reverse, TKey startKey);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse, TKey startKey);
    }
}
