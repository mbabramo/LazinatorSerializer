using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedKeyMultivalueContainer)]
    public interface ISortedKeyMultivalueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        IEnumerable<TValue> GetAllValues(TKey key);
        bool TryRemoveAll(TKey item);

        TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne);
        bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne);
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne);
        bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne);
        void AddValueForKey(TKey key, TValue value, IComparer<TKey> comparer);
    }
}
