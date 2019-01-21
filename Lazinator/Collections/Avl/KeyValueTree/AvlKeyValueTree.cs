using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Avl.KeyValueTree
{
    public partial class AvlKeyValueTree<TKey, TValue> : IAvlKeyValueTree<TKey, TValue>, IKeyValueContainer<TKey, TValue>, IKeyMultivalueContainer<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>> where TKey : ILazinator where TValue : ILazinator
    {
        #region Construction

        public AvlKeyValueTree(ContainerFactory innerContainerFactory, bool allowDuplicates, bool unbalanced)
        {
            UnderlyingContainer = (IMultivalueContainer<LazinatorKeyValue<TKey, TValue>>)innerContainerFactory.CreateContainerOfKeyValues<TValue>();
            AllowDuplicates = UnderlyingContainer.AllowDuplicates;
            Unbalanced = UnderlyingContainer.Unbalanced;
            if (AllowDuplicates != allowDuplicates || Unbalanced != unbalanced)
                throw new Exception("KeyValueTree settings must be same as those of inner container.");
        }

        public virtual IKeyValueContainer<TKey, TValue> CreateNewWithSameSettings()
        {
            return new AvlKeyValueTree<TKey, TValue>(InnerContainerFactory, AllowDuplicates, Unbalanced);
        }

        public string ToTreeString()
        {
            if (UnderlyingContainer is AvlTree<LazinatorKeyValue<TKey, TValue>> tree)
                return tree.ToTreeString();
            return "N/A";
        }

        #endregion

        #region Keys

        protected LazinatorKeyValue<TKey, TValue> KeyPlusDefault(TKey key) => new LazinatorKeyValue<TKey, TValue>(key, default);

        protected static CustomComparer<LazinatorKeyValue<TKey, TValue>> KeyComparer(IComparer<TKey> comparer) => LazinatorKeyValue<TKey, TValue>.GetKeyComparer(comparer);

        protected static CustomComparer<LazinatorKeyValue<TKey, TValue>> KeyValueComparer(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer) => LazinatorKeyValue<TKey, TValue>.GetKeyValueComparer(keyComparer, valueComparer);

        public bool ContainsKey(TKey key, IComparer<TKey> comparer) => UnderlyingContainer.Contains(KeyPlusDefault(key), KeyComparer(comparer));

        #endregion

        #region Values

        public IEnumerable<TValue> GetAllValues(TKey key, IComparer<TKey> comparer)
        {
            var match = UnderlyingContainer.FindContainerLocation(KeyPlusDefault(key), MultivalueLocationOptions.First, KeyComparer(comparer));
            var location = match.location;
            while (location != null)
            {
                LazinatorKeyValue<TKey, TValue> lazinatorKeyValue = UnderlyingContainer.GetAt(location);
                if (lazinatorKeyValue.Key.Equals(key))
                {
                    yield return lazinatorKeyValue.Value;
                    location = location.GetNextLocation();
                }
                else
                    location = null;
            }
        }

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
            bool found = UnderlyingContainer.GetValue(KeyPlusDefault(key), whichOne, KeyComparer(comparer), out var match);
            if (!found)
                throw new KeyNotFoundException();
            return match.Value;
        }

        public bool SetValueForKey(TKey key, TValue value, IComparer<TKey> comparer) => SetValueForKey(key, value, MultivalueLocationOptions.Any, comparer);

        public bool SetValueForKey(TKey key, TValue value, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            var result = UnderlyingContainer.InsertOrReplace(new LazinatorKeyValue<TKey, TValue>(key, value), whichOne, KeyComparer(comparer));
            return result.insertedNotReplaced;
        }

        public void AddValueForKey(TKey key, TValue value, IComparer<TKey> comparer)
        {
            if (AllowDuplicates)
                UnderlyingContainer.InsertOrReplace(new LazinatorKeyValue<TKey, TValue>(key, value), MultivalueLocationOptions.InsertAfterLast, KeyComparer(comparer));
            else
                SetValueForKey(key, value, comparer);
        }

        #endregion

        #region Removal

        public bool TryRemove(TKey key, IComparer<TKey> comparer) => TryRemove(key, MultivalueLocationOptions.Any, comparer);

        public bool TryRemove(TKey key, MultivalueLocationOptions whichOne, IComparer<TKey> comparer)
        {
            return UnderlyingContainer.TryRemove(KeyPlusDefault(key), whichOne, KeyComparer(comparer));
        }

        public bool TryRemoveKeyValue(TKey key, TValue value, IComparer<TKey> comparer)
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
                // first, find a matching key
                var match = UnderlyingContainer.FindContainerLocation(keyValue, MultivalueLocationOptions.First, KeyComparer(comparer));
                if (match.found == false)
                    return false;
                // Now, find a matching value
                bool keepGoing = match.found;
                LazinatorKeyValue<TKey, TValue> keyValueWithKeyMatch = UnderlyingContainer.GetAt(match.location);
                while (keepGoing) 
                {
                    if (EqualityComparer<TValue>.Default.Equals(keyValueWithKeyMatch.Value, value))
                        keepGoing = false; // value found
                    else
                    {
                        match.location = match.location.GetNextLocation();
                        if (match.location == null)
                        {
                            keepGoing = false; // value doesn't exist
                            match.found = false;
                        }
                        else
                        {
                            keyValueWithKeyMatch = UnderlyingContainer.GetAt(match.location);
                        }
                    }
                }
                if (match.found)
                {
                    UnderlyingContainer.RemoveAt(match.location);
                    return true;
                }
                return false;
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
            UnderlyingContainer.Clear();
        }

        #endregion

        #region Enumeration

        public IEnumerator<TKey> GetKeyEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, TKey>(UnderlyingContainer.GetEnumerator(reverse, skip), x => x.Key);
        }

        public IEnumerator<TValue> GetValueEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, TValue>(UnderlyingContainer.GetEnumerator(reverse, skip), x => x.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<LazinatorKeyValue<TKey, TValue>, KeyValuePair<TKey, TValue>>(UnderlyingContainer.GetEnumerator(reverse, skip), x => x.KeyValuePair);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return GetKeyValuePairEnumerator(false, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetKeyValuePairEnumerator(false, 0);
        }

        #endregion
    }
}
