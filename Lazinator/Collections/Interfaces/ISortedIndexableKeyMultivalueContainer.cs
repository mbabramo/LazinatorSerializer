using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableKeyMultivalueContainer)]
    public interface ISortedIndexableKeyMultivalueContainer<TKey, TValue> : ISortedIndexableKeyValueContainer<TKey, TValue>, ISortedKeyMultivalueContainer<TKey, TValue>, IIndexableKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool exists) Find(TKey key, MultivalueLocationOptions whichOne);

        (long index, bool insertedNotReplaced) InsertGetIndex(TKey key, TValue value, MultivalueLocationOptions whichOne);
    }
}
