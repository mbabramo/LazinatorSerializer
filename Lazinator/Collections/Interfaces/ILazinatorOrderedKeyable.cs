using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorOrderedKeyable)]
    public interface ILazinatorOrderedKeyable<TKey, TValue> : ILazinator, ILazinatorKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        IEnumerator<TKey> GetKeyEnumerator(long skip = 0);
        IEnumerator<TValue> GetValueEnumerator(long skip = 0);
        TValue ValueAtIndex(long i);
        void SetValueAtIndex(long i, TValue value);
        void InsertAtIndex(TKey key, TValue value, long index);
        bool RemoveAt(long i);
        long Count { get; }
    }
}
