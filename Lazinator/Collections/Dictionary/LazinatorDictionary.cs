using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    public partial class LazinatorDictionary<TKey, TValue> : ILazinatorDictionary<TKey, TValue>, IDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        private const int InitialNumBuckets = 10;
        private int NumBuckets => Buckets.Count;

        public int Count { get; set; }

        public LazinatorDictionary()
        {
            Clear();
        }

        public TValue this[TKey key]
        {
            get
            {
                GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
                bool contained = bucket.ContainsKey(key, hash);
                if (!contained)
                    throw new KeyNotFoundException();
                return bucket.GetValueAtKey(key, hash);
            }
            set
            {
                GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
                bucket.InsertItemAtKey(key, value, hash);
            }
        }

        private void GetHashAndBucket(TKey key, out uint hash, out DictionaryBucket<TKey, TValue> bucket)
        {
            hash = key.GetBinaryHashCode32();
            bucket = Buckets[(int)hash % InitialNumBuckets];
        }

        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this[item.Key] = item.Value;
        }

        public void Clear()
        {
            Buckets = new LazinatorList<DictionaryBucket<TKey, TValue>>();
            for (int i = 0; i < InitialNumBuckets; i++)
                Buckets.Add(new DictionaryBucket<TKey, TValue>());
            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            GetHashAndBucket(item.Key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            if (bucket.ContainsKey(item.Key, hash))
            {
                TValue value = bucket.GetValueAtKey(item.Key, hash);
                return value.Equals(item.Value);
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            return bucket.ContainsKey(key, hash);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public class DictionaryEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private bool _initialized;
            public bool _completed;
            private LazinatorList<DictionaryBucket<TKey, TValue>> _buckets;
            public int currentBucket;
            public int indexInCurrentBucket = -1;

            public DictionaryEnumerator(LazinatorList<DictionaryBucket<TKey, TValue>> buckets)
            {
                _buckets = buckets;
            }

            public KeyValuePair<TKey, TValue> Current =>
                _initialized && !_completed ? _buckets[currentBucket].GetKeyValuePair(indexInCurrentBucket) : default(KeyValuePair<TKey, TValue>);


            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_completed)
                    return false;
                if (!_initialized)
                {
                    currentBucket = 0;
                    indexInCurrentBucket = -1;
                    _initialized = true;
                }

                while (currentBucket < _buckets.Count)
                {
                    if (_buckets[currentBucket].Count > indexInCurrentBucket + 1)
                    {
                        indexInCurrentBucket++;
                        return true;
                    }
                    currentBucket++;
                }

                _completed = false;
                return false;
            }

            public void Reset()
            {
                _initialized = false;
                _completed = false;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new DictionaryEnumerator(Buckets);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DictionaryEnumerator(Buckets);
        }

        public bool Remove(TKey key)
        {
            GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            if (bucket.ContainsKey(key, hash))
            {
                bucket.RemoveItemAtKey(key, hash);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            GetHashAndBucket(item.Key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            if (bucket.ContainsKey(item.Key, hash))
            {
                TValue itemAtKey = bucket.GetValueAtKey(item.Key, hash);
                if (item.Value.Equals(itemAtKey))
                {
                    bucket.RemoveItemAtKey(item.Key, hash);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            if (bucket.ContainsKey(key, hash))
            {
                value = bucket.GetValueAtKey(key, hash);
                return true;
            }

            value = default;
            return false;
        }
    }
}
