using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.NodeResults;
using CountedTree.Queries;
using CountedTree.UintSets;
using Lazinator.Core;

namespace CountedTree.QueryExecution
{
    public class LeafResultTrackerLinear<TKey> : LeafResultTrackerBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        SortedList<Tuple<uint, uint>, NodeResultLinearBase<TKey>> QueryResults = new SortedList<Tuple<uint, uint>, NodeResultLinearBase<TKey>>();
        uint WaitingForExpectedFirstIndexFiltered;
        public uint ExpectedFirstIndexFiltered; // initially, the expected first index will be 0, unless we're using a skip.

        public void SetExpectedFirstIndex(uint expectedFirstIndexInFirstNodeWithResults)
        {
            ExpectedFirstIndexFiltered = WaitingForExpectedFirstIndexFiltered = expectedFirstIndexInFirstNodeWithResults;
        }

        public override uint CumulativeAvailableResultCount()
        {
            if (CachedReadyResultsCount == null)
            {
                WaitingForExpectedFirstIndexFiltered = ExpectedFirstIndexFiltered;
                CachedReadyResultsCount = CountAvailableResults_NoCache();
            }
            return (uint)CachedReadyResultsCount + NumItemsAlreadyReturned;
        }

        private uint CountAvailableResults_NoCache()
        {
            uint numItemsProcessed = 0;
            foreach (var queryResult in QueryResults.AsEnumerable())
            {
                if (queryResult.Key.Item1 == WaitingForExpectedFirstIndexFiltered)
                {
                    numItemsProcessed += queryResult.Value.ResultsCount;
                    WaitingForExpectedFirstIndexFiltered = queryResult.Key.Item2 + 1;
                }
                else
                    return numItemsProcessed;
            }
            return numItemsProcessed;
        }

        public override UintSet GetResultsAsUintSet()
        {
            var results = QueryResults.AsEnumerable().Select(x => (NodeResultBase<TKey>)x.Value).ToList();
            return ConvertResultsToUintSet(results);
        }

        public override IEnumerable<object> TakeResultsAvailable(int? maxToTakeNow)
        {
            var availableResults = QueryResults.AsEnumerable();
            if (maxToTakeNow != null)
                availableResults = availableResults.Take((int)maxToTakeNow);
            int numResultSetsProcessed = 0;
            foreach (var queryResult in availableResults)
            {
                if (queryResult.Key.Item1 == ExpectedFirstIndexFiltered)
                {
                    numResultSetsProcessed++;
                    var resultsCount = queryResult.Value.ResultsCount;
                    NumItemsAlreadyReturned += resultsCount;
                    ExpectedFirstIndexFiltered = queryResult.Key.Item2 + 1;
                    foreach (var itemResult in queryResult.Value.GetResults())
                        yield return itemResult;
                }
                else
                    break;
            }
            for (int i = 0; i < numResultSetsProcessed; i++)
                QueryResults.RemoveAt(0);
            CachedReadyResultsCount = 0;
        }

        public override void AddLeafResult(NodeResultBase<TKey> result)
        {
            lock (QueryResults)
            {
                NodeResultLinearBase<TKey> linearResult = (NodeResultLinearBase<TKey>)result;
                Tuple<uint, uint> expectedIndexRange;
                if (linearResult.IncludedIndices == null)
                    expectedIndexRange = new Tuple<uint, uint>(0, linearResult.ResultsCount - 1);
                else
                    expectedIndexRange = ExpectedIncludedIndices(linearResult);
                if (expectedIndexRange.Item1 == WaitingForExpectedFirstIndexFiltered)
                    CachedReadyResultsCount = null;
                QueryResults.Add(expectedIndexRange, linearResult);
            }
        }

        public override void UpdateExpectations(IFurtherQueries<TKey> result)
        {
            var furtherQueryResult = result as NodeResultLinearFurtherQueries<TKey>;
            if (ExpectedFirstIndexFiltered == furtherQueryResult.IncludedIndices.FirstIndexInSuperset && furtherQueryResult.FurtherQueries.Any())
            {
                NodeQueryLinearBase<TKey> query = (NodeQueryLinearBase<TKey>) furtherQueryResult.FurtherQueries.FirstOrDefault();
                SetExpectedFirstIndex(query.IncludedIndices.FirstIndexInFilteredSet);
            }
        }

        public static Tuple<uint, uint> ExpectedIncludedIndices(NodeResultLinearBase<TKey> result)
        {
            return new Tuple<uint, uint>(result.IncludedIndices.FirstIndexInFilteredSet, result.IncludedIndices.LastIndexInFilteredSet);
        }
    }
}
