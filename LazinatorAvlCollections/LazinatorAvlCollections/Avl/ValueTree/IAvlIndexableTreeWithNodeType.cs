using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using LazinatorAvlCollections.Avl.BinaryTree;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableTreeWithNodeType)]
    public interface IAvlIndexableTreeWithNodeType<T, N> where T : ILazinator where N : class, ILazinator, IIndexableNode<T, N>, new()
    {
    }
}