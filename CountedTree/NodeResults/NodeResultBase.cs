using CountedTree.UintSets;
using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace CountedTree.NodeResults
{
    public abstract partial class NodeResultBase<TKey> : INodeResultBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeResultBase(uint filteredMatches, uint supersetMatches)
        {
            FilteredMatches = filteredMatches;
            SupersetMatches = supersetMatches;
        }

        public abstract IEnumerable<object> GetResults();

        public abstract UintSet GetUintSet();

    }
}
