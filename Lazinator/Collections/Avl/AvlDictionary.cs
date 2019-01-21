using Lazinator.Buffers;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Support;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlDictionary<TKey, TValue> : IAvlDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorDictionaryable<TKey, TValue>, ILazinatorMultivalueDictionaryable<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>> where TKey : ILazinator where TValue : ILazinator
    {

        public AvlDictionary()
        {
        }

        public AvlDictionary(bool allowDuplicates, ContainerFactory innerFactory)
        {
            UnderlyingTree = (ISortedKeyMultivalueContainer<WUint, LazinatorKeyValue<TKey, TValue>>) innerFactory.GetHashableKeyValueContainer<TKey, TValue>();
            if (UnderlyingTree.AllowDuplicates == false)
                throw new Exception("AvlDictionary requires an UnderlyingTree that allows duplicates."); // the underlying tree is organized by the hash value, and multiple items can share a hash value, regardless of whether multiple items can share a key
            AllowDuplicates = allowDuplicates;
        }

        public bool IsSorted => false;

        public IEnumerable<TValue> GetAllValues(TKey key)
        {
            uint hash = key.GetBinaryHashCode32();
            return GetHashMatches(key, hash);
        }

        private IEnumerable<TValue> GetHashMatches(TKey key, uint hash)
        {
            return GetAllInHashBucket(hash).Where(x => EqualityComparer<TKey>.Default.Equals(x.Key, key)).Select(x => x.Value);
        }

        private IEnumerable<LazinatorKeyValue<TKey, TValue>> GetAllInHashBucket(uint hash)
        {
            foreach (LazinatorKeyValue<TKey, TValue> keyValue in UnderlyingTree.GetAllValues(hash))
            {
                if (keyValue.Key.GetBinaryHashCode32().Equals(hash))
                    yield return keyValue;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return GetAllValues(key).Any();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            uint hash = key.GetBinaryHashCode32();
            var values = GetAllValues(key);
            if (values.Any())
            {
                value = values.First();
                return true;
            }
            value = default;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                bool result = TryGetValue(key, out TValue value);
                if (result == false)
                    throw new KeyNotFoundException();
                return value;
            }
            set
            {
                if (AllowDuplicates)
                    throw new Exception("With multiple keys, use AddValue method to add item.");
                Remove(key);
                UnderlyingTree.AddValueForKey(key.GetBinaryHashCode32(), new LazinatorKeyValue<TKey, TValue>(key, value));
            }
        }

        public long Count => UnderlyingTree is ICountableContainer countable ? countable.LongCount : UnderlyingTree.Count();

        public bool IsReadOnly => false;

        public void Clear()
        {
            UnderlyingTree.Clear();
        }

        public bool Remove(TKey key)
        {
            uint hash = key.GetBinaryHashCode32();
            bool result = TryGetValue(key, out TValue value2);
            if (result)
                UnderlyingTree.TryRemoveKeyValue(hash, new LazinatorKeyValue<TKey, TValue>(key, value2));
            return result;
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

        int ICollection<KeyValuePair<TKey, TValue>>.Count => (int) Count;

        private IEnumerable<LazinatorKeyValue<TKey, TValue>> GetKeysAndValues()
        {
            var enumerator = UnderlyingTree.GetValueEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
            enumerator.Dispose();
        }

        public void AddValue(TKey key, TValue value)
        {
            if (AllowDuplicates)
            {
                uint hash = key.GetBinaryHashCode32();
                UnderlyingTree.AddValueForKey(hash, new LazinatorKeyValue<TKey, TValue>(key, value));
            }
            else
                this[key] = value;
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

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return UnderlyingTree.ContainsKeyValue(item.Key.GetBinaryHashCode32(), new LazinatorKeyValue<TKey, TValue>(item.Key, item.Value));
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (LazinatorKeyValue<TKey, TValue> pair in GetKeysAndValues())
            {
                array[arrayIndex + i++] = new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            uint hash = item.Key.GetBinaryHashCode32();
            bool result = UnderlyingTree.TryRemoveKeyValue(hash, new LazinatorKeyValue<TKey, TValue>(item.Key, item.Value));
            return result;
        }

        public bool RemoveKeyValue(TKey key, TValue value)
        {
            uint hash = key.GetBinaryHashCode32();
            bool result = UnderlyingTree.TryRemoveKeyValue(hash, new LazinatorKeyValue<TKey, TValue>(key, value));
            return result;
        }

        public IEnumerator GetEnumerator()
        {
            return UnderlyingTree.GetKeyValuePairEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetKeyValuePairEnumerator();
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator()
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, KeyValuePair<TKey, TValue>>(GetUnderlyingTree2ValueEnumerator(), x => new KeyValuePair<TKey, TValue>(x.Key, x.Value));
        }

        public IEnumerator<TKey> GetKeyEnumerator()
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, TKey>(GetUnderlyingTree2ValueEnumerator(), x => x.Key);
        }

        public IEnumerator<TValue> GetValueEnumerator()
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, TValue>(GetUnderlyingTree2ValueEnumerator(), x => x.Value);
        }

        private IEnumerator<LazinatorKeyValue<TKey, TValue>> GetUnderlyingTree2ValueEnumerator()
        {
            return UnderlyingTree.GetValueEnumerator();
        }

        public void AddValueForKey(TKey key, TValue value)
        {
            ConfirmMultivalue();
            uint hash = key.GetBinaryHashCode32();
            LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key, value);
            UnderlyingTree.AddValueForKey(hash, keyValue);
        }

        public bool TryRemoveKeyValue(TKey key, TValue value)
        {
            ConfirmMultivalue();
            uint hash = key.GetBinaryHashCode32();
            LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key, value);
            bool result = UnderlyingTree.TryRemoveKeyValue(hash, keyValue);
            return result;
        }

        public bool TryRemoveAll(TKey key)
        {
            ConfirmMultivalue();
            return RemoveAll(key);
        }

        private void ConfirmMultivalue()
        {
            if (!AllowDuplicates)
                throw new NotSupportedException("AddValueForKey is for dictionaries with duplicates only.");
        }
    }
}
