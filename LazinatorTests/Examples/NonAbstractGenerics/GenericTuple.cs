﻿using System;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Support;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class GenericTuple<T, U> : IGenericTuple<T, U>, IComparable<GenericTuple<T,U>> where T : ILazinator where U : ILazinator
    {
        public GenericTuple()
        {
        }

        public GenericTuple(T item1, U item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override string ToString()
        {
            return $"({Item1?.ToString()}, {Item2?.ToString()})";
        }

        public int CompareTo(GenericTuple<T, U> other)
        {
            return ((Item1, Item2)).CompareTo((other.Item1, other.Item2));
        }

        public override bool Equals(object obj)
        {
            GenericTuple<T, U> other = obj as GenericTuple<T, U>;
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
