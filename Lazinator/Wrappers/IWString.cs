using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWString, -1)]
    [NonbinaryHash]
    [GenerateRefStruct] // DEBUG
    interface IWString : IW<string>
    {
    }
}