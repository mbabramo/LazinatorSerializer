using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWIntArray, -1)]
    interface IWIntArray : IW<int[]>
    {
    }
}