using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.ContainerInterfaces
{
    [NonexclusiveLazinator((int)LazinatorCoreUniqueIDs.ISortedIndexableContainer)]
    public interface ISortedIndexableContainer<T> : ISortedValueContainer<T>, IIndexableValueContainer<T>, ILazinator where T : ILazinator, IComparable<T>
    {
        (long index, bool exists) FindIndex(T target);
    }
}
