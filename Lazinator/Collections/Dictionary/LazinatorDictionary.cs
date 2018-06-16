using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    public partial class LazinatorDictionary<TKey, TValue> : ILazinatorDictionary<TKey, TValue>, IDictionary<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator
    {
        private const int InitialNumBuckets = 10;
        private int NumBuckets => Buckets.Count;

        public bool IsReadOnly => false;

        private DictionaryBucket<TKey, TValue> GetBucketAtIndex(int index, LazinatorList<DictionaryBucket<TKey, TValue>> buckets = null)
        {
            if (buckets == null)
                buckets = Buckets;
            var bucket = buckets[index];
            if (bucket == null)
            {
                bucket = new DictionaryBucket<TKey, TValue>();
                buckets[index] = bucket;
            }
            return bucket;
        }

        private void GetHashAndBucket(TKey key, out uint hash, out DictionaryBucket<TKey, TValue> bucket)
        {
            hash = key.GetBinaryHashCode32();
            int bucketIndex = (int)(hash % NumBuckets);
            bucket = GetBucketAtIndex(bucketIndex);
        }

        private void GetHashAndBucket(TKey key, LazinatorList<DictionaryBucket<TKey, TValue>> buckets, out uint hash, out DictionaryBucket<TKey, TValue> bucket)
        {
            // This method is for use when using a replacement set of buckets.
            hash = key.GetBinaryHashCode32();
            int bucketIndex = (int)(hash % buckets.Count);
            bucket = GetBucketAtIndex(bucketIndex, buckets);
        }

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
                bool contained = bucket.ContainsKey(key, hash);
                bucket.InsertItemAtKey(key, value, hash);
                if (!contained)
                {
                    Count++;
                    ConsiderResize();
                }
            }
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

        public void Add(TKey key, TValue value)
        {
            GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            bool contained = bucket.ContainsKey(key, hash);
            if (contained)
                throw new ArgumentException();
            this[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            GetHashAndBucket(item.Key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            bool contained = bucket.ContainsKey(item.Key, hash);
            if (contained)
                throw new ArgumentException();
            this[item.Key] = item.Value;
        }

        private void AddToReplacementBucket(KeyValuePair<TKey, TValue> item, LazinatorList<DictionaryBucket<TKey, TValue>> buckets)
        {
            GetHashAndBucket(item.Key, buckets, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            bool contained = bucket.ContainsKey(item.Key, hash);
            if (contained)
                throw new ArgumentException();
            bucket.InsertItemAtKey(item.Key, item.Value, hash);
        }

        public bool Remove(TKey key)
        {
            GetHashAndBucket(key, out uint hash, out DictionaryBucket<TKey, TValue> bucket);
            if (bucket.ContainsKey(key, hash))
            {
                bucket.RemoveItemAtKey(key, hash);
                Count--;
                ConsiderResize();
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
                    Count--;
                    ConsiderResize();
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            Buckets = new LazinatorList<DictionaryBucket<TKey, TValue>>();
            for (int i = 0; i < InitialNumBuckets; i++)
                Buckets.Add(new DictionaryBucket<TKey, TValue>());
            Count = 0;
        }

        internal class DictionaryEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private bool _initialized;
            public bool _completed;
            private LazinatorList<DictionaryBucket<TKey, TValue>> _buckets;
            public int currentBucket;
            public int indexInCurrentBucket = -1;

            internal DictionaryEnumerator(LazinatorList<DictionaryBucket<TKey, TValue>> buckets)
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
                if (_initialized && _completed)
                    return false;
                if (!_initialized)
                {
                    currentBucket = 0;
                    indexInCurrentBucket = -1;
                    _initialized = true;
                }

                while (currentBucket < _buckets.Count)
                {
                    int itemsInCurrentBucket = _buckets[currentBucket]?.Count ?? 0;
                    if (itemsInCurrentBucket > indexInCurrentBucket + 1)
                    {
                        indexInCurrentBucket++;
                        return true;
                    }
                    currentBucket++;
                    indexInCurrentBucket = -1;
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

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (KeyValuePair<TKey, TValue> pair in GetKeysAndValues())
            {
                array[arrayIndex + i++] = pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DictionaryEnumerator(Buckets);
        }

        public ICollection<TKey> Keys => GetKeysAndValues().Select(x => x.Key).ToList();

        public ICollection<TValue> Values => GetKeysAndValues().Select(x => x.Value).ToList();

        private IEnumerable<KeyValuePair<TKey, TValue>> GetKeysAndValues()
        {
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
            enumerator.Dispose();
        }

        private void ConsiderResize()
        {
            const double sizeDownThreshold = 0.1; // size down rarely
            const double sizeUpThreshold = 0.8;
            if (Count < (int) NumBuckets * sizeDownThreshold && NumBuckets > InitialNumBuckets)
                CompleteResize();
            else if (Count > (int) NumBuckets * sizeUpThreshold)
                CompleteResize();
        }

        private void CompleteResize()
        {
            // Note: This is a bit inefficient because it deserializes everything.
            // It does, however, enumerate, so all items need not be deserialized at once.
            // We might be able to reconstitute the dictionary in binary fashion.

            int numBuckets = Count * 3;
            if (numBuckets < InitialNumBuckets)
                numBuckets = InitialNumBuckets;
            var results = GetKeysAndValues();

            var replacementBuckets = new LazinatorList<DictionaryBucket<TKey, TValue>>();
            for (int i = 0; i < numBuckets; i++)
                replacementBuckets.Add(null);

            int count = 0;

            foreach (var item in results)
            {
                AddToReplacementBucket(item, replacementBuckets);
                count++;
            }

            Buckets = replacementBuckets;
            Count = count;
        }
    }
}
