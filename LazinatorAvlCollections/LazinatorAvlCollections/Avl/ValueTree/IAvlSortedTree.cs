using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedTree)]
    public interface IAvlSortedTree<T> : ISortedMultivalueContainer<T> where T : IComparable<T>, ILazinator
    {
    }
}