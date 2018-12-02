using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WString, -1)]
    [NonbinaryHash]
    interface IWString : IW<string>
    {
    }
}