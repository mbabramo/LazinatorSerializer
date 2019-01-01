using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorListable<T>, ILazinatorIndexable<T>, ILazinatorCountable where T : ILazinator, IComparable<T>
    {
        void Insert(T item);
        bool Remove(T item);
    }
}
