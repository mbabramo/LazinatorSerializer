using Lazinator.Attributes;
using Lazinator.Core;
using System.Collections.Generic;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A nonexclusive Lazinator interfaces for dictionary types.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorDictionaryable)]
    public interface ILazinatorDictionaryable<TKey, TValue> : IDictionary<TKey, TValue>, ILazinator where TKey : ILazinator where TValue : ILazinator
    {
        bool IsSorted { get; }
    }
}
