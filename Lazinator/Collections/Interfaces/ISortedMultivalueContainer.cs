using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedMultivalueContainer)]
    public interface ISortedMultivalueContainer<T> : IMultivalueContainer<T> where T : ILazinator, IComparable<T>
    {
        bool TryInsert(T item, MultivalueLocationOptions whichOne);
        bool TryRemove(T item, MultivalueLocationOptions whichOne);
        int Count(T item);
        bool TryRemoveAll(T item);
    }
}
