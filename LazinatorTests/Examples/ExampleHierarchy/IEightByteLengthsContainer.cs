using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.EightByteLengthsContainer)]
    public interface IEightByteLengthsContainer
    {
        EightByteLengths Contents { get; set; }
    }
}