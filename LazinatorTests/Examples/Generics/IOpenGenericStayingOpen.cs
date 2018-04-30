using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Generics
{
    [Lazinator((int) ExampleUniqueIDs.OpenGenericStayingOpen)]
    public interface IOpenGenericStayingOpen<T> where T : ILazinator, new()
    {
        T MyT { get; set; }
        List<T> MyListT { get; set; }
    }
}
