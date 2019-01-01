using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWFloatArray, -1)]
    interface IWFloatArray : IW<float[]>
    {
    }
}