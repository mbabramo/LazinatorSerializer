using Lazinator.Core;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public partial class AvlTree<T> : AvlTreeWithNodeType<T, AvlNode<T>>, IAvlTree<T> where T : ILazinator
    {
        public AvlTree(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {

        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlTree<T>(AllowDuplicates, Unbalanced, CacheEnds);
        }
    }
}
