using System;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted, indexable containers
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableContainer)]
    public interface ISortedIndexableContainer<T> : ISortedValueContainer<T>, IIndexableValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) FindIndex(T target);
    }
}
