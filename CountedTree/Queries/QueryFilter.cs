using CountedTree.UintSets;
using System;

namespace CountedTree.Queries
{
    public partial class QueryFilter : IQueryFilter
    {
        public QueryFilter(UintSet searchWithin, QueryFilterRankingOptions rankingOptions, UintSet superset)
        {
            if ((superset != null) != (rankingOptions == QueryFilterRankingOptions.RankWithinSupersetOfItems))
                throw new Exception("Superset should be provided when ranking within a superset.");
            SearchWithin = searchWithin;
            RankingOptions = rankingOptions;
            Superset = superset;
        }

        public override bool Equals(object obj)
        {
            QueryFilter other = obj as QueryFilter;
            if (other == null)
                return false;

            return Equals(SearchWithin, other.SearchWithin) && RankingOptions == other.RankingOptions && Equals(Superset, other.Superset);
        }

        public override int GetHashCode()
        {
            return new Tuple<UintSet, QueryFilterRankingOptions, UintSet>(SearchWithin, RankingOptions, Superset).GetHashCode();
        }

        public bool FilteringRequired()
        {
            return SearchWithin != null || Superset != null;
        }

    }
}
