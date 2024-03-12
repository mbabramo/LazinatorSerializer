using System;
using System.Threading.Tasks;
using CountedTree.Core;
using CountedTree.NodeResults;
using CountedTree.Queries;
using CountedTree.Node;
using Lazinator.Core;

namespace CountedTree.QueryExecution
{
    public class TreeQueryExecutorLinear<TKey> : TreeQueryExecutorBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        LeafResultTrackerLinear<TKey> CompletedLeafResultsLinear => (LeafResultTrackerLinear<TKey>)CompletedLeafResults;
        
        bool Ascending;
        KeyAndID<TKey>? StartingValue;
        KeyAndID<TKey>? EndingValue;

        // Example: Suppose we query 1-1000 and there are 10 children nodes per interior nodes and 10 items per leaf node. We put this range into PendingQueries. If skip = 150, we get back nine further queries: 100-199 (skipping 50), 200-299, ... , or fewer if take is low enough. So we initiate these queries. 100-199 returns further queries of 150-159, ... ,. 

        public TreeQueryExecutorLinear(TreeInfo treeInfo, DateTime asOfTime, uint skip, uint? take, QueryResultType resultType, bool ascending, KeyAndID<TKey>? startingValue, KeyAndID<TKey>? endingValue, QueryFilter filter) : base(treeInfo, asOfTime, skip, take, resultType, filter)
        {
            TreeInfo = treeInfo;
            Ascending = ascending;
            if (filter != null && (StartingValue != null || EndingValue != null))
                throw new Exception("StartingValue/EndingValue queries are not supported in conjunction with filters."); // with a value range query, we can't know exactly how many items will be returned with a filter, and we thus can't do skip/take correctly. The alternative approach should be to use StartingValue and EndingValue to create a filter, and then use this filter to search along the specified range.
            StartingValue = startingValue;
            EndingValue = endingValue;
        }

        internal async override Task SetInitialQuery()
        {
            var pendingChanges = await GetPendingChanges();
            IncludedIndices includedIndices = null;
            if (StartingValue != null || EndingValue != null)
                InitialQuery = new NodeQueryValueRange<TKey>(TreeInfo.CurrentRootID, true, Ascending, Skip, Take, includedIndices, pendingChanges, ResultType, StartingValue, EndingValue);
            else
                InitialQuery = new NodeQueryIndexRange<TKey>(TreeInfo.CurrentRootID, true, Ascending, Skip, Take, includedIndices, pendingChanges, ResultType, Filter); 
        }

        internal override void InitializeCompletedLeafResults()
        {
            CompletedLeafResults = new LeafResultTrackerLinear<TKey>();
            CompletedLeafResultsLinear.SetExpectedFirstIndex(0); // for now, we are expecting that our first results will come from this node (but we'll change it if we get further queries)
        }

    }
}
