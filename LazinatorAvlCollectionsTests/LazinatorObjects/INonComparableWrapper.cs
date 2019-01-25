using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace LazinatorCollectionsTests
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)CollectionsTestsObjectIDs.INonComparableWrapper, -1)]
    interface INonComparableWrapper : IW<int>
    {
    }
}
