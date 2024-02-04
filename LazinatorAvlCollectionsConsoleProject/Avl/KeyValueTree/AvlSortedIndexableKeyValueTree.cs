using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    /// <summary>
    /// An Avl key-value tree, for one or more values, where the items are stored in sorted order and can be accessed by index.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class AvlSortedIndexableKeyValueTree<TKey, TValue> : AvlIndexableKeyValueTree<TKey, TValue>, IAvlSortedIndexableKeyValueTree<TKey, TValue>, ISortedIndexableKeyValueContainer<TKey, TValue>, ISortedIndexableKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedIndexableKeyValueTree(ContainerFactory innerContainerFactory, bool allowDuplicates, bool unbalanced) : base(innerContainerFactory, allowDuplicates, unbalanced)
        {
        }

        public override IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableKeyValueTree<TKey, TValue>(InnerContainerFactory, AllowDuplicates, Unbalanced);
        }

        // from indexable

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key) => FindIndex(key, Comparer<TKey>.Default);

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value) => InsertOrReplace(key, value, Comparer<TKey>.Default);

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne) => FindIndex(key, whichOne, Comparer<TKey>.Default);

        public (long index, bool found) FindIndex(TKey key, TValue value) => FindIndex(key, value, Comparer<TKey>.Default);

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, MultivalueLocationOptions whichOne) => InsertOrReplace(key, value, whichOne, Comparer<TKey>.Default);

        public bool ContainsKey(TKey key) => ContainsKey(key, Comparer<TKey>.Default);

        // from sortable

        public bool ContainsKeyValue(TKey key, TValue value) => ContainsKeyValue(key, value, Comparer<TKey>.Default);

        public IEnumerable<TValue> GetAllValues(TKey key) => GetAllValues(key, Comparer<TKey>.Default);

        public bool TryRemoveAll(TKey key) => TryRemoveAll(key, Comparer<TKey>.Default);

        public bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne) => SetValueForKey(key, value, whichOne, Comparer<TKey>.Default);

        public bool SetValueForKey(TKey key, TValue value) => SetValueForKey(key, value, MultivalueLocationOptions.Any);

        public void AddValueForKey(TKey key, TValue value) => AddValueForKey(key, value, Comparer<TKey>.Default);

        public bool TryRemove(TKey key, MultivalueLocationOptions whichOne) => TryRemove(key, whichOne, Comparer<TKey>.Default);

        public bool TryRemove(TKey key) => TryRemove(key, MultivalueLocationOptions.First);

        public bool TryRemoveKeyValue(TKey key, TValue value) => TryRemoveKeyValue(key, value, Comparer<TKey>.Default);

        public TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne) => GetValueForKey(key, whichOne, Comparer<TKey>.Default);

        public TValue GetValueForKey(TKey key) => GetValueForKey(key, MultivalueLocationOptions.Any);

        public IEnumerator<TKey> GetKeyEnumerator(bool reverse, TKey startKey) => GetKeyEnumerator(reverse, startKey, Comparer<TKey>.Default);
        public IEnumerator<TValue> GetValueEnumerator(bool reverse, TKey startKey) => GetValueEnumerator(reverse, startKey, Comparer<TKey>.Default);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse, TKey startKey) => GetKeyValuePairEnumerator(reverse, startKey, Comparer<TKey>.Default);
        public IEnumerable<TKey> KeysAsEnumerable(bool reverse, TKey startKey) => KeysAsEnumerable(reverse, startKey, Comparer<TKey>.Default);
        public IEnumerable<TValue> ValuesAsEnumerable(bool reverse, TKey startKey) => ValuesAsEnumerable(reverse, startKey, Comparer<TKey>.Default);
        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairsAsEnumerable(bool reverse, TKey startKey) => KeyValuePairsAsEnumerable(reverse, startKey, Comparer<TKey>.Default);
    }
}
