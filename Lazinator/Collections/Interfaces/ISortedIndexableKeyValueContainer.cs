using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableKeyValueContainer)]
    public interface ISortedIndexableKeyValueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue>, IIndexableKeyValueContainer<TKey, TValue>, ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) Find(TKey key);
        (long index, bool found) Find(TKey key, TValue value);
        (long index, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value);
    }
}
