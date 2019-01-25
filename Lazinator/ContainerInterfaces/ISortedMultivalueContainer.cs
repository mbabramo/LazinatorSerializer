using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.ContainerInterfaces.Location;
using Lazinator.Core;

namespace Lazinator.ContainerInterfaces
{
    [NonexclusiveLazinator((int)LazinatorCoreUniqueIDs.ISortedMultivalueContainer)]
    public interface ISortedMultivalueContainer<T> : IMultivalueContainer<T>, ISortedValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne);
        bool TryRemove(T item, MultivalueLocationOptions whichOne);
        long Count(T item);
        bool TryRemoveAll(T item);
    }
}
