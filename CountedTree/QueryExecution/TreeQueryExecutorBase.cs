using CountedTree.PendingChanges;
using CountedTree.NodeResults;
using CountedTree.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountedTree.Core;
using CountedTree.UintSets;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace CountedTree.QueryExecution
{
    public abstract class TreeQueryExecutorBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        public TreeInfo TreeInfo;
        public Guid TreeID => TreeInfo.TreeID;
        public DateTime AsOfTime;
        public QueryResultType ResultType;
        internal NodeQueryBase<TKey> InitialQuery;
        internal LeafResultTrackerBase<TKey> CompletedLeafResults;
        internal bool Complete = false;
        internal int NumItemsReturned = 0;
        internal List<Task<NodeResultBase<TKey>>> PendingTasks;
        internal uint Skip;
        internal uint? Take;
        internal QueryFilter Filter;

        public TreeQueryExecutorBase(TreeInfo treeInfo, DateTime asOfTime, uint skip, uint? take, QueryResultType resultType, QueryFilter filter)
        {
            TreeInfo = treeInfo;
            AsOfTime = asOfTime;
            ResultType = resultType;
            Skip = skip;
            Take = take;
            Filter = filter;
        }

        internal abstract Task SetInitialQuery();

        internal abstract void InitializeCompletedLeafResults();

        internal async Task<PendingChangesCollection<TKey>> GetPendingChanges()
        {
            return await StorageFactory.GetPendingChangesStorage().GetPendingChangesAsOfTime<TKey>(TreeID, TreeInfo.CurrentRootID, AsOfTime);
        }

        internal async Task Start()
        {
            await SetInitialQuery();
            if (InitialQuery == null)
                Complete = true; // tree is empty, taking into account pending changes; nothing to search for
            else
            {
                NodeResultBase<TKey> initialResult = await ProcessQuery(InitialQuery);
                PendingTasks = GetFurtherQueryResults(initialResult).ToList();
            }
        }


        internal Task<NodeResultBase<TKey>> ProcessQuery(NodeQueryBase<TKey> query)
        {
            // Note that that this is not async. It immediately returns a task. One can await ProcessQuery, however, to ensure that the Task is completed.
            // We use this approach because GetFurtherQueryResults calls ProcessQuery many times and returns all of these tasks, and the code calling it ensures that the tasks execute.
            if (query.NodeHasStorage)
            {
                var result = StorageFactory.GetNodeStorage().ProcessQuery(TreeInfo.TreeID, query.NodeID, query);
                return result;
            }
            else
            {
                return query.ProcessOnEmptyNode();
            }
        }


        public async Task<List<KeyAndID<TKey>>> GetKeysAndIDs(bool waitForAll = true)
        {
            if (ResultType != QueryResultType.KeysAndIDs)
                throw new Exception("Inconsistent query result types.");
            if (waitForAll)
                return await GetAllResults<KeyAndID<TKey>>();
            else
                return await GetSomeResults<KeyAndID<TKey>>();
        }

        public async Task<List<WUInt32>> GetIDs(bool waitForAll = true)
        {
            if (ResultType != QueryResultType.IDsOnly)
                throw new Exception("Inconsistent query result types.");
            if (waitForAll)
                return await GetAllResults<WUInt32>();
            else
                return await GetSomeResults<WUInt32>();
        }


        public async Task<UintSet> GetIDsAsBitSet()
        {
            if (ResultType != QueryResultType.IDsAsBitSet)
                throw new Exception("Inconsistent query result types.");
            UintSet u = await GetAllResultsAsUintSet();
            return u;
        }

        public async Task<List<TKey>> GetKeys(bool waitForAll = true)
        {
            if (ResultType != QueryResultType.KeysOnly)
                throw new Exception("Inconsistent query result types.");
            if (waitForAll)
                return await GetAllResults<TKey>();
            else
                return await GetSomeResults<TKey>();
        }

        internal async Task<UintSet> GetAllResultsAsUintSet()
        {
            await LoadCompleteResultsAtOnce();
            if (CompletedLeafResults == null)
                return new UintSet();
            return CompletedLeafResults.GetResultsAsUintSet();
        }

        internal async Task<List<U>> GetAllResults<U>()
        {
            var results = await GetAllResultsUntyped();
            return results.Select(x => (U) x).ToList();
        }

        private async Task<IEnumerable<object>> GetAllResultsUntyped()
        {
            await LoadCompleteResultsAtOnce();
            if (CompletedLeafResults == null)
                return new List<object>();
            var results = CompletedLeafResults.TakeResultsAvailable(null);
            return results;
        }

        private async Task LoadCompleteResultsAtOnce()
        {
            if (PendingTasks == null)
                await Start();
            if (Complete)
                return;
            while (true)
            {
                if (Take == 0)
                    Complete = true;
                else
                {
                    uint numComplete = CompletedLeafResults.CumulativeAvailableResultCount();
                    Complete = (numComplete >= Take); // will be false if Take is null
                }
                if (!PendingTasks.Any())
                    Complete = true; // we're completing early -- this can happen in a geo query when the number of results is less than the maximum we're willing to take (because many are outside the specified circle)
                else
                {
                    Task<NodeResultBase<TKey>> completedTask = await Task.WhenAny(PendingTasks.ToArray());
                    PendingTasks.Remove(completedTask);
                    var furtherQueries = GetFurtherQueryResults(await completedTask);
                    PendingTasks.AddRange(furtherQueries);
                }
                if (Complete)
                    return;
            }
        }


        internal async Task<List<U>> GetSomeResults<U>(int? maxToTakeNow = null)
        {
            var results = await GetSomeResultsUntyped(maxToTakeNow);
            return results.Select(x => (U)x).ToList();
        }

        private async Task<List<object>> GetSomeResultsUntyped(int? maxToTakeNow)
        {
            if (PendingTasks == null)
                await Start();
            while (true)
            {
                if (Take == 0)
                    Complete = true;
                var resultsToReturn = GetResultsToReturn(maxToTakeNow);
                if (resultsToReturn != null)
                    return resultsToReturn;
                if (!PendingTasks.Any())
                {
                    Complete = true; // we're completing early -- this can happen in a geo query when the number of results is less than the maximum we're willing to take (because many are outside the specified circle)
                    return new List<object>();
                }
                Task<NodeResultBase<TKey>> completedTask = await Task.WhenAny(PendingTasks.ToArray());
                PendingTasks.Remove(completedTask);
                var furtherQueries = GetFurtherQueryResults(await completedTask);
                PendingTasks.AddRange(furtherQueries);
            }
        }

        internal List<object> GetResultsToReturn(int? maxToTakeNow)
        {
            if (Complete) // no more results should be sent
                return new List<object>();
            uint numComplete = CompletedLeafResults.CumulativeAvailableResultCount();
            Complete = (numComplete >= Take);
            if (numComplete > NumItemsReturned)
            { // some more results to return
                NumItemsReturned = (int)numComplete;
                return CompletedLeafResults.TakeResultsAvailable(maxToTakeNow).ToList();
            }
            return null;
        }

        private IEnumerable<Task<NodeResultBase<TKey>>> GetFurtherQueryResults(NodeResultBase<TKey> result)
        {
            bool notInitialized = CompletedLeafResults == null;
            if (notInitialized)
                InitializeCompletedLeafResults();
            if (result is IFurtherQueries<TKey>)
            {
                var furtherQueryResult = (IFurtherQueries<TKey>)result;
                CompletedLeafResults.UpdateExpectations(furtherQueryResult);
                foreach (var furtherQuery in furtherQueryResult.FurtherQueries)
                    yield return ProcessQuery(furtherQuery);
            }
            else
            {
                CompletedLeafResults.AddLeafResult((NodeResultBase<TKey>)result);
                yield break;
            }
        }
    }
}
