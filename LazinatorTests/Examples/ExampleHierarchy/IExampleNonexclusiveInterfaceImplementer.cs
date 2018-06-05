using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleNonexclusiveInterfaceImplementer)]
    public interface IExampleNonexclusiveInterfaceImplementer
    {
        int MyInt { get; set; }
    }
}