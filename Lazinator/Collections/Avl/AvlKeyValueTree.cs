﻿using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlKeyValueTree<TKey, TValue> : IAvlKeyValueTree<TKey, TValue>, IKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        public AvlKeyValueTree(bool allowDuplicates)
        {
            UnderlyingTree = new AvlTree<LazinatorKeyValue<TKey, TValue>>() { AllowDuplicates = allowDuplicates };
        }


        public IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return (IKeyValueContainer<TKey, TValue>) new AvlTree<LazinatorKeyValue<TKey, TValue>>() { AllowDuplicates = AllowDuplicates };
        }

        LazinatorKeyValue<TKey, TValue> KeyPlusDefault(TKey key) => new LazinatorKeyValue<TKey, TValue>(key, default);

        static CustomComparer<LazinatorKeyValue<TKey, TValue>> KeyComparer(IComparer<TKey> comparer) => LazinatorKeyValue<TKey, TValue>.GetKeyComparer(comparer);
        static CustomComparer<LazinatorKeyValue<TKey, TValue>> KeyValueComparer(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer) => LazinatorKeyValue<TKey, TValue>.GetKeyValueComparer(keyComparer, valueComparer);

        public bool ContainsKey(TKey key, IComparer<TKey> comparer) => UnderlyingTree.Contains(KeyPlusDefault(key), KeyComparer(comparer));

        public bool ContainsKeyValue(TKey key, TValue value, IComparer<TKey> comparer)
        {
            if (AllowDuplicates)
            {
                var all = GetAllValues(key, comparer);
                return all.Any();
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

        public bool SetValueForKey(TKey key, TValue value, IComparer<TKey> comparer) => SetValueForKey(key, MultivalueLocationOptions.Any, value, comparer);

        public bool SetValueForKey(TKey key, MultivalueLocationOptions whichOne, TValue value, IComparer<TKey> comparer)
        {
            return UnderlyingTree.TryInsert(new LazinatorKeyValue<TKey, TValue>(key, value), whichOne, KeyComparer(comparer));
        }

        public bool TryRemove(TKey key, IComparer<TKey> comparer) => TryRemove(key, MultivalueLocationOptions.Any, comparer);

        public bool TryRemove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            return UnderlyingTree.TryRemove(KeyPlusDefault(key), whichOne, KeyComparer(comparer));
        }

        public bool TryRemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer) => TryRemoveKeyValue(key, value, MultivalueLocationOptions.Any, comparer);

        public bool TryRemoveKeyValue(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            if (!AllowDuplicates)
            {
                bool exists = ContainsKeyValue(key, value, comparer);
                if (exists)
                    return TryRemove(key, comparer);
                return false;
            }
            else
            {

                LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key, value);
                var match = UnderlyingTree.GetMatchingOrNextNode(keyValue, whichOne, KeyComparer(comparer));
                if (match.found == false)
                    return false;
                UnderlyingTree.RemoveNode(match.node);
                return true;
            }
        }

        public IEnumerable<TValue> GetAllValues(TKey key, IComparer<TKey> comparer)
        {
            var match = UnderlyingTree.GetMatchingOrNextNode(KeyPlusDefault(key), MultivalueLocationOptions.First, KeyComparer(comparer));
            var node = match.node;
            while (node != null && node.Value.Key.Equals(key))
            {
                yield return node.Value.Value;
                node = node.GetNextNode();
            }
        }

        public bool TryRemoveAll(TKey key, IComparer<TKey> comparer)
        {
            bool found = false;
            bool foundAny = false;
            do
            {
                found = TryRemove(key, MultivalueLocationOptions.Any, comparer);
                if (found)
                    foundAny = true;
            } while (found);
            return foundAny;
        }

        public void Clear()
        {
            UnderlyingTree.Clear();
        }

        public IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, TKey>(UnderlyingTree.GetEnumerator(), x => x.Key);
        }

        public IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, TValue>(UnderlyingTree.GetEnumerator(), x => x.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, KeyValuePair<TKey, TValue>>(UnderlyingTree.GetEnumerator(), x => x.KeyValuePair);
        }
    }
}
