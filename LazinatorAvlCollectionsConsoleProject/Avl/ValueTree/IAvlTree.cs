using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlTree<T> : IAvlTreeWithNodeType<T, AvlNode<T>> where T : ILazinator
    {
    }
}
