using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableTreeFactory)]
    public interface IAvlSortedIndexableTreeFactory<T> : ISortedIndexableContainerFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}