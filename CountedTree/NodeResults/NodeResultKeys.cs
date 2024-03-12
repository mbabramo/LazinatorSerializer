using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.UintSets;
using Lazinator.Core;

namespace CountedTree.NodeResults
{
    public partial class NodeResultKeys<TKey> : NodeResultLinearBase<TKey>, INodeResultKeys<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultKeys(IncludedIndices includedIndices, uint filteredMatches, uint supersetMatches) : base(includedIndices, filteredMatches, supersetMatches)
        {
        }

        public override void SetResults(List<RankKeyAndID<TKey>> matches)
        {
            base.SetResults(matches);
            Keys = matches.Select(x => x.Key).ToArray();
        }

        public override IEnumerable<object> GetResults()
        {
            return Keys.Select(x => (object) x);
        }

        public override UintSet GetUintSet()
        {
            throw new NotImplementedException();
        }
    }
}
