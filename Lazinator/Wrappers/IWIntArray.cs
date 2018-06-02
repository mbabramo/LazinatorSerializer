using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WIntArray, -1)]
    interface IWIntArray : IW<int[]>
    {
    }
}