using Lazinator.Core;
using Lazinator.Attributes;
using System.Collections.Generic;
using System;

namespace CountedTree.Node
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.MultipleNodesReplacementResults)]
    public interface IMultipleNodesReplacementResults<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        CountedInternalNode<TKey> InPlaceReplacement { get; set; }
        List<CountedNode<TKey>> AdditionalNodes { get; set; }
    }
}