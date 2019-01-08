using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedTree)]
    public interface IAvlSortedTree<T> : ISortedMultivalueContainer<T> where T : IComparable<T>, ILazinator
    {
    }
}