using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlKeyValueTree<TKey, TValue> : IAvlKeyValueTree<TKey, TValue>, IKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        public AvlTree<LazinatorKeyValue<TKey, TValue>> UnderlyingTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlKeyValueTree(bool allowDuplicates)
        {
            UnderlyingTree = new AvlTree<LazinatorKeyValue<TKey, TValue>>() { AllowDuplicates = allowDuplicates };
        }

        LazinatorKeyValue<TKey, TValue> KeyPlusDefault(TKey key) => new LazinatorKeyValue<TKey, TValue>(key, default);

        static CustomComparer<LazinatorKeyValue<TKey, TValue>> KeyComparer(IComparer<TKey> comparer) => LazinatorKeyValue<TKey, TValue>.GetKeyComparer(comparer);

        public bool ContainsKey(TKey key, IComparer<TKey> comparer) => UnderlyingTree.Contains(KeyPlusDefault(key), KeyComparer(comparer));

        public bool ContainsKeyValue(TKey key, TValue value, IComparer<TKey> comparer)
        {
            if (AllowDuplicates)
            {
                var all = GetAllValues(key, comparer);
            }
            if (ContainsKey(key, comparer))
                return GetValueForKey(key, comparer).Equals(value);
            return false;
        }

        public TValue GetValueForKey(TKey key, IComparer<TKey> comparer) => GetValueForKey(key, MultivalueLocationOptions.Any, comparer);

        public TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            bool found = UnderlyingTree.GetValue(KeyPlusDefault(key), whichOne, KeyComparer(comparer), out var match);
            if (!found)
                throw new KeyNotFoundException();
            return match.Value;
        }

        public bool SetValueForKey(TKey key, TValue value, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public bool SetValueForKey(TKey key, MultivalueLocationOptions whichOne, TValue value, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(TKey key, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }




        public void AddValue(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TValue> GetAllValues(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(TKey item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TValue> GetAllValues(TKey key, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveAll(TKey key, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }
    }
}
