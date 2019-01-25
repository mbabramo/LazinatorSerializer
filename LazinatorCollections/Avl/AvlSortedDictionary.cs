using Lazinator.Buffers;
using LazinatorCollections.Factories;
using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LazinatorCollections.Avl
{
    public partial class AvlSortedDictionary<TKey, TValue> : IAvlSortedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorDictionaryable<TKey, TValue>, ILazinatorMultivalueDictionaryable<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedDictionary()
        {
        }

        public AvlSortedDictionary(bool allowDuplicates, ContainerFactory factory)
        {
            UnderlyingTree = (ISortedKeyMultivalueContainer<TKey, TValue>)factory.CreatePossiblySortedKeyValueContainer<TKey, TValue>();
            AllowDuplicates = allowDuplicates;
        }

        public AvlSortedDictionary(bool allowDuplicates, ISortedKeyMultivalueContainer<TKey, TValue> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
            AllowDuplicates = allowDuplicates;
        }

        public bool IsSorted => true;

        public bool ContainsKey(TKey key)
        {
            bool result = UnderlyingTree.ContainsKey(key);
            return result;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return UnderlyingTree.ContainsKeyValue(item.Key, item.Value);
        }

        public bool TryGetValue(TKey key, out TValue value) => ValueAtKey(key, out value); // for IDictionary

        public bool ValueAtKey(TKey key, out TValue value)
        {
            bool result = UnderlyingTree.ContainsKey(key);
            if (result)
            {
                value = UnderlyingTree.GetValueForKey(key, AllowDuplicates ? MultivalueLocationOptions.First : MultivalueLocationOptions.Any);
                return true;
            }
            value = default;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                bool result = ValueAtKey(key, out TValue value);
                if (result == false)
                    throw new KeyNotFoundException();
                return value;
            }
            set
            {
                if (AllowDuplicates)
                    throw new Exception("With multiple keys, use AddValue method to add item.");
                bool insertedNotReplaced = UnderlyingTree.SetValueForKey(key, value);
            }
        }


        public void AddValue(TKey key, TValue value)
        {
            UnderlyingTree.AddValueForKey(key, value);
        }

        public void Add(TKey key, TValue value)
        {
            if (AllowDuplicates)
                throw new Exception("With multiple keys, use AddValue method to add item.");
            if (this.ContainsKey(key))
                throw new ArgumentException();
            this[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }


        public long Count => UnderlyingTree is ICountableContainer countable ? countable.LongCount : UnderlyingTree.Count();

        public bool IsReadOnly => false;

        public void Clear()
        {
            UnderlyingTree.Clear();
        }

        public bool Remove(TKey key)
        {
            return UnderlyingTree.TryRemove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return UnderlyingTree.TryRemoveKeyValue(item.Key, item.Value);
        }

        public bool RemoveKeyValue(TKey key, TValue value)
        {
            return UnderlyingTree.TryRemoveKeyValue(key, value);
        }

        public bool RemoveAll(TKey key)
        {
            bool any = Remove(key);
            if (any)
            {
                do
                {
                } while (Remove(key));
            }
            return any;
        }

        public ICollection<TKey> Keys => GetKeysAndValues().Select(x => x.Key).ToList();

        public ICollection<TValue> Values => GetKeysAndValues().Select(x => x.Value).ToList();

        private IEnumerable<KeyValuePair<TKey, TValue>> GetKeysAndValues()
        {
            var enumerator = UnderlyingTree.GetKeyValuePairEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
            enumerator.Dispose();
        }

        public IEnumerable<TKey> GetKeysDistinct()
        {
            bool isFirst = true;
            TKey lastKey = default;
            var enumerator = UnderlyingTree.GetKeyValuePairEnumerator();
            while (enumerator.MoveNext())
            {
                if (isFirst || !EqualityComparer<TKey>.Default.Equals(lastKey, enumerator.Current.Key))
                {
                    lastKey = enumerator.Current.Key;
                    yield return lastKey;
                    isFirst = false;
                }
            }
            enumerator.Dispose();
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count => (int)Count;

        public IEnumerable<TValue> GetAllValues(TKey key)
        {
            return UnderlyingTree.GetAllValues(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (KeyValuePair<TKey, TValue> pair in GetKeysAndValues())
            {
                array[arrayIndex + i++] = pair;
            }
        }

        public IEnumerable<TKey> KeysAsEnumerable(bool reverse = false, long skip = 0) => UnderlyingTree.KeysAsEnumerable(reverse, skip);
        public IEnumerable<TValue> ValuesAsEnumerable(bool reverse = false, long skip = 0) => UnderlyingTree.ValuesAsEnumerable(reverse, skip);
        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairsAsEnumerable(bool reverse = false, long skip = 0) => UnderlyingTree.KeyValuePairsAsEnumerable(reverse, skip);
        public IEnumerable<TKey> KeysAsEnumerable(bool reverse, TKey startKey) => UnderlyingTree.KeysAsEnumerable(reverse, startKey);
        public IEnumerable<TValue> ValuesAsEnumerable(bool reverse, TKey startKey) => UnderlyingTree.ValuesAsEnumerable(reverse, startKey);
        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairsAsEnumerable(bool reverse, TKey startKey) => UnderlyingTree.KeyValuePairsAsEnumerable(reverse, startKey);
        public IEnumerator GetEnumerator() => UnderlyingTree.GetKeyValuePairEnumerator();
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => UnderlyingTree.GetKeyValuePairEnumerator();
        public virtual IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0) => UnderlyingTree.GetKeyEnumerator(reverse, skip);
        public virtual IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0) => UnderlyingTree.GetValueEnumerator(reverse, skip);
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0) => UnderlyingTree.GetKeyValuePairEnumerator(reverse, skip);
        public IEnumerator<TKey> GetKeyEnumerator(bool reverse, TKey startKey) => UnderlyingTree.GetKeyEnumerator(reverse, startKey);
        public IEnumerator<TValue> GetValueEnumerator(bool reverse, TKey startKey) => UnderlyingTree.GetValueEnumerator(reverse, startKey);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse, TKey startKey) => UnderlyingTree.GetKeyValuePairEnumerator(reverse, startKey);

        public void AddValueForKey(TKey key, TValue value)
        {
            ConfirmMultivalue();
            UnderlyingTree.AddValueForKey(key, value);
        }

        public bool TryRemoveKeyValue(TKey key, TValue value)
        {
            ConfirmMultivalue();
            return UnderlyingTree.TryRemoveKeyValue(key, value);
        }

        public bool TryRemoveAll(TKey key)
        {
            ConfirmMultivalue();
            return UnderlyingTree.TryRemoveAll(key);
        }

        private void ConfirmMultivalue()
        {
            if (!AllowDuplicates)
                throw new NotSupportedException("AddValueForKey is for dictionaries with duplicates only.");
        }
    }
}
