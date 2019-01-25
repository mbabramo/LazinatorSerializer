using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWString, -1)]
    [NonbinaryHash]
    [GenerateRefStruct] // DEBUG
    interface IWString : IW<string>
    {
    }
}