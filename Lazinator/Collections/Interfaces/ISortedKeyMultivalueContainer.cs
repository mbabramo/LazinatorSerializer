using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedKeyMultivalueContainer]
    public interface ISortedKeyMultivalueContainer<TKey, TValue> : ISortedKeyValueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        void AddValue(TKey key, TValue value);
        IEnumerable<TValue> GetAllValues(TKey key);
        bool RemoveAll(TKey item);

        TValue ValueForKey(TKey key, MultivalueLocationOptions whichOne);
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne);
        bool SetValueForKey(TKey key, MultivalueLocationOptions whichOne, TValue value);
        bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne);
    }
}
