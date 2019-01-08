using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedIndexableKeyValueTree)]
    public interface ISortedIndexableKeyValueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue>, IIndexableKeyValueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key);
        (bool inserted, long index) Insert(TKey key, TValue value);
        (bool removed, long index) Remove(TKey key);
        (bool removed, long index) RemoveKeyValue(TKey key, TValue value);
    }
}
