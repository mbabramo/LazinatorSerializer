using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    public partial class ExampleNonexclusiveInterfaceImplementer : IExampleNonexclusiveInterface, IExampleNonexclusiveInterfaceImplementer
    {
    }
}