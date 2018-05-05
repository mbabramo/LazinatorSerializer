using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUshort, -1)]
    public interface ILazinatorWrapperUshort : ILazinatorWrapper<ushort>
    {
    }
}