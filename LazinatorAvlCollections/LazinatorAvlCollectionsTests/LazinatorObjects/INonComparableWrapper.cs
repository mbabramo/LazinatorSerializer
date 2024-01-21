using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace Lazinator.CollectionsTests
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)CollectionsTestsObjectIDs.INonComparableWrapper, -1)]
    interface INonComparableWrapper : IW<int>
    {
    }
}
