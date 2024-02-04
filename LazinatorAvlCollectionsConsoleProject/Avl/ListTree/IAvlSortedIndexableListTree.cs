using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorAvlCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedIndexableListTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlSortedIndexableListTree<T> : IAvlIndexableListTree<T> where T : ILazinator, IComparable<T>
    {
    }
}