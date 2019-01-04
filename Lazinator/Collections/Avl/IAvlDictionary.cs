using Lazinator.Attributes;
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
        ILazinatorOrderedKeyable<WUint, LazinatorTuple<TKey, TValue>> UnderlyingTree { get; set; }
        bool AllowDuplicateKeys { get; set; }
    }
}
