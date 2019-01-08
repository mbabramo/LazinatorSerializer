using System;
using System.Collections.Generic;
using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Support;

namespace Lazinator.Collections.Tuples
{
    /// <summary>
    /// A key-value pair, neither of which is required to implement IComparable<T>.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    public partial struct LazinatorKeyValue<TKey, TValue> : ILazinatorKeyValue<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {

        public LazinatorKeyValue(TKey key, TValue value) : this()
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"({Key?.ToString()}, {Value?.ToString()})";
        }

        public static CustomComparer<LazinatorKeyValue<TKey, TValue>> GetKeyComparer(IComparer<TKey> keyComparer)
        {
            return new CustomComparer<LazinatorKeyValue<TKey, TValue>>((t, u) =>
            {
                var keyComparison = keyComparer.Compare(t.Key, u.Key);
                return keyComparison;
            });
        }

        public static CustomComparer<LazinatorKeyValue<TKey, TValue>> GetKeyValueComparer(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
        {
            return new CustomComparer<LazinatorKeyValue<TKey, TValue>>((t, u) =>
            {
                var keyComparison = keyComparer.Compare(t.Key, u.Key);
                if (keyComparison != 0)
                    return keyComparison;
                return valueComparer.Compare(t.Value, u.Value);
            });
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorKeyValue<TKey, TValue> otherKeyValue)
            {
                return EqualityComparer<TKey>.Default.Equals(Key, otherKeyValue.Key) && EqualityComparer<TValue>.Default.Equals(Value, otherKeyValue.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 233;
                if (!EqualityComparer<TKey>.Default.Equals(Key, default(TKey)))
                    hash = hash * 23 + Key.GetHashCode();
                if (!EqualityComparer<TValue>.Default.Equals(Value, default(TValue)))
                    hash = hash * 29 + Value.GetHashCode();
                return hash;
            }
        }
        
    }
}
