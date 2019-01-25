using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)ExampleUniqueIDs.IStructWithBadHashFunction, -1)]
    interface IStructWithBadHashFunction : IW<int>
    {
    }
}