using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.TwoByteLengths)]
    [SizeOfLength(2)]
    public interface ITwoByteLengths
    {
        Example Example { get; set; } // should not be affected
        TwoByteLengths Inner { get; set; } // it's the child object that has two byte lengths
    }
}