using Lazinator.Core;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    /// <summary>
    /// A binary tree. Because it stores a value that need not implement IComparable, and because it is not countable,
    /// direct users of this class must specify a custom comparer when searching, inserting or removing. Subclasses add
    /// functionality for balancing, for accessing items by index, and for adding items that implement IComparable without a custom comparer.
    /// Note that Lazinator.Collections implements a separate simple LazinatorBinaryTree<typeparamref name="T"/> where T : IComparable. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BinaryTree<T> : BinaryTreeWithNodeType<T, BinaryNode<T>>, IBinaryTree<T> where T : ILazinator
    {
        public BinaryTree(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {

        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new BinaryTree<T>(AllowDuplicates, Unbalanced, CacheEnds);
        }
    }
}
