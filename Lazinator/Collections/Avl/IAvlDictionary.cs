using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlDictionary)]
    public interface IAvlDictionary<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        ISortedKeyMultivalueContainer<WUint, LazinatorTuple<TKey, TValue>> UnderlyingTree { get; set; }
        bool AllowDuplicates { get; set; }
    }
}
