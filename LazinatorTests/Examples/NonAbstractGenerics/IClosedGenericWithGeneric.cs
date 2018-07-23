using Lazinator.Attributes;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int)ExampleUniqueIDs.ClosedGenericWithGeneric)]
    interface IClosedGenericWithGeneric : IOpenGeneric<OpenGeneric<ExampleChild>>
    {
        int AnotherPropertyAddedHereToo { get; set; }
    }
}
