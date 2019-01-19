using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListable)]
    public interface ILazinatorListable<T> : IList<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        T GetAtIndex(long index);
        void SetAtIndex(long index, T value);
        void InsertAtIndex(long index, T item);
        void RemoveAtIndex(long index);
        IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0);
        bool Any();
        T First();
        T FirstOrDefault();
        T Last();
        T LastOrDefault();
    }
}
