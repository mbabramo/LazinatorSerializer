using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.DerivedGenericContainer)]
    interface IDerivedGenericContainer<T> where T : ILazinator
    {
        AbstractGeneric1<T> Item { get; set; }
    }
}