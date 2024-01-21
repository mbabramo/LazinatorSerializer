using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace Lazinator.CollectionsTests
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)CollectionsTestsObjectIDs.IStructWithBadHashFunction, -1)]
    interface IStructWithBadHashFunction : IW<int>
    {
    }
}