using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWLongArray, -1)]
    interface IWLongArray : IW<long[]>
    {
    }
}