using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WFloatArray, -1)]
    interface IWFloatArray : IW<float[]>
    {
    }
}