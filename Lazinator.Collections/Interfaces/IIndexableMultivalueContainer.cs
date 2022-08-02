using Lazinator.Attributes;
using Lazinator.Core;
using System.Collections.Generic;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for Lazinator containers supporting multiple values and indexing.
    /// </summary>
    /// <typeparam name="T">The type of the item within the container</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableMultivalueContainer)]
    public interface IIndexableMultivalueContainer<T> : IIndexableValueContainer<T>, IMultivalueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        /// <summary>
        /// Finds the index of an item using the specified comparer
        /// </summary>
        /// <param name="target">The item to find</param>
        /// <param name="whichOne">The item to find (first, last, or any), if there is more than one of the item.</param>
        /// <param name="comparer">The comparer to use to find one of the item in the sorted container</param>
        /// <returns>The index at which the item is found, if it exists, and an indication of whether it exists</returns>
        (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
