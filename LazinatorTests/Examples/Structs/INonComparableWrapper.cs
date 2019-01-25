using Lazinator.Attributes;
using Lazinator.Collections;
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
