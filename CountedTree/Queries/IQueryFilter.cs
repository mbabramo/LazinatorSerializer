using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.UintSets;

namespace CountedTree.Queries
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.QueryFilter)]
    
    public interface IQueryFilter
    {
        /// <summary>
        /// A bitset filtering the items to search, or null if no filter is needed. Only items matching the filter will be returned. For example, this might represent items that have not been deleted or items that are within a certain distance. We will then take from these items only.
        /// </summary>
        [SetterAccessibility("private")]
        UintSet SearchWithin { get; }
        /// <summary>
        /// Indicates whether to rank search results relative to the filter, a superset of the filter, or all items. 
        /// </summary>
        [SetterAccessibility("private")]
        QueryFilterRankingOptions RankingOptions { get; }
        /// <summary>
        /// Specifies a Supserset. Will be null if RankingOptions does not specify to use a superset.
        /// </summary>
        [SetterAccessibility("private")]
        UintSet Superset { get; }

    }
}