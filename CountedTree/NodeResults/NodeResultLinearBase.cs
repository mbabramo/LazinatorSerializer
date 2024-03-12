using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.Node;
using Lazinator.Core;

namespace CountedTree.NodeResults
{
    public abstract partial class NodeResultLinearBase<TKey> : NodeResultBase<TKey>, INodeResultLinearBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultLinearBase() : base(0, 0)
        {

        }

        public NodeResultLinearBase(IncludedIndices includedIndices, uint filteredMatches, uint supersetMatches) : base(filteredMatches, supersetMatches)
        {
            IncludedIndices = includedIndices;
        }

        public virtual void SetResults(List<RankKeyAndID<TKey>> matches)
        {
            ResultsCount = (uint)matches.Count();
        }

    }
}
