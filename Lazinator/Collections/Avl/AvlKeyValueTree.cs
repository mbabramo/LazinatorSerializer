using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlKeyValueTree<TKey, TValue> : IAvlKeyValueTree<TKey, TValue>, IKeyValueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        // DEBUG; // integrating the multivalue together with the single value for the key value store. We need this to make ContainsKeyValue work properly if duplicates are allowed.

        public AvlTree<LazinatorKeyValue<TKey, TValue>> UnderlyingTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



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

        LazinatorKeyValue<TKey, TValue> KeyPlusDefault(TKey key) => new LazinatorKeyValue<TKey, TValue>(key, default);

        static CustomComparer<LazinatorKeyValue<TKey, TValue>> KeyComparer(IComparer<TKey> comparer) => LazinatorKeyValue<TKey, TValue>.GetKeyComparer(comparer);

        public bool ContainsKey(TKey key, IComparer<TKey> comparer) => UnderlyingTree.Contains(KeyPlusDefault(key), KeyComparer(comparer));

        public bool ContainsKeyValue(TKey key, TValue value, IComparer<TKey> comparer)
        {
            // DEBUG debug; // need to consider multivalue possibility -- effectively, must implement GetAllValues. But we really can't do that, because we have no way to enumerate since we can't index. 
            if (ContainsKey(key, comparer))
                return GetValueForKey(key, comparer).Equals(value);
            return false;
        }

        public TValue GetValueForKey(TKey key, IComparer<TKey> comparer) => GetValueForKey(key, MultivalueLocationOptions.Any, comparer);

        public TValue GetValueForKey(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            bool found = UnderlyingTree.GetMatchingItem(KeyPlusDefault(key), whichOne, KeyComparer(comparer), out var match);
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
    }
}
