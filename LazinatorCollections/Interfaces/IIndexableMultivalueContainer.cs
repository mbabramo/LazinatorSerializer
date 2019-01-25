using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableMultivalueContainer)]
    public interface IIndexableMultivalueContainer<T> : IIndexableValueContainer<T>, IMultivalueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer);
    }
}
