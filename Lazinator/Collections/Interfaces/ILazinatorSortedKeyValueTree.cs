using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedKeyValueTree)]
    public interface ILazinatorSortedKeyValueTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        bool ContainsKey(TKey key);
        bool ContainsKeyValue(TKey key, TValue value);

        TValue ValueForKey(TKey key);
        TValue ValueForKey(TKey key, MultivalueLocationOptions whichOne);
        bool SetValueForKey(TKey key, TValue value);
        bool SetValueForKey(TKey key, MultivalueLocationOptions whichOne, TValue value);

        bool TryRemove(TKey key);
        bool TryRemoveKeyValue(TKey key, TValue value);
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne);
        bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne);
    }
}
