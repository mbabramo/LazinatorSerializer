using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Buffers;
using Lazinator.Core;

namespace Lazinator.Collections
{
    public partial class LazinatorTriple<T, U, V> : ILazinatorTriple<T, U, V>, IComparable<LazinatorTriple<T, U, V>> where T : ILazinator, new() where U : ILazinator, new() where V : ILazinator, new()
    {
        public LazinatorTriple()
        {
        }

        public LazinatorTriple(T item1, U item2, V item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public int CompareTo(LazinatorTriple<T, U, V> other)
        {
            return ((Item1, Item2, Item3)).CompareTo((other.Item1, other.Item2, other.Item3));
        }
    }
}
