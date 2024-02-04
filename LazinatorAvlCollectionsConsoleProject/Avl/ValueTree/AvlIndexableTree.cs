using Lazinator.Core;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    /// <summary>
    /// An indexable Avl tree, where individual items can be referenced by index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlIndexableTree<T> : AvlIndexableTreeWithNodeType<T, AvlCountedNode<T>>, IAvlIndexableTree<T>, IIndexableMultivalueContainer<T> where T : ILazinator
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
