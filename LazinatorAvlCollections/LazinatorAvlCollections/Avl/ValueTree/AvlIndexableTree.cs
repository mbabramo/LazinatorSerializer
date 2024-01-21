using Lazinator.Core;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public partial class AvlIndexableTree<T> : AvlIndexableTreeWithNodeType<T, AvlCountedNode<T>>, IAvlTree<T>, IIndexableMultivalueContainer<T> where T : ILazinator
    {
        public AvlIndexableTree(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {

        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlIndexableTree<T>(AllowDuplicates, Unbalanced, CacheEnds);
        }
    }
}
