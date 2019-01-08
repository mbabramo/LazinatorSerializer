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
        (TValue valueIfFound, long index, bool found) Find(TKey key);
        (long index, bool insertedNotReplaced) InsertGetIndex(TKey key, TValue value);
    }
}
