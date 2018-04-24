using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    public partial class ExampleChildInherited : ExampleChild, IExampleNonexclusiveInterface, IExampleChildInherited
    {
    }
}
