using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.UnformedGenericContainer)]
    interface IUnformedGenericContainer<T> where T : ILazinator, new()
    {
        AbstractGeneric1<T> Item { get; set; }
    }
}