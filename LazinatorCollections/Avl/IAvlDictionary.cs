using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace LazinatorCollections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlDictionary)]
    public interface IAvlDictionary<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        ISortedKeyMultivalueContainer<WUint, LazinatorKeyValue<TKey, TValue>> UnderlyingTree { get; set; }
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
    }
}
