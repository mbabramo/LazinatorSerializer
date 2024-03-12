using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountedTree.TypeAhead
{
    /// <summary>
    /// This is the interface for interacting with the type-ahead nodes. 
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public interface ITypeAheadStorage<R> where R : ILazinator, IComparable, IComparable<R>, IEquatable<R>
    {
        Task AddTypeAheadItemsFromChildren(Guid nodeID, List<TypeAheadItem<R>> typeAheadItems);
        Task<bool> AddTypeAheadItem(string stringToHere, Guid nodeID, char? nextChar, TypeAheadItem<R> typeAheadItem);
        Task<List<R>> GetSearchResults(Guid nodeID, int numToTake);
        Task<List<TypeAheadItem<R>>> GetTypeAheadItems(Guid nodeID, int numToTake, bool prioritizeExactMatches = false);
        Task<TypeAheadItemsForParent<R>> GetItemsForParent(Guid nodeID, string searchString, TypeAheadItem<R> lowestItemOnParent, int maxNumResults);
        Task<MoreResultsRequest<R>> RemoveResult(Guid nodeID, R result);
        int MinThreshold { get; set; }
        int MaxThreshold { get; set; }
    }
}