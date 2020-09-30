using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.WrapperContainer)]
    interface IWrapperContainer
    {
        WInt32 WrappedInt { get; set; }
    }
}
