using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IKeyValueContainer)]
    public interface IKeyValueContainer<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings();
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }

        bool ContainsKey(TKey key, IComparer<TKey> comparer);
        bool ContainsKeyValue(TKey key, TValue value, IComparer<TKey> comparer);

        TValue GetValueForKey(TKey key, IComparer<TKey> comparer);
        bool SetValueForKey(TKey key, TValue value, IComparer<TKey> comparer);

        bool TryRemove(TKey key, IComparer<TKey> comparer);
        bool TryRemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer);

        void Clear();
        
        IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0);
    }
}
