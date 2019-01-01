using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBigListInteriorContainer)]
    internal interface IBigListInteriorContainer<T> where T : ILazinator
    {
        LazinatorList<WLong> ChildContainerCounts { get; set; }
        LazinatorList<WInt> ChildContainerMaxPathToLeaf { get; set; }
        WInt MaxPathToLeaf { get; set; }
    }
}