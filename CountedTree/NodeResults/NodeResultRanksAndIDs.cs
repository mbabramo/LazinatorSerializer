using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.UintSets;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace CountedTree.NodeResults
{
    public partial class NodeResultRanksAndIDs<TKey> : NodeResultLinearBase<TKey>, INodeResultRanksAndIDs<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultRanksAndIDs(IncludedIndices includedIndices, uint filteredMatches, uint supersetMatches) : base(includedIndices, filteredMatches, supersetMatches)
        {
        }

        public override void SetResults(List<RankKeyAndID<TKey>> matches)
        {
            base.SetResults(matches);
            Values = matches.Select(x => x.GetRankAndID()).ToArray();
        }

        public override IEnumerable<object> GetResults()
        {
            return Values.AsEnumerable();
        }

        public override UintSet GetUintSet()
        {
            return new UintSet(new HashSet<WUInt32>(Values.Select(x => (WUInt32)x.ID)));
        }
    }
}
