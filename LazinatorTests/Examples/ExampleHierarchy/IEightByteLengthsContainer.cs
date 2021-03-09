using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.EightByteLengthsContainer)]
    [SizeOfLength(8)]
    public interface IEightByteLengthsContainer
    {
        EightByteLengths Contents { get; set; }
    }
}