using Lazinator.Attributes;
using LazinatorTests.AnotherNamespace;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ContainerForStructInAnotherNamespace)]
    public interface IContainerForStructInAnotherNamespace
    {
        // Omit this because problem occured when it is omitted: StructInAnotherNamespace MyStruct { get; set; }
        StructInAnotherNamespace? MyNullableStruct { get; set; }
    }
}