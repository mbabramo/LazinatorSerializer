using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedDictionary<TKey, TValue> : IAvlSortedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorKeyable<TKey, TValue>, ILazinatorOrderedKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedDictionary()
        {
        }

        public AvlSortedDictionary(bool allowDuplicates)
        {
            UnderlyingTree = new AvlTree<TKey, TValue>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
            if (allowDuplicates)
                throw new NotImplementedException();
        }

        public bool AllowDuplicates
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
            bool result = UnderlyingTree.Search(key, out TValue value);
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            bool result = UnderlyingTree.Search(key, out value);
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
                UnderlyingTree.Insert(key, value);
            }
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            bool allowDuplicates = AllowDuplicates;
            UnderlyingTree = new AvlTree<TKey, TValue>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
        }


        public bool Remove(TKey item)
        {
            return UnderlyingTree.Remove(item);
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

        public IEnumerable<TKey> EnumerateFrom(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            if (Count == 0)
                yield break;
            foreach (var node in UnderlyingTree.Skip(index))
                yield return node.Key;
        }

        public void Add(TKey key, TValue value)
        {
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
            bool found = TryGetValue(item.Key, out TValue value);
            return found && System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(value, item.Value);
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
                return Remove(item.Key);
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
