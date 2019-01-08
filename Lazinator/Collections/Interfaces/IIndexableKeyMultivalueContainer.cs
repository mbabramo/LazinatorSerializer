using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableKeyMultivalueContainer)]
    public interface IIndexableKeyMultivalueContainer<TKey, TValue> : IIndexableKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (bool inserted, long index) Insert(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (bool removed, long index) Remove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (bool removed, long index) RemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
    }
}
