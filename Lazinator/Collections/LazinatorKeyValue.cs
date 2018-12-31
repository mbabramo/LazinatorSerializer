﻿using System;
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
