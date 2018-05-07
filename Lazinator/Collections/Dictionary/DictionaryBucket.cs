using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    public partial class DictionaryBucket<TKey, TValue> : IDictionaryBucket<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
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

        public void DeleteItemAtKey(TKey key, uint? binaryHashOfKey = null)
        {
            int index = GetKeyIndex(key, binaryHashOfKey);
            if (index == -1)
                throw new Exception("Must confirm key exists before deleting value at key.");
            Values.RemoveAt(index);
            Keys.RemoveAt(index);
            _lastSearchRemembered = false;
        }

        public void InsertItemAtKey(TKey key, TValue value, uint? binaryHashOfKey = null)
        {
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
            if (_lastSearchRemembered && _lastKeySearched.Equals(key))
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
    }
}
