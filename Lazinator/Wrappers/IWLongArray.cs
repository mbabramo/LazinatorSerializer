using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WLongArray, -1)]
    interface IWLongArray : IW<long[]>
    {
    }
}