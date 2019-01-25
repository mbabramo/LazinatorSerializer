using Lazinator.Attributes;
using LazinatorCollections;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)ExampleUniqueIDs.INonComparableWrapper, -1)]
    interface INonComparableWrapper : IW<int>
    {
    }
}
