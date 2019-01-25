using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace LazinatorCollectionsTests
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)CollectionsTestsObjectIDs.IStructWithBadHashFunction, -1)]
    interface IStructWithBadHashFunction : IW<int>
    {
    }
}