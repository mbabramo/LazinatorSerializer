using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListInteriorContainer)]
    internal interface IBigListInteriorContainer<T> where T : ILazinator
    {
        LazinatorList<WLong> ChildContainerCounts { get; set; }
    }
}