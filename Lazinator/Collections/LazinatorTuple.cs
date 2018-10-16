﻿using System;
using System.Collections.Generic;
using Lazinator.Core;

namespace Lazinator.Collections
{
    public partial class LazinatorTuple<T, U> : ILazinatorTuple<T, U>, IComparable<LazinatorTuple<T,U>> where T : ILazinator, new() where U : ILazinator, new()
    {
        public LazinatorTuple()
        {
        }

        public LazinatorTuple(T item1, U item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public int CompareTo(LazinatorTuple<T, U> other)
        {
            return ((Item1, Item2)).CompareTo((other.Item1, other.Item2));
        }

        public override bool Equals(object obj)
        {
            LazinatorTuple<T, U> other = obj as LazinatorTuple<T, U>;
            if (other == null)
                return false;
            return EqualityComparer<T>.Default.Equals(Item1, other.Item1) && EqualityComparer<U>.Default.Equals(Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 233;
                if (!EqualityComparer<T>.Default.Equals(Item1, default(T)))
                    hash = hash * 23 + Item1.GetHashCode();
                if (!EqualityComparer<U>.Default.Equals(Item2, default(U)))
                    hash = hash * 29 + Item2.GetHashCode();
                return hash;
            }
        }
    }
}
