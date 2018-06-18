using Lazinator.Attributes;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int)ExampleUniqueIDs.ClosedGeneric)]
    interface IClosedGeneric : IOpenGeneric<ExampleChild>
    {
        int AnotherPropertyAdded { get; set; }
    }
}
