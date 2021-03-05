using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.EightByteLengthsContainer)]
    [SizeOfLength(8)] // DEBUG -- add a malformed eight byte long container without the attribute
    public interface IEightByteLengthsContainer
    {
        EightByteLengths Contents { get; set; }
    }
}