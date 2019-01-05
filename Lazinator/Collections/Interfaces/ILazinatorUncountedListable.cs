using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorUncountedListable)]
    public interface ILazinatorUncountedListable<T> : ILazinatorIndexable<T>, IEnumerable<T>, ILazinator, ILazinatorSplittable where T : ILazinator
    {
        void InsertAt(long index, T item);
        void RemoveAt(long index);
        IEnumerable<T> AsEnumerable(long index);
    }
}
