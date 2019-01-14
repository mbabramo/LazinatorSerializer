﻿using Lazinator.Buffers;
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
    public partial class AvlSortedDictionary<TKey, TValue> : IAvlSortedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorDictionaryable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedDictionary()
        {
        }

        public AvlSortedDictionary(bool allowDuplicates, ISortedKeyMultivalueContainerFactory<TKey, TValue> factory)
        {
            UnderlyingTree = factory.Create();
            AllowDuplicates = allowDuplicates;
        }

        public AvlSortedDictionary(bool allowDuplicates, ISortedKeyMultivalueContainer<TKey, TValue> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
            AllowDuplicates = allowDuplicates;
        }

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
            bool result = UnderlyingTree.ValueAtKey(key, Comparer<TKey>.Default, out TValue value);
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value) => ValueAtKey(key, out value); // for IDictionary

        public bool ValueAtKey(TKey key, out TValue value)
        {
            bool result = UnderlyingTree.ValueAtKey(key, Comparer<TKey>.Default, out value);
            return result;
        }

        public bool ValueAtKey(TKey key, IComparer<TKey> comparer, out TValue value)
        {
            bool result = UnderlyingTree.ValueAtKey(key, comparer, out value);
            return result;
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
                UnderlyingTree.Insert(key, Comparer<TKey>.Default, value);
            }
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            UnderlyingTree.Clear();
        }

        public bool Remove(TKey key, IComparer<TKey> comparer)
        {
            return UnderlyingTree.Remove(key, comparer);
        }
        public bool Remove(TKey key)
        {
            return UnderlyingTree.Remove(key, Comparer<TKey>.Default);
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

        int ICollection<KeyValuePair<TKey, TValue>>.Count => (int)Count;

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
            var result = UnderlyingTree.GetMatchingOrNext(key, Comparer<TKey>.Default);
            var enumerator = UnderlyingTree.GetKeyValuePairEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public void AddValue(TKey key, TValue value)
        {
            UnderlyingTree.Insert(key, Comparer<TKey>.Default, value);
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
            if (AllowDuplicates)
            {
                foreach (var v in GetAllValues(item.Key))
                    if (System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(v, item.Value))
                        return true;
                return false;
            }
            else
            {
                bool found = ValueAtKey(item.Key, out TValue value);
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
                if (AllowDuplicates)
                { // removes a single instance of the key-value pair. can remove all items within a key with RemoveAll.
                    var result = UnderlyingTree.GetMatchingOrNext(item.Key, Comparer<TKey>.Default);
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

        public IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0)
        {
            return UnderlyingTree.GetKeyEnumerator(skip);
        }

        public IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0)
        {
            return UnderlyingTree.GetValueEnumerator(skip);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(long skip = 0)
        {
            return UnderlyingTree.GetKeyValuePairEnumerator(skip);
        }

        public TValue ValueAtIndex(long i)
        {
            return UnderlyingTree.ValueAtIndex(i);
        }

        public void SetValueAtIndex(long i, TValue value)
        {
            UnderlyingTree.SetValueAtIndex(i, value);
        }

        public void InsertAtIndex(TKey key, TValue value, long index)
        {
            UnderlyingTree.InsertAtIndex(key, value, index);
        }

        public bool RemoveAt(long index)
        {
            return UnderlyingTree.RemoveAt(index);
        }

        public TKey KeyAtIndex(long i)
        {
            return UnderlyingTree.KeyAtIndex(i);
        }

        public void SetKeyAtIndex(long i, TKey key)
        {
            UnderlyingTree.SetKeyAtIndex(i, key);
        }

        public (bool inserted, long index) Insert(TKey key, IComparer<TKey> comparer, TValue value)
        {
            return UnderlyingTree.Insert(key, comparer, value);
        }

        public (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, IComparer<TKey> comparer)
        {
            return UnderlyingTree.GetMatchingOrNext(key, comparer);
        }

        public bool ContainsKeyValue(TKey key, TValue value, out long index) => ContainsKeyValue(key, null, value, out index);

        public bool ContainsKeyValue(TKey key, IComparer<TKey> comparer, TValue value, out long index)
        {
            if (AllowDuplicates)
            {
                bool result = UnderlyingTree.C
            }
            else
            {

            }
            uint hash = key.GetBinaryHashCode32();
            var hashMatches = GetHashMatches(key, hash);
            var match = hashMatches.FirstOrDefault(x => x.keyValue.Item2.Equals(value)); // DEBUG: Equality comparer
            if (match.keyValue == null)
            {
                index = default;
                return false;
            }
            index = match.index;
            return true;
        }

        public bool RemoveKeyValue(TKey key, TValue value) => RemoveKeyValue(key, Comparer<TKey>.Default, value);

        public bool RemoveKeyValue(TKey key, IComparer<TKey> comparer, TValue value)
        {
            bool exists = ContainsKeyValue(key, comparer, value, out long index);
            if (!exists)
                return false;
            UnderlyingTree.RemoveAt(index);
            return true;
        }


        public ILazinatorSplittable SplitOff()
        {
            AvlSortedDictionary<TKey, TValue> partSplitOff = new AvlSortedDictionary<TKey, TValue>(AllowDuplicates, (ILazinatorOrderedKeyable<TKey, TValue>)UnderlyingTree.SplitOff());
            return partSplitOff;
        }

    }
}
