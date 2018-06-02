using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.SmallWrappersContainer)]
    public interface ISmallWrappersContainer
    {
        WByte WrappedByte { get; set; }
        WSByte WrappedSByte { get; set; }
        WChar WrappedChar { get; set; }
    }
}