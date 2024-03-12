using System;
using System.Collections.Generic;
using CountedTree.Node;
using CountedTree.UintSets;
using Lazinator.Core;

namespace CountedTree.NodeResults
{
    public partial class NodeResultUintSet<TKey> : NodeResultLinearBase<TKey>, INodeResultUintSet<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultUintSet(UintSet uintSet, IncludedIndices includedIndices, uint filteredMatches, uint supersetMatches) : base(includedIndices, filteredMatches, supersetMatches)
        {
            UintSet = uintSet;
        }

        public override IEnumerable<object> GetResults()
        {
            throw new NotImplementedException();
        }

        public override UintSet GetUintSet()
        {
            return UintSet;
        }
    }
}
