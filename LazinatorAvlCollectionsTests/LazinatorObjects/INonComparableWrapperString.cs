using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace LazinatorCollectionsTests
{
    [Lazinator((int)CollectionsTestsObjectIDs.INonComparableWrapperString, -1)]
    [NonbinaryHash]
    interface INonComparableWrapperString : IW<string>
    {
    }
}