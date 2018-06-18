using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.WrapperContainer)]
    interface IWrapperContainer
    {
        WInt WrappedInt { get; set; }
    }
}
