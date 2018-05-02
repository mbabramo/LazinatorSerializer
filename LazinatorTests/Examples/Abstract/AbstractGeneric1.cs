using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public abstract partial class AbstractGeneric1<T> : IAbstractGeneric1<T>
    {
        public T MyT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
