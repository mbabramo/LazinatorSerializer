using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int) ExampleUniqueIDs.OpenGeneric)]
    public interface IOpenGeneric<T> where T : ILazinator, new()
    {
        [Autoclone]
        T MyT { get; set; }
        List<T> MyListT { get; set; }
    }
}
