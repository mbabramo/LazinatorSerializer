using CountedTree.Core;
using CountedTree.Queries;
using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace CountedTree.Node
{
    public partial class CountedLeafNodeMatcher<TKey> : ICountedLeafNodeMatcher<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public CountedLeafNodeMatcher(NodeQueryLinearBase<TKey> request)
        {
            this.Request = request;
            FilteredMatches = 0;
            SupersetMatches = 0;
            IndexNum = 0;
            SkipsProcessed = 0;
            RankInSuperset = request.IncludedIndices?.FirstIndexInSuperset ?? 0;
            Matches = new List<RankKeyAndID<TKey>>();
            Done = false;
        }

        public void ProcessItem(KeyAndID<TKey> item)
        {
            bool supersetMatch = Request.ItemMatches(item, (uint)IndexNum, false);
            bool filteredMatch = supersetMatch && Request.ItemMatches(item, (uint)IndexNum, true);
            if (filteredMatch)
            {
                if (SkipsProcessed < Request.Skip)
                    SkipsProcessed++;
                else
                {
                    Matches.Add(new RankKeyAndID<TKey>(RankInSuperset, item.Key, item.ID));
                    FilteredMatches++;
                }
            }
            if (supersetMatch)
            {
                SupersetMatches++;
                RankInSuperset++;
            }
            if (FilteredMatches == Request.Take)
                Done = true;
            else
                IndexNum++;
        }
    }
}
