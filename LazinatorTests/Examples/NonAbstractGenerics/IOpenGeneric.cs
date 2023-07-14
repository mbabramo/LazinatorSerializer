using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int) ExampleUniqueIDs.OpenGeneric)]
    public interface IOpenGeneric<T> : ILazinator where T : ILazinator
    {
        T MyT { get; set; }
        List<T> MyListT { get; set; }
    }
}
