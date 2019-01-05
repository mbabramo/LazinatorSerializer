using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorOrderedKeyableFactory)]
    public interface ILazinatorOrderedKeyableFactory<TKey, TValue> : ILazinator, ILazinatorKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        ILazinatorOrderedKeyable<TKey, TValue> Create();
    }
}
