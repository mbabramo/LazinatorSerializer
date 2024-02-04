using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedIndexableTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlSortedIndexableTree<T> : IAvlIndexableTreeWithNodeType<T, AvlCountedNode<T>> where T : ILazinator, IComparable<T>
    {

    }
}