using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorOffsetList)]
    interface ILazinatorOffsetList
    {
        LazinatorFastReadListInt16 TwoByteItems { get; set; }
        LazinatorFastReadListInt32 FourByteItems { get; set; }
    }
}
