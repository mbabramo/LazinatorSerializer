using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWFloatArray, -1)]
    interface IWFloatArray : IW<float[]>
    {
    }
}