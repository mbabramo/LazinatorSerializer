using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorOrderedKeyable)]
    public interface ILazinatorOrderedKeyable<TKey, TValue> : ILazinator, ILazinatorKeyable<TKey, TValue>, ILazinatorSplittable where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
        IEnumerator<TKey> GetKeyEnumerator(long skip = 0);
        IEnumerator<TValue> GetValueEnumerator(long skip = 0);
        IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(long skip = 0);

        (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, IComparer<TKey> comparer);
        bool ValueAtKey(TKey key, IComparer<TKey> comparer, out TValue value);
        TValue ValueAtIndex(long i);
        void SetValueAtIndex(long i, TValue value);
        TKey KeyAtIndex(long i);
        void SetKeyAtIndex(long i, TKey key);

        (bool inserted, long location) Insert(TKey key, IComparer<TKey> comparer, TValue value);
        void InsertAtIndex(TKey key, TValue value, long index);
        bool Remove(TKey key, IComparer<TKey> comparer);
        bool RemoveAt(long i);
        long Count { get; }
        
        void Clear();
    }
}
