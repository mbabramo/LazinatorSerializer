using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableTree)]
    public interface IAvlSortedIndexableTree<T> where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
    }
}