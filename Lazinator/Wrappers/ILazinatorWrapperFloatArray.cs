using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperFloatArray, -1)]
    interface ILazinatorWrapperFloatArray : ILazinatorWrapper<float[]>
    {
    }
}