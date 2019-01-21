using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
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
        public AvlIndexableKeyValueTree(ContainerFactory innerContainerFactory, bool allowDuplicates, bool unbalanced) : base(innerContainerFactory, allowDuplicates, unbalanced)
        {
        }

        public override IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlIndexableKeyValueTree<TKey, TValue>(InnerContainerFactory.InnerFactorySameType, AllowDuplicates, Unbalanced);
        }

        protected IIndexableMultivalueContainer<LazinatorKeyValue<TKey, TValue>> UnderlyingIndexableContainer => (IIndexableMultivalueContainer<LazinatorKeyValue<TKey, TValue>>)UnderlyingContainer;

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key, IComparer<TKey> comparer) => FindIndex(key, MultivalueLocationOptions.Any, comparer);

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingIndexableContainer.FindContainerLocation(KeyPlusDefault(key), whichOne, KeyComparer(comparer));
            if (result.found)
            {
                var item = UnderlyingIndexableContainer.GetAt(result.location);
                return (item.Value, ((IndexLocation)result.location).Index, true);
            }
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
            return UnderlyingIndexableContainer.GetAt(new IndexLocation(index, UnderlyingIndexableContainer.LongCount));
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
            var location = new IndexLocation(index, UnderlyingIndexableContainer.LongCount);
            UnderlyingIndexableContainer.SetAt(location, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public void RemoveAtIndex(long index)
        {
            var location = new IndexLocation(index, UnderlyingIndexableContainer.LongCount);
            UnderlyingIndexableContainer.RemoveAt(location);
        }

        public void InsertAtIndex(long index, TKey key, TValue value)
        {
            var location = new IndexLocation(index, UnderlyingIndexableContainer.LongCount);
            UnderlyingIndexableContainer.InsertAt(location, new LazinatorKeyValue<TKey, TValue>(key, value));
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingIndexableContainer.InsertOrReplace(new LazinatorKeyValue<TKey, TValue>(key, value), whichOne, KeyComparer(comparer));
            return result;
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(TKey key, TValue value, IComparer<TKey> comparer) => InsertOrReplace(key, value, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
    }
}
