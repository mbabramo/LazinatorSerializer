using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlDictionary<TKey, TValue> : IAvlDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ILazinatorKeyable<TKey, TValue>, ILazinatorKeyableMultivalue<TKey, TValue>, ILazinatorOrderedKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlDictionary()
        {
        }

        public AvlDictionary(bool allowDuplicates)
        {
            UnderlyingTree = new AvlTree<WUint, LazinatorTuple<TKey, TValue>>();
            UnderlyingTree.AllowDuplicateKeys = true; // we always allow multiple items with same hash code
            AllowDuplicateKeys = allowDuplicates; // sometimes we allow multiple items with same key
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
            return GetAllInHashBucket(hash).Where(x => x.Item1.Equals(key));
        }

        private IEnumerable<(LazinatorTuple<TKey, TValue> keyValue, long index)> GetAllInHashBucket(uint hash)
        {
            foreach ((KeyValuePair<WUint, LazinatorTuple<TKey, TValue>> keyValue, long index) p in EnumerateFrom(hash))
            {
                if (p.keyValue.Key.GetHashCode().Equals(hash))
                    yield return (p.keyValue.Value, p.index);
                else
                    yield break;
            }
        }

        private IEnumerable<(KeyValuePair<WUint, LazinatorTuple<TKey, TValue>> keyValue, long index)> EnumerateFrom(uint hash)
        {
            var result = UnderlyingTree.SearchMatchOrNext(hash);
            foreach (var node in UnderlyingTree.Skip(result.index))
                yield return (node.KeyValuePair, node.Index);
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
                if (AllowDuplicateKeys)
                    throw new Exception("With multiple keys, use AddValue method to add item.");
                UnderlyingTree.Insert(key.GetBinaryHashCode32(), new LazinatorTuple<TKey, TValue>(key, value));
            }
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            bool allowDuplicates = AllowDuplicateKeys;
            UnderlyingTree = new AvlTree<WUint, LazinatorTuple<TKey, TValue>>();
            UnderlyingTree.AllowDuplicateKeys = true;
        }

        public bool Remove(TKey key)
        {
            uint hash = key.GetBinaryHashCode32();
            var match = GetHashMatches(key, hash).FirstOrDefault();
            if (match.keyValue == null)
            {
                return false;
            }
            UnderlyingTree.Remove(hash, match.index);
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
            var enumerator = UnderlyingTree.GetValueEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
            enumerator.Dispose();
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count => (int)Count;

        public void AddValue(TKey key, TValue value)
        {
            uint hash = key.GetBinaryHashCode32();
            UnderlyingTree.Insert(hash, new LazinatorTuple<TKey, TValue>(key, value));
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
            foreach (LazinatorTuple<TKey, TValue> pair in GetKeysAndValues())
            {
                array[arrayIndex + i++] = new KeyValuePair<TKey, TValue>(pair.Item1, pair.Item2);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                if (AllowDuplicateKeys)
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
                        UnderlyingTree.Remove(hash, firstMatch.index + distanceFromFirst);
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

        // the dictionary interface requires us to define a key-value pair enumerator
        private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private AvlNodeValueEnumerator<WUint, LazinatorTuple<TKey, TValue>> UnderlyingEnumerator;

            public Enumerator(AvlNodeValueEnumerator<WUint, LazinatorTuple<TKey, TValue>> underlyingEnumerator)
            {
                UnderlyingEnumerator = underlyingEnumerator;
            }

            KeyValuePair<TKey, TValue> Current => new KeyValuePair<TKey, TValue>(UnderlyingEnumerator.Current.Item1, UnderlyingEnumerator.Current.Item2);

            object IEnumerator.Current => Current;

            KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current => Current;

            public void Dispose()
            {
                UnderlyingEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                return UnderlyingEnumerator.MoveNext();
            }

            public void Reset()
            {
                UnderlyingEnumerator.Reset();
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(UnderlyingTree.GetValueEnumerator());
        }


        public AvlTree<WUint, LazinatorTuple<TKey, TValue>> UnderlyingTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DescendantHasChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DescendantIsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorMemory LazinatorMemoryStorage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicateKeys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            throw new NotImplementedException();
        }

        public void DeserializeLazinator(LazinatorMemory serialized)
        {
            throw new NotImplementedException();
        }

        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            throw new NotImplementedException();
        }

        public void UpdateStoredBuffer()
        {
            throw new NotImplementedException();
        }

        public void FreeInMemoryObjects()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            throw new NotImplementedException();
        }

        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            throw new NotImplementedException();
        }

        public int GetByteLength()
        {
            throw new NotImplementedException();
        }

        public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            throw new NotImplementedException();
        }

        public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            throw new NotImplementedException();
        }

        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            throw new NotImplementedException();
        }

        public IncludeChildrenMode OriginalIncludeChildrenMode => throw new NotImplementedException();

        public bool IsStruct => throw new NotImplementedException();

        public bool NonBinaryHash32 => throw new NotImplementedException();

        public LazinatorParentsCollection LazinatorParents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int LazinatorObjectVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
