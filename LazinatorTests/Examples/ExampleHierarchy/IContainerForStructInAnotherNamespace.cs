using Lazinator.Attributes;
using LazinatorTests.AnotherNamespace;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ContainerForStructInAnotherNamespace)]
    public interface IContainerForStructInAnotherNamespace
    {
        StructInAnotherNamespace MyStruct { get; set; }
        StructInAnotherNamespace? MyNullableStruct { get; set; }
    }
}