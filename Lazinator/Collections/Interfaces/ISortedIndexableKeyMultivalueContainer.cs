using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableKeyMultivalueContainer)]
    public interface ISortedIndexableKeyMultivalueContainer<TKey, TValue> : ISortedIndexableKeyValueContainer<TKey, TValue>, ISortedKeyMultivalueContainer<TKey, TValue>, IIndexableKeyMultivalueContainer<TKey, TValue>, ILazinator where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne);

        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, MultivalueLocationOptions whichOne);
    }
}
