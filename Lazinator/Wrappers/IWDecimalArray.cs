using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWDecimalArray, -1)]
    interface IWDecimalArray : IW<decimal[]>
    {
    }
}