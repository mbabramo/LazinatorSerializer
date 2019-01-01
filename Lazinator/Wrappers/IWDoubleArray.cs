using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWDoubleArray, -1)]
    interface IWDoubleArray : IW<double[]>
    {
    }
}