using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorDictionaryableFactory)]
    public interface ILazinatorDictionaryableFactory<TKey, TValue> : ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        ILazinatorDictionaryable<TKey, TValue> CreateDictionaryable();
    }
}
