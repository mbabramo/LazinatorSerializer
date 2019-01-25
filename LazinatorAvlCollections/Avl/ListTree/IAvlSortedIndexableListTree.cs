using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorAvlCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableListTree)]
    public interface IAvlSortedIndexableListTree<T> where T : ILazinator, IComparable<T>
    {
    }
}