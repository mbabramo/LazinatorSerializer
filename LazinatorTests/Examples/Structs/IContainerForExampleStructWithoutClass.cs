using Lazinator.Attributes;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.ContainerForExampleStructWithoutClass)]
    public interface IContainerForExampleStructWithoutClass
    {
        ExampleStructWithoutClass ExampleStructWithoutClass { get; set; }
    }
}