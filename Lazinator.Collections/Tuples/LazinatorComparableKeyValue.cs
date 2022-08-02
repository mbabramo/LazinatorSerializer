using System;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Support;

namespace Lazinator.Collections.Tuples
{
    /// <summary>
    /// A key-value pair. This implements IComparable by comparing the keys only, but implements equality by comparing both key and value.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    public partial struct LazinatorComparableKeyValue<TKey, TValue> : ILazinatorComparableKeyValue<TKey, TValue>, IComparable<LazinatorComparableKeyValue<TKey, TValue>> where TKey : ILazinator, IComparable<TKey>, IComparable where TValue : ILazinator
    {
        public LazinatorComparableKeyValue(TKey key, TValue value) : this()
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"({Key?.ToString()}, {Value?.ToString()})";
        }

        public KeyValuePair<TKey, TValue> KeyValuePair => new KeyValuePair<TKey, TValue>(Key, Value);

        public int CompareTo(LazinatorComparableKeyValue<TKey, TValue> other)
        {
            return Comparer<TKey>.Default.Compare(Key, other.Key);
        }

        public static IComparer<LazinatorComparableKeyValue<TKey, TValue>> GetKeyOnlyComparer()
        {
            return Comparer<LazinatorComparableKeyValue<TKey, TValue>>.Default;
        }

        public static CustomComparer<LazinatorComparableKeyValue<TKey, TValue>> GetKeyValueComparer(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
        {
            return new CustomComparer<LazinatorComparableKeyValue<TKey, TValue>>((t, u) =>
            {
                var keyComparison = keyComparer.Compare(t.Key, u.Key);
                if (keyComparison != 0)
                    return keyComparison;
                return valueComparer.Compare(t.Value, u.Value);
            });
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorComparableKeyValue<TKey, TValue> otherKeyValue)
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
