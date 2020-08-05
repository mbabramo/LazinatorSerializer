using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for key-value containers.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IKeyValueContainer)]
    public interface IKeyValueContainer<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IKeyAndValueEnumerators<TKey, TValue>, ILazinator where TKey : ILazinator where TValue : ILazinator
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
    }
}
