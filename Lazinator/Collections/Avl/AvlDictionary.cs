using Lazinator.Buffers;
using Lazinator.Collections.Factories;
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
    public partial class AvlDictionary<TKey, TValue> : IAvlDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorKeyableDictionary<TKey, TValue>, ILazinatorKeyableMultivalueDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        public AvlDictionary()
        {
        }

        public AvlDictionary(ILazinatorOrderedKeyableFactory<WUint, LazinatorTuple<TKey, TValue>> factory)
        {
            UnderlyingTree = factory.Create();
            if (UnderlyingTree.AllowDuplicates == false)
                throw new Exception("AvlDictionary requires an UnderlyingTree that allows duplicates."); // sometimes we allow multiple items with same key
        }

        public IEnumerable<TValue> GetAllValues(TKey key)
        {
            uint hash = key.GetBinaryHashCode32();
            foreach (var result in GetHashMatches(key, hash))
            {
                if (result.keyValue.Item1.Equals(key))
                    yield return result.keyValue.Item2;
                else
                    yield break;
            }
        }

        private IEnumerable<(LazinatorTuple<TKey, TValue> keyValue, long index)> GetHashMatches(TKey key, uint hash)
        {
            return GetAllInHashBucket(hash).Where(x => x.keyValue.Item1.Equals(key));
        }

        private IEnumerable<(LazinatorTuple<TKey, TValue> keyValue, long index)> GetAllInHashBucket(uint hash)
        {
            foreach ((KeyValuePair<WUint, LazinatorTuple<TKey, TValue>> keyValue, long index) p in EnumerateFrom(hash))
            {
                if (p.keyValue.Key.GetBinaryHashCode32().Equals(hash))
                    yield return (p.keyValue.Value, p.index);
                else
                    yield break;
            }
        }

        private IEnumerable<(KeyValuePair<WUint, LazinatorTuple<TKey, TValue>> keyValue, long index)> EnumerateFrom(uint hash)
        {
            var result = UnderlyingTree.GetMatchingOrNext(hash, Comparer<WUint>.Default);
            long index = result.index;
            var enumerator = UnderlyingTree.GetKeyValuePairEnumerator(index);
            while (enumerator.MoveNext())
                yield return (enumerator.Current, index++);
        }

        public bool ContainsKey(TKey key)
        {
            uint hash = key.GetBinaryHashCode32();
            bool result = GetHashMatches(key, hash).Any();
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            uint hash = key.GetBinaryHashCode32();
            var item = GetHashMatches(key, hash).FirstOrDefault();
            if (item.keyValue == null)
            {
                value = default;
                return false;
            }
            value = item.keyValue.Item2;
            return true;
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
                uint hash = key.GetBinaryHashCode32();
                var item = GetHashMatches(key, hash).FirstOrDefault();
                if (item.keyValue == null)
                {
                    UnderlyingTree.Insert(key.GetBinaryHashCode32(), Comparer<WUint>.Default, new LazinatorTuple<TKey, TValue>(key, value));
                }
                else
                    item.keyValue.Item2 = value;
            }
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            bool allowDuplicates = AllowDuplicates;
            UnderlyingTree.Clear();
            UnderlyingTree.AllowDuplicates = true;
        }

        public bool Remove(TKey key)
        {
            uint hash = key.GetBinaryHashCode32();
            var match = GetHashMatches(key, hash).FirstOrDefault();
            if (match.keyValue == null)
            {
                return false;
            }
            UnderlyingTree.RemoveAt(match.index);
            return true;
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

        public ICollection<TKey> Keys => GetKeysAndValues().Select(x => x.Item1).ToList();

        public ICollection<TValue> Values => GetKeysAndValues().Select(x => x.Item2).ToList();

        private IEnumerable<LazinatorTuple<TKey, TValue>> GetKeysAndValues()
        {
            var enumerator = UnderlyingTree.GetValueEnumerator(0);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
            enumerator.Dispose();
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count => (int)Count;

        public void AddValue(TKey key, TValue value)
        {
            uint hash = key.GetBinaryHashCode32();
            UnderlyingTree.Insert(hash, Comparer<WUint>.Default, new LazinatorTuple<TKey, TValue>(key, value));
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
                bool found = TryGetValue(item.Key, out TValue value);
                return found && System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(value, item.Value);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (LazinatorTuple<TKey, TValue> pair in GetKeysAndValues())
            {
                array[arrayIndex + i++] = new KeyValuePair<TKey, TValue>(pair.Item1, pair.Item2);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                if (AllowDuplicates)
                { // removes a single instance of the key-value pair. can remove all items within a key with RemoveAll.

                    uint hash = item.Key.GetBinaryHashCode32();
                    var firstMatch = GetHashMatches(item.Key, hash).FirstOrDefault();
                    if (firstMatch.keyValue == null)
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
                        UnderlyingTree.RemoveAt(firstMatch.index + distanceFromFirst);
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
            return GetKeyValuePairEnumerator();
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator()
        {
            return new TransformEnumerator<LazinatorTuple<TKey, TValue>, KeyValuePair<TKey, TValue>>(GetUnderlyingTree2ValueEnumerator(), x => new KeyValuePair<TKey, TValue>(x.Item1, x.Item2));
        }

        public IEnumerator<TKey> GetKeyEnumerator()
        {
            return new TransformEnumerator<LazinatorTuple<TKey, TValue>, TKey>(GetUnderlyingTree2ValueEnumerator(), x => x.Item1);
        }

        public IEnumerator<TValue> GetValueEnumerator()
        {
            return new TransformEnumerator<LazinatorTuple<TKey, TValue>, TValue>(GetUnderlyingTree2ValueEnumerator(), x => x.Item2);
        }

        private IEnumerator<LazinatorTuple<TKey, TValue>> GetUnderlyingTree2ValueEnumerator()
        {
            return UnderlyingTree.GetValueEnumerator();
        }

        // DEBUG -- the following methods must be tested
        public TValue ValueAtIndex(long i)
        {
            return UnderlyingTree.ValueAtIndex(i).Item2;
        }

        public void SetValueAtIndex(long i, TValue value)
        {
            var keyValue = UnderlyingTree.ValueAtIndex(i);
            UnderlyingTree.SetValueAtIndex(i, new LazinatorTuple<TKey, TValue>(keyValue.Item1, value));
        }

        public bool ContainsKeyValue(TKey key, TValue value, out long index) => ContainsKeyValue(key, null, value, out index);

        public bool ContainsKeyValue(TKey key, IComparer<TKey> comparer, TValue value, out long index)
        {
            // Note: comaprer isn't used, since we are storing by hash matches
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

        public bool RemoveKeyValue(TKey key, TValue value) => RemoveValue(key, Comparer<TKey>.Default, value);

        public bool RemoveValue(TKey key, IComparer<TKey> comparer, TValue value)
        {
            bool exists = ContainsKeyValue(key, comparer, value, out long index);
            if (!exists)
                return false;
            UnderlyingTree.RemoveAt(index);
            return true;
        }
    }
}
