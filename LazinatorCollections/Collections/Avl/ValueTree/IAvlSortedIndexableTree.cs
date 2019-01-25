using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableTree)]
    public interface IAvlSortedIndexableTree<T> where T : ILazinator, IComparable<T>
    {
    }
}