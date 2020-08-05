using System.Collections.Generic;
using Lazinator.Attributes;
using LazinatorCollections.Location;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for multivalue containers.
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultivalueContainer)]
    public interface IMultivalueContainer<T> : IValueContainer<T>, ILazinator where T : ILazinator
    {
        /// <summary>
        /// Whether more than one of an item may exist in a container. If yes, the container takes advantage
        /// of the multivalue container features. If no, the container works like an ordinary sorted container.
        /// </summary>
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        /// <summary>
        /// Finds the item in the sorted container, using the comparer. If the item is found, the exact location is returned. Otherwise,
        /// the next location is returned (or null, if it would be after the end of the list).
        /// </summary>
        /// <param name="value">The value to search for</param>
        /// <param name="whichOne">The item to find (first, last, or any), if there is more than one of the item.</param>
        /// <param name="comparer">The comparer to use to find one of the item in the sorted container</param>
        /// <returns>The location, along with an indication of whether an exact match was found</returns>
        (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        /// <summary>
        /// Searches for an item matching the comparer. If found, it returns true and sets match to the item. Note that a comparer may
        /// look at only some components of an item, so the match will not necessarily be equal to the item.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="whichOne">The item to find (first, last, or any), if there is more than one of the item.</param>
        /// <param name="comparer">The comparer to use to find one of the item in the sorted container</param>
        /// <param name="match">A match for the item, or default if not found</param>
        /// <returns></returns>
        bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match);
        /// <summary>
        /// Inserts an item in a container (if there is no match) or replaces it (if there is a match using the specified comparer).
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="whichOne">The item to find (first, last, or any), if there is more than one of the item.</param>
        /// <param name="comparer">The comparer to use to find one of the item in the sorted container</param>
        /// <returns>The location in the container of the item and an indication of whether an insertion or a replacement occurred.</returns>
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        /// <summary>
        /// Removes the item from the container, if possible
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="whichOne">The item to find (first, last, or any), if there is more than one of the item.</param>
        /// <param name="comparer">The comparer to use to find one of the item in the sorted container</param>
        /// <returns>Whether the item was found</returns>
        bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        /// <summary>
        /// Removes all instances of the item from the container
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="comparer">The comparer to use to find one of the item in the sorted container</param>
        /// <returns>Whether any of the item previously existed in the container</returns>
        bool TryRemoveAll(T item, IComparer<T> comparer);
        /// <summary>
        /// The number of items in the container.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        long Count(T item, IComparer<T> comparer);
    }
}