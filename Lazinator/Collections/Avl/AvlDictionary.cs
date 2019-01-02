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
            UnderlyingTree = new AvlTree<WInt, LazinatorTuple<TKey, TValue>>();
            UnderlyingTree.AllowDuplicateKeys = true; // we always allow multiple items with same hash code
            AllowDuplicateKeys = allowDuplicates; // sometimes we allow multiple items with same key
        }

        public IEnumerable<LazinatorTuple<TKey, TValue>> GetAllInHashBucket(int hash)
        {
            foreach (var p in EnumerateFrom(hash))
            {
                if (p.Key.Equals(hash))
                    yield return p.Value;
                else
                    yield break;
            }
        }

        public IEnumerable<KeyValuePair<WInt, LazinatorTuple<TKey, TValue>>> EnumerateFrom(int hash)
        {
            var result = UnderlyingTree.SearchMatchOrNext(hash);
            foreach (var node in UnderlyingTree.Skip(result.index))
                yield return node.KeyValuePair;
        }

        public bool ContainsKey(TKey key)
        {
            int hash = key.GetHashCode();
            bool result = GetHashMatches(key, hash).Any();
            return result;
        }

        private IEnumerable<LazinatorTuple<TKey, TValue>> GetHashMatches(TKey key, int hash)
        {
            return GetAllInHashBucket(hash).Where(x => x.Item1.Equals(key));
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var item = GetHashMatches(key, key.GetHashCode()).FirstOrDefault();
            if (item == null)
            {
                value = default;
                return false;
            }
            value = item.Item2;
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
                UnderlyingTree.Insert(key.GetHashCode(), value);
            }
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            bool allowDuplicates = AllowDuplicateKeys;
            UnderlyingTree = new AvlTree<WInt, LazinatorTuple<TKey, TValue>>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
        }

        public bool Remove(TKey item)
        {
            throw new NotImplementedException();
            // return UnderlyingTree.Remove(item);
        }

        public bool RemoveAll(TKey item)
        {
            bool any = Remove(item);
            if (any)
            {
                do
                {
                } while (Remove(item));
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


        public IncludeChildrenMode OriginalIncludeChildrenMode => throw new NotImplementedException();

        public bool IsStruct => throw new NotImplementedException();

        public bool NonBinaryHash32 => throw new NotImplementedException();

        public LazinatorParentsCollection LazinatorParents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int LazinatorObjectVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            var result = UnderlyingTree.SearchMatchOrNext(key);
            foreach (var node in UnderlyingTree.Skip(result.index))
                yield return node.KeyValuePair;
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
                    var result = UnderlyingTree.SearchMatchOrNext(item.Key);
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
                        UnderlyingTree.Remove(item.Key, result.index + distanceFromFirst);
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


        public AvlTree<WInt, LazinatorTuple<TKey, TValue>> UnderlyingTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
    }
}
