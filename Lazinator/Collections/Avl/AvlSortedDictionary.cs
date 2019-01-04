using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedDictionary<TKey, TValue> : IAvlSortedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorKeyable<TKey, TValue>, ILazinatorKeyableMultivalue<TKey, TValue>, ILazinatorOrderedKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedDictionary()
        {
        }

        public AvlSortedDictionary(bool allowDuplicates)
        {
            UnderlyingTree = new AvlTree<TKey, TValue>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
        }

        public bool AllowDuplicateKeys
        {
            get
            {
                return UnderlyingTree.AllowDuplicateKeys;
            }
            set
            {
                UnderlyingTree.AllowDuplicateKeys = value;
            }
        }

        public bool ContainsKey(TKey key)
        {
            bool result = UnderlyingTree.ValueAtKey(key, out TValue value);
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            bool result = UnderlyingTree.ValueAtKey(key, out value);
            return result;
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
                if (AllowDuplicateKeys)
                    throw new Exception("With multiple keys, use AddValue method to add item.");
                UnderlyingTree.Insert(key, value);
            }
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            bool allowDuplicates = AllowDuplicateKeys;
            UnderlyingTree = new AvlTree<TKey, TValue>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
        }
        
        public bool Remove(TKey key)
        {
            return UnderlyingTree.Remove(key);
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

        int ICollection<KeyValuePair<TKey, TValue>>.Count => (int) Count;

        public IEnumerable<TValue> GetAllValues(TKey key)
        {
            foreach (var p in EnumerateFrom(key))
            {
                if (p.Key.Equals(key))
                    yield return p.Value;
                else
                    yield break;
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> EnumerateFrom(TKey key)
        {
            var result = UnderlyingTree.GetMatchingOrNextNode(key);
            foreach (var keyValuePair in UnderlyingTree.KeyValuePairs(result.index))
                yield return keyValuePair;
        }

        public void AddValue(TKey key, TValue value)
        {
            UnderlyingTree.Insert(key, value);
        }

        public void Add(TKey key, TValue value)
        {
            if (AllowDuplicateKeys)
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
            if (AllowDuplicateKeys)
            {
                foreach (var v in GetAllValues(item.Key))
                    if (System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(v, item.Value))
                        return true;
                return false;
            }
            else
            {
                bool found = TryGetValue(item.Key, out TValue value);
                return found && System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(value, item.Value);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (KeyValuePair<TKey, TValue> pair in GetKeysAndValues())
            {
                array[arrayIndex + i++] = pair;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                if (AllowDuplicateKeys)
                { // removes a single instance of the key-value pair. can remove all items within a key with RemoveAll.
                    var result = UnderlyingTree.GetMatchingOrNextNode(item.Key);
                    if (result.found == false)
                        return false;
                    bool valueFound = false;
                    int distanceFromFirst = 0;
                    foreach (var value in GetAllValues(item.Key))
                    { 
                        if (item.Value.Equals(value))
                        {
                            valueFound = true;
                            break;
                        }
                        else
                            distanceFromFirst++;
                    }
                    if (valueFound)
                    {
                        UnderlyingTree.RemoveAt(result.index + distanceFromFirst);
                        return true;
                    }
                    return false;
                }
                return Remove(item.Key);
            }
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return UnderlyingTree.GetKeyValuePairEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return UnderlyingTree.GetKeyValuePairEnumerator();
        }

        public IEnumerator<TKey> GetKeyEnumerator()
        {
            return UnderlyingTree.GetKeyEnumerator();
        }

        public IEnumerator<TValue> GetValueEnumerator()
        {
            return UnderlyingTree.GetValueEnumerator();
        }
    }
}
