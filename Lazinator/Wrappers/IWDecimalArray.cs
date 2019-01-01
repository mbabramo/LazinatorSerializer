using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWDecimalArray, -1)]
    interface IWDecimalArray : IW<decimal[]>
    {
    }
}