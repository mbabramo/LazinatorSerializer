using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace Lazinator.Examples.Structs
{
    [Lazinator((int)ExampleUniqueIDs.INonComparableWrapperString, -1)]
    [NonbinaryHash]
    interface INonComparableWrapperString : IW<string>
    {
    }
}