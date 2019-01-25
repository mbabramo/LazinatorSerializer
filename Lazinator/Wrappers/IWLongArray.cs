using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWLongArray, -1)]
    interface IWLongArray : IW<long[]>
    {
    }
}