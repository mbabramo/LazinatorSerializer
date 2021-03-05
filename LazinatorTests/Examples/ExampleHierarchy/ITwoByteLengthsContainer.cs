using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.TwoByteLengthsContainer)]
    public interface ITwoByteLengthsContainer
    {
        TwoByteLengths Contents { get; set; }
    }
}