using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUshort, -1)]
    interface ILazinatorWrapperUshort : ILazinatorWrapper<ushort>
    {
    }
}