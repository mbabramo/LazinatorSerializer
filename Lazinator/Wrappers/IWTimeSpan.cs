using System;
using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWTimeSpan, -1)]
    interface IWTimeSpan : IW<TimeSpan>
    {
    }
}