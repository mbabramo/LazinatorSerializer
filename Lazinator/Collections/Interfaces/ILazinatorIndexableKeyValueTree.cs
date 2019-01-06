using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorIndexableKeyValueTree)]
    public interface ILazinatorIndexableKeyValueTree<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        TValue GetValueAt(long i);
        void SetValueAt(long i, TValue value);
        void InsertAt(TKey key, TValue value, long index);
        TKey GetKeyAt(long i);
        void SetKeyAt(long i, TKey key);
        bool RemoveAt(long i);

        (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, IComparer<TKey> comparer);
        (bool inserted, long index) Insert(TKey key, TValue value, IComparer<TKey> comparer);
        (bool removed, long index) Remove(TKey key, IComparer<TKey> comparer);
        (bool removed, long index) Remove(TKey key, TValue value, IComparer<TKey> comparer);
        
        (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (bool inserted, long index) Insert(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (bool removed, long index) Remove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);
        (bool removed, long index) Remove(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer);

        IEnumerator<TKey> GetKeyEnumerator(long skip = 0);
        IEnumerator<TValue> GetValueEnumerator(long skip = 0);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(long skip = 0);
    }
}