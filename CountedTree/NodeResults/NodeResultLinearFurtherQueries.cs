using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.Queries;
using CountedTree.Node;
using CountedTree.UintSets;
using Lazinator.Core;

namespace CountedTree.NodeResults
{
    public partial class NodeResultLinearFurtherQueries<TKey> : NodeResultLinearBase<TKey>, IFurtherQueries<TKey>, INodeResultLinearFurtherQueries<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultLinearFurtherQueries(IncludedIndices includedIndices, List<NodeQueryBase<TKey>> furtherQueries, uint filteredMatches, uint supersetMatches) : base(includedIndices, filteredMatches, supersetMatches)
        {
            FurtherQueries = furtherQueries;
        }

        public override void SetResults(List<RankKeyAndID<TKey>> matches)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<object> GetResults()
        {
            return FurtherQueries.AsEnumerable();
        }

        public override UintSet GetUintSet()
        {
            throw new NotImplementedException();
        }
    }
}
