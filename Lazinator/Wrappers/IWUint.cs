using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWUint, -1)]
    interface IWUint : IW<uint>
    {
    }
}