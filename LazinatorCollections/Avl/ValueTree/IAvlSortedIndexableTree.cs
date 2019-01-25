using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableTree)]
    public interface IAvlSortedIndexableTree<T> where T : ILazinator, IComparable<T>
    {
    }
}