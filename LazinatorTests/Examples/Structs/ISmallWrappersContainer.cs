using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.SmallWrappersContainer)]
    public interface ISmallWrappersContainer
    {
        WBool WrappedBool { get; set; }
        WByte WrappedByte { get; set; }
        WSByte WrappedSByte { get; set; }
        WChar WrappedChar { get; set; }
        WNullableBool WrappedNullableBool { get; set; }
        WNullableByte WrappedNullableByte { get; set; }
        WNullableSByte WrappedNullableSByte { get; set; }
        WNullableChar WrappedNullableChar { get; set; }
        LazinatorList<WByte> ListWrappedBytes { get; set; }
    }
}