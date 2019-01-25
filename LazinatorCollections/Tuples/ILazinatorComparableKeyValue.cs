using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorComparableKeyValue)]
    public interface ILazinatorComparableKeyValue<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }
}
