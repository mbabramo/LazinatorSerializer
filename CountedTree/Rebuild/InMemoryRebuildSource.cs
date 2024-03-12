using CountedTree.Core;
using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Lazinator.Collections;

namespace CountedTree.Rebuild
{
    /// <summary>
    /// An in-memory source for rebuilding a tree. It stores the items to be added to the tree and then enumerate through them.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public partial class InMemoryRebuildSource<TKey> : IInMemoryRebuildSource<TKey>, IRebuildSource<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public InMemoryRebuildSource(List<KeyAndID<TKey>> items, KeyAndID<TKey> firstExclusive, KeyAndID<TKey> lastInclusive)
        {
            if (!items.IsSorted())
                Items = new LazinatorList<KeyAndID<TKey>>(items.OrderBy(x => x), true);
            else
                Items = new LazinatorList<KeyAndID<TKey>>(items, true);
            NumProcessed = 0;
            FirstExclusive = firstExclusive;
            LastInclusive = lastInclusive;
        }

        public Task<WUInt32> GetNumberItems()
        {
            return Task.FromResult((WUInt32)(uint) Items.LongCount());
        }

        public Task<KeyAndID<TKey>?> GetFirstExclusive()
        {
            return Task.FromResult(FirstExclusive);
        }

        public Task<KeyAndID<TKey>?> GetLastInclusive()
        {
            return Task.FromResult(LastInclusive);
        }

        public Task<List<KeyAndID<TKey>>> GetNextItems(int numValues)
        {
            List<KeyAndID<TKey>> results = new List<KeyAndID<TKey>>();
            for (int i = 0; i < numValues; i++)
                if (NumProcessed < Items.Count())
                    results.Add(Items[NumProcessed++]);
            return Task.FromResult(results);
        }

        public Task ReportComplete()
        {
            return Task.CompletedTask;
        }
    }
}
