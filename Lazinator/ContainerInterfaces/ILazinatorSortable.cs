using Lazinator.Attributes;
using Lazinator.ContainerInterfaces;
using Lazinator.ContainerInterfaces.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.ContainerInterfaces
{
    /// <summary>
    /// An interface allowing insertion and removal of sorted items, using a custom comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCoreUniqueIDs.ILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorListable<T>, ILazinator where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer);
        bool TryRemove(T item, IComparer<T> comparer);
        (long index, bool exists) FindIndex(T target, IComparer<T> comparer);

        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer);
        (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
