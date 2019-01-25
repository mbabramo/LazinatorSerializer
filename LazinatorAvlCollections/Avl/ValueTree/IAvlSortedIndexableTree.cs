using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableTree)]
    public interface IAvlSortedIndexableTree<T> where T : ILazinator, IComparable<T>
    {
    }
}