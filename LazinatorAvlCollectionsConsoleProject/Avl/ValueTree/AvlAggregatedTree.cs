using Lazinator.Core;
using Lazinator.Collections;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public partial class AvlAggregatedTree<T> : AvlAggregatedTreeWithNodeType<T, AvlAggregatedNode<T>>, IAvlAggregatedTree<T> where T : ILazinator, ICountableContainer
    {
        public AvlAggregatedTree(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced)
        {

        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlAggregatedTree<T>(AllowDuplicates, Unbalanced);
        }
    }
}
