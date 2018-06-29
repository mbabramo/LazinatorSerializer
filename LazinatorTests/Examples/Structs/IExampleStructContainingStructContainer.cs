using Lazinator.Attributes;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.ExampleStructContainingStructContainer)]
    interface IExampleStructContainingStructContainer
    {
        ExampleStructContainingStruct Subcontainer { get; set; }
    }
}