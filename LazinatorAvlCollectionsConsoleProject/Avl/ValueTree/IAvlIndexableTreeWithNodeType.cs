using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using LazinatorAvlCollections.Avl.BinaryTree;
using LazinatorAvlCollections.Avl.ValueTree;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableTreeWithNodeType)]
    public interface IAvlIndexableTreeWithNodeType<T, N> : IAvlTreeWithNodeType<T, N> where T : ILazinator where N : class, ILazinator, IIndexableNode<T, N>, new()
    {
    }
}