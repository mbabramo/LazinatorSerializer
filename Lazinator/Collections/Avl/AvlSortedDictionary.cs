using Lazinator.Buffers;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedDictionary<TKey, TValue> : IAvlSortedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorDictionaryable<TKey, TValue>, ILazinatorMultivalueDictionaryable<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedDictionary()
        {
        }

        public AvlSortedDictionary(bool allowDuplicates, SortedContainerFactory<TKey> factory)
        {
            UnderlyingTree = (ISortedKeyMultivalueContainer<TKey, TValue>)factory.CreateKeyValueContainer<TValue>();
            AllowDuplicates = allowDuplicates;
        }

        public AvlSortedDictionary(bool allowDuplicates, ISortedKeyMultivalueContainer<TKey, TValue> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
            AllowDuplicates = allowDuplicates;
        }

        public bool IsSorted => true;

        public bool AllowDuplicates
        {
            get => UnderlyingTree.AllowDuplicates;
            set
            {
                if (value != UnderlyingTree.AllowDuplicates)
                    throw new Exception("AllowDuplicates must be same for sorted list and underlying tree.");
            }
        }

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

        public IEnumerator GetEnumerator()
        {
            return UnderlyingTree.GetKeyValuePairEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return UnderlyingTree.GetKeyValuePairEnumerator();
        }

        public IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0)
        {
            return UnderlyingTree.GetKeyEnumerator(reverse, skip);
        }

        public IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0)
        {
            return UnderlyingTree.GetValueEnumerator(reverse, skip);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0)
        {
            return UnderlyingTree.GetKeyValuePairEnumerator(reverse, skip);
        }

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
