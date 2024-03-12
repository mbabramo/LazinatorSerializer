using CountedTree.NodeResults;
using CountedTree.UintSets;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.QueryExecution
{

    public abstract class LeafResultTrackerBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        internal uint? CachedReadyResultsCount = null;
        internal uint NumItemsAlreadyReturned = 0;

        public abstract uint CumulativeAvailableResultCount();

        public abstract void AddLeafResult(NodeResultBase<TKey> result);

        public abstract IEnumerable<object> TakeResultsAvailable(int? maxToTakeNow);

        public abstract UintSet GetResultsAsUintSet();

        public abstract void UpdateExpectations(IFurtherQueries<TKey> result);

        internal static UintSet ConvertResultsToUintSet(List<NodeResultBase<TKey>> results)
        {
            if (!results.Any())
                return new UintSet();
            var firstUintSet = results.First().GetUintSet();
            if (results.Count() == 1)
                return firstUintSet;
            else
            {
                UintSet u = firstUintSet;
                foreach (var r in results)
                    u = u.Union(r.GetUintSet());
                return u;
            }
        }

    }
}
