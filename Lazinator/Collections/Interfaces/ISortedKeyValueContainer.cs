using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedKeyValueTree)]
    public interface ISortedKeyValueContainer<TKey, TValue> : IKeyValueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        bool ContainsKey(TKey key);
        bool ContainsKeyValue(TKey key, TValue value);

        TValue GetValueForKey(TKey key);
        bool SetValueForKey(TKey key, TValue value);

        bool TryRemove(TKey key);
        bool TryRemoveKeyValue(TKey key, TValue value);
    }
}
