using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListable)]
    public interface ILazinatorListable<T> : ILazinatorIndexable<T> where T : ILazinator
    {
        void InsertAt(long index, T item);
        void RemoveAt(long index);
        IEnumerable<T> EnumerateFromStart();
        IEnumerable<T> EnumerateFrom(long index);
    }
}
