using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorDictionaryable)]
    public interface ILazinatorDictionaryable<TKey, TValue> : IDictionary<TKey, TValue>, ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        bool IsSorted { get; }
    }
}
