using Lazinator.Core;
using Lazinator.Collections;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    /// <summary>
    /// An aggregated Avl tree, where each node indicates how many items of that type it contains.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlAggregatedTree<T> : AvlAggregatedTreeWithNodeType<T, AvlAggregatedNode<T>>, IAvlAggregatedTree<T> where T : ILazinator, ICountableContainer
    {
        public AvlAggregatedTree(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced)
        {

        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlAggregatedTree<T>(AllowDuplicates, Unbalanced);
        }

        public override bool ShouldSplit(int splitThreshold)
        {
            if (this is ICountableContainer countable)
            {
                if (AvlRoot == null)
                    return false;
                return AvlRoot.LongCount > splitThreshold; // note that this is not the aggregated count
            }
            return GetApproximateDepth() > splitThreshold;
        }
    }
}
