using Lazinator.Attributes;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.EightByteLengths)]
    [SizeOfLength(8)]
    public interface IEightByteLengths
    {
        Example Example { get; set; }
    }
}