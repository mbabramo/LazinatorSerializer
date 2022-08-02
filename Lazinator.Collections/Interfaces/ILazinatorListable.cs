using Lazinator.Attributes;
using Lazinator.Core;
using System.Collections.Generic;

namespace Lazinator.Collections
{
    /// <summary>
    /// A Lazinator interface for lists of Lazinator items. This is implemented by types including LazinatorLinkedList, LazinatorList, LazinatorSortedLinkedList, and LazinatorSortedList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListable)]
    public interface ILazinatorListable<T> : IList<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        T GetAtIndex(long index);
        void SetAtIndex(long index, T value);
        void InsertAtIndex(long index, T item);
        void RemoveAtIndex(long index);
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
        IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer);
        bool Any();
        T First();
        T FirstOrDefault();
        T Last();
        T LastOrDefault();
    }
}
