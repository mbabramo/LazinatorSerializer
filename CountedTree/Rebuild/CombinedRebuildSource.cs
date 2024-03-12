using CountedTree.Core;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountedTree.Rebuild
{
    /// <summary>
    /// A source for rebuilding a tree that combines multiple rebuild sources, each of which contains some subset of the data. 
    /// It proceeds by maintaining a priority queue of the next item to be added to the tree, determined by considering the next
    /// item from each of the sources.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    public partial class CombinedRebuildSource<TKey> : ICombinedRebuildSource<TKey>, IRebuildSource<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        public static int CombinedNumValuesToHoldInBuffers = 10000; 

        public CombinedRebuildSource(List<IRebuildSource<TKey>> rebuildSources, KeyAndID<TKey> firstExclusive, KeyAndID<TKey> lastInclusive)
        {
            RebuildSources = rebuildSources;
            NextItemsInSources = new Queue<KeyAndID<TKey>>[rebuildSources.Count()];
            FirstExclusive = firstExclusive;
            LastInclusive = lastInclusive;
            SourceComplete = new bool[rebuildSources.Count()];
            LowestItemInEachSubsource = new KeyAndID<TKey>?[SourceComplete.Length];
        }

        public async Task<WUInt32> GetNumberItems()
        {
            Task<WUInt32>[] tasks = RebuildSources.Select(x => x.GetNumberItems()).ToArray();
            await Task.WhenAll(tasks);
            return (uint) tasks.Sum(x => x.Result);
        }

        public Task<KeyAndID<TKey>?> GetFirstExclusive()
        {
            return Task.FromResult(FirstExclusive);
        }

        public Task<KeyAndID<TKey>?> GetLastInclusive()
        {
            return Task.FromResult(LastInclusive);
        }

        public async Task UpdateSource(int i)
        {
            int numValuesPerSource = CombinedNumValuesToHoldInBuffers / SourceComplete.Length;
            if (!SourceComplete[i] && NeedToUpdateSource(i))
            {
                NextItemsInSources[i] = new Queue<KeyAndID<TKey>>(await RebuildSources[i].GetNextItems(numValuesPerSource));
                if (!NextItemsInSources[i].Any())
                    SourceComplete[i] = true;
            }
            AllComplete = SourceComplete.All(x => x);
        }

        private async Task InitializeLowestItemArray()
        {
            for (int i = 0; i < SourceComplete.Length; i++)
                await UpdateNextForSource(i);
            Initialized = true;
        }

        private async Task UpdateNextForSource(int i)
        {
            if (!SourceComplete[i])
            {
                if (NeedToUpdateSource(i))
                    await UpdateSource(i);
                if (NextItemsInSources[i].Any())
                    LowestItemInEachSubsource[i] = NextItemsInSources[i].Dequeue();
                else
                    LowestItemInEachSubsource[i] = null;
            }
        }

        private bool NeedToUpdateSource(int i)
        {
            return NextItemsInSources[i] == null || !NextItemsInSources[i].Any();
        }

        private Tuple<KeyAndID<TKey>, int> GetLowestAndItsIndex()
        {
            var lowest = LowestItemInEachSubsource.Select((item, index) => new { Item = item, Index = index }).Where(x => x.Item != null).OrderBy(x => x.Item).First();
            return new Tuple<KeyAndID<TKey>, int>((KeyAndID<TKey>)lowest.Item, lowest.Index);
        }

        public async Task<List<KeyAndID<TKey>>> GetNextItems(int numItems)
        {
            if (!Initialized)
                await InitializeLowestItemArray();
            List<KeyAndID<TKey>> results = new List<KeyAndID<TKey>>();
            for (int i = 0; i < numItems; i++)
            {
                if (AllComplete)
                    break;
                Tuple<KeyAndID<TKey>, int> lowest = GetLowestAndItsIndex();
                results.Add(lowest.Item1);
                await UpdateNextForSource(lowest.Item2);
            }
            return results;
        }

        public async Task ReportComplete()
        {
            Task[] tasks = RebuildSources.Select(x => x.ReportComplete()).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
