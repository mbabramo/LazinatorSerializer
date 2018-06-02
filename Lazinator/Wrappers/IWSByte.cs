using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WSByte, -1)]
    interface IWSByte : IW<sbyte>
    {
    }
}