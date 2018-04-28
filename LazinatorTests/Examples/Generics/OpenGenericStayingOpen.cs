using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace LazinatorTests.Examples.Generics
{
    public partial class OpenGenericStayingOpen<T> : IOpenGenericStayingOpen<T> where T : ILazinator, new()
    {
        public T MyT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
