using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.EightByteLengthsContainer)]
    [SizeOfLength(8)] // DEBUG -- add a malformed eight byte long container without the attribute. Must also try to inherit from a class with a lower byte count. Finally, must check proper inheritance with same byte count.
    public interface IEightByteLengthsContainer
    {
        EightByteLengths Contents { get; set; }
    }
}