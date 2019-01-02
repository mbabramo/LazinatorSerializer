using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyableMultivalue)]
    public interface ILazinatorKeyableMultivalue<TKey, TValue> : ILazinatorKeyable<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        void AddValue(TKey key, TValue value);
        IEnumerable<TValue> GetAllValues(TKey key);
        bool RemoveAll(TKey item);
    }
}