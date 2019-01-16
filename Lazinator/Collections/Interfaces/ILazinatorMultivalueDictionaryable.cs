using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorMultivalueDictionaryable)]
    public interface ILazinatorMultivalueDictionaryable<TKey, TValue> : IDictionary<TKey, TValue>, ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        bool IsSorted { get; }

        // The following are also included in ISortedKeyMultivalueContainer. However, ILazinatorMultivalueDictionaryable<TKey, TValue> does not require TKey to be comparable (because implementations may store by hash).
        IEnumerable<TValue> GetAllValues(TKey key);
        void AddValueForKey(TKey key, TValue value);
        bool TryRemoveKeyValue(TKey key, TValue value);
        bool TryRemoveAll(TKey key);
    }
}