using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Collections
{
    public partial class LazinatorTuple<T, U> : ILazinatorTuple<T, U> where T : ILazinator, new() where U : ILazinator, new()
    {
        public LazinatorTuple()
        {
            Item1 = default(T);
            Item2 = default(U);
        }

        public LazinatorTuple(T item1, U item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
