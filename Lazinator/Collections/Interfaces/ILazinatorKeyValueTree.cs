using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyValueTree)]
    public interface ILazinatorKeyValueTree<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        bool ContainsKey(TKey key, IComparer<TKey> comparer);
        bool ContainsKeyValue(TKey key, TValue value, IComparer<TKey> comparer);

        TValue ValueForKey(TKey key, IComparer<TKey> comparer);
        TValue ValueForKey(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        bool SetValueForKey(TKey key, TValue value, IComparer<TKey> comparer);
        bool SetValueForKey(TKey key, MultivalueLocationOptions whichOne, TValue value, IComparer<TKey> comparer);

        bool TryRemove(TKey key, IComparer<TKey> comparer);
        bool TryRemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer);
        bool TryRemove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
    }
}
