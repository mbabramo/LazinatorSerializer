using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListInteriorNode)]
    internal interface IBigListInteriorNode<T> where T : ILazinator
    {
        LazinatorList<WLong> Counts { get; set; }
    }
}