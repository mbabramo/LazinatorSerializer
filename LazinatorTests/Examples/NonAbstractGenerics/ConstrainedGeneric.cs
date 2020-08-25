using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public class ConstrainedGeneric<T, U> : IConstrainedGeneric<T, U> where T : struct, ILazinator where U : ILazinator, new()
    {
        public T MyT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public U MyU { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
