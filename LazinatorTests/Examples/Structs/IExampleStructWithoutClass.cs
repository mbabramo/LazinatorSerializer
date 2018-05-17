using Lazinator.Attributes;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.ExampleStructWithoutClass)]
    public interface IExampleStructWithoutClass
    {
        int MyInt { get; set; }
    }
}