using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableListTree)]
    public interface IAvlSortedIndexableListTree<T> where T : ILazinator, IComparable<T>
    {
    }
}