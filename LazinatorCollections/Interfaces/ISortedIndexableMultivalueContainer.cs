using System;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for sorted, indexable multivalue containers
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableMultivalueContainer)]
    public interface ISortedIndexableMultivalueContainer<T> : ISortedMultivalueContainer<T>, IIndexableValueContainer<T>, ISortedIndexableContainer<T>,  IIndexableMultivalueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne);
    }
}
