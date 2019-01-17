using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.KeyValueTree
{
    public partial class AvlSortedKeyValueTree<TKey, TValue> : AvlKeyValueTree<TKey, TValue>, IAvlSortedKeyValueTree<TKey, TValue>, ISortedKeyValueContainer<TKey, TValue>, ISortedKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedKeyValueTree(bool allowDuplicates) : base(allowDuplicates)
        {
        }

        public override IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlSortedKeyValueTree<TKey, TValue>(AllowDuplicates);
        }

        public bool ContainsKey(TKey key) => ContainsKey(key, Comparer<TKey>.Default);

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
    }
}
