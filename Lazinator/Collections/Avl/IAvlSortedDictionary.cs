using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedDictionary)]
    public interface IAvlSortedDictionary<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        ILazinatorOrderedKeyable<TKey, TValue> UnderlyingTree { get; set; }
    }
}