using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlIndexableKeyValueTree<TKey, TValue> : AvlKeyValueTree<TKey, TValue>, IAvlIndexableKeyValueTree<TKey, TValue>, IIndexableKeyValueContainer<TKey, TValue>, IIndexableKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        public override IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlIndexableKeyValueTree<TKey, TValue>() { AllowDuplicates = AllowDuplicates };
        }

        protected AvlIndexableTree<LazinatorKeyValue<TKey, TValue>> UnderlyingIndexableTree => (AvlIndexableTree<LazinatorKeyValue<TKey, TValue>>)UnderlyingTree;

        public (TValue valueIfFound, long index, bool found) Find(TKey key, IComparer<TKey> comparer) => Find(key, MultivalueLocationOptions.Any, comparer);

        public (TValue valueIfFound, long index, bool found) Find(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingIndexableTree.GetMatchingOrNextNode(KeyPlusDefault(key), whichOne, KeyComparer(comparer));
            if (result.found)
                return (result.node.Value.Value, ((AvlCountedNode<LazinatorKeyValue<TKey, TValue>>)result.node).Index, true);
            return (default, -1, false);
        }

        public (long index, bool found) Find(TKey key, TValue value, IComparer<TKey> comparer)
        {
            // Finds the first occurrence of the key-value pair (note that there is no option to find the last)
            var firstValue = Find(key, MultivalueLocationOptions.First, comparer);
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

    public TKey GetKeyAt(long index)
        {
            return UnderlyingIndexableTree.GetAt(index).Key;
        }

        public TValue GetValueAt(long index)
        {
            return UnderlyingIndexableTree.GetAt(index).Value;
        }

        public void SetKeyAt(long index, TKey key)
        {
            var value = GetValueAt(index);
            UnderlyingIndexableTree.SetAt(index, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public void SetValueAt(long index, TValue value)
        {
            var key = GetKeyAt(index);
            UnderlyingIndexableTree.SetAt(index, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public void RemoveAt(long index)
        {
            UnderlyingIndexableTree.RemoveAt(index);
        }

        public void InsertAt(long index, TKey key, TValue value)
        {
            UnderlyingIndexableTree.InsertAt(index, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public (long index, bool insertedNotReplaced) InsertGetIndex(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingIndexableTree.InsertGetIndex(new LazinatorKeyValue<TKey, TValue>(key, value), whichOne, KeyComparer(comparer));
            return result;
        }

        public (long index, bool insertedNotReplaced) InsertGetIndex(TKey key, TValue value, IComparer<TKey> comparer) => InsertGetIndex(key, value, MultivalueLocationOptions.AfterLast, comparer);
    }
}
