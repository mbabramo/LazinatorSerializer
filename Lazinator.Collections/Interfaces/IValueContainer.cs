using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interface for containers of values, allowing access to items and operations such as enumeration and 
    /// finding items using a custom comparer. This is compatible with non-indexable containers and thus uses IContainerLocation to 
    /// identify locations within the container instead of specific indices. 
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IValueContainer)]
    public interface IValueContainer<T> : IEnumerable<T>, ILazinator where T : ILazinator
    {
        /// <summary>
        /// Indicates whether the container type is balanced or not.
        /// </summary>
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        IValueContainer<T> CreateNewWithSameSettings();
        bool IsShorterThan(IValueContainer<T> second);
        /// <summary>
        /// Indicates whether this container should be split in two, given the split threshold.
        /// </summary>
        /// <param name="splitThreshold"></param>
        /// <returns></returns>
        bool ShouldSplit(int splitThreshold);
        /// <summary>
        /// Splits off the beginning of this container.
        /// </summary>
        /// <returns></returns>
        IValueContainer<T> SplitOff();
        bool Any();
        T First();
        T FirstOrDefault();
        T Last();
        T LastOrDefault();
        /// <summary>
        /// Finds the item in a sorted container, using the comparer. If the item is found, the exact location is returned. Otherwise,
        /// the next location is returned (or null, if it would be after the end of the list).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns>The location, along with an indication of whether an exact match was found</returns>
        (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer);
        IContainerLocation FirstLocation();
        IContainerLocation LastLocation();
        T GetAt(IContainerLocation location);
        void SetAt(IContainerLocation location, T value);
        void RemoveAt(IContainerLocation location);
        void InsertAt(IContainerLocation location, T item);
        bool Contains(T item, IComparer<T> comparer);
        /// <summary>
        /// Gets a matching value using a custom comparer, which may match on only part of the item.
        /// </summary>
        bool GetValue(T item, IComparer<T> comparer, out T match);
        /// <summary>
        /// Inserts a matching value, replacing any existing match.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns>True if the result is an insertion rather than a replacement</returns>
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer);
        /// <summary>
        /// Removes a matching value.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        bool TryRemove(T item, IComparer<T> comparer);
        void Clear();
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
        IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0);
        IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer);
        IEnumerator<T> GetEnumerator(bool reverse, T startValue, IComparer<T> comparer);
    }
}
