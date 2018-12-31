using System;
using Lazinator.Core;

namespace Lazinator.Collections
{
    public partial class LazinatorTriple<T, U, V> : ILazinatorTriple<T, U, V>, IComparable<LazinatorTriple<T, U, V>> where T : ILazinator where U : ILazinator where V : ILazinator
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

        public override string ToString()
        {
            return $"({Item1?.ToString()}, {Item2?.ToString()},, {Item3?.ToString()})";
        }

        public int CompareTo(LazinatorTriple<T, U, V> other)
        {
            return ((Item1, Item2, Item3)).CompareTo((other.Item1, other.Item2, other.Item3));
        }
    }
}
