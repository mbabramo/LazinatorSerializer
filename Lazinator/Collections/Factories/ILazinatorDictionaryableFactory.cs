using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorDictionaryableFactory)]
    public interface ILazinatorDictionaryableFactory<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        ILazinatorDictionaryable<TKey, TValue> CreateDictionaryable();
    }
}
