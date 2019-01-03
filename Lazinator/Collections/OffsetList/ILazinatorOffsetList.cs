using Lazinator.Attributes;

namespace Lazinator.Collections.OffsetList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorOffsetList)]
    interface ILazinatorOffsetList
    {
        LazinatorFastReadListInt16 TwoByteItems { get; set; }
        LazinatorFastReadListInt32 FourByteItems { get; set; }
    }
}
