using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWFloat, -1)]
    interface IWFloat : IW<float>
    {
    }
}