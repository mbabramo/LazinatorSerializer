using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.LazinatorIndexable)]
    public interface ILazinatorIndexable<T> where T : ILazinator
    {
        T GetAt(long index);
        void SetAt(long index, T value);
    }
}
