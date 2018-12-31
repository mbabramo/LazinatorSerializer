using System;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Support;

namespace Lazinator.Collections
{
    public partial class LazinatorKeyValue<T, U> : ILazinatorKeyValue<T, U>, IComparable<LazinatorKeyValue<T, U>> where T : ILazinator, IComparable<T> where U : ILazinator
    {
        public LazinatorKeyValue()
        {
        }

        public LazinatorKeyValue(T key, U value)
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


        static CustomComparer<LazinatorKeyValue<T, U>> KeyOnlyComparer = null;
        public static CustomComparer<LazinatorKeyValue<T, U>> GetKeyOnlyComparer()
        {
            if (KeyOnlyComparer == null)
                return new CustomComparer<LazinatorKeyValue<T, U>>((t, u) => t.Key.CompareTo(u.Key));
            return KeyOnlyComparer;
        }
        // For the value-only comparer, we need to constraint the value type to icomparable.
        static class ValueOnlyComparerContainer<T2, U2> where T2 : ILazinator, IComparable<T2> where U2 : ILazinator, IComparable<U2>
        {
            public static CustomComparer<LazinatorKeyValue<T, U2>> ValueOnlyComparer = null;
        }
        public static CustomComparer<LazinatorKeyValue<T, U2>> GetValueOnlyComparer<U2>() where U2 : ILazinator, IComparable<U2>
        {
            if (ValueOnlyComparerContainer<T, U2>.ValueOnlyComparer == null)
                ValueOnlyComparerContainer<T, U2>.ValueOnlyComparer = new CustomComparer<LazinatorKeyValue<T, U2>>((t, u) => t.Value.CompareTo(u.Value));
            return ValueOnlyComparerContainer<T, U2>.ValueOnlyComparer;
        }


        public override bool Equals(object obj)
        {
            LazinatorKeyValue<T, U> other = obj as LazinatorKeyValue<T, U>;
            if (other == null)
                return false;
            return EqualityComparer<T>.Default.Equals(Key, other.Key) && EqualityComparer<U>.Default.Equals(Value, other.Value);
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
