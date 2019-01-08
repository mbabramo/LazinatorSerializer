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
        bool TryInsertSorted(T item, MultivalueLocationOptions whichOne);
        bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne);
        int Count(T item);
        bool TryRemoveAll(T item);
    }
}
