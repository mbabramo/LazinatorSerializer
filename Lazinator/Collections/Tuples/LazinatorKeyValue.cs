using System;
using System.Collections.Generic;
using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Support;

namespace Lazinator.Collections.Tuples
{
    public partial struct LazinatorKeyValue<T, U> : ILazinatorKeyValue<T, U>, IComparable<LazinatorKeyValue<T, U>> where T : ILazinator, IComparable<T> where U : ILazinator
    {

        public LazinatorKeyValue(T key, U value) : this()
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"({Key?.ToString()}, {Value?.ToString()})";
        }

        public int CompareTo(LazinatorKeyValue<T, U> other)
        {
            return ((Key, Value)).CompareTo((other.Key, other.Value));
        }

        public static CustomComparer<LazinatorKeyValue<T, U>> GetKeyOnlyComparer()
        {
            return new CustomComparer<LazinatorKeyValue<T, U>>((t, u) => t.Key.CompareTo(u.Key));
        }

        public static CustomComparer<LazinatorKeyValue<T, U>> GetKeyValueComparer(IComparer<T> keyComparer, IComparer<U> valueComparer)
        {
            return new CustomComparer<LazinatorKeyValue<T, U>>((t, u) =>
            {
                var keyComparison = keyComparer.Compare(t.Key, u.Key);
                if (keyComparison != 0)
                    return keyComparison;
                return valueComparer.Compare(t.Value, u.Value);
            });
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorKeyValue<T, U> otherKeyValue)
            {
                return EqualityComparer<T>.Default.Equals(Key, otherKeyValue.Key) && EqualityComparer<U>.Default.Equals(Value, otherKeyValue.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 233;
                if (!EqualityComparer<T>.Default.Equals(Key, default(T)))
                    hash = hash * 23 + Key.GetHashCode();
                if (!EqualityComparer<U>.Default.Equals(Value, default(U)))
                    hash = hash * 29 + Value.GetHashCode();
                return hash;
            }
        }
        
    }
}
