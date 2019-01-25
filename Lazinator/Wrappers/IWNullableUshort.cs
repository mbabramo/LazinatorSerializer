using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableUshort, -1)]
    interface IWNullableUshort : IW<ushort?>
    {
    }
}