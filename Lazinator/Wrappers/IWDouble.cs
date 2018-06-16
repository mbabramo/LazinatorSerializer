using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WDouble, -1)]
    interface IWDouble : IW<double>
    {
    }
}