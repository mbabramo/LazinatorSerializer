using Lazinator.Attributes;
using Lazinator.Core;
using System.Collections.Generic;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for a Lazinator container that allows items to be accessed by index. 
    /// </summary>
    /// <typeparam name="T">The type of the item within the Lazinator container</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableValueContainer)]
    public interface IIndexableValueContainer<T> : IValueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        /// <summary>
        /// Returns the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index</param>
        /// <returns></returns>
        T GetAtIndex(long index);
        /// <summary>
        /// Sets the item at the specified index
        /// </summary>
        /// <param name="index">The zero-based index</param>
        /// <param name="value">The value to set to</param>
        void SetAtIndex(long index, T value);
        /// <summary>
        /// Inserts the item at the specified index
        /// </summary>
        /// <param name="index">The zero-based index</param>
        /// <param name="value">The value to insert</param>
        void InsertAtIndex(long index, T item);
        /// <summary>
        /// Removes the item at the specified index
        /// </summary>
        /// <param name="index">The zero-based index</param>
        void RemoveAt(long index);
        /// <summary>
        /// Finds the index of an item, assuming that the container is sorted, if it exists.
        /// </summary>
        /// <param name="target">The item to find</param>
        /// <param name="comparer">The comparer to use to find and match the item</param>
        /// <returns>The index at which the item exists, if it exists, and an indication of whether it exists</returns>
        (long index, bool exists) FindIndex(T target, IComparer<T> comparer);
    }
}
