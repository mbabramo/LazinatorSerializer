using System;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    public partial class DictionaryBucket<TKey, TValue> : IDictionaryBucket<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        private void Initialize()
        {
            Keys = new LazinatorList<TKey>();
            Values = new LazinatorList<TValue>();
            Initialized = true;
        }

        public int Count => Initialized ? Keys.Count : 0;
        
        public bool ContainsKey(TKey key, uint? binaryHashOfKey = null)
        {
            int index = GetKeyIndex(key, binaryHashOfKey);
            return index != -1;
        }

        public TValue GetValueAtKey(TKey key, uint? binaryHashOfKey = null)
        {
            int index = GetKeyIndex(key, binaryHashOfKey);
            if (index == -1)
                throw new Exception("Must confirm key exists before getting value at key.");
            return Values[index];
        }

        public void RemoveItemAtKey(TKey key, uint? binaryHashOfKey = null)
        {
            int index = GetKeyIndex(key, binaryHashOfKey);
            if (index == -1)
                throw new Exception("Must confirm key exists before deleting value at key.");
            Values.RemoveAt(index);
            Keys.RemoveAt(index);
            _lastSearchRemembered = false;
            if (!Keys.Any())
            {
                Keys = null;
                Values = null;
                Initialized = false;
            }
        }

        public void InsertItemAtKey(TKey key, TValue value, uint? binaryHashOfKey = null)
        {
            if (!Initialized)
                Initialize();
            int index = GetKeyIndex(key, binaryHashOfKey);
            if (index != -1)
                Values[index] = value;
            else
            {
                Keys.Add(key);
                Values.Add(value);
            }
            _lastSearchRemembered = false;
        }

        private bool _lastSearchRemembered;
        private TKey _lastKeySearched;
        private int _lastResult;

        private int GetKeyIndex(TKey key, uint? binaryHashOfKey)
        {
            if (!Initialized)
                return -1;
            if (_lastSearchRemembered && (_lastKeySearched.LazinatorMemoryStorage == null || _lastKeySearched.LazinatorMemoryStorage.Disposed == false) && _lastKeySearched.Equals(key))
                return _lastResult;
            _lastKeySearched = key;
            _lastSearchRemembered = true;
            uint searchHash = binaryHashOfKey ?? key.GetBinaryHashCode32();
            for (int i = 0; i < Keys.Count; i++)
            {
                uint itemHash = Keys.GetListMemberHash32(i);
                if (searchHash == itemHash)
                {
                    TKey item = Keys[i];
                    if (key.Equals(item))
                        return _lastResult = i;
                }
            }
            return _lastResult = -1;
        }

        public KeyValuePair<TKey, TValue> GetKeyValuePair(int index)
        {
            return new KeyValuePair<TKey, TValue>(Keys[index], Values[index]);
        }
    }
}
