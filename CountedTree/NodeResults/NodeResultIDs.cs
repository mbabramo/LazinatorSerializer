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
    public partial class NodeResultIDs<TKey> : NodeResultLinearBase<TKey>, INodeResultIDs<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultIDs(IncludedIndices includedIndices, uint filteredMatches, uint supersetMatches) : base(includedIndices, filteredMatches, supersetMatches)
        {
        }

        public override void SetResults(List<RankKeyAndID<TKey>> matches) 
        {
            base.SetResults(matches);
            IDs = matches.Select(x => (WUInt32)x.ID).ToArray();
        }

        public override IEnumerable<object> GetResults()
        {
            return IDs.Select(x => (object)x);
        }

        public override UintSet GetUintSet()
        {
            return new UintSet(new HashSet<WUInt32>(IDs));
        }
    }
}
