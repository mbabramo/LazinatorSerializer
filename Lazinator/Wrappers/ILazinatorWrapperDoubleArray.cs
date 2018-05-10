using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDoubleArray, -1)]
    interface ILazinatorWrapperDoubleArray : ILazinatorWrapper<double[]>
    {
    }
}