using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListContainer)]
    public interface IBigListContainer<T> where T : ILazinator
    {
        int BranchingFactor { get; set; }
        long Count { get; set; }
    }
}