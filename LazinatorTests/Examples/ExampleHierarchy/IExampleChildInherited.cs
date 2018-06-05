using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleChildInherited)]
    public interface IExampleChildInherited : IExampleChild
    {
        int MyInt { get; set; }
    }
}