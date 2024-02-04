using Lazinator.Attributes;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int)ExampleUniqueIDs.ClosedGenericSealed)]
    internal interface IClosedGenericSealed : IOpenGeneric<ExampleChild>
    {
        int AnotherPropertyAdded { get; set; }
    }
}