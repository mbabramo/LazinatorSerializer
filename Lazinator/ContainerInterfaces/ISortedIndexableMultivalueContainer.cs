using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.ContainerInterfaces
{
    [NonexclusiveLazinator((int)LazinatorCoreUniqueIDs.ISortedIndexableMultivalueContainer)]
    public interface ISortedIndexableMultivalueContainer<T> : ISortedMultivalueContainer<T>, IIndexableValueContainer<T>, ISortedIndexableContainer<T>,  IIndexableMultivalueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne);
    }
}
