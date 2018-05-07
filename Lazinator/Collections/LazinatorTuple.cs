using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
