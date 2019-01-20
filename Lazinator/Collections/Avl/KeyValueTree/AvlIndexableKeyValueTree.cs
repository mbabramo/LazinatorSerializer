using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.KeyValueTree
{
    public partial class AvlIndexableKeyValueTree<TKey, TValue> : AvlKeyValueTree<TKey, TValue>, IAvlIndexableKeyValueTree<TKey, TValue>, IIndexableKeyValueContainer<TKey, TValue>, IIndexableKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        public AvlIndexableKeyValueTree(bool allowDuplicates, bool unbalanced)
        {
            UnderlyingTree = new AvlIndexableTree<LazinatorKeyValue<TKey, TValue>>(allowDuplicates, unbalanced);
        }

        public override IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlIndexableKeyValueTree<TKey, TValue>(AllowDuplicates, Unbalanced);
        }

        protected AvlIndexableTree<LazinatorKeyValue<TKey, TValue>> UnderlyingIndexableTree => (AvlIndexableTree<LazinatorKeyValue<TKey, TValue>>)UnderlyingTree;

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key, IComparer<TKey> comparer) => FindIndex(key, MultivalueLocationOptions.Any, comparer);

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingIndexableTree.GetMatchingOrNextNode(KeyPlusDefault(key), whichOne, KeyComparer(comparer));
            if (result.found)
                return (result.node.Value.Value, ((AvlCountedNode<LazinatorKeyValue<TKey, TValue>>)result.node).Index, true);
            return (default, -1, false);
        }

        public (long index, bool found) FindIndex(TKey key, TValue value, IComparer<TKey> comparer)
        {
            // Finds the first occurrence of the key-value pair (note that there is no option to find the last)
            var firstValue = FindIndex(key, MultivalueLocationOptions.First, comparer);
            if (firstValue.found == false)
                return (-1, false);
            var values = GetAllValues(key, comparer);
            long index = firstValue.index;
            foreach (var existingValue in values)
            {
                if (existingValue.Equals(value))
                    return (index, true);
                else
                    index++;
            }
            return (-1, false);
        }

        public TKey GetKeyAtIndex(long index)
        {
            return GetKeyValueAtIndex(index).Key;
        }

        public TValue GetValueAtIndex(long index)
        {
            return GetKeyValueAtIndex(index).Value;
        }

        public LazinatorKeyValue<TKey, TValue> GetKeyValueAtIndex(long index)
        {
            return UnderlyingIndexableTree.GetAtIndex(index);
        }

        public void SetKeyAtIndex(long index, TKey key)
        {
            var value = GetValueAtIndex(index);
            SetKeyValueAtIndex(index, key, value);
        }

        public void SetValueAtIndex(long index, TValue value)
        {
            var key = GetKeyAtIndex(index);
            SetKeyValueAtIndex(index, key, value);
        }

        public void SetKeyValueAtIndex(long index, TKey key, TValue value)
        {
            UnderlyingIndexableTree.SetAtIndex(index, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public void RemoveAtIndex(long index)
        {
            UnderlyingIndexableTree.RemoveAt(index);
        }

        public void InsertAtIndex(long index, TKey key, TValue value)
        {
            UnderlyingIndexableTree.InsertAtIndex(index, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingIndexableTree.InsertOrReplace(new LazinatorKeyValue<TKey, TValue>(key, value), whichOne, KeyComparer(comparer));
            return result;
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, IComparer<TKey> comparer) => InsertOrReplace(key, value, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
    }
}
