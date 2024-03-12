using CountedTree.Core;
using CountedTree.NodeResults;
using CountedTree.QueryExecution;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountedTree.Rebuild
{
    /// <summary>
    /// Rebuilds a tree from an existing tree. The new tree will be balanced, even if the old tree was not.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public partial class RebuildFromTreeSource<TKey> : IRebuildFromTreeSource<TKey>, IRebuildSource<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public RebuildFromTreeSource(TreeInfo sourceTree, DateTime asOfTime)
        {
            SourceTree = sourceTree;
            AsOfTime = asOfTime;
        }

        public async Task<WUInt32> GetNumberItems()
        {
            if (!LoadedRootInfo)
                await LoadRootInfo();
            return NumberItemsToGet;
        }

        public async Task<KeyAndID<TKey>?> GetFirstExclusive()
        {
            if (!LoadedRootInfo)
                await LoadRootInfo();
            return FirstExclusive;
        }

        public async Task<KeyAndID<TKey>?> GetLastInclusive()
        {
            if (!LoadedRootInfo)
                await LoadRootInfo();
            return LastInclusive;
        }

        private async Task LoadRootInfo()
        {
            var root = await SourceTree.GetRoot<TKey>();
            NumberItemsToGet = root.NodeInfo.NumSubtreeValues;
            FirstExclusive = root.NodeInfo.FirstExclusive;
            LastInclusive = root.NodeInfo.LastInclusive;
        }
        
        public async Task<List<KeyAndID<TKey>>> GetNextItems(int numValues)
        {
            TreeQueryExecutorLinear<TKey> executor = new TreeQueryExecutorLinear<TKey>(SourceTree, AsOfTime, NumberItemsGot, (uint?) numValues, QueryResultType.KeysAndIDs, true, null, null, null);
            var items = await executor.GetAllResults<KeyAndID<TKey>>();
            NumberItemsGot += (uint) items.Count();
            return items;
        }

        public Task ReportComplete()
        {
            return Task.CompletedTask;
        }
    }
}
