using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWDoubleArray, -1)]
    interface IWDoubleArray : IW<double[]>
    {
    }
}