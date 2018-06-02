using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WDecimalArray, -1)]
    interface IWDecimalArray : IW<decimal[]>
    {
    }
}