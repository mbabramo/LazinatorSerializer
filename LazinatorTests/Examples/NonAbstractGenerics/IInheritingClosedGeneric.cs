using Lazinator.Attributes;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    [Lazinator((int)ExampleUniqueIDs.InheritingClosedGeneric)]
    interface IInheritingClosedGeneric : IClosedGeneric
    {
        int YetAnotherInt { get; set; }
    }
}
