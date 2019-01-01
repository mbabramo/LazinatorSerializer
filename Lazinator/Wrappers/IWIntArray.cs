using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWIntArray, -1)]
    interface IWIntArray : IW<int[]>
    {
    }
}