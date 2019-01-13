using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedIndexableKeyValueTree<TKey, TValue> : AvlIndexableKeyValueTree<TKey, TValue>, IAvlSortedIndexableKeyValueTree<TKey, TValue>, ISortedIndexableKeyValueContainer<TKey, TValue>, ISortedIndexableKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedIndexableKeyValueTree(bool allowDuplicates) : base(allowDuplicates)
        {
        }

        public override IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableKeyValueTree<TKey, TValue>(AllowDuplicates);
        }

        // from indexable

        public (TValue valueIfFound, long index, bool found) Find(TKey key) => Find(key, Comparer<TKey>.Default);

        public (long index, bool insertedNotReplaced) InsertGetIndex(TKey key, TValue value) => InsertGetIndex(key, value, Comparer<TKey>.Default);

        public (TValue valueIfFound, long index, bool found) Find(TKey key, MultivalueLocationOptions whichOne) => Find(key, whichOne, Comparer<TKey>.Default);

        public (long index, bool found) Find(TKey key, TValue value) => Find(key, value, Comparer<TKey>.Default);

        public (long index, bool insertedNotReplaced) InsertGetIndex(TKey key, TValue value, MultivalueLocationOptions whichOne) => InsertGetIndex(key, value, whichOne, Comparer<TKey>.Default);

        public bool ContainsKey(TKey key) => ContainsKey(key, Comparer<TKey>.Default);

        // from sortable

        public bool ContainsKeyValue(TKey key, TValue value) => ContainsKeyValue(key, value, Comparer<TKey>.Default);

        public IEnumerable<TValue> GetAllValues(TKey key) => GetAllValues(key, Comparer<TKey>.Default);

        public bool TryRemoveAll(TKey item) => TryRemoveAll(item, Comparer<TKey>.Default);

        public bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne) => SetValueForKey(key, value, whichOne, Comparer<TKey>.Default);

        public bool SetValueForKey(TKey key, TValue value) => SetValueForKey(key, value, MultivalueLocationOptions.Any);

        public void AddValueForKey(TKey key, TValue value) => AddValueForKey(key, value, Comparer<TKey>.Default);

        public bool TryRemove(TKey key, MultivalueLocationOptions whichOne) => TryRemove(key, whichOne, Comparer<TKey>.Default);

        public bool TryRemove(TKey key) => TryRemove(key, MultivalueLocationOptions.First);

        public bool TryRemoveKeyValue(TKey key, TValue value) => TryRemoveKeyValue(key, value, Comparer<TKey>.Default);

        public TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne) => GetValueForKey(key, whichOne, Comparer<TKey>.Default);

        public TValue GetValueForKey(TKey key) => GetValueForKey(key, MultivalueLocationOptions.Any);
    }
}
