using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyValueTree)]
    public interface IKeyValueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool ContainsKey(TKey key, IComparer<TKey> comparer);
        bool ContainsKeyValue(TKey key, TValue value, IComparer<TKey> comparer);

        TValue GetValueForKey(TKey key, IComparer<TKey> comparer);
        bool SetValueForKey(TKey key, TValue value, IComparer<TKey> comparer);

        bool TryRemove(TKey key, IComparer<TKey> comparer);
        bool TryRemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer);

        // DEBUG: Should be in base but implemented better in indexable and its derivatives
        IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0);
    }
}
