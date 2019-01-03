using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IStructWithBadHashFunction, -1)]
    interface IStructWithBadHashFunction : IW<int>
    {
    }
}