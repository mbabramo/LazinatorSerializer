using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableDouble, -1)]
    interface IWNullableDouble : IW<double?>
    {
    }
}